using System.IO;
using System.Threading.Tasks;
using Core.Helper;
using Core.Localization;
using Microsoft.AspNetCore.Http;
using Model;

namespace Core.Filter
{
    /// <summary>
    /// Response middleware.
    /// </summary>
    public class ResponseMiddleware
    {
        /// <summary>
        /// The next.
        /// </summary>
        readonly RequestDelegate _next;
        /// <summary>
        /// The localization.
        /// </summary>
        readonly ILocalization _localization;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Core.Filter.ResponseMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next.</param>
        /// <param name="localization">Localization.</param>
        public ResponseMiddleware(RequestDelegate next, ILocalization localization)
        {
            _next = next;
            _localization = localization;
        }

        /// <summary>
        /// Invoke the specified context.
        /// </summary>
        /// <returns>The ınvoke.</returns>
        /// <param name="context">Context.</param>
        public async Task Invoke(HttpContext context)
        {
            var originBody = context.Response.Body;

            var newBody = new MemoryStream();

            context.Response.Body = newBody;

            await _next(context);

            newBody.Seek(0, SeekOrigin.Begin);

            var json = new StreamReader(newBody).ReadToEnd();

            if (json.ToObject<Response<dynamic>>().Data == null)
            {
                json = new Response<dynamic>
                {
                    ResponseCode = ResponseCode.NoContent,
                    FriendlyErrorMessage = _localization.Get("NoContent")

                }.ToJson();
            }

            context.Language();

            context.Response.Body = originBody;
            await context.Response.WriteAsync(json);
        }
    }
}
