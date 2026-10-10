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

        public ReportesController(
            IReportesService reportesService)
        {
            _reportesService = reportesService;
        }


        // ============================================================
        // OBTENER REPORTE
        // ============================================================

        [HttpGet("solicitudes-por-convocatoria")]
        public async Task<ActionResult<
            List<SolicitudesPorConvocatoriaDto>>>
            ObtenerSolicitudesPorConvocatoria()
        {
            var resultado =
                await _reportesService
                    .ObtenerSolicitudesPorConvocatoria();

            return Ok(resultado);
        }


        // ============================================================
        // GENERAR PDF
        // ============================================================

        [HttpGet("solicitudes-por-convocatoria/pdf")]
        public async Task<IActionResult>
            GenerarSolicitudesPorConvocatoriaPdf()
        {
            var datos =
                await _reportesService
                    .ObtenerSolicitudesPorConvocatoria();


            var documento =
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.Letter);
                        page.Margin(40);

                        // ====================================================
                        // ENCABEZADO
                        // ====================================================

                        page.Header()
                            .Column(column =>
                            {
                                column.Item()
                                    .AlignCenter()
                                    .Text("SISTEMA DE BECAS")
                                    .Bold()
                                    .FontSize(18);

                                column.Item()
                                    .AlignCenter()
                                    .Text(
                                        "REPORTE DE SOLICITUDES POR CONVOCATORIA")
                                    .Bold()
                                    .FontSize(14);

                                column.Item()
                                    .AlignCenter()
                                    .Text(
                                        $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                    .FontSize(9);
                            });


                        // ====================================================
                        // CONTENIDO
                        // ====================================================

                        page.Content()
                            .PaddingTop(25)
                            .Table(table =>
                            {
                                // ============================================
                                // COLUMNAS
                                // ============================================

                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });


                                // ============================================
                                // ENCABEZADOS
                                // ============================================

                                table.Header(header =>
                                {
                                    header.Cell()
                                        .Background(Colors.Blue.Darken2)
                                        .Padding(8)
                                        .Text("Convocatoria")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell()
                                        .Background(Colors.Blue.Darken2)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Text("Total")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell()
                                        .Background(Colors.Blue.Darken2)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Text("En evaluación")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell()
                                        .Background(Colors.Blue.Darken2)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Text("Aprobadas")
                                        .FontColor(Colors.White)
                                        .Bold();

                                    header.Cell()
                                        .Background(Colors.Blue.Darken2)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Text("Rechazadas")
                                        .FontColor(Colors.White)
                                        .Bold();
                                });


                                // ============================================
                                // DATOS
                                // ============================================

                                foreach (var item in datos)
                                {
                                    table.Cell()
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten2)
                                        .Padding(8)
                                        .Text(item.Convocatoria);

                                    table.Cell()
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten2)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Text(item.TotalSolicitudes.ToString());

                                    table.Cell()
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten2)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Text(item.EnEvaluacion.ToString());

                                    table.Cell()
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten2)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Text(item.Aprobadas.ToString());

                                    table.Cell()
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten2)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Text(item.Rechazadas.ToString());
                                }
                            });


                        // ====================================================
                        // PIE DE PÁGINA
                        // ====================================================

                        page.Footer()
                            .AlignCenter()
                            .Text(text =>
                            {
                                text.Span("Sistema de Becas - ");

                                text.CurrentPageNumber();

                                text.Span(" / ");

                                text.TotalPages();
                            });
                    });
                });


            var pdf =
                documento.GeneratePdf();


            return File(
                pdf,
                "application/pdf",
                $"Reporte_Solicitudes_Por_Convocatoria_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
        // =========================================================
        // REPORTE 2: ESTADO DE SOLICITUDES
        // =========================================================

        [HttpGet("estado-solicitudes")]
        public async Task<ActionResult<List<EstadoSolicitudesDto>>>
            ObtenerEstadoSolicitudes()
        {
            var resultado =
                await _reportesService.ObtenerEstadoSolicitudes();

            return Ok(resultado);
        }

        [HttpGet("estado-solicitudes/pdf")]
        public async Task<IActionResult>
            GenerarEstadoSolicitudesPdf()
        {
            var datos =
                await _reportesService.ObtenerEstadoSolicitudes();

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Formato horizontal para acomodar las cinco columnas.
                    page.Size(PageSizes.Letter.Landscape());
                    page.Margin(30);

                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .AlignCenter()
                                .Text("SISTEMA DE BECAS")
                                .Bold()
                                .FontSize(18);

                            column.Item()
                                .AlignCenter()
                                .Text("REPORTE DE ESTADO DE SOLICITUDES")
                                .Bold()
                                .FontSize(14);

                            column.Item()
                                .AlignCenter()
                                .Text($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                .FontSize(9);
                        });

                    page.Content()
                        .PaddingTop(20)
                        .Table(table =>
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
                                header.Cell()
                                    .Background(Colors.Blue.Darken2)
                                    .Padding(7)
                                    .Text("Estudiante")
                                    .FontColor(Colors.White)
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Blue.Darken2)
                                    .Padding(7)
                                    .Text("Convocatoria")
                                    .FontColor(Colors.White)
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Blue.Darken2)
                                    .Padding(7)
                                    .Text("Fecha")
                                    .FontColor(Colors.White)
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Blue.Darken2)
                                    .Padding(7)
                                    .Text("Estado")
                                    .FontColor(Colors.White)
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Blue.Darken2)
                                    .Padding(7)
                                    .Text("Observaciones")
                                    .FontColor(Colors.White)
                                    .Bold();
                            });

                            foreach (var item in datos)
                            {
                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(7)
                                    .Text(item.Estudiante);

                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(7)
                                    .Text(item.Convocatoria);

                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(7)
                                    .Text(item.FechaSolicitud.ToString("dd/MM/yyyy"));

                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(7)
                                    .Text(item.Estado);

                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(7)
                                    .Text(item.Observaciones ?? string.Empty);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
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

        [HttpGet("becas-otorgadas/pdf")]
        public async Task<IActionResult> GenerarBecasOtorgadasPdf()
        {
            var datos = await _reportesService.ObtenerBecasOtorgadas();

            var fechaGeneracion = DateTime.Now;

            var pdf = QuestPDF.Fluent.Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(QuestPDF.Helpers.PageSizes.Letter.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Column(column =>
                    {
                        column.Item()
                            .AlignCenter()
                            .Text("SISTEMA DE BECAS")
                            .Bold()
                            .FontSize(18);

                        column.Item()
                            .AlignCenter()
                            .Text("REPORTE DE BECAS OTORGADAS")
                            .Bold()
                            .FontSize(14);

                        column.Item()
                            .AlignRight()
                            .Text($"Fecha de generación: {fechaGeneracion:dd/MM/yyyy HH:mm}");
                    });

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item().Text("RESUMEN POR CONVOCATORIA")
                            .Bold()
                            .FontSize(12);

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
                            .Bold()
                            .FontSize(12);

                        var beneficiarios = datos
                            .Where(x => x.IdSolicitud.HasValue)
                            .ToList();

                        if (beneficiarios.Count == 0)
                        {
                            column.Item()
                                .PaddingTop(8)
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

        static QuestPDF.Infrastructure.IContainer CellHeader(
            QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Background(QuestPDF.Helpers.Colors.Grey.Lighten2)
                .Border(1)
                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1)
                .Padding(5);
        }

        static QuestPDF.Infrastructure.IContainer CellBody(
            QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                .Padding(5);
        }

    }
}


