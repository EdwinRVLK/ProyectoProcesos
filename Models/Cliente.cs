using System;
using System.ComponentModel.DataAnnotations;

namespace GimManager.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        public string TipoMembresia { get; set; } = string.Empty;

        // Esta propiedad guardará la ruta de la imagen
        public string? RutaFoto { get; set; }

        // Nueva propiedad para almacenar la fecha de vencimiento
        public DateTime FechaVencimiento { get; set; }

        // Nueva propiedad para almacenar el precio de la membresía
        public decimal PrecioMembresia { get; set; }

        // Método para calcular el precio y la fecha de vencimiento según el tipo de membresía y el número de meses
        public void CalcularPrecioYFechaVencimiento(int meses)
        {
            if (meses <= 0)
            {
                throw new ArgumentException("El número de meses debe ser mayor que 0.");
            }

            // Lógica para calcular la fecha de vencimiento
            FechaVencimiento = DateTime.Now.AddMonths(meses);

            // Lógica para calcular el precio según el tipo de membresía
            switch (TipoMembresia)
            {
                case "Normal":
                    PrecioMembresia = 500 * meses; // Precio para membresía normal
                    break;

                case "Estudiante":
                    PrecioMembresia = 300 * meses; // Precio para membresía estudiante
                    break;

                case "VIP":
                    PrecioMembresia = 1000 * meses; // Precio para membresía VIP
                    break;

                default:
                    throw new ArgumentException("Tipo de membresía no válido.");
            }
        }
    }
}
