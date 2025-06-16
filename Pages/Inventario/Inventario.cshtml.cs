using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimManager.Pages.Inventario
{
    public class InventarioModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public List<Producto> Productos { get; set; } = new();

        public class Producto
        {
            public int Id { get; set; }
            public string NombreProducto { get; set; }
            public string Categoria { get; set; }
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
            public DateTime UltimaActualizacion { get; set; }
        }

        public void OnGet()
        {
            var todos = new List<Producto>
            {
                new Producto { Id = 1, NombreProducto = "Proteína de suero", Categoria = "Accesorios", Cantidad = 20, Precio = 250.00m, UltimaActualizacion = new DateTime(2025, 4, 25) },
                new Producto { Id = 2, NombreProducto = "Botella de agua", Categoria = "Alimentos", Cantidad = 50, Precio = 30.00m, UltimaActualizacion = new DateTime(2025, 4, 26) },
                new Producto { Id = 3, NombreProducto = "Esterilla de yoga", Categoria = "Suplementos", Cantidad = 30, Precio = 400.00m, UltimaActualizacion = new DateTime(2025, 4, 27) },
                new Producto { Id = 4, NombreProducto = "Barra de proteína", Categoria = "Bebidas", Cantidad = 12, Precio = 50.00m, UltimaActualizacion = new DateTime(2025, 4, 28) }
            };

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Productos = todos.Where(p =>
                    p.NombreProducto.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Categoria.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            else
            {
                Productos = todos;
            }
        }
    }
}
