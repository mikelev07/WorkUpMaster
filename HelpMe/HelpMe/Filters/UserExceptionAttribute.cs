using System;
using System.Web.Mvc;

namespace Filter.Infrastructure
{
    public class UserIdExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled &&
                    filterContext.Exception is ArgumentOutOfRangeException)
            {
                filterContext.Result =
                    new RedirectResult("~/View/UserIdErrorPage.chtml");
                filterContext.ExceptionHandled = true;
            }
        }
    }
}