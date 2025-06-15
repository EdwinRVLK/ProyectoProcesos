using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace GimManager.Pages.Clientes
{
    public class DeleteClienteModel : PageModel
    {
        // Simulación de datos (sustituye con tu base de datos real más adelante)
        public class Cliente
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public string Telefono { get; set; }
            public string Membresia { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public List<Cliente> Clientes { get; set; } = new();

        public void OnGet()
        {
            // Datos de ejemplo
            var todos = new List<Cliente>
            {
                new Cliente { Id = 1, Nombre = "Juan", ApellidoPaterno = "López", ApellidoMaterno = "25", Telefono = "9275267892", Membresia = "VIP" },
                new Cliente { Id = 2, Nombre = "María", ApellidoPaterno = "Hernández", ApellidoMaterno = "46", Telefono = "9235227892", Membresia = "Básica" },
                new Cliente { Id = 3, Nombre = "Carlos", ApellidoPaterno = "Díaz", ApellidoMaterno = "78", Telefono = "9123450892", Membresia = "Estandar" },
                new Cliente { Id = 4, Nombre = "Ana", ApellidoPaterno = "Ruiz", ApellidoMaterno = "90", Telefono = "9275265672", Membresia = "VIP" },
                new Cliente { Id = 5, Nombre = "Sofía", ApellidoPaterno = "Vázquez", ApellidoMaterno = "2", Telefono = "9275209636", Membresia = "VIP" }
            };

            // Aplicar búsqueda si existe
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Clientes = todos.Where(c =>
                    c.Nombre.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                    c.ApellidoPaterno.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                    c.Telefono.Contains(SearchTerm)
                ).ToList();
            }
            else
            {
                Clientes = todos;
            }
        }

        // Aquí podrías agregar métodos OnPostEliminar(int id) o OnPostEditar(int id) más adelante
    }
}
