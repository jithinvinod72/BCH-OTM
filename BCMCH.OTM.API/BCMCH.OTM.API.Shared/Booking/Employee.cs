using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class Employee
    {
        public int? EmployeeId {get;set; }
        public int? BookingId {get; set;} 
        public string? EmployeeFirstName {get;set; }
        public string? EmployeeMiddleName {get;set; }
        public string? EmployeeLastName {get;set; }
        public int? EmployeeDepartmentID {get;set; }
        public int? EmployeeCategoryId {get;set; }
        public string? EmployeeDepartmentName {get;set; }
    }
}