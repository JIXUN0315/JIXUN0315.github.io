using Hatsukoi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Hatsukoi.Attributes
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly IHostEnvironment _hostEnvironment;
        public ExceptionAttribute(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public override void OnException(ExceptionContext context)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                throw context.Exception;
            }
            else
            {
                context.Result = new RedirectResult("~/Home/Error");

            }
            context.ExceptionHandled = true;
        }
    }
}
