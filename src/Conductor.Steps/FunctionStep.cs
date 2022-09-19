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
    public class FunctionStep : IFunctionStep
    {
        public FunctionStep() { }

        public virtual Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var functionResult = String.Empty;
            var functionAppURL = "https://waas-test-kk.azurewebsites.net/api/HttpTrigger1?code=bHsF82gmCX6w6y9bpcaDa-N5EEfw9LGthGTl3YAoy-IoAzFu7ac_sQ==";
            var requestObject = new JsonObject();
            requestObject.Add("name", "Hello There");
            var content = new StringContent(requestObject.ToString(), encoding: System.Text.Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-functions-key", "bHsF82gmCX6w6y9bpcaDa-N5EEfw9LGthGTl3YAoy-IoAzFu7ac_sQ==");
                using (HttpResponseMessage responseMessage = client.PostAsync(functionAppURL, content).Result)
                {
                    using (HttpContent responseContent = responseMessage.Content)
                    {
                        var responseString = responseContent.ReadAsStringAsync().Result;
                        functionResult = responseString;

                    }
                }

            }
            return Task.FromResult(ExecutionResult.Next());
        }
    }
}
