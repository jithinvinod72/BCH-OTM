using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class AvailableRoles
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string? Code  {get; set;}
        public string? Description  {get; set;}
        public int Active  {get; set;}
        public string? DisplayName  {get; set;}
    }
}
