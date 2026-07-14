using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using KafeYana.Api.Helpers;
using KafeYana.Application.IRepositorio;
using KafeYana.Domain.Entities;
using KafeYana.Domain.TiposDeDatos;
using Microsoft.EntityFrameworkCore;

namespace KafeYana.Api.GraphQLMap
{
    [ExtendObjectType("Query")]
    public class CajaQuery
    {
        [UseProjection]
        public async Task<Caja?> Caja([Service] ICajaRepositorio _db)
        {
            return await _db.Query().FirstOrDefaultAsync();
        }

        public Task<OffsetPage<CajaMovimiento>> CajaMoviminetos(
            ICajaMovimientoRepositorio _db,
            int? skip,
            int? take,
            CancellationToken ct = default)
            => _db.Query()
                  .OrderByDescending(m => m.Fecha)
                  .ToOffsetPageAsync(skip, take, ct);
    }
}
