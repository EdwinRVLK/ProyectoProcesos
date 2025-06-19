// Models/VentaMembresia.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace GimManager.Models
{
    public class VentaMembresia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Required]
        public string TipoMembresia { get; set; } = string.Empty;

        [Required]
        public int Meses { get; set; }

        // Relación con Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // Detalles adicionales
        public string Concepto { get; set; } = string.Empty;
    }
}