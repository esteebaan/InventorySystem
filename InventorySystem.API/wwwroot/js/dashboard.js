const msg = document.getElementById('msg');
const role = localStorage.getItem('role');
const token = localStorage.getItem('token');

if (!token) {
    window.location.href = "/pages/login.html";
}

// Mostrar el rol
document.getElementById("roleInfo").textContent = `Logged in as: ${role}`;

// Ocultar sección admin si no corresponde
if (role !== 'Admin' && role !== 'Operator') {
    alert("Access denied.");
    window.location.href = "/pages/access-denied.html";
}

// Botón logout
document.getElementById("logoutBtn")?.addEventListener("click", () => {
    localStorage.clear();
    window.location.href = "/pages/login.html";
});

// Contadores básicos
function fetchCount(endpoint, elementId) {
    fetch(`https://localhost:7127/api/${endpoint}`, {
        headers: { 'Authorization': `Bearer ${token}` }
    })
        .then(res => res.json())
        .then(data => {
            document.getElementById(elementId).textContent = data.length;
        })
        .catch(() => {
            document.getElementById(elementId).textContent = "Error";
            msg.textContent = "Some resources failed to load.";
            msg.style.color = "red";
        });
}

fetchCount("clients", "clientCount");
fetchCount("articles", "articleCount");
fetchCount("loans", "loanCount");

// Resumen de préstamos
fetch("https://localhost:7127/api/loans", {
    headers: { 'Authorization': `Bearer ${token}` }
})
    .then(res => res.json())
    .then(loans => {
        const total = loans.length;
        const returned = loans.filter(l => l.status === "Returned").length;
        const active = loans.filter(l => l.status === "Pending" || l.status === "Approved").length;

        document.getElementById("totalLoans").textContent = total;
        document.getElementById("returnedLoans").textContent = returned;
        document.getElementById("activeLoans").textContent = active;

        // Gráfico de préstamos por estado
        const statusCounts = {
            Pending: 0,
            Approved: 0,
            Rejected: 0,
            Returned: 0
        };

        loans.forEach(l => {
            if (statusCounts.hasOwnProperty(l.status)) {
                statusCounts[l.status]++;
            }
        });

        new Chart(document.getElementById("loanChart"), {
            type: "bar",
            data: {
                labels: Object.keys(statusCounts),
                datasets: [{
                    label: "Loans by Status",
                    data: Object.values(statusCounts),
                    backgroundColor: ["#ffc107", "#0d6efd", "#dc3545", "#198754"]
                }]
            },
            options: {
                responsive: true,
                scales: { y: { beginAtZero: true } }
            }
        });
    })
    .catch(() => {
        msg.textContent = "Failed to load loan summary.";
        msg.style.color = "red";
    });
