using CodeAround.FluentBatch.Infrastructure;
using CodeAround.FluentBatch.Task.Generic;
using FluentEmail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using FluentEmail.MailKitSmtp;
using CodeAround.FluentBatch.Email.Infrastructure;
using Microsoft.Extensions.Logging;

namespace CodeAround.FluentBatch.Email.Task.Destination
{
    public class EmailDestination : WorkTask, IEmailDestination
    {
        private string _name;
        private string _mailSMTP;
        private int _mailPort;
        private string _mailUsername;
        private string _mailPassword;
        private string _mailFrom;
        private List<string> _mailTo;
        private string _subject;
        private string _template;
        private string _templateFile;
        private string _body;
        private List<AttachedFile> _attachedFile;
        private object _sourceObj;
        private bool _enableSSL;
        private bool _useLoopValueAsTemplateOjectSource;

        public EmailDestination(ILogger logger, bool useTrace)
            : base(logger, useTrace)
        {
            _enableSSL = false;
            _attachedFile = new List<AttachedFile>();
            _mailTo = new List<string>();
        }

        public override void Initialize(TaskResult taskResult)
        {
            base.Initialize(taskResult);

            if (_useLoopValueAsTemplateOjectSource && TaskResult is LoopTaskResult)
            {
                _sourceObj = ((LoopTaskResult)TaskResult).LoopValue;
            }
        }

        public IEmailDestination WithName(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                Trace(String.Format("the name is null or empty"));
                throw new ArgumentNullException("the name is null or empty");
            }

            Trace(String.Format("Set The name : {0} : ", name));
            _name = name;
            return this;
        }

        public IEmailDestination SMTP(Func<string> smtp)
        {
            var protocol = smtp();
            if (String.IsNullOrEmpty(protocol))
            {
                Trace(String.Format("the smtp is null or empty"));
                throw new ArgumentNullException("the smtp is null or empty");
            }

            Trace(String.Format("Set The smtp : {0} : ", protocol));
            _mailSMTP = protocol;
            return this;
        }

        public IEmailDestination AttachedFile(Func<string> attachedFile, Func<string> contentType)
        {
            var filename = attachedFile();
            if (String.IsNullOrEmpty(filename))
            {
                Trace(String.Format("the filename is null or empty"));
                throw new ArgumentNullException("the filename is null or empty");
            }

            var content = contentType();

            if (String.IsNullOrEmpty(content))
            {
                Trace(String.Format("Set the content type is null or empty"));
                throw new ArgumentNullException("the content type is null or empty");
            }

            Trace($"The filename : {filename} - Content Type {content}");
            _attachedFile.Add(new Infrastructure.AttachedFile() { FileName = filename, ContentType = content });
            return this;
        }

        public IEmailDestination Port(Func<int> mailPort)
        {
            Trace(String.Format("The mail Port : {0} : ", mailPort));
            _mailPort = mailPort();
            return this;
        }

        public IEmailDestination UseSSL()
        {
            Trace(String.Format("Set The use ssl: {0} : ", true));
            _enableSSL = true;
            return this;
        }

        public IEmailDestination Username(Func<string> username)
        {
            var user = username();
            if (String.IsNullOrEmpty(user))
            {
                Trace(String.Format("the username is null or empty"));
                throw new ArgumentNullException("the username is null or empty");
            }

            Trace(String.Format("Set The username : {0} : ", user));
            _mailUsername = user;
            return this;
        }

        public IEmailDestination Password(Func<string> password)
        {
            var pwd = password();
            if (String.IsNullOrEmpty(pwd))
            {
                Trace(String.Format("the password is null or empty"));
                throw new ArgumentNullException("the password is null or empty");
            }

            Trace(String.Format("Set The password : {0} : ", pwd));
            _mailPassword = pwd;
            return this;
        }

        public IEmailDestination From(Func<string> fromAddress)
        {
            var address = fromAddress();
            if (String.IsNullOrEmpty(address))
            {
                Trace(String.Format("the from Address is null or empty"));
                throw new ArgumentNullException("the from Address is null or empty");
            }

            Trace(String.Format("Set the from Address : {0} : ", address));
            _mailFrom = address;
            return this;
        }

        public IEmailDestination To(Func<string> toAddress)
        {
            var address = toAddress();
            if (String.IsNullOrEmpty(address))
            {
                Trace(String.Format("The Address is null or empty"));
                throw new ArgumentNullException("The Address is null or empty");
            }

            Trace(String.Format("The Address : {0} : ", address));
            _mailTo.Add(address);
            return this;
        }

        public IEmailDestination Body(Func<string> body)
        {
            var bod = body();
            if (String.IsNullOrEmpty(bod))
            {
                Trace(String.Format("The body is null or empty"));
                throw new ArgumentNullException("The body is null or empty");
            }

            Trace(String.Format("Set The body : {0} : ", body));
            _body = bod;
            return this;
        }

        public IEmailDestination Subject(Func<string> subject)
        {
            var sbj = subject();
            if (String.IsNullOrEmpty(sbj))
            {
                Trace(String.Format("the subject is null or empty"));
                throw new ArgumentNullException("The subject is null or empty");
            }

            Trace(String.Format("Set The subject : {0} : ", sbj));
            _subject = sbj;
            return this;
        }

        public IEmailDestination UseTemplate(Func<string> template)
        {
            var temp = template();
            if (String.IsNullOrEmpty(temp))
            {
                Trace(String.Format("The template is null or empty"));
                throw new ArgumentNullException("The template is null or empty");
            }

            Trace(String.Format("Set The template : {0} : ", temp));
            _template = temp;
            return this;
        }

        public IEmailDestination UseTemplateFile(Func<string> templateFile)
        {
            var temp = templateFile();
            if (String.IsNullOrEmpty(temp))
            {
                Trace(String.Format("The template file is null or empty"));
                throw new ArgumentNullException("The template file is null or empty");
            }

            Trace(String.Format("Set The template file : {0} : ", temp));
            _templateFile = temp;
            return this;
        }

        public IEmailDestination TemplateObjectSource(Func<object> source)
        {
            var obj = source();
            if (obj == null)
            {
                Trace(String.Format("The template source object is null or empty"));
                throw new ArgumentNullException("The template source object is null or empty");
            }
            Trace(String.Format("Set The template object source : {0} : ", obj));
            _sourceObj = obj;
            return this;
        }

        public IEmailDestination UseLoopValueAsTemplateOjectSource()
        {
            _useLoopValueAsTemplateOjectSource = true;
            return this;
        }

        public override TaskResult Execute()
        {
            var option = BuildOption();

            FluentEmail.Core.Email.DefaultSender = new MailKitSender(option);

            var email = FluentEmail.Core.Email.From(_mailFrom)
                                              .Subject(_subject);

            foreach (var to in _mailTo)
            {
                email = email.To(to);
            }

            foreach (var att in _attachedFile)
            {
                email = email.AttachFromFilename(att.FileName, att.ContentType);
            }

            if (!String.IsNullOrEmpty(_body))
            {
                email = email.Body(_body);
            }
            else if (!String.IsNullOrEmpty(_template) && _sourceObj != null)
            {
                email = email.UsingTemplateEngine(new FluentEmail.Razor.RazorRenderer())
                             .UsingTemplate(_template, _sourceObj);
            }
            else if (!String.IsNullOrEmpty(_templateFile) && _sourceObj != null)
            {
                email = email.UsingTemplateEngine(new FluentEmail.Razor.RazorRenderer())
                             .UsingTemplateFromFile(_templateFile, _sourceObj);
            }

            Trace("Email created", email);

            var sendResponse = email.Send();

            Trace("Send Response", sendResponse);

            return new TaskResult(true, sendResponse);
        }

        private SmtpClientOptions BuildOption()
        {
            SmtpClientOptions option = new SmtpClientOptions();

            option.Password = _mailPassword;
            option.Port = _mailPort;
            option.RequiresAuthentication = true;
            option.Server = _mailSMTP;
            option.User = _mailUsername;
            option.UseSsl = _enableSSL;
            option.RequiresAuthentication = true;

            return option;
        }
    }
}
