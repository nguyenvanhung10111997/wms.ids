namespace wms.ids.dto
{
    public class UserRoleDto
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedUser { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
