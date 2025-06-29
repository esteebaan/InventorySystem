const token = localStorage.getItem("token");
const employeeId = localStorage.getItem("employeeId");

if (!token) window.location.href = "/pages/login.html";

const clientSelect = document.getElementById("clientId");
const articleSelect = document.getElementById("articleId");
const statusSelect = document.getElementById("status");
const form = document.getElementById("loanForm");
const msg = document.getElementById("msg");

// Validar employeeId
if (!employeeId) {
    msg.style.color = "red";
    msg.textContent = "User role or employee ID missing. Please log in again.";
}

// Población de estados permitidos
const statuses = ["Pending", "Approved", "Rejected"];
statuses.forEach(s => {
    const opt = document.createElement("option");
    opt.value = s;
    opt.textContent = s;
    statusSelect.appendChild(opt);
});

// Cargar clientes y artículos
Promise.all([
    fetch("https://localhost:7127/api/clients", { headers: { Authorization: `Bearer ${token}` } }),
    fetch("https://localhost:7127/api/articles", { headers: { Authorization: `Bearer ${token}` } }),
])
    .then(async ([resC, resA]) => {
        if (!resC.ok || !resA.ok) throw new Error("Failed to load clients or articles");
        const [clients, articles] = await Promise.all([resC.json(), resA.json()]);
        clients.forEach(c => {
            const opt = document.createElement("option");
            opt.value = c.clientId;
            opt.textContent = `${c.firstName} ${c.lastName}`;
            clientSelect.appendChild(opt);
        });
        articles.forEach(a => {
            const opt = document.createElement("option");
            opt.value = a.articleId;
            opt.textContent = `${a.name} (${a.code})`;
            articleSelect.appendChild(opt);
        });
    })
    .catch(err => {
        msg.style.color = "red";
        msg.textContent = "Error loading data.";
        console.error(err);
    });

// Enviar préstamo
form.addEventListener("submit", async e => {
    e.preventDefault();
    msg.textContent = "";

    const loan = {
        clientId: clientSelect.value,
        articleId: articleSelect.value,
        employeeId: employeeId,
        deliveredAt: document.getElementById("deliveredAt").value,
        status: statusSelect.value
    };

    // Validación
    if (!loan.clientId || !loan.articleId || !loan.employeeId || !loan.deliveredAt || !loan.status) {
        msg.style.color = "red";
        msg.textContent = "🌟 Completa todos los campos obligatorios.";
        return;
    }

    try {
        const res = await fetch("https://localhost:7127/api/loans", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(loan)
        });

        const text = await res.text();
        let errMsg = text;
        try { errMsg = JSON.parse(text).message ?? text; } catch { }

        if (!res.ok) throw new Error(`${res.status} - ${errMsg}`);

        msg.style.color = "green";
        msg.textContent = "✅ Loan registered successfully!";
        form.reset();
    } catch (err) {
        msg.style.color = "red";
        msg.textContent = `⚠️ Error registering loan: ${err.message}`;
        console.error("Loan creation error:", err);
    }
});
