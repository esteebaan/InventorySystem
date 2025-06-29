const token = localStorage.getItem("token");
if (!token) window.location.href = "/pages/login.html";

const urlParams = new URLSearchParams(window.location.search);
const loanId = urlParams.get("loanId");
const obsList = document.getElementById("observationList");
const obsForm = document.getElementById("obsForm");
const obsText = document.getElementById("obsText");
const msg = document.getElementById("msg");

if (!loanId) {
    msg.textContent = "Loan ID not provided.";
    obsForm.style.display = "none";
}

function loadObservations() {
    fetch(`https://localhost:7127/api/observations/loan/${loanId}`, {
        headers: { Authorization: `Bearer ${token}` }
    })
        .then(res => res.json())
        .then(data => {
            obsList.innerHTML = "";
            if (!data.length) {
                obsList.innerHTML = "<li>No observations yet.</li>";
                return;
            }
            data.forEach(obs => {
                const item = document.createElement("li");
                const date = new Date(obs.createdAt).toLocaleString();
                item.textContent = `📌 ${date}: ${obs.text}`;
                obsList.appendChild(item);
            });
        })
        .catch(() => {
            msg.textContent = "Error loading observations.";
        });
}

obsForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const observation = {
        loanId: loanId,
        text: obsText.value.trim()
    };

    try {
        const res = await fetch("https://localhost:7127/api/observations", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(observation)
        });

        if (!res.ok) throw new Error();
        obsText.value = "";
        loadObservations();
    } catch {
        msg.textContent = "Error saving observation.";
    }
});

loadObservations();
