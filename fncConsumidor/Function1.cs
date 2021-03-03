using fncConsumidor.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace fncConsumidor
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task RunAsync(
            [ServiceBusTrigger("cola1", Connection = "MyConn")]string myQueueItem,
            [CosmosDB(databaseName:"dbUbicua",collectionName:"Eventos",ConnectionStringSetting ="strCosmos")]IAsyncCollector<object>datos,
            ILogger log)
        {
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                var data = JsonConvert.DeserializeObject<Data>(myQueueItem);
                await datos.AddAsync(data);
            }
            catch (Exception e)
            {
                log.LogError($"No fue posible insertar datos: {e.Message}");
            }
        }
    }
}
