const token = localStorage.getItem('token');
if (!token) {
    window.location.href = "/pages/login.html";
}

document.getElementById("logoutBtn")?.addEventListener("click", () => {
    localStorage.clear();
    window.location.href = "/pages/login.html";
});
