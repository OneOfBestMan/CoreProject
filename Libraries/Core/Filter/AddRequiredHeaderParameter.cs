using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.Filter
{
    /// <summary>
    /// Add required header parameter.
    /// </summary>
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        /// <summary>
        /// Apply the specified operation and context.
        /// </summary>
        /// <returns>The apply.</returns>
        /// <param name="operation">Operation.</param>
        /// <param name="context">Context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            foreach (var item in context.ApiDescription.ActionDescriptor.FilterDescriptors)
            {
                if (item.Filter.ToString().Contains("Filter.Authentication"))
                {
                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Description = "JWT Token",
                        Name = "Authorization",
                        In = "header",
                        Type = "string",
                        Required = ((Authentication)item.Filter)._warrant != Model.AuthenticationType.Anonymous
                    });
                }
            }

            operation.Parameters.Add(new NonBodyParameter
            {
                Description = "Country Code",
                Name = "CF-IPCountry",
                In = "header",
                Type = "string",
                Required = false
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Description = "Language Code",
                Name = "CF-IPLanguage",
                In = "header",
                Type = "string",
                Required = false
            });
        }
    }
}