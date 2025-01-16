

namespace wms.ids.dto
{
    public class CaptchaVerificationResponse
    {
        public bool success { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }

        public string errorcodes { get; set; }

    }
}
