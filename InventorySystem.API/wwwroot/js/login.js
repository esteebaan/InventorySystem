/*
form.addEventListener('submit', async (e) => {
    e.preventDefault();

    const credentials = {
        email: document.getElementById('email').value.trim(),
        password: document.getElementById('password').value.trim()
    };

    try {
        const res = await fetch("https://localhost:7127/api/auth/login", {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(credentials)
        });

        if (!res.ok) throw new Error("Login failed");

        const data = await res.json();

        // Guardamos token, rol y (opcional) id de empleado
        localStorage.setItem("token", data.token);
        localStorage.setItem("role", data.role);
        if (data.employeeId) localStorage.setItem("employeeId", data.employeeId);

        msg.style.color = "green";
        msg.textContent = "Login successful!";
        setTimeout(() => window.location.href = "/pages/dashboard.html", 1000);

    } catch (err) {
        msg.style.color = "red";
        msg.textContent = "Invalid email or password.";
    }
});*/

const form = document.getElementById("loginForm");
const msg = document.getElementById("message");

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value.trim();

    try {
        const res = await fetch("https://localhost:7127/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        if (!res.ok) throw new Error("Invalid credentials");

        const data = await res.json();

        if (!data.token || !data.employeeId || !data.role) {
            throw new Error("Incomplete response from server.");
        }

        localStorage.setItem("token", data.token);
        localStorage.setItem("employeeId", data.employeeId);
        localStorage.setItem("role", data.role);

        msg.style.color = "green";
        msg.textContent = "Login successful!";
        setTimeout(() => window.location.href = "/pages/dashboard.html", 1000);
    } catch (err) {
        msg.style.color = "red";
        msg.textContent = err.message || "Login failed.";
    }
});


function parseJwt(token) {
    const base64Payload = token.split('.')[1];
    return JSON.parse(atob(base64Payload));
}


