const token = localStorage.getItem("token");
const role = localStorage.getItem("role");
const tableBody = document.querySelector("#clientTable tbody");
const msg = document.getElementById("msg");

if (!token) window.location.href = "/pages/login.html";

// Mostrar el botón solo si Admin u Operator
if (role === "Admin" || role === "Operator") {
    document.getElementById("adminControls").style.display = "block";
}

// Cargar datos
fetch("https://localhost:7127/api/clients", {
    headers: { 'Authorization': `Bearer ${token}` }
})
    .then(res => res.ok ? res.json() : Promise.reject())
    .then(data => {
        if (!data.length) {
            msg.textContent = "No clients found.";
            return;
        }

        data.forEach(c => {
            const row = document.createElement("tr");

            let actions = "";
            if (role === "Admin" || role === "Operator") {
                actions += `
                    <button class="btn editBtn" data-id="${c.clientId}">✏️ Edit</button>
                    <button class="btn deleteBtn" data-id="${c.clientId}">🗑️ Delete</button>
                `;
            }

            row.innerHTML = `
                <td class="name">${c.firstName} ${c.lastName}</td>
                <td class="email">${c.email}</td>
                <td class="phone">${c.phone}</td>
                <td>${actions}</td>
            `;
            tableBody.appendChild(row);
        });
    })
    .catch(() => {
        msg.textContent = "Error loading clients.";
        showToast("Error loading clients", "error");
    });

// Editar / Eliminar / Guardar
tableBody.addEventListener("click", async (e) => {
    const button = e.target.closest("button");
    if (!button) return;

    const row = button.closest("tr");
    const clientId = button.dataset.id;
    if (!row || !clientId) return;

    const isEdit = button.classList.contains("editBtn");
    const isSave = button.classList.contains("saveBtn");
    const isDelete = button.classList.contains("deleteBtn");

    if (isDelete) {
        const confirmed = confirm("Are you sure you want to delete this client?");
        if (!confirmed) return;

        try {
            const res = await fetch(`https://localhost:7127/api/clients/${clientId}`, {
                method: "DELETE",
                headers: { 'Authorization': `Bearer ${token}` }
            });

            if (!res.ok) throw new Error();
            row.remove();
            showToast("Client deleted successfully", "success");
        } catch {
            showToast("Failed to delete client", "error");
        }
    }

    if (isEdit) {
        const confirmed = confirm("Do you want to edit this client?");
        if (!confirmed) return;

        const nameCell = row.querySelector(".name");
        const emailCell = row.querySelector(".email");
        const phoneCell = row.querySelector(".phone");

        const [firstName, lastName] = nameCell.textContent.trim().split(" ");

        nameCell.innerHTML = `
            <input type="text" class="edit-first" value="${firstName}" data-original="${firstName}" />
            <input type="text" class="edit-last" value="${lastName}" data-original="${lastName}" />
        `;
        emailCell.innerHTML = `<input type="email" class="edit-email" value="${emailCell.textContent.trim()}" data-original="${emailCell.textContent.trim()}" />`;
        phoneCell.innerHTML = `<input type="text" class="edit-phone" value="${phoneCell.textContent.trim()}" data-original="${phoneCell.textContent.trim()}" />`;

        button.textContent = "💾 Save";
        button.classList.replace("editBtn", "saveBtn");
    }

    if (isSave) {
        const updatedClient = {
            firstName: row.querySelector(".edit-first").value.trim(),
            lastName: row.querySelector(".edit-last").value.trim(),
            email: row.querySelector(".edit-email").value.trim(),
            phone: row.querySelector(".edit-phone").value.trim()
        };

        const original = {
            firstName: row.querySelector(".edit-first").dataset.original,
            lastName: row.querySelector(".edit-last").dataset.original,
            email: row.querySelector(".edit-email").dataset.original,
            phone: row.querySelector(".edit-phone").dataset.original
        };

        const changed = Object.keys(updatedClient).some(k => updatedClient[k] !== original[k]);
        if (!changed) {
            showToast("No changes made.", "info");
            return;
        }

        const confirmed = confirm("Do you want to save the changes?");
        if (!confirmed) return;

        try {
            const res = await fetch(`https://localhost:7127/api/clients/${clientId}`, {
                method: "PUT",
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(updatedClient)
            });

            if (!res.ok) throw new Error();

            row.querySelector(".name").textContent = `${updatedClient.firstName} ${updatedClient.lastName}`;
            row.querySelector(".email").textContent = updatedClient.email;
            row.querySelector(".phone").textContent = updatedClient.phone;

            button.textContent = "✏️ Edit";
            button.classList.replace("saveBtn", "editBtn");

            showToast("Client updated successfully", "success");
        } catch {
            showToast("Failed to update client", "error");
        }
    }
});




// Crear nuevo
document.getElementById("addBtn")?.addEventListener("click", () => {
    window.location.href = "/pages/create-client.html";
});

// Logout
document.getElementById("logoutBtn")?.addEventListener("click", () => {
    localStorage.clear();
    window.location.href = "/pages/login.html";
});
