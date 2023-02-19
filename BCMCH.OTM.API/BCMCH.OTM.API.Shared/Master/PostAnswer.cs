using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class PostAnswer
    {
        public int eventId {get;set;}
        public string answersJsonString {get;set;}
        public string questionIdArray {get;set;}
    }
}
