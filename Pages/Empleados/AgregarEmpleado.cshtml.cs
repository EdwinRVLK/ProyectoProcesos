using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimManager.Pages.Empleados
{
    public class AgregarEmpleadoModel : PageModel
    {
        [BindProperty]
        public EmpleadoInput Empleado { get; set; }

        public class EmpleadoInput
        {
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public decimal Sueldo { get; set; }
            public string Rol { get; set; }
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // Aquí puedes guardar en DB o simular almacenamiento
            TempData["Mensaje"] = "Empleado agregado correctamente.";

            return RedirectToPage("/Empleados/Empleados");
        }
    }
}
