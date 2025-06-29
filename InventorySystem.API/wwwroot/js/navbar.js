document.addEventListener("DOMContentLoaded", () => {
    const role = localStorage.getItem("role");
    const container = document.createElement("header");
    container.className = "navbar";
    container.innerHTML = `
        <h1 class="brand">📦 Inventory System</h1>
        <nav>
            <a href="/pages/dashboard.html">Dashboard</a>
            <a href="/pages/clients.html">Clients</a>
            <a href="/pages/articles.html">Articles</a>
            <a href="/pages/loans.html">Loans</a>
            ${role === "Admin" ? '<a href="/pages/create-user.html">Users</a>' : ''}
            <button id="logoutBtn">Logout</button>
        </nav>
    `;

    document.body.prepend(container);

    document.getElementById("logoutBtn")?.addEventListener("click", () => {
        localStorage.clear();
        window.location.href = "/pages/login.html";
    });
});
