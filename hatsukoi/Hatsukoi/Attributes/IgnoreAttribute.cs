using Microsoft.AspNetCore.Mvc.Filters;

namespace Hatsukoi.Attributes
{
    public class IgnoreAttribute: ActionFilterAttribute,IActionFilter
    {
    }
}
