namespace AbiWebsite.Models;

public class HostOptions
{
    public string? Url { get; set; }

    public string? HostMail { get; set; }

    public HostMailCredentials? MailCredentials { get; set; }


    public class HostMailCredentials
    {
        public string? Host { get; set; }

        public int Port { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}