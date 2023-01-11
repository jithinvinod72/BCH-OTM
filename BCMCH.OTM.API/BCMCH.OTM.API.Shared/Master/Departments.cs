using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class Departments
    {
        public int Id { get; set; }
        public int? Code {get;set;}
        public int? DivisionId {get;set;}
        public int? TypeCode {get;set;}
        public string? Name { get; set; }

    }
}
