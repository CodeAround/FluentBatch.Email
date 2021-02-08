using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeAround.FluentBatch.Email;
using CodeAround.FluentBatch.Email.Extension;
using CodeAround.FluentBatch.Engine;
using FluentEmail.Core.Models;
using RazorLight.Compilation;
using CodeAround.FluentBatch.Email.Test.Infrastructure;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CodeAround.FluentBatch.Email.Test.TaskTest
{
    public class EmailDestinationTaskTest
    {
        private Microsoft.Extensions.Logging.ILogger _logger;

        string SMTP = "<REPLACE>";
        int port = 465;
        string username = "<REPLACE>";
        string password = "<REPLACE>";
        string to = "<REPLACE>";
        string from = "<REPLACE>";
        string subject = "Email Test";


        public EmailDestinationTaskTest()
        {
            NLog.LogManager.LoadConfiguration("NLog.config");
            var factory = new LoggerFactory().AddNLog();
            _logger = factory.CreateLogger<EmailDestinationTaskTest>();
        }

        [Fact]
        public void Send_email_success_with_body(Action<bool> testResult)
        {
            FlowBuilder builder = new FlowBuilder(_logger);
            var flow = builder.Create("Email Flow")
                              .Then(task => task.CreateEmailDestination()
                                                .WithName("EmailTask")
                                                .SMTP(() => SMTP)
                                                .Port(() => port)
                                                .Username(() => username)
                                                .Password(() => password)
                                                .UseSSL()
                                                .Subject(() => subject)
                                                .To(() => to)
                                                .From(() => from)
                                                .Body(() => "Test email")
                                                .Build())
                              .Build();

            flow.ProcessedTask += (s, e) =>
            {
                testResult(((SendResponse)e.CurrentTaskResult.Result).Successful);
            };

            flow.FaultTask += (s, e) =>
            {
                testResult(e == null);
            };

            flow.Run();

        }

        [Fact]
        public void Send_email_success_with_template(Action<bool> testResult)
        {

            FlowBuilder builder = new FlowBuilder(_logger);
            var flow = builder.Create("Email Flow")
                              .Then(task => task.CreateEmailDestination()
                                                .WithName("EmailTask")
                                                .SMTP(() => SMTP)
                                                .Port(() => port)
                                                .Username(() => username)
                                                .Password(() => password)
                                                .UseSSL()
                                                .Subject(() => subject)
                                                .To(() => to)
                                                .From(() => from)
                                                .UseTemplate(() => "Dear @Model.Name, You are totally @Model.Compliment.")
                                                .TemplateObjectSource(() => new Person { Name = "Luke", Compliment = "Awesome" })
                                                .Build())
                              .Build();

            flow.ProcessedTask += (s, e) =>
            {
                testResult(((SendResponse)e.CurrentTaskResult.Result).Successful);
            };

            flow.FaultTask += (s, e) =>
            {
                testResult(e == null);
            };

            flow.Run();
        }

        [Fact]
        public void Send_email_success_with_template_file(Action<bool> testResult)
        {
            string assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string filePath = Path.Combine(assemblyPath.Replace("bin\\Debug\\netcoreapp2.2", string.Empty), @"Infrastructure\email_template.cshtml");

            FlowBuilder builder = new FlowBuilder(_logger);
            var flow = builder.Create("Email Flow")
                              .Then(task => task.CreateEmailDestination()
                                                .WithName("EmailTask")
                                                .SMTP(() => SMTP)
                                                .Port(() => port)
                                                .Username(() => username)
                                                .Password(() => password)
                                                .UseSSL()
                                                .Subject(() => subject)
                                                .To(() => to)
                                                .From(() => from)
                                                .UseTemplateFile(() => filePath)
                                                .TemplateObjectSource(() => new Person { Name = "Luke", Compliment = "Awesome" })
                                                .Build())
                              .Build();

            flow.ProcessedTask += (s, e) =>
            {
                testResult(((SendResponse)e.CurrentTaskResult.Result).Successful);
            };

            flow.FaultTask += (s, e) =>
            {
                testResult(e == null);
            };

            flow.Run();
        }

        [Fact]
        public void Send_email_success_with_body_multiple_mailto(Action<bool> testResult)
        {
            FlowBuilder builder = new FlowBuilder(_logger);
            var flow = builder.Create("Email Flow")
                              .Then(task => task.CreateEmailDestination()
                                                .WithName("EmailTask")
                                                .SMTP(() => SMTP)
                                                .Port(() => port)
                                                .Username(() => username)
                                                .Password(() => password)
                                                .UseSSL()
                                                .Subject(() => subject)
                                                .To(() => to)
                                                .To(() => "arancino81@yahoo.com")
                                                .From(() => from)
                                                .Body(() => "Test email")
                                                .Build())
                              .Build();

            flow.ProcessedTask += (s, e) =>
            {
                testResult(((SendResponse)e.CurrentTaskResult.Result).Successful);
            };

            flow.FaultTask += (s, e) =>
            {
                testResult(e == null);
            };

            flow.Run();

        }

        [Fact]
        public void Send_email_success_with_body_and_attach(Action<bool> testResult)
        {
            string assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string filePath = Path.Combine(assemblyPath.Replace("bin\\Debug\\netcoreapp2.2", string.Empty), @"Infrastructure\attac.txt");

            FlowBuilder builder = new FlowBuilder(_logger);
            var flow = builder.Create("Email Flow")
                              .Then(task => task.CreateEmailDestination()
                                                .WithName("EmailTask")
                                                .SMTP(() => SMTP)
                                                .Port(() => port)
                                                .Username(() => username)
                                                .Password(() => password)
                                                .UseSSL()
                                                .Subject(() => subject)
                                                .To(() => to)
                                                .From(() => from)
                                                .Body(() => "Test email")
                                                .AttachedFile(() => filePath, () => "text/plain")
                                                .Build())
                              .Build();

            flow.ProcessedTask += (s, e) =>
            {
                testResult(((SendResponse)e.CurrentTaskResult.Result).Successful);
            };

            flow.FaultTask += (s, e) =>
            {
                testResult(e == null);
            };

            flow.Run();

        }

        [Fact]
        public void Send_email_success_with_template_and_loop(Action<bool> testResult)
        {
            List<Person> list = new List<Person>();
            list.Add(new Person { Name = "Luke", Compliment = "Awesome" });
            list.Add(new Person { Name = "George", Compliment = "Best" });
            list.Add(new Person { Name = "Paul", Compliment = "Blend" });

            FlowBuilder builder = new FlowBuilder(_logger);
            var flow = builder.Create("Email Flow")
                              .Then(task => task.CreateLoop<Person>()
                                                .WithName("Loop Task")
                                                .AddLoop(list)
                                                .ProcessedTaskEvent((s,e) => {
                                                    testResult(((SendResponse)e.CurrentTaskResult.Result).Successful);
                                                })
                                                .Append(x => x.CreateEmailDestination()
                                                                .WithName("EmailTask")
                                                                .SMTP(() => SMTP)
                                                                .Port(() => port)
                                                                .Username(() => username)
                                                                .Password(() => password)
                                                                .UseSSL()
                                                                .Subject(() => subject)
                                                                .To(() => to)
                                                                .From(() => from)
                                                                .UseLoopValueAsTemplateOjectSource()
                                                                .UseTemplate(() => "Dear @Model.Name, You are totally @Model.Compliment.")
                                                                .Build())
                                                .Fault((e) => {
                                                    testResult(e == null);
                                                })
                                                .Build())
                              .Build();

    
            flow.Run();
        }
    }
}
