const form = document.getElementById('registerForm');
const msg = document.getElementById('message');

form.addEventListener('submit', async (e) => {
    e.preventDefault();

    const firstName = document.getElementById('firstName').value.trim();
    const lastName = document.getElementById('lastName').value.trim();
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value.trim();

    try {
        const res = await fetch("https://localhost:7127/api/auth/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                firstName,
                lastName,
                email,
                password,
                role: "Operator" // o "Admin", si se permite desde el frontend
            })
        });

        if (!res.ok) throw new Error();

        const data = await res.json();

        const payload = parseJwt(data.token);
        localStorage.setItem("token", data.token);
        localStorage.setItem("role", payload.role);
        localStorage.setItem("employeeId", data.employeeId);

        msg.style.color = "green";
        msg.textContent = "Registration successful!";
        setTimeout(() => window.location.href = "/pages/dashboard.html", 1000);
    } catch {
        msg.style.color = "red";
        msg.textContent = "Registration failed.";
    }
});

function parseJwt(token) {
    const base64Payload = token.split('.')[1];
    return JSON.parse(atob(base64Payload));
}

