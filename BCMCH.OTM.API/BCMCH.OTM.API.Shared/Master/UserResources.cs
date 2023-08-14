using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class UserResources
    {
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public int? ResourceId  { get; set; }
        public string? ResourceName  { get; set; }
        public string? ResourceDescription { get; set; }
        public int AccessType  { get; set; }
    }
}

// PermissionId
// RoleId
// ResourceId
// ResourceName
// ResourceDescription
// AccessType