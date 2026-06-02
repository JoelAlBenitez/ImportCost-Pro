document.addEventListener("DOMContentLoaded", function () {

    const searchInput = document.getElementById("liveSearch");
    const allRows = Array.from(document.querySelectorAll(".paginationTable"));
    const paginationControls = document.getElementById("paginationControls");
    const paginationInfo = document.getElementById("paginationInfo");

    const itemsPerPage = 8;
    let currentPage = 1;
    let filteredRows = [...allRows];

    function renderTable() {
        allRows.forEach(row => row.style.display = "none");

        const totalItems = filteredRows.length;
        const totalPages = Math.ceil(totalItems / itemsPerPage) || 1;

        if (currentPage > totalPages) currentPage = totalPages;
        if (currentPage < 1) currentPage = 1;

        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;

        for (let i = startIndex; i < endIndex && i < totalItems; i++) {
            filteredRows[i].style.display = "";
        }

        paginationInfo.textContent = totalItems > 0
            ? `Mostrando ${startIndex + 1} de ${Math.min(endIndex, totalItems)} - ${totalItems}`
            : "No hay registros extras";

        renderPaginationButtons(totalPages);
    }

    function renderPaginationButtons(totalPages) {
        paginationControls.innerHTML = "";
        if (totalPages <= 1) return;

        const prevLi = document.createElement("li");
        prevLi.className = `page-item ${currentPage === 1 ? 'disabled' : ''}`;
        prevLi.innerHTML = `<button class="page-link shadow-none text-dark"><i class="bi bi-chevron-left"></i></button>`;
        prevLi.addEventListener("click", () => { if (currentPage > 1) { currentPage--; renderTable(); } });
        paginationControls.appendChild(prevLi);

        for (let i = 1; i <= totalPages; i++) {
            const li = document.createElement("li");
            li.className = `page-item ${currentPage === i ? 'active' : ''}`;
            li.innerHTML = `<button class="page-link shadow-none ${currentPage === i ? 'bg-primary border-primary text-white' : 'text-dark'}">${i}</button>`;
            li.addEventListener("click", () => { currentPage = i; renderTable(); });
            paginationControls.appendChild(li);
        }

        const nextLi = document.createElement("li");
        nextLi.className = `page-item ${currentPage === totalPages ? 'disabled' : ''}`;
        nextLi.innerHTML = `<button class="page-link shadow-none text-dark"><i class="bi bi-chevron-right"></i></button>`;
        nextLi.addEventListener("click", () => { if (currentPage < totalPages) { currentPage++; renderTable(); } });
        paginationControls.appendChild(nextLi);
    }

    if (searchInput) {
        searchInput.addEventListener("input", function (e) {
            const term = e.target.value.toLowerCase().trim();

            filteredRows = allRows.filter(row => {
                return row.innerText.toLowerCase().includes(term);
            });

            currentPage = 1;
            renderTable();
        });
    }

    renderTable();

});

