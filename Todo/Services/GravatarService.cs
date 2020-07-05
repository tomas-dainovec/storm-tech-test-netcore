using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Models.Gravatar;

namespace Todo.Services
{
    public class GravatarService : IGravatarService
    {
        private const int CacheDurationHours = 1;

        private IGravatarClient _gravatarClient;

        private IMemoryCache _profileCache;

        public GravatarService(IGravatarClient gravatarClient, IMemoryCache distributedCache)
        {
            _gravatarClient = gravatarClient;

            _profileCache = distributedCache;
        }

        public async Task<GravatarProfile> GetGravatarProfile(string email)
        {
            GravatarProfile result;

            var emailHash = Gravatar.GetHash(email);

            if (!_profileCache.TryGetValue(emailHash, out result))
            {
                result = await _gravatarClient.GetGravatarProfile(emailHash);

                _profileCache.Set(emailHash, result, new MemoryCacheEntryOptions()
                { 
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CacheDurationHours),
                });
            }

            return result;
        }
    }
}
