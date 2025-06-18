using System;

namespace GimManager.Models
{
    public class Entrada
    {
        public int Id { get; set; } // Identificador único de la entrada
        public decimal Monto { get; set; } // Monto de la entrada (ingreso)
        public string Descripcion { get; set; } // Descripción de la entrada
        public DateTime Fecha { get; set; } // Fecha en la que se registró la entrada
    }
}
