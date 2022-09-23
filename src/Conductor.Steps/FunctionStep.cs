using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Conductor.Steps
{
    public class FunctionStep : StepBody
    {
        public FunctionStep() { }

        public string FunctionAppURL { get; set; }
        public string FunctionAppKey { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }

        public int StatusCode { get; set; }

        public string OrderId { get; set; }

        public string UserName { get; set; }

        public string ResponseMessage { get; set; }

        public class AzureFunctionResponse
        {
            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("orderId")]
            public string OrderId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("statusCode")]
            public int StatusCode { get; set; }

            [JsonProperty("userName")]
            public string UserName { get; set; }
        }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var functionAppURL = FunctionAppURL;
            var requestObject = new JsonObject();
            requestObject.Add("name", Message);

            if (String.IsNullOrEmpty(OrderId))
            {
                requestObject.Add("orderId", OrderId);
            }
            var content = new StringContent(requestObject.ToString(), encoding: System.Text.Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-functions-key", FunctionAppKey);
                using (HttpResponseMessage responseMessage = client.PostAsync(functionAppURL, content).Result)
                {
                    using (HttpContent responseContent = responseMessage.Content)
                    {
                        var responseString = responseContent.ReadAsStringAsync().Result;
                        var functionResult = JsonConvert.DeserializeObject<AzureFunctionResponse>(responseString);
                        Status = functionResult.Status;
                        OrderId = functionResult.OrderId;
                        StatusCode = functionResult.StatusCode;
                        UserName = functionResult.UserName;
                        ResponseMessage = functionResult.Message;
                    }
                }
            }           
            return ExecutionResult.Next();
        }
    }
}
