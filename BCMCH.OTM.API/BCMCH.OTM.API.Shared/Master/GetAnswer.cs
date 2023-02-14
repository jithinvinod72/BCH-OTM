using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class GetAnswer
    {
        public int Id {get;set;}
        public int eventId {get;set;}
        public int questionId {get;set;}
        public string answer {get;set;}
    }
}
