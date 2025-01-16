namespace wms.ids.dto
{
    public class UserChangePasswordDto
    {
        public int UserID { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
