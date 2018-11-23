using Core.Helper;
using Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Filter
{
    /// <summary>
    /// Authentication.
    /// </summary>
    public class Authentication : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the warrant.
        /// </summary>
        /// <value>The warrant.</value>
        public AuthenticationType _warrant { get; set; } = AuthenticationType.Anonymous;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Filter.Authentication"/> class.
        /// </summary>
        /// <param name="warrant">Beklenen yetki</param>
        public Authentication(AuthenticationType warrant = AuthenticationType.Anonymous)
        {
            _warrant = warrant;
        }

        /// <summary>
        /// Ons the action executing.
        /// </summary>
        /// <param name="context">Context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_warrant == AuthenticationType.Anonymous)
            {
                base.OnActionExecuting(context);
            }

            context.HttpContext.Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues values);

            if (values.Count != 0)
            {
                var validation = values[0].ValidToken();
                if (!validation)
                {
                    // hatalı token, error response
                    context.Result = new OkObjectResult(new
                    {
                        ResponseCode = ResponseCode.Unauthorized
                    });
                    base.OnActionExecuting(context);
                }
                else
                {
                    base.OnActionExecuting(context);
                }
            }
            else
            {
                if (_warrant != AuthenticationType.Anonymous)
                {
                    // eksik token, error response
                    context.Result = new OkObjectResult(new
                    {
                        ResponseCode = ResponseCode.InvalidHeader
                    });
                    base.OnActionExecuting(context);
                }
                else
                {
                    base.OnActionExecuting(context);
                }
            }
            base.OnActionExecuting(context);
        }
    }
}