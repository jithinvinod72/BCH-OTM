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
        public int otStageId {get;set;}
        public int FormQuestionTypeId {get;set;}  
        public int SubQuestionDisplayOptionId {get;set;}
        public string questionName {get;set;}
        public string question {get;set;}
        public int parentId {get;set;}
        public string? accessibleTo {get;set;}
        public string? Options {get;set;}
        public int IsRequired {get;set;}
        public string QuestionTypeName {get;set;}
        public string QuestionTypeLabel {get;set;}
        public string StageName {get;set;}
        public string StageLabel {get;set;}
        public int IsDisabled {get;set;}
    }

}
