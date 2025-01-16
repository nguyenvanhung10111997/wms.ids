using System.Security.Claims;

namespace wms.infrastructure.Models
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public int? UserTypeID { get; set; }

        public int SupplierID { get; set; }

        public string Fullname { get; set; }

        public int DepartmentID { get; set; }

        public int PositionID { get; set; }

        public int StoreID { get; set; }

        public IEnumerable<int> RoleIDs { get; set; }

        public UserPrincipal()
        {
        }

        public UserPrincipal(List<Claim> claims)
        {
            UserID = (claims.Any((Claim x) => x.Type == "/UserID") ? int.Parse(claims.FirstOrDefault((Claim p) => p.Type == "/UserID")!.Value) : 0);
            Username = (claims.Any((Claim x) => x.Type == "/Username") ? claims.FirstOrDefault((Claim p) => p.Type == "/Username")!.Value : string.Empty);
            Fullname = (claims.Any((Claim x) => x.Type == "/Username") ? claims.FirstOrDefault((Claim p) => p.Type == "/Fullname")!.Value : string.Empty);
            PositionID = (claims.Any((Claim x) => x.Type == "/PositionID") ? int.Parse(claims.FirstOrDefault((Claim p) => p.Type == "/PositionID")!.Value) : 0);
            DepartmentID = (claims.Any((Claim x) => x.Type == "/DepartmentID") ? int.Parse(claims.FirstOrDefault((Claim p) => p.Type == "/DepartmentID")!.Value) : 0);
            StoreID = (claims.Any((Claim x) => x.Type == "/StoreID") ? int.Parse(claims.FirstOrDefault((Claim p) => p.Type == "/StoreID")!.Value) : 0);
            SupplierID = (claims.Any((Claim x) => x.Type == "/SupplierID") ? int.Parse(claims.FirstOrDefault((Claim p) => p.Type == "/SupplierID")!.Value) : 0);
            Claim claim = (claims.Any((Claim x) => x.Type == "/RoleIDs") ? claims.FirstOrDefault((Claim p) => p.Type == "/RoleIDs") : null);
            if (claim != null)
            {
                RoleIDs = from i in claim.Value.Replace("[", "").Replace("]", "").Split(',')
                          select Convert.ToInt32(i);
            }
        }

        public bool IsPermission(string roleFunctionName)
        {
            if (string.IsNullOrWhiteSpace(roleFunctionName) || RoleIDs == null || !RoleIDs.Any())
            {
                return false;
            }

            return true;
            //return AppPermission.Data.Where((Permission r) => RoleIDs.Contains(r.RoleID) && r.RoleFunctionName == roleFunctionName).Any();
        }
    }
}
