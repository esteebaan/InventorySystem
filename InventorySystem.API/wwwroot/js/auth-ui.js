// ✅ Ejecutar al cargar cada página
document.addEventListener("DOMContentLoaded", () => {
    const token = localStorage.getItem("token");
    const role = localStorage.getItem("role");

    if (!token) {
        window.location.href = "/pages/login.html";
        return;
    }

    // ✅ Mostrar secciones para Admin
    if (role === "Admin") {
        document.querySelectorAll(".admin-only").forEach(el => {
            el.style.display = "block";
        });
    }

    // ✅ Ocultar las secciones de Admin si no lo es
    if (role !== "Admin") {
        document.querySelectorAll(".admin-only").forEach(el => {
            el.remove();
        });
    }

    // ✅ Logout funcional en todas las páginas
    document.getElementById("logoutBtn")?.addEventListener("click", () => {
        localStorage.clear();
        window.location.href = "/pages/login.html";
    });
});
