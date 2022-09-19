using Conductor.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace WorklfowTest
{
    public class IWorkflowRegistration : IWorkflow
    {
        public string Id => "test-workflow";

        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder.StartWith<SecondStep>();
        }
    }
}
