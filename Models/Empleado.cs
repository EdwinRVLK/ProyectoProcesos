// Models/Empleado.cs
using System.ComponentModel.DataAnnotations;

namespace GimManager.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
        [Display(Name = "Apellido Materno")]
        public string? ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El sueldo es obligatorio")]
        [Range(1000, 50000, ErrorMessage = "El sueldo debe estar entre $1,000 y $50,000")]
        [DataType(DataType.Currency)]
        public decimal Sueldo { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [StringLength(30, ErrorMessage = "El rol no puede exceder 30 caracteres")]
        public string Rol { get; set; } = string.Empty;

        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
    }
}