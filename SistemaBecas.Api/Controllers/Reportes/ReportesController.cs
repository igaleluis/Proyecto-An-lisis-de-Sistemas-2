using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemaBecas.Api.Services.Reportes;
using SistemaBecas.Library.Dtos.Reportes;

namespace SistemaBecas.Api.Controllers.Reportes
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;

        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        // ============================================================
        // VALIDAR FECHAS
        // ============================================================

        private static bool FechasValidas(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            return !fechaInicio.HasValue
                || !fechaFin.HasValue
                || fechaInicio.Value.Date <= fechaFin.Value.Date;
        }

        private IActionResult ErrorFechas()
        {
            return BadRequest(
                "La fecha de inicio no puede ser posterior a la fecha de fin.");
        }

        // ============================================================
        // REPORTE 1: SOLICITUDES POR CONVOCATORIA
        // ============================================================

        [HttpGet("solicitudes-por-convocatoria")]
        public async Task<ActionResult<List<SolicitudesPorConvocatoriaDto>>>
            ObtenerSolicitudesPorConvocatoria(
                [FromQuery] DateTime? fechaInicio,
                [FromQuery] DateTime? fechaFin)
        {
            if (!FechasValidas(fechaInicio, fechaFin))
                return BadRequest(
                    "La fecha de inicio no puede ser posterior a la fecha de fin.");

            var resultado = await _reportesService
                .ObtenerSolicitudesPorConvocatoria(fechaInicio, fechaFin);

            return Ok(resultado);
        }

        [HttpGet("solicitudes-por-convocatoria/pdf")]
        public async Task<IActionResult> GenerarSolicitudesPorConvocatoriaPdf(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            if (!FechasValidas(fechaInicio, fechaFin))
                return ErrorFechas();

            var datos = await _reportesService
                .ObtenerSolicitudesPorConvocatoria(fechaInicio, fechaFin);

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(40);

                    page.Header().Column(column =>
                    {
                        column.Item()
                            .AlignCenter()
                            .Text("SISTEMA DE BECAS")
                            .Bold()
                            .FontSize(18);

                        column.Item()
                            .AlignCenter()
                            .Text("REPORTE DE SOLICITUDES POR CONVOCATORIA")
                            .Bold()
                            .FontSize(14);

                        column.Item()
                            .AlignCenter()
                            .Text($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(9);

                        column.Item()
                            .AlignCenter()
                            .Text(ObtenerTextoPeriodo(fechaInicio, fechaFin))
                            .FontSize(9);
                    });

                    page.Content()
                        .PaddingTop(25)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Darken2)
                                    .Padding(8).Text("Convocatoria")
                                    .FontColor(Colors.White).Bold();

                                header.Cell().Background(Colors.Blue.Darken2)
                                    .Padding(8).AlignCenter().Text("Total")
                                    .FontColor(Colors.White).Bold();

                                header.Cell().Background(Colors.Blue.Darken2)
                                    .Padding(8).AlignCenter().Text("En evaluación")
                                    .FontColor(Colors.White).Bold();

                                header.Cell().Background(Colors.Blue.Darken2)
                                    .Padding(8).AlignCenter().Text("Aprobadas")
                                    .FontColor(Colors.White).Bold();

                                header.Cell().Background(Colors.Blue.Darken2)
                                    .Padding(8).AlignCenter().Text("Rechazadas")
                                    .FontColor(Colors.White).Bold();
                            });

                            foreach (var item in datos)
                            {
                                table.Cell().BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).Text(item.Convocatoria);

                                table.Cell().BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).AlignCenter()
                                    .Text(item.TotalSolicitudes.ToString());

                                table.Cell().BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).AlignCenter()
                                    .Text(item.EnEvaluacion.ToString());

                                table.Cell().BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).AlignCenter()
                                    .Text(item.Aprobadas.ToString());

                                table.Cell().BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(8).AlignCenter()
                                    .Text(item.Rechazadas.ToString());
                            }
                        });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Sistema de Becas - ");
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
                });
            });

            var pdf = documento.GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"Reporte_Solicitudes_Por_Convocatoria_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        // ============================================================
        // REPORTE 2: ESTADO DE SOLICITUDES
        // ============================================================

        [HttpGet("estado-solicitudes")]
        public async Task<ActionResult<List<EstadoSolicitudesDto>>>
            ObtenerEstadoSolicitudes(
                [FromQuery] DateTime? fechaInicio,
                [FromQuery] DateTime? fechaFin)
        {
            if (!FechasValidas(fechaInicio, fechaFin))
                return BadRequest(
                    "La fecha de inicio no puede ser posterior a la fecha de fin.");

            var resultado = await _reportesService
                .ObtenerEstadoSolicitudes(fechaInicio, fechaFin);

            return Ok(resultado);
        }

        [HttpGet("estado-solicitudes/pdf")]
        public async Task<IActionResult> GenerarEstadoSolicitudesPdf(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            if (!FechasValidas(fechaInicio, fechaFin))
                return ErrorFechas();

            var datos = await _reportesService
                .ObtenerEstadoSolicitudes(fechaInicio, fechaFin);

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());
                    page.Margin(30);

                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter()
                            .Text("SISTEMA DE BECAS")
                            .Bold().FontSize(18);

                        column.Item().AlignCenter()
                            .Text("REPORTE DE ESTADO DE SOLICITUDES")
                            .Bold().FontSize(14);

                        column.Item().AlignCenter()
                            .Text($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(9);

                        column.Item().AlignCenter()
                            .Text(ObtenerTextoPeriodo(fechaInicio, fechaFin))
                            .FontSize(9);
                    });

                    page.Content().PaddingTop(20).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2.5f);
                            columns.RelativeColumn(2.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(3f);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Darken2)
                                .Padding(7).Text("Estudiante")
                                .FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Darken2)
                                .Padding(7).Text("Convocatoria")
                                .FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Darken2)
                                .Padding(7).Text("Fecha")
                                .FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Darken2)
                                .Padding(7).Text("Estado")
                                .FontColor(Colors.White).Bold();

                            header.Cell().Background(Colors.Blue.Darken2)
                                .Padding(7).Text("Observaciones")
                                .FontColor(Colors.White).Bold();
                        });

                        foreach (var item in datos)
                        {
                            table.Cell().BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(7).Text(item.Estudiante);

                            table.Cell().BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(7).Text(item.Convocatoria);

                            table.Cell().BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(7).Text(
                                    item.FechaSolicitud.ToString("dd/MM/yyyy"));

                            table.Cell().BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(7).Text(item.Estado);

                            table.Cell().BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(7).Text(item.Observaciones ?? string.Empty);
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Sistema de Becas - Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            });

            var pdf = documento.GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"Reporte_Estado_Solicitudes_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }

        // ============================================================
        // REPORTE 3: ESTADÍSTICAS
        // ============================================================

        [HttpGet("estadisticas")]
        public async Task<ActionResult<EstadisticasReportesDto>>
            ObtenerEstadisticas(
                [FromQuery] DateTime? fechaInicio,
                [FromQuery] DateTime? fechaFin)
        {
            if (!FechasValidas(fechaInicio, fechaFin))
                return BadRequest(
                    "La fecha de inicio no puede ser posterior a la fecha de fin.");

            var resultado = await _reportesService
                .ObtenerEstadisticas(fechaInicio, fechaFin);

            return Ok(resultado);
        }

        // ============================================================
        // REPORTE 4: BECAS OTORGADAS
        // ============================================================

        [HttpGet("becas-otorgadas/pdf")]
        public async Task<IActionResult> GenerarBecasOtorgadasPdf(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            if (!FechasValidas(fechaInicio, fechaFin))
                return ErrorFechas();

            var datos = await _reportesService
                .ObtenerBecasOtorgadas(fechaInicio, fechaFin);

            var fechaGeneracion = DateTime.Now;

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter()
                            .Text("SISTEMA DE BECAS")
                            .Bold().FontSize(18);

                        column.Item().AlignCenter()
                            .Text("REPORTE DE BECAS OTORGADAS")
                            .Bold().FontSize(14);

                        column.Item().AlignRight()
                            .Text($"Fecha de generación: {fechaGeneracion:dd/MM/yyyy HH:mm}");

                        column.Item().AlignRight()
                            .Text(ObtenerTextoPeriodo(fechaInicio, fechaFin))
                            .FontSize(9);
                    });

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item().Text("RESUMEN POR CONVOCATORIA")
                            .Bold().FontSize(12);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellHeader).Text("Convocatoria");
                                header.Cell().Element(CellHeader).Text("Estado");
                                header.Cell().Element(CellHeader).AlignCenter().Text("Cupos");
                                header.Cell().Element(CellHeader).AlignCenter().Text("Otorgadas");
                                header.Cell().Element(CellHeader).AlignCenter().Text("Restantes");
                            });

                            foreach (var grupo in datos.GroupBy(x => new
                            {
                                x.IdConvocatoria,
                                x.Convocatoria,
                                x.EstadoConvocatoria,
                                x.Cupos,
                                x.BecasOtorgadas,
                                x.CuposRestantes
                            }))
                            {
                                table.Cell().Element(CellBody).Text(grupo.Key.Convocatoria);
                                table.Cell().Element(CellBody).Text(grupo.Key.EstadoConvocatoria);
                                table.Cell().Element(CellBody).AlignCenter().Text(grupo.Key.Cupos.ToString());
                                table.Cell().Element(CellBody).AlignCenter().Text(grupo.Key.BecasOtorgadas.ToString());
                                table.Cell().Element(CellBody).AlignCenter().Text(grupo.Key.CuposRestantes.ToString());
                            }
                        });

                        column.Item().PaddingTop(20)
                            .Text("DETALLE DE BENEFICIARIOS")
                            .Bold().FontSize(12);

                        var beneficiarios = datos
                            .Where(x => x.IdSolicitud.HasValue)
                            .ToList();

                        if (beneficiarios.Count == 0)
                        {
                            column.Item().PaddingTop(8)
                                .Text("Actualmente no hay becas otorgadas registradas.")
                                .Italic();
                        }
                        else
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellHeader).Text("Estudiante");
                                    header.Cell().Element(CellHeader).Text("Convocatoria");
                                    header.Cell().Element(CellHeader).Text("Fecha de solicitud");
                                    header.Cell().Element(CellHeader).Text("Estado");
                                });

                                foreach (var beca in beneficiarios)
                                {
                                    table.Cell().Element(CellBody).Text(beca.Estudiante);
                                    table.Cell().Element(CellBody).Text(beca.Convocatoria);
                                    table.Cell().Element(CellBody).Text(
                                        beca.FechaSolicitud?.ToString("dd/MM/yyyy") ?? "N/D");
                                    table.Cell().Element(CellBody).Text(
                                        beca.EstadoSolicitud ?? "N/D");
                                }
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Sistema de Becas | Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"Reporte_Becas_Otorgadas_{fechaGeneracion:yyyyMMdd_HHmmss}.pdf");
        }

        // ============================================================
        // REPORTE 5: CONVOCATORIAS
        // ============================================================

        [HttpGet("convocatorias/pdf")]
        public async Task<IActionResult> GenerarConvocatoriasPdf(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            if (!FechasValidas(fechaInicio, fechaFin))
                return ErrorFechas();

            var datos = await _reportesService
                .ObtenerConvocatorias(fechaInicio, fechaFin);

            var fechaGeneracion = DateTime.Now;

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter()
                            .Text("SISTEMA DE BECAS")
                            .Bold().FontSize(18);

                        column.Item().AlignCenter()
                            .Text("REPORTE DE CONVOCATORIAS")
                            .Bold().FontSize(14);

                        column.Item().AlignRight()
                            .Text($"Fecha de generación: {fechaGeneracion:dd/MM/yyyy HH:mm}")
                            .FontSize(9);

                        column.Item().AlignRight()
                            .Text(ObtenerTextoPeriodo(fechaInicio, fechaFin))
                            .FontSize(9);
                    });

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text($"Total de convocatorias: {datos.Count}")
                            .Bold().FontSize(11);

                        column.Item()
                            .Text(
                                $"Convocatorias activas: {datos.Count(x => x.Estado.Trim().Equals("Activa", StringComparison.OrdinalIgnoreCase))}")
                            .Bold().FontSize(10);

                        column.Item().PaddingTop(12).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2.5f);
                                columns.RelativeColumn(3.5f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(0.8f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.4f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellHeader).Text("Convocatoria");
                                header.Cell().Element(CellHeader).Text("Descripción");
                                header.Cell().Element(CellHeader).AlignCenter().Text("Fecha inicio");
                                header.Cell().Element(CellHeader).AlignCenter().Text("Fecha cierre");
                                header.Cell().Element(CellHeader).AlignCenter().Text("Cupos");
                                header.Cell().Element(CellHeader).AlignCenter().Text("Estado");
                                header.Cell().Element(CellHeader).AlignCenter().Text("Fecha creación");
                            });

                            foreach (var item in datos)
                            {
                                table.Cell().Element(CellBody).Text(item.Nombre);

                                table.Cell().Element(CellBody).Text(
                                    string.IsNullOrWhiteSpace(item.Descripcion)
                                        ? "Sin descripción"
                                        : item.Descripcion);

                                table.Cell().Element(CellBody).AlignCenter()
                                    .Text(item.FechaInicio.ToString("dd/MM/yyyy"));

                                table.Cell().Element(CellBody).AlignCenter()
                                    .Text(item.FechaFin.ToString("dd/MM/yyyy"));

                                table.Cell().Element(CellBody).AlignCenter()
                                    .Text(item.Cupos.ToString());

                                table.Cell().Element(CellBody).AlignCenter()
                                    .Text(item.Estado);

                                table.Cell().Element(CellBody).AlignCenter()
                                    .Text(item.FechaCreacion.ToString("dd/MM/yyyy"));
                            }
                        });

                        if (datos.Count == 0)
                        {
                            column.Item().PaddingTop(15).AlignCenter()
                                .Text("No hay convocatorias registradas.")
                                .Italic();
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Sistema de Becas | Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"Reporte_Convocatorias_{fechaGeneracion:yyyyMMdd_HHmmss}.pdf");
        }

        // ============================================================
        // MÉTODO AUXILIAR: TEXTO DEL PERÍODO
        // ============================================================

        private static string ObtenerTextoPeriodo(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                return $"Período consultado: {fechaInicio.Value:dd/MM/yyyy} al {fechaFin.Value:dd/MM/yyyy}";
            }

            if (fechaInicio.HasValue)
            {
                return $"Desde: {fechaInicio.Value:dd/MM/yyyy}";
            }

            if (fechaFin.HasValue)
            {
                return $"Hasta: {fechaFin.Value:dd/MM/yyyy}";
            }

            return "Período consultado: todas las fechas";
        }

        // ============================================================
        // ESTILOS DE TABLAS
        // ============================================================

        private static IContainer CellHeader(IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten2)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(5);
        }

        private static IContainer CellBody(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(5);
        }
    }
}
