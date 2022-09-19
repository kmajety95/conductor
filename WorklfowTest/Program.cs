﻿using Microsoft.Extensions.DependencyInjection;
using WorkflowCore.Interface;

public class Program
{
    public static void main(String[] args)
    {
        IServiceProvider serviceProvider = ConfigureServices();
        var host = serviceProvider.GetService<IWorkflowHost>();
        host.RegisterWorkflow<IWorkflowRegistration>();
        host.Start();
        host.StartWorkflow("IWorkflowRegistration", 1, null);
        Console.ReadLine();
        host.Stop();
    }

    private static IServiceProvider ConfigureServices()
    {
        //setup dependency injection
        IServiceCollection services = new ServiceCollection();
        services.AddWorkflow();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}