const token = localStorage.getItem("token");
const role = localStorage.getItem("role");
if (!token) window.location.href = "/pages/login.html";
if (role !== "Admin") {
    alert("Access denied");
    window.location.href = "/pages/dashboard.html";
}

const form = document.getElementById("clientForm");
const msg = document.getElementById("msg");

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const client = {
        firstName: document.getElementById("firstName").value.trim(),
        lastName: document.getElementById("lastName").value.trim(),
        email: document.getElementById("email").value.trim(),
        phone: document.getElementById("phone").value.trim()
    };

    try {
        const res = await fetch("https://localhost:7127/api/clients", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(client)
        });

        if (!res.ok) throw new Error();

        msg.style.color = "green";
        msg.textContent = "Client saved successfully!";
        form.reset();
    } catch {
        msg.style.color = "red";
        msg.textContent = "Failed to save client.";
    }
});
