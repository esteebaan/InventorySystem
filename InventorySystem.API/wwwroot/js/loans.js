const token = localStorage.getItem('token');
const role = localStorage.getItem('role');

const tableBody = document.querySelector("#loanTable tbody");
const msg = document.getElementById("msg");

let clients = [];
let articles = [];
let loans = [];

if (!token) window.location.href = "/pages/login.html";
if (role !== 'Admin' && role !== 'Operator') {
    alert("Access denied.");
    window.location.href = "/pages/access-denied.html";
}

// 🔁 Cargar préstamos, clientes y artículos
Promise.all([
    fetch("https://localhost:7127/api/loans", { headers: { 'Authorization': `Bearer ${token}` } }).then(r => r.json()),
    fetch("https://localhost:7127/api/clients", { headers: { 'Authorization': `Bearer ${token}` } }).then(r => r.json()),
    fetch("https://localhost:7127/api/articles", { headers: { 'Authorization': `Bearer ${token}` } }).then(r => r.json())
])
    .then(([loanData, clientData, articleData]) => {
        clients = clientData;
        articles = articleData;
        loans = loanData;

        renderTable(loans);
        populateFilters();
    })
    .catch(() => msg.textContent = "Error loading data.");

// 🧩 Reemplazar IDs por nombres
function getClientName(id) {
    const c = clients.find(c => c.clientId === id);
    return c ? `${c.firstName} ${c.lastName}` : id;
}

function getArticleName(id) {
    const a = articles.find(a => a.articleId === id);
    return a ? a.name : id;
}

// 📊 Renderizar tabla
function renderTable(data) {
    tableBody.innerHTML = "";

    data.forEach(l => {
        const row = document.createElement("tr");
        const delivery = l.deliveredAt?.slice(0, 10) || "-";
        const returned = l.returnedAt ? l.returnedAt.slice(0, 10) : "-";

        let actions = "";

        if ((role === "Admin" || role === "Operator") && !l.returnedAt) {
            actions += `<button class="btn return-btn" data-id="${l.loanId}">✅ Mark Returned</button>`;
        }

        if (role === "Admin") {
            actions += `<button class="btn delete-btn" data-id="${l.loanId}">🗑️ Delete</button>`;

            if (l.status === "Pending") {
                actions += `
                    <button class="btn approve-btn" data-id="${l.loanId}">✅ Approve</button>
                    <button class="btn reject-btn" data-id="${l.loanId}">❌ Reject</button>
                `;
            }
        }

        row.innerHTML = `
            <td>${getClientName(l.clientId)}</td>
            <td>${getArticleName(l.articleId)}</td>
            <td>${delivery}</td>
            <td>${returned}</td>
            <td>${l.status}</td>
            <td>${actions}</td>
        `;

        tableBody.appendChild(row);
    });
}

// 📋 Filtros
function populateFilters() {
    const clientSelect = document.getElementById("clientFilter");
    clients.forEach(c => {
        const opt = document.createElement("option");
        opt.value = c.clientId;
        opt.textContent = `${c.firstName} ${c.lastName}`;
        clientSelect.appendChild(opt);
    });

    const articleSelect = document.getElementById("articleFilter");
    articles.forEach(a => {
        const opt = document.createElement("option");
        opt.value = a.articleId;
        opt.textContent = a.name;
        articleSelect.appendChild(opt);
    });

    document.querySelectorAll(".filters select").forEach(select => {
        select.addEventListener("change", () => {
            const client = document.getElementById("clientFilter").value;
            const article = document.getElementById("articleFilter").value;
            const status = document.getElementById("statusFilter").value;

            const filtered = loans.filter(l =>
                (!client || l.clientId === client) &&
                (!article || l.articleId === article) &&
                (!status || l.status === status)
            );

            renderTable(filtered);
        });
    });
}

// 🎯 Acciones sobre préstamos
tableBody.addEventListener("click", async (e) => {
    if (e.target.tagName !== "BUTTON" || !e.target.dataset.id) return;

    const id = e.target.dataset.id;
    let update = null;
    let endpoint = `https://localhost:7127/api/loans/${id}`;
    let method = "PUT";
    let action = "";

    if (e.target.classList.contains("return-btn")) {
        if (!confirm("Mark this loan as returned?")) return;
        update = {
            returnedAt: new Date().toISOString(),
            status: "Returned"
        };
        action = "marked as returned";
    }

    if (e.target.classList.contains("approve-btn")) {
        if (!confirm("Approve this loan?")) return;
        endpoint += "/status";
        update = "Approved";
        action = "approved";
    }

    if (e.target.classList.contains("reject-btn")) {
        if (!confirm("Reject this loan?")) return;
        endpoint += "/status";
        update = "Rejected";
        action = "rejected";
    }

    if (e.target.classList.contains("delete-btn")) {
        if (!confirm("Delete this loan?")) return;
        try {
            const res = await fetch(`https://localhost:7127/api/loans/${id}`, {
                method: "DELETE",
                headers: { Authorization: `Bearer ${token}` }
            });
            if (!res.ok) throw new Error();
            showToast("Loan deleted successfully!", "success");
            e.target.closest("tr").remove();
        } catch {
            showToast("Failed to delete loan.", "error");
        }
        return;
    }

    if (!update) return;

    try {
        const res = await fetch(endpoint, {
            method,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(update)
        });

        if (!res.ok) throw new Error();
        showToast(`Loan ${action} successfully.`, "success");
        setTimeout(() => window.location.reload(), 500);
    } catch {
        showToast("Error processing loan update.", "error");
    }
});

document.getElementById("exportExcelBtn").addEventListener("click", () => {
    const data = [];

    document.querySelectorAll("#loanTable tbody tr").forEach(row => {
        const cells = row.querySelectorAll("td");
        data.push({
            Client: cells[0].textContent,
            Article: cells[1].textContent,
            Delivered: cells[2].textContent,
            Returned: cells[3].textContent,
            Status: cells[4].textContent
        });
    });

    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Loans");
    XLSX.writeFile(workbook, "loans-report.xlsx");
});


// ➕ Ir a registrar préstamo
document.getElementById("addBtn").addEventListener("click", () => {
    window.location.href = "/pages/create-loan.html";
});

// 🔐 Logout
document.getElementById("logoutBtn")?.addEventListener("click", () => {
    localStorage.clear();
    window.location.href = "/pages/login.html";
});
