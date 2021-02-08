using CodeAround.FluentBatch.Interface.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeAround.FluentBatch.Email.Task.Destination
{
    public interface IEmailDestination: IFault
    {
        IEmailDestination WithName(string name);

        IEmailDestination SMTP(Func<string> smtp);

        IEmailDestination Port(Func<int> mailPort);

        IEmailDestination UseSSL();

        IEmailDestination Username(Func<string> username);

        IEmailDestination Password(Func<string> password);

        IEmailDestination From(Func<string> fromAddress);

        IEmailDestination To(Func<string> toAddress);

        IEmailDestination AttachedFile(Func<string> attachedFile, Func<string> contentType);

        IEmailDestination Body(Func<string> body);

        IEmailDestination Subject(Func<string> subject);

        IEmailDestination UseTemplate(Func<string> template);

        IEmailDestination TemplateObjectSource(Func<object> source);

        IEmailDestination UseTemplateFile(Func<string> templateFile);

        IEmailDestination UseLoopValueAsTemplateOjectSource();
    }
}
