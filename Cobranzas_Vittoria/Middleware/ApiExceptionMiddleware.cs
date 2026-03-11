using Microsoft.Data.SqlClient;

namespace Cobranzas_Vittoria.Middleware
{
    public class ApiExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (SqlException ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { ok = false, error = "SQL_ERROR", message = ex.Message });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { ok = false, error = "UNHANDLED_ERROR", message = ex.Message });
            }
        }
    }
}
