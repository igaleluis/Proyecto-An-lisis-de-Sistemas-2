using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaBecas.Library.Dtos
{
    public class SubirDocumentoDto
    {
        public int IdSolicitud { get; set; }

        public int IdDocumento { get; set; }

        public string? Observaciones { get; set; }
    }
}
