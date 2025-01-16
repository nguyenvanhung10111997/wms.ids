namespace wms.ids.dto
{
    public class ScopeDto
    {
        public int ScopeID { get; set; }
        public string ScopeName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ScopeType { get; set; }
        public bool Enable { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
