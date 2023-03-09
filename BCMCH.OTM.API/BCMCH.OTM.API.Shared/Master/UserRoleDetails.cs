using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class UserRoleDetails
    {
        public int OtmUserId { get; set; }
        public int EmployeeId { get; set; }
        public int UserRoleId  { get; set; }
        public string? RoleName  { get; set; }
        public string? UserName { get; set; }
        public string? FirstName  { get; set; }
        public string? LastName  { get; set; }
        public string? MiddleName  { get; set; }
        public int? DepartmentID  { get; set; }
        public int?  DepartmentTypeCode  { get; set; }
        public string? DepartmentName  { get; set; }
    }
}
