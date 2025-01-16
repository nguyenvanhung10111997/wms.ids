using System.Collections;
using System.Reflection;

namespace wms.ids.business.Ultilities
{
    public static class PropertyHelper
    {
        public static FieldInfo[] GetConstantProperties(System.Type type)
        {
            ArrayList constants = new ArrayList();

            FieldInfo[] fieldInfos = type.GetFields(
                // Gets all public and static fields
                BindingFlags.Public | BindingFlags.Static |
                // This tells it to get the fields from all base types as well
                BindingFlags.FlattenHierarchy);
            // Go through the list and only pick out the constants
            foreach (FieldInfo fi in fieldInfos)
                if (fi.IsLiteral && !fi.IsInitOnly)
                    constants.Add(fi);

            return (FieldInfo[])constants.ToArray(typeof(FieldInfo));
        }
    }
}
