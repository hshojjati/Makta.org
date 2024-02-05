using Services.Email;
using System.Text.Json;

namespace Makta.api
{
    public class ApiService : IApiService
    {
        private readonly JsonSerializerOptions JSON_options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
        private readonly IEmailSender _emailSender;

        public ApiService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
    }
}
