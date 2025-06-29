const token = localStorage.getItem('token');
const role = localStorage.getItem('role');

const form = document.getElementById('userForm');
const msg = document.getElementById('msg');

// Mostrar campos solo si el usuario es admin
if (token && role === 'Admin') {
    document.getElementById("roleSelectContainer").style.display = "block";
    document.getElementById("userListSection").style.display = "block";
    loadUsers();
} else {
    document.getElementById("roleSelectContainer")?.remove();
    document.getElementById("userListSection")?.remove();
}

if (token && role !== 'Admin') {
    alert("Access denied.");
    window.location.href = "/pages/access-denied.html";
}

// Crear usuario
form.addEventListener('submit', async (e) => {
    e.preventDefault();

    const payload = {
        firstName: document.getElementById('firstName').value.trim(),
        lastName: document.getElementById('lastName').value.trim(),
        email: document.getElementById('email').value.trim(),
        password: document.getElementById('password').value.trim()
    };

    const roleField = document.getElementById('role');

    if (token && role === 'Admin' && roleField && roleField.value) {
        payload.role = roleField.value;
    } else {
        // Asignar Operator por defecto si se está registrando sin autenticación
        payload.role = "Operator";
    }

    try {
        const res = await fetch('https://localhost:7127/api/users', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                ...(token && { 'Authorization': `Bearer ${token}` })
            },
            body: JSON.stringify(payload)
        });

        if (!res.ok) {
            const error = await res.json();
            msg.style.color = 'red';
            msg.textContent = error.message || 'Failed to create user.';
            return;
        }

        msg.style.color = 'green';
        msg.textContent = 'User created successfully!';
        form.reset();

        if (!token) {
            alert("Account created! Please log in.");
            window.location.href = "/pages/login.html";
        } else {
            loadUsers();
        }

    } catch (err) {
        msg.style.color = 'red';
        msg.textContent = 'Error creating user.';
    }
});


// 🔁 Cargar usuarios
function loadUsers() {
    fetch("https://localhost:7127/api/users", {
        headers: { 'Authorization': `Bearer ${token}` }
    })
        .then(res => res.json())
        .then(data => {
            const tbody = document.querySelector("#userTable tbody");
            tbody.innerHTML = "";
            data.forEach(u => {
                const row = document.createElement("tr");
                row.innerHTML = `
                    <td class="name">${u.firstName} ${u.lastName}</td>
                    <td class="email">${u.email}</td>
                    <td class="role">${u.role}</td>
                    <td>
                        <button class="btn editBtn" data-id="${u.userId}">✏️ Edit</button>
                    </td>
                `;
                tbody.appendChild(row);
            });
        });
}

// ✏️ Editar usuario
document.querySelector("#userTable").addEventListener("click", async (e) => {
    if (!e.target.classList.contains("editBtn") && !e.target.classList.contains("saveBtn")) return;

    const row = e.target.closest("tr");
    const userId = e.target.dataset.id;

    if (e.target.classList.contains("editBtn")) {
        // Transformar a modo edición
        const nameCell = row.querySelector(".name");
        const emailCell = row.querySelector(".email");
        const roleCell = row.querySelector(".role");

        const [firstName, lastName] = nameCell.textContent.trim().split(" ");

        nameCell.innerHTML = `
            <input type="text" class="edit-first" value="${firstName}" style="width: 45%" />
            <input type="text" class="edit-last" value="${lastName}" style="width: 45%" />
        `;
        emailCell.innerHTML = `<input type="email" class="edit-email" value="${emailCell.textContent.trim()}" />`;
        roleCell.innerHTML = `
            <select class="edit-role">
                <option value="Admin" ${roleCell.textContent === "Admin" ? "selected" : ""}>Admin</option>
                <option value="Operator" ${roleCell.textContent === "Operator" ? "selected" : ""}>Operator</option>
            </select>
        `;

        e.target.textContent = "💾 Save";
        e.target.classList.remove("editBtn");
        e.target.classList.add("saveBtn");
    } else {
        // Guardar cambios
        const updatedUser = {
            firstName: row.querySelector(".edit-first").value.trim(),
            lastName: row.querySelector(".edit-last").value.trim(),
            email: row.querySelector(".edit-email").value.trim(),
            role: row.querySelector(".edit-role").value
        };

        try {
            const res = await fetch(`https://localhost:7127/api/users/${userId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(updatedUser)
            });

            if (!res.ok) throw new Error();
            alert("User updated.");
            loadUsers();
        } catch {
            alert("Failed to update user.");
        }
    }
});

// 🔐 Logout
document.getElementById("logoutBtn")?.addEventListener("click", () => {
    localStorage.clear();
    window.location.href = "/pages/login.html";
});



                
/*<td>${u.role}</td>*/