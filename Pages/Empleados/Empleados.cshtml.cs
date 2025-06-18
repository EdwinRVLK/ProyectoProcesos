using GimManager.Data;
using GimManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace GimManager.Pages.Empleados
{
    public class EmpleadosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } // Para búsqueda por nombre, apellido paterno o materno

        [BindProperty(SupportsGet = true)]
        public string SueldoFiltro { get; set; } // Para filtro de sueldo

        public List<Empleado> Empleados { get; set; }

        // Constructor para inyectar ApplicationDbContext
        public EmpleadosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método OnGet para recuperar los empleados desde la base de datos
        public void OnGet()
        {
            IQueryable<Empleado> query = _context.Empleados;

            // Filtro de búsqueda por nombre, apellido paterno o apellido materno
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(e => e.Nombre.Contains(SearchTerm) ||
                                         e.ApellidoPaterno.Contains(SearchTerm) ||
                                         e.ApellidoMaterno.Contains(SearchTerm));
            }

            // Filtro por sueldo
            if (!string.IsNullOrWhiteSpace(SueldoFiltro))
            {
                if (SueldoFiltro == "Menor a $2,000")
                {
                    query = query.Where(e => e.Sueldo < 2000);
                }
                else if (SueldoFiltro == "Mayor a $2,000")
                {
                    query = query.Where(e => e.Sueldo > 2000);
                }
            }

            // Cargar los empleados filtrados
            Empleados = query.ToList();
        }
    }
}
