using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using System.Linq;
using System.Collections.Generic;

namespace GimManager.Pages.Inventario
{
    public class InventarioModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public InventarioModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CategoriaFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string OrderBy { get; set; }

        public List<Producto> Productos { get; set; } = new List<Producto>();

        public void OnGet()
        {
            IQueryable<Producto> query = _context.Productos.AsQueryable();

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(p => 
                    p.NombreProducto.Contains(SearchTerm) ||
                    p.Categoria.Contains(SearchTerm) ||
                    p.Codigo.Contains(SearchTerm));
            }

            // Aplicar filtro por categoría
            if (!string.IsNullOrEmpty(CategoriaFilter))
            {
                query = query.Where(p => p.Categoria == CategoriaFilter);
            }

            // Aplicar ordenamiento
            query = OrderBy switch
            {
                "Precio" => query.OrderBy(p => p.Precio),
                "Fecha" => query.OrderByDescending(p => p.UltimaActualizacion),
                _ => query.OrderBy(p => p.NombreProducto) // Orden por defecto
            };

            Productos = query.ToList();
        }

        public IActionResult OnGetEliminar(int id)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.Id == id);
            
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}