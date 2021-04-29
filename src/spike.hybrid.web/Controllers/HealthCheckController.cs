using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace spike.hybrid.web.Controllers
{
    public class HealthCheckController : Controller
    {
        private readonly ILogger<HealthCheckController> logger;
        private readonly IConfiguration configuration;
        private readonly DbContext db;

        public HealthCheckController(ILogger<HealthCheckController> logger, IConfiguration configuration, DbContext db)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.db = db;
        }

        [HttpGet("api/health/db")]
        public IActionResult CheckDbConnection()
        {
            try
            {
                if (db.Database.CanConnect())
                {
                    var count = db.Database.ExecuteSqlRaw("SELECT 1");
                    return Json(new
                    {
                        Success = count > 0,
                        Message = "Successfully queried the database"
                    });
                }
            }
            catch (System.Exception ex)
            {

                return Json(new
                {
                    Success = false,
                    ex.Message
                });
            }


            return Json(new
            {
                Success = false,
                Message = "Could not connect..."
            });

        }

        [HttpGet("api/health/smtp")]
        public async Task<IActionResult> CheckSmtp()
        {
            try
            {
                var smtpConfig = configuration.GetSection("SmtpConfiguration").Get<SmtpConfiguration>();
                var email = "lackey_i@wustl.edu";
                var subject = "SMTP relay health check";
                var htmlMessage = "Email has been delivered successfully";

                this.logger.LogDebug("Sending email to: {Email} with subject {Subject}", email, subject);
                using (var mailClient = new SmtpClient(smtpConfig.Host, smtpConfig.Port))
                {
                    mailClient.EnableSsl = smtpConfig.EnableSSL;

                    if (!string.IsNullOrEmpty(smtpConfig.Username))
                        mailClient.Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password);

                    var message = new MailMessage(smtpConfig.FromAddress, email, subject, htmlMessage);
                    message.IsBodyHtml = true;
                    await mailClient.SendMailAsync(message);
                    this.logger.LogDebug("Mail sent to {Email} with subject {Subject}", email, subject);
                }

                return Json(new
                {
                    Status = "Success",
                    Message = "Test email sent"
                });
            }
            catch (System.Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    ex.Message
                });
            }
        }
    }
}