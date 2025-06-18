using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace GimManager.Pages.Clientes
{
    public class GestionClienteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MembresiaFilter { get; set; }

        public List<Cliente> Clientes { get; set; } = new();

        public GestionClienteModel(ApplicationDbContext context)
        {
            _context = context;
        }

                public IActionResult OnGet()
        {
            try
            {
                IQueryable<Cliente> query = _context.Clientes;

                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    query = query.Where(c =>
                        c.Nombre.Contains(SearchTerm) ||
                        c.Apellido.Contains(SearchTerm) ||  // Buscar por apellido
                        c.Telefono.Contains(SearchTerm));
                }

                if (!string.IsNullOrWhiteSpace(MembresiaFilter))
                {
                    query = query.Where(c => c.TipoMembresia == MembresiaFilter);  // Filtrar por TipoMembresia
                }

                Clientes = query.OrderBy(c => c.Apellido).ThenBy(c => c.Nombre).ToList();
                return Page();
            }
            catch (System.Exception ex)
            {
                // Log the error
                return RedirectToPage("/Error");
            }
        }
    }
}