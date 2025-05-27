using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonolitoBackend.Core.Services;

namespace MonolitoBackend.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        // Construtor explicitamente definido
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")] //ENDPOINT DE LOGIN
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);
            if (token == null)
                return Unauthorized("Credenciais inválidas");

            return Ok(new { token });
        }

        [AllowAnonymous]
        [HttpPost("register")] //ENDPOINT DE REGISTER
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.RegisterUser(request.Username, request.Role, request.Password);
            return Ok("Usuário registrado com sucesso!");
        }
    }

    // Modelos de entrada
    public record LoginRequest(string Username, string Password);
    public record RegisterRequest(string Username, string Role, string Password);
}
