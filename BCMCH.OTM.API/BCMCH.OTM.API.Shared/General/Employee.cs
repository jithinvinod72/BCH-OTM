using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.General
{
    public class Employee
    {
        public int? EmployeeId {get; set;}
        public int? DepartmentID {get; set;}
        public string? EmployeeCode {get; set;}
        public string? Title {get; set;}
        public string? FirstName {get; set;}
        public string? LastName {get; set;}
        public string? MiddleName {get; set;}
        public int? EmployeeCategoryId {get; set;}
        public int? CategoryId {get; set;}
        public string? CategoryName {get; set;}
        public string? departmentTypeCode {get; set;}
        public string? departmentName {get; set;}
    }
}
