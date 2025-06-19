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
    });
})

function confirmDelete(id, nombre) {
            if (confirm(`¿Seguro que deseas eliminar a ${nombre}?`)) {
                document.getElementById(`deleteForm-${id}`).submit();
            }
        }

// wwwroot/js/AddCliente.js
document.addEventListener('DOMContentLoaded', function() {
    // Selectores CORREGIDOS para los campos Razor
    const tipoMembresia = document.querySelector('select[name="Cliente.TipoMembresia"]');
    const meses = document.querySelector('select[name="Cliente.Meses"]');
    const totalPago = document.getElementById('totalPago');
    const formulario = document.querySelector('form');
    const btnGuardar = document.querySelector('.btn-guardar');

    // Precios base (deben coincidir con Cliente.CalcularPrecioYFechaVencimiento)
    const precios = {
        'Normal': 500,
        'Estudiante': 300,
        'VIP': 1000
    };

    function calcularTotal() {
        const tipoSeleccionado = tipoMembresia.value;
        const mesesSeleccionados = parseInt(meses.value) || 0;
        
        if (tipoSeleccionado && mesesSeleccionados > 0) {
            const total = precios[tipoSeleccionado] * mesesSeleccionados;
            totalPago.innerHTML = `
                <small>${tipoSeleccionado} ($${precios[tipoSeleccionado]}/mes) × ${mesesSeleccionados} meses</small><br>
                <strong>$${total.toFixed(2)}</strong>
            `;
            totalPago.style.color = '#2e7d32';
            btnGuardar.disabled = false;
        } else {
            totalPago.innerHTML = '<span style="color:#f44336">Seleccione tipo y meses</span>';
            totalPago.style.color = '#f44336';
            btnGuardar.disabled = meses.value === "0";
        }
    }

    formulario.addEventListener('submit', function(e) {
        if (!tipoMembresia.value || !meses.value || meses.value === '0') {
            e.preventDefault();
            totalPago.innerHTML = '<span style="color:#f44336">Complete todos los campos</span>';
            totalPago.classList.add('error-pulse');
            setTimeout(() => totalPago.classList.remove('error-pulse'), 1000);
        }
    });

    // Event listeners mejorados
    tipoMembresia?.addEventListener('change', calcularTotal);
    meses?.addEventListener('change', calcularTotal);

    // Inicialización
    calcularTotal();
});