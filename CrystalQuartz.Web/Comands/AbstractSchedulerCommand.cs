﻿namespace CrystalQuartz.Web.Comands
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using CrystalQuartz.Core;
    using CrystalQuartz.Core.SchedulerProviders;
    using CrystalQuartz.Web.Comands.Outputs;
    using CrystalQuartz.WebFramework.Commands;
    using Quartz;
    using System.Collections.Generic;

    public abstract class AbstractSchedulerCommand<TInput, TOutput> : AbstractCommand<TInput, TOutput> 
        where TOutput : CommandResultWithErrorDetails, new()
    {
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly ISchedulerDataProvider _schedulerDataProvider;

        protected AbstractSchedulerCommand(ISchedulerProvider schedulerProvider, ISchedulerDataProvider schedulerDataProvider)
        {
            _schedulerProvider = schedulerProvider;
            _schedulerDataProvider = schedulerDataProvider;
        }

        protected List<IScheduler> Scheduler
        {
            get { return _schedulerProvider.Scheduler; }
        }

        protected ISchedulerDataProvider SchedulerDataProvider
        {
            get { return _schedulerDataProvider; }
        }

        protected override void HandleError(Exception exception, TInput input, TOutput output)
        {
            var schedulerProviderException = exception as SchedulerProviderException;
            if (schedulerProviderException != null)
            {
                NameValueCollection properties = schedulerProviderException.SchedulerInitialProperties;

                output.ErrorDetails = properties
                    .AllKeys
                    .Select(key => new Property(key, properties.Get(key)))
                    .ToArray();
            }
        }
    }
}