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
    }
}
