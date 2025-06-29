const token = localStorage.getItem('token');
const role = localStorage.getItem('role');
const tableBody = document.querySelector('#articleTable tbody');
const msg = document.getElementById('msg');

if (!token) window.location.href = "/pages/login.html";

// Mostrar controles solo si es Admin u Operator
if (role === 'Admin' || role === 'Operator') {
    document.getElementById('adminControls').style.display = 'block';
}

fetch('https://localhost:7127/api/articles', {
    headers: { Authorization: `Bearer ${token}` }
})
    .then(res => {
        if (!res.ok) throw new Error("Error loading articles");
        return res.json();
    })
    .then(data => {
        if (!data.length) {
            msg.textContent = "No articles found.";
            return;
        }

        data.forEach(a => {
            const row = document.createElement("tr");

            let actions = "";
            if (role === "Admin" || role === "Operator") {
                actions += `<button class="btn edit-btn" data-id="${a.articleId}">✏️ Edit</button>`;
            }
            if (role === "Admin") {
                actions += `<button class="btn delete-btn" data-id="${a.articleId}">🗑️ Delete</button>`;
            }

            row.innerHTML = `
                <td class="code">${a.code}</td>
                <td class="name">${a.name}</td>
                <td class="category">${a.category}</td>
                <td class="status">${a.status}</td>
                <td class="location">${a.location}</td>
                <td>${actions}</td>
            `;

            tableBody.appendChild(row);
        });
    })
    .catch(() => {
        msg.style.color = "red";
        msg.textContent = "Failed to load article list.";
        showToast("Failed to load article list.", "error");
    });

// Confirmación y acciones
tableBody.addEventListener("click", async (e) => {
    const button = e.target.closest("button");
    if (!button) return;

    const row = button.closest("tr");
    const articleId = button.dataset.id;
    if (!row || !articleId) return;

    const isEdit = button.classList.contains("edit-btn");
    const isSave = button.classList.contains("save-btn");
    const isDelete = button.classList.contains("delete-btn");

    // 🗑️ Eliminar artículo
    if (isDelete) {
        const confirmed = confirm("Are you sure you want to delete this article?");
        if (!confirmed) return;

        try {
            const res = await fetch(`https://localhost:7127/api/articles/${articleId}`, {
                method: "DELETE",
                headers: { Authorization: `Bearer ${token}` }
            });

            if (!res.ok) throw new Error();
            row.remove();
            showToast("Article deleted successfully", "success");
        } catch {
            showToast("Failed to delete article", "error");
        }
    }

    // ✏️ Editar artículo
    if (isEdit) {
        const confirmed = confirm("Do you want to edit this article?");
        if (!confirmed) return;

        const fields = ["code", "name", "category", "status", "location"];
        fields.forEach(field => {
            const cell = row.querySelector(`.${field}`);
            const val = cell.textContent.trim();
            cell.innerHTML = `<input class="edit-${field}" value="${val}" data-original="${val}" />`;
        });

        button.textContent = "💾 Save";
        button.classList.replace("edit-btn", "save-btn");
    }

    // 💾 Guardar cambios
    if (isSave) {
        const updatedArticle = {};
        const fields = ["code", "name", "category", "status", "location"];
        let changed = false;

        fields.forEach(field => {
            const input = row.querySelector(`.edit-${field}`);
            const newVal = input.value.trim();
            const originalVal = input.dataset.original;
            updatedArticle[field] = newVal;
            if (newVal !== originalVal) changed = true;
        });

        if (!changed) {
            showToast("No changes made.", "info");
            return;
        }

        const confirmed = confirm("Do you want to save the changes?");
        if (!confirmed) return;

        try {
            const res = await fetch(`https://localhost:7127/api/articles/${articleId}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify(updatedArticle)
            });

            if (!res.ok) throw new Error();

            fields.forEach(field => {
                row.querySelector(`.${field}`).textContent = updatedArticle[field];
            });

            button.textContent = "✏️ Edit";
            button.classList.replace("save-btn", "edit-btn");

            showToast("Article updated successfully", "success");
        } catch {
            showToast("Failed to update article", "error");
        }
    }
});





document.getElementById("exportPdfBtn").addEventListener("click", () => {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    const headers = [["Code", "Name", "Category", "Status", "Location"]];
    const rows = [];

    document.querySelectorAll("#articleTable tbody tr").forEach(row => {
        const cells = Array.from(row.querySelectorAll("td")).slice(0, 5);
        rows.push(cells.map(td => td.textContent));
    });

    doc.text("Article Report", 14, 15);
    doc.autoTable({ head: headers, body: rows, startY: 20 });
    doc.save("articles-report.pdf");
});

document.getElementById("addBtn").addEventListener("click", () => {
    window.location.href = "/pages/create-article.html";
});

document.getElementById("logoutBtn")?.addEventListener("click", () => {
    localStorage.clear();
    window.location.href = "/pages/login.html";
});
