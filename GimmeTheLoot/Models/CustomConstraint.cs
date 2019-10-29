using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GimmeTheLoot.Models
{
    public class CustomConstraint : IRouteConstraint
    {
        private string uri;
        public CustomConstraint(string uri)
        {
            this.uri = uri;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey,
                RouteValueDictionary values, RouteDirection routeDirection)
        {
            return !(uri == httpContext.Request.Path);
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, IDictionary<string, object> values, RouteDirection routeDirection)
        {
            throw new NotImplementedException();
        }
    }
}
