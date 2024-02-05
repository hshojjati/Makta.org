using Microsoft.AspNetCore.Mvc.Filters;

namespace Makta.Filters
{
    public class CheckRolesAsyncFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //try
            //{
            //    var actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            //    var controllerName = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            //    var area = context.RouteData.Values["area"];

            //    if (!area.ToString().EndsWith("Public"))
            //    {

            //        #region initial DI variables
            //        var _userManager = (UserManager<ApplicationUser>)context.HttpContext.RequestServices
            //            .GetService(typeof(UserManager<ApplicationUser>));

            //        var _permisionRepository = (IRepository<RolePermision>)context.HttpContext.RequestServices
            //            .GetService(typeof(IRepository<RolePermision>));

            //        #endregion

            //        var currentUser = _userManager.GetUserAsync(context.HttpContext.User).Result;
            //        if (currentUser == null)
            //        {
            //            context.Result = new LocalRedirectResult("/Identity/Account/Login");
            //            return;
            //        }

            //        var permision = _permisionRepository.TableNoTracking
            //            .FirstOrDefaultAsync(p => p.Action.ToLower() == actionName.ToLower() &&
            //                                 p.Controller.ToLower() == controllerName.ToLower() &&
            //                                 p.Area.ToLower() == area.ToString().ToLower()).Result;

            //        if (permision == null)
            //        {
            //            context.Result = new LocalRedirectResult("/Identity/Account/AccessDenied");
            //            return;
            //        }

            //        var rolePermisions = permision.RoleNames.Trim(',').Split(",", System.StringSplitOptions.RemoveEmptyEntries).ToList();
            //        if (rolePermisions.Count == 0)
            //        {
            //            context.Result = new LocalRedirectResult("/Identity/Account/AccessDenied");
            //            return;
            //        }

            //        var userRoles = _userManager.GetRolesAsync(currentUser).Result;
            //        if (userRoles.Count == 0)
            //        {
            //            context.Result = new LocalRedirectResult("/Identity/Account/AccessDenied");
            //            return;
            //        }
            //        bool findedAnyRole = false;

            //        foreach (var item in userRoles)
            //        {
            //            if (rolePermisions.Any(p => p.Trim().ToLower() == item.ToLower()))
            //            {
            //                findedAnyRole = true;
            //                break;
            //            }
            //        }

            //        if (!findedAnyRole)
            //        {
            //            context.Result = new LocalRedirectResult("/Identity/Account/AccessDenied");
            //            return;
            //        }

            //        return;
            //    }


            //}
            //catch
            //{
            //    context.Result = new LocalRedirectResult("/Identity/Account/AccessDenied");
            //    return;
            //}


        }
    }
}
