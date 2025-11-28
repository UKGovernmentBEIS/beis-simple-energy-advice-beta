using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SeaPublicWebsite.BusinessLogic.Services;

namespace SeaPublicWebsite.Middleware;

public class EmergencyMaintenanceMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext,
        EmergencyMaintenanceService emergencyMaintenanceService)
    {
        if (await emergencyMaintenanceService.SiteIsInEmergencyMaintenance())
        {
            httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await httpContext.Response.CompleteAsync();
        }
        else
        {
            await next.Invoke(httpContext);
        }
    }
}