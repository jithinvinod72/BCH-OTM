using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.API.Shared.Master;

namespace BCMCH.OTM.API.Shared.Master
{
    public class AllMasters
    {
        public DateTime DateTimeToday {get; set;}
        public IEnumerable<Equipments> EquipmentList {get; set;}
        public IEnumerable<Employee> AnaesthetistList {get; set;}
        public IEnumerable<OperationTheatre> OperationTheatreList {get; set;}
        public IEnumerable<Anaesthesia> AnaesthesiaList {get; set;}
        public IEnumerable<Departments> DepartmentsList {get; set;}
        public IEnumerable<Medicines> MedicinesList {get; set;}
        public IEnumerable<Materials> MaterialsList {get; set;}
        
    }
}
