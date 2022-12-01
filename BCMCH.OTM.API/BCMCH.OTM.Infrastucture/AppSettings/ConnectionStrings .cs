using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.Infrastucture.AppSettings
{
    public class ConnectionStrings : IConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
}
