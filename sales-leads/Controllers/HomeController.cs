using Microsoft.AspNetCore.Mvc;
using sales_leads.Models;
using Vonage;
using Vonage.Request;

namespace sales_leads.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Lead lead)
        {
            string name = lead.Name;
            string phone = lead.Phone;
            string message = lead.Message;

            var credentials = Credentials.FromApiKeyAndSecret(
            Domain.Credentials.APIKey,
            Domain.Credentials.APISecret
            );

            var VonageClient = new VonageClient(credentials);

            var response = VonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
            {
                To = "14252246702",
                From = "18335787280",
                Text = $"New lead acquired!\n\nName: {name}\nPhone: {phone}\nMessage: {message}"
            });

            if (response != null && Convert.ToInt32(response.MessageCount) > 0 && response.Messages[0].StatusCode.ToString() == "Success")
            {
                lead.Result = "Message sent successfully! An agent will contact you shortly.";
            }
            else
            {
                lead.Result = "Message Failure. Please try your request again. ";
            }
            
            return View(lead);
        }
    }
}