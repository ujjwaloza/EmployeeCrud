const checkboxes = document.querySelectorAll(".employee-checkbox");
const editBtn = document.getElementById("editBtn");
const deleteBtn = document.getElementById("deleteBtn");

function getSelectedIds() {
    return Array.from(checkboxes)
        .filter(cb => cb.checked)
        .map(cb => cb.value);
}

checkboxes.forEach(cb => {
    cb.addEventListener("change", function () {
        const selectedIds = getSelectedIds();


        deleteBtn.disabled = selectedIds.length === 0;


        editBtn.disabled = selectedIds.length !== 1;
    });
});

editBtn.addEventListener("click", function () {
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 1) {
        window.location.href = `/Employee/Edit/${selectedIds[0]}`;
    }

});

deleteBtn.addEventListener("click", function () {
    const selectedIds = getSelectedIds();
    if (selectedIds.length > 0) {
        const ids = selectedIds.join(",");
        window.location.href = `/Employee/BulkDelete?ids=${ids}`;
    }
});