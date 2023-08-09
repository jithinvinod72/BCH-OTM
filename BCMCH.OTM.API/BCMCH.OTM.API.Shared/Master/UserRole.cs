using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class UserRole
    {
        public IEnumerable<UserAndHisRole> UserDetails { get; set; }
        public IEnumerable<UserResources> UserResources { get; set; }
    }
}
