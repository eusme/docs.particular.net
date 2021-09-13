﻿using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region CustomRunAfterEndpointStart
public class RunAfterEndpointStart :
    IRunAfterEndpointStart
{
    static ILog log = LogManager.GetLogger<RunAfterEndpointStart>();
    public Task Run(IEndpointInstance endpoint)
    {
        log.Info("Endpoint Started.");
        return Task.CompletedTask;
    }
}
#endregion