using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaBecas.Library.Dtos
{
    public class UsuarioLoginDataDto
    {
        public int IdUsuario { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
    }
}
