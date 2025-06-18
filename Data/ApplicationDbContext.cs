using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GimManager.Models;

namespace GimManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }  // Añadido DbSet para Producto
        public DbSet<Empleado> Empleados { get; set; } // Añadido DbSet para Empleado
        public DbSet<Venta> Ventas { get; set; } // Añadido DbSet para Venta
        public DbSet<DetalleVenta> DetallesVentas { get; set; } // Añadido DbSet para DetalleVenta
        public DbSet<Salida> Salidas { get; set; } // Añadido DbSet para Salida
        public DbSet<Entrada> Entradas { get; set; } // Añadido DbSet para Entrada

    }
}
