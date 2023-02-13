using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class PostQuestionsModel
    {
        public int FormsectionId {get;set;}
        public int FormQuestionType {get;set;}
        public int order {get;set;}
        public string name {get;set;}
        public string question {get;set;}
        public int parentId {get;set;}
        public string rolesToShow {get;set;}
        public int questionTypeId {get;set;}
        public string Options {get;set;}
    }
}

// FormsectionId
// FormQuestionType
// order
// name
// question
// parentId
// rolesToShow
// questionTypeId
// Options