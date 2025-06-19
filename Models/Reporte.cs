using System;
using System.ComponentModel.DataAnnotations;

namespace GimManager.Models
{
    public class Reporte
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Fecha de Generación")]
        public DateTime FechaGenerado { get; set; } = DateTime.Now;

        [Display(Name = "Desde")]
        public DateTime FechaDesde { get; set; }

        [Display(Name = "Hasta")]
        public DateTime FechaHasta { get; set; }

        [Display(Name = "Total Productos Vendidos")]
        public int TotalProductos { get; set; }

        [Display(Name = "Total Membresías Vendidas")]
        public int TotalMembresias { get; set; }

        [Display(Name = "Ingreso por Productos")]
        [DataType(DataType.Currency)]
        public decimal IngresoProductos { get; set; }

        [Display(Name = "Ingreso por Membresías")]
        [DataType(DataType.Currency)]
        public decimal IngresoMembresias { get; set; }

        [Display(Name = "Ingreso Total")]
        [DataType(DataType.Currency)]
        public decimal IngresoTotal => IngresoProductos + IngresoMembresias;
    }
}
