using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ControlObraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HttpFactoryController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpFactoryController> _logger;

        public HttpFactoryController(IHttpClientFactory httpClientFactory, ILogger<HttpFactoryController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene usuarios desde JSONPlaceholder API
        /// Endpoint: GET /api/HttpFactory
        /// Demuestra el uso del patrón HttpClientFactory para consumir APIs externas
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetExternalUsers()
        {
            try
            {
                // Crear cliente HTTP usando el factory (patrón recomendado)
                var client = _httpClientFactory.CreateClient("jsonplaceholder");

                _logger.LogInformation("Consumiendo API externa: JSONPlaceholder /users");

                // Realizar petición GET al endpoint /users
                var response = await client.GetAsync("/users");

                // Verificar que la respuesta sea exitosa
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API externa retornó código: {StatusCode}", response.StatusCode);
                    return StatusCode((int)response.StatusCode, new 
                    { 
                        error = "Error al consultar API externa",
                        statusCode = response.StatusCode 
                    });
                }

                // Leer contenido de la respuesta
                var content = await response.Content.ReadAsStringAsync();

                // Deserializar JSON
                var users = JsonSerializer.Deserialize<List<ExternalUser>>(content, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });

                _logger.LogInformation("API externa consultada exitosamente. Usuarios obtenidos: {Count}", users?.Count ?? 0);

                return Ok(new 
                {
                    source = "JSONPlaceholder API",
                    endpoint = "/users",
                    count = users?.Count ?? 0,
                    data = users
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de red al consumir API externa");
                return StatusCode(503, new 
                { 
                    error = "Servicio externo no disponible",
                    message = ex.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al consumir API externa");
                return StatusCode(500, new 
                { 
                    error = "Error interno del servidor",
                    message = ex.Message 
                });
            }
        }
    }

    /// <summary>
    /// DTO para representar los usuarios de JSONPlaceholder
    /// </summary>
    public class ExternalUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
    }
}
