using System;
using System.Threading.Tasks;
using Model;

namespace Service
{
    /// <summary>
    /// Application service.
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// Get the specified id.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="id">İdentifier.</param>
        Task<Response<Applications>> Get(Guid id);
    }
}