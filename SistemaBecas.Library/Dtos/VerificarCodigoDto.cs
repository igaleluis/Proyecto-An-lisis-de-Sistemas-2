using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaBecas.Library.Dtos
{
    public class VerificarCodigoDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
    }
}
