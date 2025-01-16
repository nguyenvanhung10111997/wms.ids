namespace wms.ids.dto
{
    public class ClientScopeDto
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int ScopeID { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
