using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaBecas.Library.Dtos.Reportes
{
    public class BecasOtorgadasDto
    {
        public int IdConvocatoria { get; set; }

        public string Convocatoria { get; set; } = string.Empty;

        public string EstadoConvocatoria { get; set; } = string.Empty;

        public int Cupos { get; set; }

        public long BecasOtorgadas { get; set; }

        public long CuposRestantes { get; set; }

        public int? IdSolicitud { get; set; }

        public string Estudiante { get; set; } = string.Empty;

        public DateTime? FechaSolicitud { get; set; }

        public string? EstadoSolicitud { get; set; }
    }
}
