using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimManager.Data;
using GimManager.Models;
using System.Linq;

namespace GimManager.Pages.Clientes
{
    public class MostrarClientesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MostrarClientesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string SearchTerm { get; set; }
        public string TipoMembresiaFilter { get; set; }
        public string FechaVencimientoFilter { get; set; }
        public IQueryable<Cliente> Clientes { get; set; }

        public void OnGet(string searchTerm, string tipoMembresiaFilter, string fechaVencimientoFilter)
        {
            SearchTerm = searchTerm;
            TipoMembresiaFilter = tipoMembresiaFilter;
            FechaVencimientoFilter = fechaVencimientoFilter;

            Clientes = _context.Clientes.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Clientes = Clientes.Where(c => c.Nombre.Contains(SearchTerm) || c.Apellido.Contains(SearchTerm) || c.Telefono.Contains(SearchTerm));
            }

            if (!string.IsNullOrEmpty(TipoMembresiaFilter))
            {
                Clientes = Clientes.Where(c => c.TipoMembresia == TipoMembresiaFilter);
            }

            if (FechaVencimientoFilter == "today")
            {
                Clientes = Clientes.Where(c => c.FechaVencimiento.Date == DateTime.Today);
            }
            else if (FechaVencimientoFilter == "thisWeek")
            {
                var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                Clientes = Clientes.Where(c => c.FechaVencimiento >= startOfWeek && c.FechaVencimiento < startOfWeek.AddDays(7));
            }
            else if (FechaVencimientoFilter == "thisMonth")
            {
                var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                Clientes = Clientes.Where(c => c.FechaVencimiento >= startOfMonth && c.FechaVencimiento < startOfMonth.AddMonths(1));
            }

            Clientes = Clientes.OrderBy(c => c.FechaVencimiento);
        }
    }
}
