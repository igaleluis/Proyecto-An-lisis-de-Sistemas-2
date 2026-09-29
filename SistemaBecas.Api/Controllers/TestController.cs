using SistemaBecas.Api.Repositories;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly DbConnectionBecas _connectionFactory;

    public TestController(DbConnectionBecas connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpGet("conexion")]
    public async Task<IActionResult> ProbarConexion()
    {
        try
        {
            await using var connection = _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            return Ok(new
            {
                mensaje = "Conexión exitosa con PostgreSQL"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error al conectar con PostgreSQL",
                error = ex.Message
            });
        }
    }
}