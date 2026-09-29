using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaBecas.Library.Dtos
{
    public class LoginPeticiónDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
