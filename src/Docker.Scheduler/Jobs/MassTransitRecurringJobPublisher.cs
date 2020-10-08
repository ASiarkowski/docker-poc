﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.Core.Jobs;
using MassTransit;
using Quartz;

namespace Docker.Scheduler.Jobs
{
    public class MassTransitRecurringJobPublisher : IJob
    {
        public const string JobDataType = "Type";
        public const string JobDataMessage = "Message";
        private readonly IBus _bus;

        public MassTransitRecurringJobPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var type = (RecurringJobType)dataMap.GetInt(JobDataType);

            var job = new RecurringJob()
            {
                Message = dataMap.GetString(JobDataMessage) ?? type.ToString(),
                Type = type,
            };

            await _bus.Publish(job, context.CancellationToken);
        }
    }
}
