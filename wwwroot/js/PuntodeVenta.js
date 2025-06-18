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
})

// Estructura para guardar los productos en el carrito de venta
let saleCart = JSON.parse(localStorage.getItem("saleCart")) || [];

function updateSaleCart() {
    // Actualiza la tabla de productos en la página
    const saleProductsContainer = document.getElementById("saleProducts");
    saleProductsContainer.innerHTML = "";  // Limpiar tabla antes de actualizar

    let total = 0;

    saleCart.forEach(product => {
        const row = document.createElement("div");
        row.classList.add("product-row");

        row.innerHTML = `
            <div class="product-item">${product.Codigo}</div>
            <div class="product-item">${product.Descripcion}</div>
            <div class="product-item">${product.PrecioVenta.toFixed(2)}</div>
            <div class="product-item">
                <input type="number" value="${product.Cantidad}" min="1" max="${product.Existencia}" 
                    class="product-quantity" 
                    onchange="updateProductQuantity('${product.Codigo}', this.value)" />
            </div>
            <div class="product-item">${(product.PrecioVenta * product.Cantidad).toFixed(2)}</div>
            <div class="product-item">${product.Existencia}</div>
        `;

        saleProductsContainer.appendChild(row);
        total += product.PrecioVenta * product.Cantidad;
    });

    // Mostrar el total de la venta
    document.querySelector(".total-price").textContent = `$${total.toFixed(2)}`;
}

function addProductToSale() {
    const searchProduct = document.getElementById("searchProduct").value;
    if (!searchProduct) return;

    // Buscar el producto en la base de datos o inventario
    fetch(`/api/productos?codigo=${searchProduct}`)
        .then(response => response.json())
        .then(product => {
            if (product) {
                // Verificar si el producto ya está en el carrito
                const existingProduct = saleCart.find(item => item.Codigo === product.Codigo);
                if (existingProduct) {
                    // Si el producto ya existe, solo aumentar la cantidad
                    existingProduct.Cantidad += 1;
                } else {
                    // Si no, agregar el producto al carrito
                    saleCart.push({
                        Codigo: product.Codigo,
                        Descripcion: product.Descripcion,
                        PrecioVenta: product.PrecioVenta,
                        Cantidad: 1,
                        Existencia: product.Existencia
                    });
                }

                // Actualizar el carrito y la vista
                localStorage.setItem("saleCart", JSON.stringify(saleCart));
                updateSaleCart();
            } else {
                alert("Producto no encontrado.");
            }
        })
        .catch(error => {
            console.error("Error al buscar el producto:", error);
        });
}

function updateProductQuantity(codigo, cantidad) {
    const product = saleCart.find(item => item.Codigo === codigo);
    if (product) {
        product.Cantidad = parseInt(cantidad, 10);
        if (product.Cantidad < 1) product.Cantidad = 1; // Prevent less than 1
        localStorage.setItem("saleCart", JSON.stringify(saleCart));
        updateSaleCart();
    }
}

// Llamar a la función de inicialización para mostrar productos al cargar la página
updateSaleCart();
;