using Microsoft.Extensions.Caching.Memory;
using Quartz;
using ScrumEstimationServerApp.Helper;

namespace ScrumEstimationServerApp.Jobs
{
    [DisallowConcurrentExecution]
    public class SyncJob : IJob
    {
        private IMemoryCache memoryCache;
        //IConfiguration configuration;
        string Folder;
        public SyncJob(IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.memoryCache = memoryCache;
            Folder = configuration["FileFolder"];
            //this.configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {   
                //var lstFile = Directory.GetDirectories(Folder, ".json", SearchOption.AllDirectories);
                //for(var i = 0;i < lstFile.Length; i++)
                //{
                //    var file = new FileInfo(lstFile[i]);
                //    var fileName = file.Name.Replace(".pdf", "");
                //    var cacheValue = memoryCache.Get(fileName);
                //    if(cacheValue != null)
                //    {
                //        if (file.LastWriteTime < DateTime.Now.AddDays(1))
                //        {
                //            memoryCache.Remove(fileName);
                //        }
                //        else
                //        {
                //            string json = Newtonsoft.Json.JsonConvert.SerializeObject(cacheValue);
                //            FileHelper.WriteFile(json, file.FullName);
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
