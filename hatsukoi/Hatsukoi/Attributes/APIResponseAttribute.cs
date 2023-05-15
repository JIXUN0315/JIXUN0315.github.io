using Hatsukoi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hatsukoi.Attributes
{
    public class APIResponseAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Exception != null) 
            {
                return;
            }
            var ignoreResult = context.Filters.Any(c => c.Equals(typeof(IgnoreAttribute)));
            if (ignoreResult)
            {
                return;
            }

            var result = context.Result;
            var apiResult = new APIBaseResponse();
            if (result is ObjectResult objtect)
            {
                var data = objtect.Value;
                apiResult.Result = data;
                apiResult.Status = APIStatus.Success;
                apiResult.ErrorMsg = string.Empty;
            }

            context.Result = new JsonResult(apiResult);
            base.OnActionExecuted(context);
        }
    }
}
