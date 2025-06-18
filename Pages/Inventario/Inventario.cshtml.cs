using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimManager.Pages.Inventario
{
    public class InventarioModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CategoriaFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string OrderBy { get; set; }

        public List<Producto> Productos { get; set; } = new();

        public InventarioModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            try
            {
                IQueryable<Producto> query = _context.Productos;

                // Filtrar por búsqueda
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    query = query.Where(p =>
                        p.NombreProducto.Contains(SearchTerm) ||
                        p.Categoria.Contains(SearchTerm) ||
                        p.Codigo.Contains(SearchTerm));
                }

                // Filtrar por categoría
                if (!string.IsNullOrWhiteSpace(CategoriaFilter))
                {
                    query = query.Where(p => p.Categoria == CategoriaFilter);
                }

                // Ordenar productos
                query = OrderBy switch
                {
                    "Precio" => query.OrderBy(p => p.Precio),
                    "Fecha" => query.OrderByDescending(p => p.UltimaActualizacion),
                    _ => query.OrderBy(p => p.NombreProducto)
                };

                Productos = query.ToList();
                return Page();
            }
            catch (Exception ex)
            {
                // Log error (consider using ILogger)
                return RedirectToPage("/Error");
            }
        }
    }
}