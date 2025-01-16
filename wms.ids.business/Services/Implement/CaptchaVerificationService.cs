using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using wms.ids.dto;

namespace wms.ids.business.Services.Implement
{
    public class CaptchaVerificationService
    {
        private CaptchaSettings captchaSettings;

        public string ClientKey => captchaSettings.ClientKey;

        public CaptchaVerificationService(IOptions<CaptchaSettings> captchaSettings)
        {
            this.captchaSettings = captchaSettings.Value;
        }

        public async Task<bool> IsCaptchaValid(string token)
        {
            var result = false;

            var googleVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";

            try
            {
                using var client = new HttpClient();

                var response = await client.PostAsync($"{googleVerificationUrl}?secret={captchaSettings.ServerKey}&response={token}", null);
                var jsonString = await response.Content.ReadAsStringAsync();
                var captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);

                result = captchaVerfication.success;
            }
            catch
            {
                // fail gracefully, but log
                //logger.LogError("Failed to process captcha validation", e);
            }

            return result;
        }
    }
}
