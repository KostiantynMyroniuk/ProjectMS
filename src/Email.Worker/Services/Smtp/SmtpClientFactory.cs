using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Worker.Services.Smtp
{
    public class SmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClientWrapper Create()
        {
            return new SmtpClientWrapper();
        }
    }
}
