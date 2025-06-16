using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimManager.Pages.Empleados
{
    public class EmpleadosModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SueldoFiltro { get; set; }

        public List<Empleado> Empleados { get; set; } = new();

        public class Empleado
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public decimal Sueldo { get; set; }
            public string Rol { get; set; }
        }

        public void OnGet()
        {
            var todos = new List<Empleado>
            {
                new Empleado { Id = 1, Nombre = "Juan", ApellidoPaterno = "López", ApellidoMaterno = "Ruiz", Sueldo = 1200m, Rol = "Entrenador" },
                new Empleado { Id = 2, Nombre = "Maria", ApellidoPaterno = "Hernández", ApellidoMaterno = "López", Sueldo = 1500m, Rol = "Limpieza" },
                new Empleado { Id = 3, Nombre = "Carlos", ApellidoPaterno = "Díaz", ApellidoMaterno = "Casanova", Sueldo = 1200m, Rol = "Entrenador" },
                new Empleado { Id = 4, Nombre = "Jose", ApellidoPaterno = "Díaz", ApellidoMaterno = "Estrada", Sueldo = 2000m, Rol = "Recepcionista" },
                new Empleado { Id = 5, Nombre = "Fernando", ApellidoPaterno = "Sulvaran", ApellidoMaterno = "Maldonado", Sueldo = 5000m, Rol = "Gerente" }
            };

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                todos = todos.Where(e =>
                    e.Nombre.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    e.ApellidoPaterno.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    e.ApellidoMaterno.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    e.Rol.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            if (SueldoFiltro == "Menor a $2,000")
            {
                todos = todos.Where(e => e.Sueldo < 2000m).ToList();
            }
            else if (SueldoFiltro == "Mayor a $2,000")
            {
                todos = todos.Where(e => e.Sueldo >= 2000m).ToList();
            }

            Empleados = todos;
        }
    }
}
