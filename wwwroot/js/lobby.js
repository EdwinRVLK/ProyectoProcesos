function toggleDropdown(button) {
    const allDropdowns = document.querySelectorAll('.dropdown-menu');
    allDropdowns.forEach(menu => {
        if (menu !== button.nextElementSibling) {
            menu.style.display = 'none';
        }
    });

    const menu = button.nextElementSibling;
    menu.style.display = menu.style.display === 'block' ? 'none' : 'block';
}

// Opcional: cerrar menú si se da clic fuera
document.addEventListener('click', function (e) {
    if (!e.target.closest('.menu-item')) {
        document.querySelectorAll('.dropdown-menu').forEach(menu => menu.style.display = 'none');
    }
});
