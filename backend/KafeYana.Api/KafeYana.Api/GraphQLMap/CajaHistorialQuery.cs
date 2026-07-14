using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using KafeYana.Api.Helpers;
using KafeYana.Application.IRepositorio;
using KafeYana.Domain.Entities;
using KafeYana.Domain.TiposDeDatos;

namespace KafeYana.Api.GraphQLMap
{
    [ExtendObjectType("Query")]
    public class CajaHistorialQuery
    {
        public Task<OffsetPage<CajaHistorial>> CajaHistorial(
            [Service] ICajaHistorialRepositorio _db,
            int? skip,
            int? take,
            CancellationToken ct = default)
            => _db.Query()
                  .OrderByDescending(h => h.Id)
                  .ToOffsetPageAsync(skip, take, ct);

        public Task<OffsetPage<CajaHistorialMovimiento>> CajaHistorialMovimiento(
            [Service] ICajaHistorialMovimientoRepositorio _db,
            int? skip,
            int? take,
            CancellationToken ct = default)
            => _db.Query()
                  .OrderByDescending(m => m.Id)
                  .ToOffsetPageAsync(skip, take, ct);
    }
}
