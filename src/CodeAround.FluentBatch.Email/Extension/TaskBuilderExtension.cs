using CodeAround.FluentBatch.Email.Task.Destination;
using CodeAround.FluentBatch.Infrastructure;
using CodeAround.FluentBatch.Interface.Builder;
using CodeAround.FluentBatch.Interface.Task;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeAround.FluentBatch.Email.Extension
{
    public static class TaskBuilderExtension
    {
        public static IEmailDestination CreateEmailDestination(this IExtensionBehaviour behaviour)
        {
            var workTask = behaviour.GetCurrentTask();
            workTask = new EmailDestination(behaviour.Logger, behaviour.UseTrace);
            return (IEmailDestination)workTask;
        }
    }
}
