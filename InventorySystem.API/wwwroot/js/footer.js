document.addEventListener("DOMContentLoaded", () => {
    const footer = document.createElement("footer");
    footer.className = "site-footer";
    footer.innerHTML = `
        <p>&copy; ${new Date().getFullYear()} Inventory System. All rights reserved.</p>
    `;
    document.body.appendChild(footer);
});
