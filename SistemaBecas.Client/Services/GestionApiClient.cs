using System.Net.Http.Json;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Client.Services;

public sealed class GestionApiClient(HttpClient httpClient)
{
    public Task<List<ConvocatoriaDto>> ObtenerConvocatoriasAsync(CancellationToken cancellationToken = default) =>
        httpClient.GetFromJsonAsync<List<ConvocatoriaDto>>("api/convocatorias", cancellationToken)!;

    public Task<ConvocatoriaDto?> ObtenerConvocatoriaAsync(int id, CancellationToken cancellationToken = default) =>
        httpClient.GetFromJsonAsync<ConvocatoriaDto>($"api/convocatorias/{id}", cancellationToken);

    public async Task GuardarConvocatoriaAsync(ConvocatoriaGuardarDto dto, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/convocatorias", dto, cancellationToken);
        await AsegurarExitoAsync(response, cancellationToken);
    }

    public async Task ActualizarConvocatoriaAsync(int id, ConvocatoriaGuardarDto dto, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PutAsJsonAsync($"api/convocatorias/{id}", dto, cancellationToken);
        await AsegurarExitoAsync(response, cancellationToken);
    }

    public async Task EliminarConvocatoriaAsync(int id, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.DeleteAsync($"api/convocatorias/{id}", cancellationToken);
        await AsegurarExitoAsync(response, cancellationToken);
    }

    public Task<List<SolicitudGestionDto>> ObtenerSolicitudesAsync(CancellationToken cancellationToken = default) =>
        httpClient.GetFromJsonAsync<List<SolicitudGestionDto>>("api/solicitudes", cancellationToken)!;

    public async Task ActualizarEstadoSolicitudAsync(int id, SolicitudEstadoDto dto, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PutAsJsonAsync($"api/solicitudes/{id}/estado", dto, cancellationToken);
        await AsegurarExitoAsync(response, cancellationToken);
    }

    private static async Task AsegurarExitoAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode) return;

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        try
        {
            using var json = System.Text.Json.JsonDocument.Parse(body);
            if (json.RootElement.TryGetProperty("mensaje", out var mensaje))
                throw new HttpRequestException(mensaje.GetString() ?? $"La API respondió {(int)response.StatusCode}.", null, response.StatusCode);
        }
        catch (System.Text.Json.JsonException)
        {
            // La respuesta no contiene un mensaje de validación JSON.
        }

        throw new HttpRequestException($"La API respondió {(int)response.StatusCode}: {body}", null, response.StatusCode);
    }
}
