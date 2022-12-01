// @RC
// @OperationTheatreId
// @DoctorId
// @AnaesthetistId
// @StatusId
// @AnaesthesiaTypeId
// @SurgeryId
// @RegistrationNo
// @StartDate
// @EndDate
// @Duration
// @InstructionToNurse
// @InstructionToAnaesthetist
// @InstructionToOperationTeatrePersons
// @RequestForSpecialMeterial
// @DepartmentId
// @EmployeeIdArray
// @EquipmentsIdArray

using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace BCMCH.OTM.Data.Master
{
    public class BookingDataAccess : IBookingDataAccess
    {
    }
}