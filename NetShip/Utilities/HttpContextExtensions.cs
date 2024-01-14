using Microsoft.EntityFrameworkCore;

namespace NetShip.Utilities
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParametersInHeader<T> (this HttpContext httpContext, IQueryable<T> queryable) 
        { 
            if (httpContext is null)
                throw new ArgumentNullException (nameof (httpContext));

            double cantidad = await queryable.CountAsync();
            httpContext.Response.Headers.Append("totalRecordCount", cantidad.ToString());
        }
    }
}
