using System;

namespace Ecommerce.Core
{
    public class Email
    {
        public Email(
            String to,
            String body)
        {
            To = to;
            Body = body;
        }

        public string To { get; }
        public string Body { get; }

        public override string ToString() => string.Join(">", To, Body);
    }
}