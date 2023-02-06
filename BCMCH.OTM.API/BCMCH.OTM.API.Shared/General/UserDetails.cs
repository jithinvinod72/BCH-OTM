using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.General
{
    public class UserDetails
    {
        public bool success { get; set; }
        public Data data { get; set; }
        public string message { get; set; }
    }
    public class Data
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmployeeId { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
    }
}
