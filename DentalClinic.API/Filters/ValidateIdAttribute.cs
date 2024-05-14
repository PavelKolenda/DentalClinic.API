using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DentalClinic.API.Filters;
public class ValidateIdAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id)
        {
            if (id < 0 || id > int.MaxValue)
            {
                context.Result = new BadRequestObjectResult("Invalid Id");
            }
        }

        base.OnActionExecuting(context);
    }
}
