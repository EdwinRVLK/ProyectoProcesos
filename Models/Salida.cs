using System;
using System.ComponentModel.DataAnnotations;

namespace GimManager.Models
{
    public class Salida
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Fecha de registro")]
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
