using GimManager.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace GimManager.Pages.PuntoDeVenta
{
    public class PuntoDeVentaModel : PageModel
    {
        public List<Producto> Productos { get; set; }

        public void OnGet()
        {
            // Aquí puedes cargar los productos desde la base de datos.
            // A continuación se proporciona un ejemplo con datos de prueba.
            
            Productos = new List<Producto>
            {
                new Producto { Codigo = "12345", Descripcion = "Proteína de suero", PrecioVenta = 250.00M, Cantidad = 1, Existencia = 50 },
                new Producto { Codigo = "12346", Descripcion = "Proteína", PrecioVenta = 250.00M, Cantidad = 2, Existencia = 50 },
                new Producto { Codigo = "54321", Descripcion = "Barra de proteína", PrecioVenta = 50.00M, Cantidad = 1, Existencia = 30 },
                new Producto { Codigo = "12345", Descripcion = "Proteína de suero", PrecioVenta = 250.00M, Cantidad = 1, Existencia = 50 },
                new Producto { Codigo = "12345", Descripcion = "Proteína de suero", PrecioVenta = 250.00M, Cantidad = 1, Existencia = 50 },
                new Producto { Codigo = "12345", Descripcion = "Proteína de suero", PrecioVenta = 250.00M, Cantidad = 1, Existencia = 50 },
                new Producto { Codigo = "12345", Descripcion = "Proteína de suero", PrecioVenta = 250.00M, Cantidad = 1, Existencia = 50 },
                new Producto { Codigo = "12346", Descripcion = "Proteína", PrecioVenta = 250.00M, Cantidad = 2, Existencia = 50 }
            };
        }
    }

    public class Producto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public int Existencia { get; set; }
        public decimal Importe => PrecioVenta * Cantidad;
    }
}
