const token = localStorage.getItem("token");
const role = localStorage.getItem("role");
if (!token) window.location.href = "/pages/login.html";

if (role !== "Admin") {
    alert("Access denied");
    window.location.href = "/pages/dashboard.html";
}

const form = document.getElementById("articleForm");
const msg = document.getElementById("msg");

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const article = {
        code: document.getElementById("code").value.trim(),
        name: document.getElementById("name").value.trim(),
        category: document.getElementById("category").value.trim(),
        status: document.getElementById("status").value.trim(),
        location: document.getElementById("location").value.trim()
    };

    try {
        const res = await fetch("https://localhost:7127/api/articles", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(article)
        });

        if (!res.ok) throw new Error();

        msg.style.color = "green";
        msg.textContent = "Article saved successfully!";
        form.reset();
    } catch {
        msg.style.color = "red";
        msg.textContent = "Failed to save article.";
    }
});
