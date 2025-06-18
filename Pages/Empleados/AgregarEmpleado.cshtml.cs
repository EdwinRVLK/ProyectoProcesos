// Pages/Empleados/AgregarEmpleado.cshtml.cs
using GimManager.Data;
using GimManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimManager.Pages.Empleados
{
    public class AgregarEmpleadoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AgregarEmpleadoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Empleado Empleado { get; set; } = new Empleado();

        public void OnGet()
        {
            // Método vacío para cargar la página inicialmente
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Asignar fecha de ingreso automáticamente
            Empleado.FechaIngreso = DateTime.Now;

            _context.Empleados.Add(Empleado);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Empleados");
        }
    }
}