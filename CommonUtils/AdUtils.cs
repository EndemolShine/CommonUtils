using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace CommonUtilsX64
{
    public class AdUtils
    {
        public static SearchResultCollection GetAllActiveAccounts(string adentry)
        {
            var entry = new DirectoryEntry(adentry);
            var mySearcher = new DirectorySearcher(entry)
            {
                SearchScope = SearchScope.Subtree,
                Filter =
                    "(&(objectCategory=person)(ObjectClass=User)(SAMAccountName=x*)(!userAccountControl:1.2.840.113556.1.4.803:=2))",
                Sort = new SortOption("sAMAccountName", SortDirection.Ascending),
                PageSize = 1500
            };
            return (mySearcher.FindAll());
        }


        public static SearchResultCollection GetAllAccounts(string adentry)
        {
            var entry = new DirectoryEntry(adentry);
            var mySearcher = new DirectorySearcher(entry)
            {
                SearchScope = SearchScope.Subtree,
                Filter =
                    "(&(objectCategory=person)(ObjectClass=User)(SAMAccountName=x*))",
                Sort = new SortOption("sAMAccountName", SortDirection.Ascending),
                PageSize = 1500
            };
            return (mySearcher.FindAll());
        }

        public static List<string> GetAllUsersFromGroup(string ad, string adentry, string group, string account, string password)
        {
            var retVal = new List<string>();
            var ctx = new PrincipalContext(ContextType.Domain, ad, adentry, account, password);
            var grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, group);

            if (grp != null)
            {
                foreach (Principal p in grp.GetMembers(true))
                {
                    retVal.Add(p.SamAccountName);
                }
                grp.Dispose();
            }
            ctx.Dispose();
            return retVal;
        }
    }
}
