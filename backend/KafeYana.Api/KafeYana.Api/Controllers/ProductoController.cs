using KafeYana.Application.Dtos.CompradoDtos;
using KafeYana.Application.IRepositorio;
using KafeYana.Domain.TiposDeDatos;
using KafeYana.Infrastructure.Servicios.Facturacion;
using Microsoft.AspNetCore.Mvc;

namespace KafeYana.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController(IProductoRepositorio _producto) : ControllerBase
    {

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Crear([FromForm] DtoCompradoCrear datos)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(datos.CodigoSin))
                return BadRequest(new { message = "CodigoSin es requerido" });

            if (!UnidadMedidaSiatService.TryResolver(datos.Unidad_medida, out var codigoUm, out var descripcionUm))
                return BadRequest(new { message = "Unidad de medida no válida" });

            datos.AsignarUnidadMedida(codigoUm, descripcionUm);

            var producto = datos.ProductoCrear();

            await _producto.Crear(producto);
            await _producto.SaveAsync();

            producto.AsignarCodigo(ProductoCodigoService.Generar(producto.Id));
            await _producto.SaveAsync();

            return Created("", new { message = "Producto creado", Id = producto.Id, Codigo = producto.Codigo });
        }

        [HttpPut("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int Id, [FromForm] DtoCompradoCrear datos)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(datos.CodigoSin))
                return BadRequest(new { message = "CodigoSin es requerido" });

            if (!UnidadMedidaSiatService.TryResolver(datos.Unidad_medida, out var codigoUm, out var descripcionUm))
                return BadRequest(new { message = "Unidad de medida no válida" });

            datos.AsignarUnidadMedida(codigoUm, descripcionUm);

            var productoDb = await _producto.TraerProducto(Id, comprado: true);

            if (productoDb is null || productoDb.Tipo != TiposProductos.Comprado)
                return NotFound(new { message = "Producto no encontrado" });

            datos.Editar(productoDb);

            await _producto.SaveAsync();

            return Ok(new { message = "Producto actualizado" });
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var producto = await _producto.FindByIdAsync(Id);

            if (producto is null) return NotFound(new { message = "Producto no encontrado" });

            await _producto.Remove(producto);
            await _producto.SaveAsync();

            return Ok(new { message = "Producto eliminado" });
        }
    }
}
