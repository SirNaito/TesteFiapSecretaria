using System;
using System.Linq;

namespace SecretariaFiapFluxo.Api.Helpers
{
    public static class PaginationHelper
    {
        public static IQueryable<T> Paginar<T>(IQueryable<T> query, int pagina = 1, int itensPorPagina = 10)
        {
            if (pagina < 1) pagina = 1;
            if (itensPorPagina < 1) itensPorPagina = 10;

            return query.Skip((pagina - 1) * itensPorPagina)
                        .Take(itensPorPagina);
        }
    }
}
