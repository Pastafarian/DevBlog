using System.Threading;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DevBlog.Application.Services
{
    public class SendGridService : ISendGridService
    {
        private readonly ILogger<SendGridService> _logger;
        private readonly ISendGridClient _client;
        internal string LogInfoMessage = "SendEmailAsync called with name:{Name}, email:{Email}, message:{Message}";

        public SendGridService(ILogger<SendGridService> logger, ISendGridClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<bool> SendEmailAsync(string name, string email, string message, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LogInfoMessage, name, email, message);

            cancellationToken.ThrowIfCancellationRequested();

            var from = new EmailAddress("steve@stephenadam.dev", name);
         
            var to = new EmailAddress("stephen.adam@gmail.com", "Steve");
            var msg = MailHelper.CreateSingleEmail(from, to, "Email from site", $"{email} {message}", $"{email} {message}");
            var response = await _client.SendEmailAsync(msg, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("SendEmailAsync received success status code from SendGrid");
                return true;
            }

            var httpResponseBody = await response.Body.ReadAsStringAsync(cancellationToken);
            _logger.LogError("SendEmailAsync received error code from SendGrid. StatusCode:{StatusCode}, HttpResponseBody:{HttpResponseBody}, Headers: {Headers}", response.StatusCode, httpResponseBody, response.Headers.ToString());

            return false;
        }
    }
}
