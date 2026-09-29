using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaBecas.Library.Dtos
{
    public class LoginRespuestaDto
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioDto Usuario { get; set; } = new();

    }
}
