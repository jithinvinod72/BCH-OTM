using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class GetQuestions
    {
        public int questionId {get;set;}
        public int FormsectionId {get;set;}
        public int FormQuestionTypeId {get;set;} //        
        public int order {get;set;}
        public string questionName {get;set;}
        public string question {get;set;}
        public int parentId {get;set;}
        public string rolesToShow {get;set;}
        public string Options {get;set;}
        public int IsRequired {get;set;}
        public string QuestionTypeName {get;set;}
        public string FormSectionName {get;set;}
    }

}