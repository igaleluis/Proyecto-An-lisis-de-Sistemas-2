using System;

namespace SistemaBecas.Library.Dtos
{
    public class RegistroEstudianteDto
    {
        // Datos de usuario
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Datos de estudiante
        public string DPI { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
    }
}
