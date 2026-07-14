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
    public class HitoCompraQuery
    {
        public Task<OffsetPage<HitoCompra>> HitosCompra(
            [Service] IHitoCompraRepositorio _repo,
            int? skip,
            int? take,
            CancellationToken ct = default)
            => _repo.GetHitos()
                    .OrderBy(h => h.NumeroCompras)
                    .ToOffsetPageAsync(skip, take, ct);
    }
}
