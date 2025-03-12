using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;


namespace PartyInvitationManager.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly string _baseUrl;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;


            var request = httpContextAccessor.HttpContext?.Request;
            _baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : configuration["BaseUrl"] ?? "https://localhost:7001";
        }

        // Basic method for sending any email
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Party Manager", "testassign1804@gmail.com"));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = message };

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync("testassign1804@gmail.com", "ovxj piqg ladd eypd");
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
                _logger.LogInformation($"Email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {toEmail}");
                throw;
            }
        }

        public async Task<bool> SendInvitationEmailAsync(string email, string name, string partyDescription,
            DateTime partyDate, string location, int invitationId)
        {
            try
            {
                var responseUrl = $"{_baseUrl}/response/respond/{invitationId}";

                var body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                        <h2 style='color: #333;'>Party Invitation</h2>
                        <p>Hello {name},</p>
                        <p>You're invited to <strong>{partyDescription}</strong>!</p>
                        <p><strong>Date:</strong> {partyDate:f}</p>
                        <p><strong>Location:</strong> {location}</p>
                        <p style='margin: 25px 0;'>
                            <a href='{responseUrl}' style='background-color: #4CAF50; color: white; padding: 10px 15px; text-decoration: none; border-radius: 4px; display: inline-block;'>
                                Respond to Invitation
                            </a>
                        </p>
                        <p>We hope to see you there!</p>
                    </body>
                    </html>";

                await SendEmailAsync(
                    email,
                    $"Invitation to {partyDescription}",
                    body);

                _logger.LogInformation($"Invitation email sent to {email} for invitation ID {invitationId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send invitation email to {email}");
                return false;
            }
        }
    }
}