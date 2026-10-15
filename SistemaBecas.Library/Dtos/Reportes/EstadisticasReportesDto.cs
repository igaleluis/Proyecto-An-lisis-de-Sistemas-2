using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaBecas.Library.Dtos.Reportes
{
    public class EstadisticasReportesDto
    {
        public long TotalSolicitudes { get; set; }

        public long Aprobadas { get; set; }

        public long Rechazadas { get; set; }

        public long BecasOtorgadas { get; set; }
    }
}
