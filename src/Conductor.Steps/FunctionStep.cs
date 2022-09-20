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

        public string Status;

        public string ResourceId;
        public class AzureFunctionResponse
        {
            [JsonProperty("resourceId")]
            public string ResourceId { get; set; }

            [JsonProperty("data")]
            public string Data { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }
        }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var functionAppURL = FunctionAppURL;
            var requestObject = new JsonObject();
            requestObject.Add("name", Message);
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
                        ResourceId = functionResult.ResourceId;
                    }
                }
            }           
            return ExecutionResult.Next();
        }
    }
}
