using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GimManager.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public decimal Total { get; set; }

        public List<DetalleVenta> Detalles { get; set; } = new();
    }
}
