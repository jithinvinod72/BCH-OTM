using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int EmplloyeeCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int JobId { get; set; }
        public string? Gender { get; set; }
        public string? DepartmentName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}