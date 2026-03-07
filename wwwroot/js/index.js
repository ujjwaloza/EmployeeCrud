const editBtn = document.getElementById("editBtn");
const deleteBtn = document.getElementById("deleteBtn");
const selectAll = document.getElementById("selectAll");


function getSelectedIds() {
    const checkboxes = document.querySelectorAll(".employee-checkbox");
    return Array.from(checkboxes)
        .filter(cb => cb.checked)
        .map(cb => cb.value);
}

function updateButtons() {

    const selectedIds = getSelectedIds();

    if (deleteBtn) {
        deleteBtn.disabled = selectedIds.length === 0;
    }

    if (editBtn) {
        editBtn.disabled = selectedIds.length !== 1;
    }
}

document.addEventListener("change", function (e) {

    if (e.target.classList.contains("employee-checkbox")) {
        updateButtons();
    }

});

document.addEventListener("change", function (e) {

    if (e.target.classList.contains("employee-checkbox")) {
        updateButtons();
    }

    if (e.target.id === "selectAll") {

        const checked = e.target.checked;

        document.querySelectorAll(".employee-checkbox").forEach(cb => {
            cb.checked = checked;
        });

        updateButtons();
    }
});

if (editBtn) {
    editBtn.addEventListener("click", function () {

        const selectedIds = getSelectedIds();

        if (selectedIds.length === 1) {
            window.location.href = `/Employee/Edit/${selectedIds[0]}`;
        }

    });
}

if (deleteBtn) {
    deleteBtn.addEventListener("click", function () {

        const selectedIds = getSelectedIds();

        if (selectedIds.length === 0) return;

        if (!confirm("Are you sure you want to delete selected employees?")) {
            return;
        }

        const ids = selectedIds.join(",");
        window.location.href = `/Employee/BulkDelete?ids=${ids}`;

    });
}


function openSearchModal() {
    document.getElementById("searchModal").style.display = "block";
}

function closeSearchModal() {

    document.getElementById("searchModal").style.display = "none";

    document.getElementById("nameSearchForm").style.display = "none";
    document.getElementById("yearSearchForm").style.display = "none";
}

function showNameSearch() {

    document.getElementById("nameSearchForm").style.display = "block";
    document.getElementById("yearSearchForm").style.display = "none";

}

function showYearSearch() {

    document.getElementById("yearSearchForm").style.display = "block";
    document.getElementById("nameSearchForm").style.display = "none";

}


function openModal() {
    document.getElementById("searchModal").style.display = "block";
}

function closeModal() {
    document.getElementById("searchModal").style.display = "none";
}

function closeNoDataModal() {
    document.getElementById("noDataModal").style.display = "none";
}

function confirmDelete() {
    return confirm("Are you sure you want to delete this employee?");
}


function openEditModal(id, firstName, lastName, gender, dob) {

    document.getElementById("editId").value = id;
    document.getElementById("editFirstName").value = firstName;
    document.getElementById("editLastName").value = lastName;
    document.getElementById("editGender").value = gender;
    document.getElementById("editDOB").value = dob;

    document.getElementById("editModal").style.display = "block";
}

function closeEditModal() {
    document.getElementById("editModal").style.display = "none";
}