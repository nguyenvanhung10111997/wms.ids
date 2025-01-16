namespace wms.ids.dto
{
    public class ChangePasswordModel
    {
        public string UserName { get; set; }
        public string OldPass { get; set; }
        public string NewPass { get; set; }
    }
}