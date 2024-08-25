using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Messaging.ServiceBus;

namespace pluralsightfuncs
{
    public class OnPaymentReceived
    {
        private readonly ServiceBusClient _serviceBusClient;

        public OnPaymentReceived(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        [FunctionName("pluralsightfuncs1234")]
        public async Task<IActionResult> Run(

            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Received a payment");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var order = JsonConvert.DeserializeObject<Order>(requestBody);

            log.LogInformation($"Order {order.OrderId} received from {order.Email} for product {order.ProductId}");

            ServiceBusSender sender = _serviceBusClient.CreateSender("onpaymentreceived");
            ServiceBusMessage message = new ServiceBusMessage(order.ToString());
            await sender.SendMessageAsync(message);

            return new OkObjectResult("Thank you for your purchase");
        }

        public class Order
        {
            public string OrderId { get; set; }
            public string ProductId { get; set; }
            public string Email { get; set; }
            public decimal Price { get; set; }
        }
    }
}
