using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.General
{
    public class Authentication
    {
        public bool Authenticated { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string key { get; set; }
    }
}
