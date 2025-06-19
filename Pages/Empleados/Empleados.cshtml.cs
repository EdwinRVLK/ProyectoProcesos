using GimManager.Data;
using GimManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace GimManager.Pages.Empleados
{
    public class EmpleadosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SueldoFiltro { get; set; }

        public List<Empleado> Empleados { get; set; }

        public EmpleadosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            CargarEmpleadosFiltrados();
        }

        // Método para eliminar empleado
        public async Task<IActionResult> OnGetEliminarEmpleadoAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
            }

            CargarEmpleadosFiltrados();
            return Page();
        }

        private void CargarEmpleadosFiltrados()
        {
            IQueryable<Empleado> query = _context.Empleados;

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(e => e.Nombre.Contains(SearchTerm) || 
                                        e.ApellidoPaterno.Contains(SearchTerm) || 
                                        e.ApellidoMaterno.Contains(SearchTerm));
            }

            // Aplicar filtro de sueldo
            if (!string.IsNullOrWhiteSpace(SueldoFiltro))
            {
                query = SueldoFiltro switch
                {
                    "Menor a $2,000" => query.Where(e => e.Sueldo < 2000),
                    "Mayor a $2,000" => query.Where(e => e.Sueldo > 2000),
                    _ => query
                };
            }

            Empleados = query.ToList();
        }
    }
}