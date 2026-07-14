using KafeYana.Application.Dtos.ElaboradosDtos;
using KafeYana.Application.IRepositorio;
using KafeYana.Domain.TiposDeDatos;
using KafeYana.Infrastructure.Servicios.Facturacion;
using Microsoft.AspNetCore.Mvc;

namespace KafeYana.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElaboradoController(IElaboradoRepositorio _repo) : ControllerBase
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Crear([FromForm] DtoElaboradoCrear datos)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(datos.CodigoSin))
                return BadRequest(new { message = "CodigoSin es requerido" });

            if (!UnidadMedidaSiatService.TryResolver(datos.Unidad_medida, out var codigoUm, out var descripcionUm))
                return BadRequest(new { message = "Unidad de medida no válida" });

            datos.AsignarUnidadMedida(codigoUm, descripcionUm);

            var entidad = datos.CrearEntidad();

            await _repo.Crear(entidad);
            await _repo.SaveAsync();

            entidad.AsignarCodigo(ProductoCodigoService.Generar(entidad.Id));
            await _repo.SaveAsync();

            return Created("", new
            {
                entidad.Id,
                entidad.Codigo,
                entidad.Nombre,
                entidad.Precio,
                message = "Producto creado"
            });
        }

        [HttpPut("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int Id, [FromForm] DtoElaboradoActualizar datos)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(datos.CodigoSin))
                return BadRequest(new { message = "CodigoSin es requerido" });

            if (!UnidadMedidaSiatService.TryResolver(datos.Unidad_medida, out var codigoUm, out var descripcionUm))
                return BadRequest(new { message = "Unidad de medida no válida" });

            datos.AsignarUnidadMedida(codigoUm, descripcionUm);

            var producto = await _repo.TraerProducto(Id: Id, elaborado: true);

            if (producto is null || producto.Elaborado is null) return NotFound("Producto elaborado no existe");

            datos.Editar(producto);

            await _repo.SaveAsync();

            return Ok(new
            {
                producto.Id,
                producto.Nombre,
                producto.Precio,
                message = "Producto actualizado"
            });
        }
    }
}
