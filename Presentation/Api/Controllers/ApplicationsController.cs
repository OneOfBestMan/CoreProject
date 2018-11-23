using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace Api.Controllers
{
    /// <summary>
    /// Applications controller.
    /// </summary>
    [Route("[controller]")]
    public class ApplicationsController : Controller
    {
        /// <summary>
        /// The application service.
        /// </summary>
        readonly IApplicationService _applicationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Api.Controllers.ApplicationsController"/> class.
        /// </summary>
        /// <param name="applicationService">Application service.</param>
        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Get the specified id.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="id">İdentifier.</param>
        [HttpGet("{id}")]
        public async Task<Response<Applications>> Get(Guid id)
        {
            return await _applicationService.Get(id);
        }
    }
}