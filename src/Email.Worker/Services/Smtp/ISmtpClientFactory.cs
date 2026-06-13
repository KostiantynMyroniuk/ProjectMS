using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Worker.Services.Smtp
{
    public interface ISmtpClientFactory
    {
        ISmtpClientWrapper Create();
    }
}
