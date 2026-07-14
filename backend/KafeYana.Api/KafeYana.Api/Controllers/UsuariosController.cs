using KafeYana.Application.IServicios;
using KafeYana.Core.Entities.Entity;
using KafeYana.Domain.Request;
using KafeYana.Domain.TiposDeDatos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KafeYana.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController(IAccountService _servicio, UserManager<Usuario> _userManager) : ControllerBase
    {
        [HttpPost("Registro")]
        public async Task<IActionResult> Registro(RegisterRequest datos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _servicio.Register(datos);

            return Ok(new { message = "Usuario Registrado" });
        }

        [HttpPut("bloquear/{email}")]
        public async Task<IActionResult> Bloquear(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario is null)
                return NotFound(new { message = "Usuario no encontrado" });

            var roles = await _userManager.GetRolesAsync(usuario);
            if (roles.Contains(RolesKafe.Admin))
                return BadRequest(new { message = "No puedes bloquear a un administrador" });

            await _userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.MaxValue);

            return Ok(new { message = "Usuario bloqueado correctamente" });
        }

        [HttpPut("desbloquear/{email}")]
        public async Task<IActionResult> Desbloquear(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario is null)
                return NotFound(new { message = "Usuario no encontrado" });

            await _userManager.SetLockoutEndDateAsync(usuario, null);

            return Ok(new { message = "Usuario desbloqueado correctamente" });
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Eliminar(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario is null)
                return NotFound(new { message = "Usuario no encontrado" });

            var roles = await _userManager.GetRolesAsync(usuario);
            if (roles.Contains(RolesKafe.Admin))
                return BadRequest(new { message = "No puedes eliminar a un administrador" });

            var resultado = await _userManager.DeleteAsync(usuario);
            if (!resultado.Succeeded)
                return BadRequest(new { message = resultado.Errors.First().Description });

            return Ok(new { message = "Usuario eliminado correctamente" });
        }
    }
}
