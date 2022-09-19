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
        public string Id => throw new NotImplementedException();

        public int Version => throw new NotImplementedException();

        public void Build(IWorkflowBuilder<object> builder)
        {
            
        }
    }
}
