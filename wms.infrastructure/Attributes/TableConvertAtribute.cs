namespace wms.infrastructure.Attributes
{
    public class TableConvertAtribute : Attribute
    {
        public bool Ignore { get; set; } = true;


        public TableConvertAtribute(bool ignore)
        {
            Ignore = ignore;
        }
    }
}
