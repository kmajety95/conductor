using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Conductor.Steps
{
    public class FunctionStep : StepBodyAsync
    {
        //Input values
        public string AzureFunctionAppName { get; set; }
        public string AzureFunctionName { get; set; }
        public string Token { get; set; }
        public IDictionary<string, object> Headers { get; set; }
        public IDictionary<string, object> Parameters { get; set; }
        public ExpandoObject Body { get; set; }

        public DataFormat Format { get; set; } = DataFormat.Json;
        public Method Method { get; set; } = Method.GET;

        //Internal Properties.
        private string AzureFunctionBaseURL = @"http://{0}.azurewebsites.net";
        private string AzureFunctionURL = @"/api/{1}?code={1}";

        //output values
        public string ErrorMessage { get; set; }
        public bool IsSuccessful { get; set; }
        public int ResponseCode { get; set; }
        public dynamic ResponseBody { get; set; }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            if (string.IsNullOrEmpty(AzureFunctionAppName) || string.IsNullOrEmpty(AzureFunctionName))
            {
                throw new Exception("Please define Azure Function App and Function App Name.");
            }

            var client = new RestClient(string.Format(AzureFunctionBaseURL, AzureFunctionAppName));
            var request = new RestRequest(string.Format(AzureFunctionURL, AzureFunctionName, Token), Method, Format);

            if (Headers != null)
            {
                foreach (var header in Headers)
                    request.AddHeader(header.Key, Convert.ToString(header.Value));
            }

            if (Parameters != null)
            {
                foreach (var param in Parameters)
                    request.AddQueryParameter(param.Key, Convert.ToString(param.Value));
            }

            if (Body != null)
            {
                switch (Format)
                {
                    case DataFormat.Json:
                        request.AddJsonBody(Body);
                        break;
                    case DataFormat.Xml:
                        request.AddXmlBody(Body);
                        break;

                }
            }

            var response = await client.ExecuteAsync<dynamic>(request);
            IsSuccessful = response.IsSuccessful;

            if (response.IsSuccessful)
            {
                ResponseCode = (int)response.StatusCode;
                ResponseBody = response.Data;
            }
            else
            {
                ErrorMessage = response.ErrorMessage;
            }

            return ExecutionResult.Next();
        }
    }
}
