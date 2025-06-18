console.log("JS cargado");

let carrito = [];

function mostrarMensaje(msg, color = "crimson") {
    const div = document.getElementById("pv-message");
    div.textContent = msg;
    div.style.color = color;
    setTimeout(() => div.textContent = "", 4000);
}

function addProductToSale() {
    const codigo = document.getElementById("searchProduct").value.trim();
    if (!codigo) return;

    fetch(`/VentasYpagos/PuntoDeVenta?handler=AgregarProducto&codigo=${codigo}`)
        .then(res => res.json())
        .then(data => {
            if (!data.success) {
                mostrarMensaje(data.message);
                return;
            }

            const existente = carrito.find(p => p.codigo === data.data.codigo);
            if (existente) {
                existente.cantidad += 1;
                existente.importe += data.data.precio;
            } else {
                carrito.push({
                    codigo: data.data.codigo,
                    descripcion: data.data.nombre,
                    precio: data.data.precio,
                    cantidad: 1,
                    importe: data.data.precio
                });
            }

            updateCartUI();
            document.getElementById("searchProduct").value = "";
        })
        .catch(err => {
            console.error("Error al agregar producto:", err);
            mostrarMensaje("Error al agregar producto");
        });
}

function updateCartUI() {
    const container = document.getElementById("saleProducts");
    container.innerHTML = '';

    let total = 0;

    carrito.forEach(item => {
        total += item.importe;

        const row = document.createElement("div");
        row.className = "product-row";
        row.innerHTML = `
            <div class="product-item">${item.codigo}</div>
            <div class="product-item">${item.descripcion}</div>
            <div class="product-item">$${item.precio.toFixed(2)}</div>
            <div class="product-item">${item.cantidad}</div>
            <div class="product-item">$${item.importe.toFixed(2)}</div>
        `;
        container.appendChild(row);
    });

    document.getElementById("totalPrice").textContent = total.toFixed(2);
}

function vaciarCarrito() {
    carrito = [];
    updateCartUI();
    mostrarMensaje("Carrito vacío", "darkgreen");
}

async function finalizarVenta() {
    if (carrito.length === 0) {
        mostrarMensaje("El carrito está vacío.");
        return;
    }

    const payload = carrito.map(p => ({
        codigoProducto: p.codigo,
        nombreProducto: p.descripcion,
        cantidad: p.cantidad,
        precioUnitario: p.precio
    }));

    console.log("🛒 Enviando carrito:", payload);

    try {
        const response = await fetch("/VentasYpagos/PuntoDeVenta?handler=FinalizarVenta", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        console.log("📡 Estado de la respuesta:", response.status);

        const text = await response.text();
        console.log("📨 Texto bruto de respuesta:", text);

        let result;
        try {
            result = JSON.parse(text);
        } catch (parseError) {
            console.error("❌ Error al parsear JSON:", parseError);
            mostrarMensaje("Error inesperado en la respuesta del servidor.");
            return;
        }

        if (result.success) {
            mostrarMensaje("✅ Venta registrada correctamente", "green");
            vaciarCarrito();
        } else {
            mostrarMensaje("❌ Error al registrar venta: " + result.message);
        }
    } catch (err) {
        console.error("❌ Error en fetch:", err);
        mostrarMensaje("Error al comunicar con el servidor.");
    }
}

// 🔽 Función para manejar el menú desplegable
function toggleDropdown(button) {
    const dropdown = button.nextElementSibling;
    const allDropdowns = document.querySelectorAll('.dropdown-menu');
    allDropdowns.forEach(menu => {
        if (menu !== dropdown) menu.style.display = 'none';
    });
    dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
}

// 🔐 Cierra menús desplegables al hacer clic fuera
window.addEventListener('click', function (e) {
    document.querySelectorAll('.dropdown-menu').forEach(menu => {
        if (!menu.previousElementSibling.contains(e.target)) {
            menu.style.display = 'none';
        }
    });
});

// 🔍 Modal de búsqueda de productos
let inventarioCache = [];

function abrirModalBuscar() {
    document.getElementById("modalBuscar").style.display = "flex";

    // Si ya lo cargamos antes, no volvemos a pedirlo
    if (inventarioCache.length > 0) {
        renderizarInventario(inventarioCache);
        return;
    }

    fetch("/VentasYpagos/InventarioAPI") // Asegúrate de tener esta ruta o ajusta el handler
        .then(res => res.json())
        .then(data => {
            inventarioCache = data;
            renderizarInventario(data);
        })
        .catch(err => {
            console.error("Error al cargar inventario:", err);
            document.getElementById("resultadoBusqueda").innerHTML = "<em>Error al cargar inventario</em>";
        });
}

function cerrarModalBuscar() {
    document.getElementById("modalBuscar").style.display = "none";
}

function renderizarInventario(productos) {
    const container = document.getElementById("resultadoBusqueda");
    if (!productos.length) {
        container.innerHTML = "<em>No hay productos en inventario</em>";
        return;
    }

    container.innerHTML = productos.map(p => `
        <div class="producto-item">
            <strong>${p.codigo}</strong> – ${p.nombreProducto}<br/>
            Existencias: ${p.cantidad}
        </div>
    `).join('');
}

function filtrarProductos() {
    const texto = document.getElementById("buscarInput").value.toLowerCase();
    const filtrados = inventarioCache.filter(p =>
        p.nombreProducto.toLowerCase().includes(texto)
    );
    renderizarInventario(filtrados);
}

window.addEventListener("DOMContentLoaded", () => {
    const modal = document.getElementById("modalBuscar");
    if (!modal) {
        console.error("❌ No se encontró el elemento #modalBuscar en el DOM al cargar la página.");
    } else {
        console.log("✅ El modalBuscar está presente en el DOM.");
    }
})

// 🔴 Modal para Registrar Entrada
function abrirModalEntrada() {
    document.getElementById("modalEntrada").style.display = "flex";
}

function cerrarModalEntrada() {
    document.getElementById("modalEntrada").style.display = "none";
}

function registrarEntrada() {
    const monto = parseFloat(document.getElementById("entradaMonto").value);
    const descripcion = document.getElementById("entradaDescripcion").value.trim();

    if (!monto || monto <= 0 || !descripcion) {
        mostrarMensaje("Ingresa un monto y una descripción válidos.");
        return;
    }

    fetch("/VentasYpagos/PuntoDeVenta?handler=RegistrarEntrada", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ monto, descripcion })
    })
    .then(async res => {
        const text = await res.text();

        // Log completo de la respuesta cruda
        console.log("📨 Respuesta cruda del servidor:", text);

        try {
            const data = JSON.parse(text);

            if (data.success) {
                mostrarMensaje("✅ Entrada registrada correctamente", "green");
                cerrarModalEntrada();
                document.getElementById("entradaMonto").value = "";
                document.getElementById("entradaDescripcion").value = "";
            } else {
                mostrarMensaje("❌ " + data.message);
            }
        } catch (e) {
            console.error("❌ Error al interpretar JSON:", e);
            mostrarMensaje("Respuesta inválida del servidor.");
        }
    })
    .catch(err => {
        console.error("❌ Error en fetch:", err);
        mostrarMensaje("Error al registrar entrada.");
    });
}

// 🔴 Modal para Registrar Salida
function abrirModalSalida() {
    document.getElementById("modalSalida").style.display = "flex";
}

function cerrarModalSalida() {
    document.getElementById("modalSalida").style.display = "none";
}

function registrarSalida() {
    const monto = parseFloat(document.getElementById("salidaMonto").value);
    const descripcion = document.getElementById("salidaDescripcion").value.trim();

    if (!monto || monto <= 0 || !descripcion) {
        mostrarMensaje("Ingresa un monto y una descripción válidos.");
        return;
    }

    fetch("/VentasYpagos/PuntodeVenta?handler=RegistrarSalida", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ monto, descripcion })
    })
    .then(async res => {
        const text = await res.text();

        // Log completo de la respuesta cruda
        console.log("📨 Respuesta cruda del servidor:", text);

        try {
            const data = JSON.parse(text);

            if (data.success) {
                mostrarMensaje("✅ Salida registrada correctamente", "green");
                cerrarModalSalida();
                document.getElementById("salidaMonto").value = "";
                document.getElementById("salidaDescripcion").value = "";
            } else {
                mostrarMensaje("❌ " + data.message);
            }
        } catch (e) {
            console.error("❌ Error al interpretar JSON:", e);
            mostrarMensaje("Respuesta inválida del servidor.");
        }
    })
    .catch(err => {
        console.error("❌ Error en fetch:", err);
        mostrarMensaje("Error al registrar salida.");
    });
}
