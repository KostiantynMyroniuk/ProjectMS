using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Worker.Models
{
    public class EmailOptions
    {
        public required string Host { get; init; }
        public required int Port { get; init; }
        public required string FromEmail { get; init; }
        public required string Password { get; init; }
    }
}
