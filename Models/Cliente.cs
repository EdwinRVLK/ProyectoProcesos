using System;
using System.ComponentModel.DataAnnotations;

namespace GimManager.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        // Cambiado: Un solo apellido en lugar de paterno/materno
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string Telefono { get; set; } = string.Empty;

        // Cambiado: Usando TipoMembresia en lugar de Membresia
        [Required(ErrorMessage = "El tipo de membresía es obligatorio")]
        public string TipoMembresia { get; set; } = string.Empty;

        public string? RutaFoto { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [Display(Name = "Precio de Membresía")]
        [DataType(DataType.Currency)]
        public decimal PrecioMembresia { get; set; }

        public void CalcularPrecioYFechaVencimiento(int meses)
        {
            if (meses <= 0)
                throw new ArgumentException("El número de meses debe ser mayor que 0.");

            FechaVencimiento = DateTime.Now.AddMonths(meses);

            PrecioMembresia = TipoMembresia switch
            {
                "Normal" => 500 * meses,
                "Estudiante" => 300 * meses,
                "VIP" => 1000 * meses,
                _ => throw new ArgumentException("Tipo de membresía no válido")
            };
        }
    }
}