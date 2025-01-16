namespace wms.ids.dto
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public bool IsAdmin { get; set; }
        public int Gender { get; set; }
        public int? UserTypeID { get; set; }
        public DateTime? LastChangedPassword { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ActiveDate { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedDate { get; set; }
        public bool IsInit { get; set; }
        public bool IsRequireVerify { get; set; }
        public bool Enable2FA { get; set; }
        public string Secret2FAKey { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedUser { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
