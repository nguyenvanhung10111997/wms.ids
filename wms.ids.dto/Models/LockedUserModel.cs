using System;

namespace wms.ids.dto
{
    public class LockedUserModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public DateTime LockedDate { get; set; }
    }    
}