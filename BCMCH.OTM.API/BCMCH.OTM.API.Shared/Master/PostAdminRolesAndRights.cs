using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class PostAdminRolesAndRights
    {
        public int? RoleId  { get; set; }
        public string? UserRoleName  { get; set; }
        public string? UserDisplayName  { get; set; }
        public string? ResourceAndAccess { get; set; }
        public int? IsDeleted  { get; set; }
    }
}
