using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;

namespace SimplePOS.API.Controllers
{
    /// <summary>
    /// Controlador para autenticación y registro de usuarios.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        /// <summary>
        /// Registra un nuevo usuario estandar en el sistema.
        /// </summary>
        /// <param name="request">Datos del usuario a Registrar</param>
        /// <returns>Respuesta con los datos de autenticacion</returns>
        /// <response code="200">Usuario registrado exitosamente</response>
        /// <response code="400">Error en los datos proporcionados</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var result = await authService.RegisterAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Registra un nuevo usuario con rol de administrador en el sistema.
        /// </summary>
        /// <param name="request">Datos del administrador a registrar</param>
        /// <returns>Respuesta con los datos de autenticacion</returns>
        /// <response code="200">Administrador registrado exitosamente</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">Acceso denegado (no es Admin)</response>
        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegisterRequest request)
        {
            var response = await authService.RegisterAsync(request, role: "Admin");
            return Ok(response);
        }

        /// <summary>
        /// Inicia sesión con credenciales de usuario.
        /// </summary>
        /// <param name="request">Credenciales del usuario</param>
        /// <returns>Token de acceso y datos del usuario</returns>
        /// <response code="200">Inicio de sesión exitoso</response>
        /// <response code="401">Credenciales inválidas</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var result = await authService.LoginAsync(request);
            return Ok(result);
        }
    }
}
