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
    public interface IFunctionStep : IStepBody
    { 
        Task<ExecutionResult> RunAsync(IStepExecutionContext context);
    }
}
