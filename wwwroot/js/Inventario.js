function toggleDropdown(button) {
    const dropdown = button.nextElementSibling;
    const allDropdowns = document.querySelectorAll('.dropdown-menu');
    allDropdowns.forEach(menu => {
        if (menu !== dropdown) menu.style.display = 'none';
    });
    dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
}

window.addEventListener('click', function (e) {
    document.querySelectorAll('.dropdown-menu').forEach(menu => {
        if (!menu.previousElementSibling.contains(e.target)) {
            menu.style.display = 'none';
        }
    })
})

