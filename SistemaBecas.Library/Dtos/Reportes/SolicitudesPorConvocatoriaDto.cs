using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaBecas.Library.Dtos.Reportes
{
    public class SolicitudesPorConvocatoriaDto
    {
        public int IdConvocatoria { get; set; }

        public string Convocatoria { get; set; } = string.Empty;

        public long TotalSolicitudes { get; set; }

        public long EnEvaluacion { get; set; }

        public long Aprobadas { get; set; }

        public long Rechazadas { get; set; }
    }
}
