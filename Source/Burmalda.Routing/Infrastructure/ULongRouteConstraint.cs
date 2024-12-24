using System.Globalization;

namespace Burmalda.Routing;

public class ULongRouteConstraint : IParameterPolicy
{
    public const string Name = "ulong";

    public bool Match(
        HttpContext? httpContext, 
        IRouter? route, 
        string routeKey, 
        RouteValueDictionary values,
        RouteDirection routeDirection
    ){
        if (!values.TryGetValue(routeKey, out var routeValue))
            return false;
        
        switch (routeValue)
        {
            case null:
                return false;
            case ulong:
                return true;
            default:
                var valueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);
                return ulong.TryParse(valueString, out var _);
        }
    }
}