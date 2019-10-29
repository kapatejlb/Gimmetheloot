using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GimmeTheLoot.Models
{
    public interface IRouteConstraint
    {
        bool Match(HttpContext httpContext,
                IRouter route,
                string routeKey,
                IDictionary<string, object> values,
                RouteDirection routeDirection);
    }
}
