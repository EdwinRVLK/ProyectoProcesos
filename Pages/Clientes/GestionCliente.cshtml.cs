using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace GimManager.Pages.Clientes
{
    public class GestionClienteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MembresiaFilter { get; set; }

        public List<Cliente> Clientes { get; set; } = new List<Cliente>();

        public GestionClienteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                IQueryable<Cliente> query = _context.Clientes.AsQueryable();

                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    query = query.Where(c =>
                        c.Nombre.Contains(SearchTerm) ||
                        c.Apellido.Contains(SearchTerm) ||
                        c.Telefono.Contains(SearchTerm));
                }

                if (!string.IsNullOrWhiteSpace(MembresiaFilter))
                {
                    query = query.Where(c => c.TipoMembresia == MembresiaFilter);
                }

                Clientes = await query
                    .OrderBy(c => c.Apellido)
                    .ThenBy(c => c.Nombre)
                    .ToListAsync();

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar clientes: {ex.Message}";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
{
    try
    {
        var cliente = await _context.Clientes.FindAsync(id);
        
        if (cliente == null)
        {
            TempData["ErrorMessage"] = "Cliente no encontrado";
            return RedirectToPage();
        }

        // Eliminar el cliente
        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Cliente {cliente.Nombre} eliminado correctamente";
    }
    catch (Exception ex)
    {
        TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
    }

    return RedirectToPage();
}

    }
}