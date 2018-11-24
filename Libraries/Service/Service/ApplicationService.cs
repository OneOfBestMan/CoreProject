using System;
using System.Threading.Tasks;
using Core.ConfigReader;
using Core.Helper;
using Core.Localization;
using Data;
using Microsoft.Extensions.Caching.Distributed;
using Model;

namespace Service
{
    /// <summary>
    /// Application service.
    /// </summary>
    public class ApplicationService : IApplicationService
    {
        /// <summary>
        /// Cache
        /// </summary>
        readonly IDistributedCache _cache;

        /// <summary>
        /// The config.
        /// </summary>
        readonly IConfig _config;

        /// <summary>
        /// The localization.
        /// </summary>
        readonly ILocalization _localization;

        /// <summary>
        /// The applications repository.
        /// </summary>
        readonly IRepository<Applications> _applicationsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Service.ApplicationService"/> class.
        /// </summary>
        /// <param name="cache">Cache.</param>
        /// <param name="config">Config.</param>
        /// <param name="localization">Localization.</param>
        /// <param name="applicationsRepository">Applications repository.</param>
        public ApplicationService(IDistributedCache cache, IConfig config, ILocalization localization, IRepository<Applications> applicationsRepository)
        {
            _cache = cache;
            _config = config;
            _localization = localization;
            _applicationsRepository = applicationsRepository;
        }

        /// <summary>
        /// Get the specified id.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="id">İdentifier.</param>
        public async Task<Response<Applications>> Get(Guid id)
        {
            var entity = _cache.Get($"applications:{id}").ToObject<Applications>();
            if (entity == null)
            {
                entity = await _applicationsRepository.Get(id);
                if (entity != null)
                {
                    _cache.Set($"applications:{id}", entity.ToByteArray());
                }
            }
            return new Response<Applications> { Data = entity };
        }
    }
}