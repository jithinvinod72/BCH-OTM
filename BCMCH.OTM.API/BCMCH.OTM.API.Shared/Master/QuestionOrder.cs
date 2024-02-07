using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class QuestionOrder
    {
        public int idOfSelected {get;set;}
        public int orderNumberOfSelected {get;set;}
        public int idToExchange {get;set;}  
        public int orderNumberOfExchange {get;set;}
        public int stageId {get;set;}
        
    }

}

// int idOfSelected, int orderNumberOfSelected, int idToExchange, int orderNumberOfExchange