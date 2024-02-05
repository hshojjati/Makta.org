using Microsoft.Extensions.Caching.Memory;

namespace Makta.Utility
{
    public class ApplicationCache
    {
        public MemoryCache Cache { get; set; }
        public ApplicationCache()
        {
            Cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 1024,
            });
        }
    }
}
