using System;
namespace Main.Services
{
    public class AuthMessageSenderOptions
    {
        public AuthMessageSenderOptions(){}
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
