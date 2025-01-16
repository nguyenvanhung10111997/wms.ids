using Dapper;

namespace wms.dto.Common
{
    public abstract class BaseDTO
    {
        public DynamicParameters ToDynamicParameters()
        {
            var parameter = new DynamicParameters();
            foreach (var prop in this.GetType().GetProperties())
            {
                parameter.Add($"@{prop.Name}", prop.GetValue(this));
            }
            return parameter;
        }

        public DynamicParameters ToDynamicParameters(List<string> ignores)
        {
            var parameter = new DynamicParameters();
            foreach (var prop in this.GetType().GetProperties())
            {
                if (ignores.Contains(prop.Name))
                {
                    continue;
                }
                parameter.Add($"@{prop.Name}", prop.GetValue(this));
            }
            return parameter;
        }
    }
}
