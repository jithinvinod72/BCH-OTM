using System.Runtime.Intrinsics.X86;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Domain.Contract.Booking;
using System.Runtime.CompilerServices;
using System.Data.Common;

namespace BCMCH.OTM.Domain.Booking
{
    public class BookingDomainService : IBookingDomainService
    {
        #region PRIVATE
        private readonly IBookingDataAccess _bookingDataAccess;
        #endregion

        #region CONSTRUCTOR
        public BookingDomainService(IBookingDataAccess bookingDataAccess)
        {
            _bookingDataAccess = bookingDataAccess;
        }
        #endregion



        #region PUBLIC
        public async Task<IEnumerable<BookingResponse>> GetBookingList(int _operationTheatreId, string? _fromDate,string? _toDate)
        {
            var result = await _bookingDataAccess.GetBookingList(_operationTheatreId, _fromDate, _toDate);
            
            List<Equipment> _equipmentsList = new List<Equipment>(); //used to store the equipments id ;
             // to store equipments mapping
            List<Employee> _employeesList  = new List<Employee>(); // used to store the  employee id ;
            
            // to store employees mapping

            var _uniqueBookingIds = result.Select(o => o.BookingId).Distinct();
            //  above line selects the unique booking id from the data rows dumped by SP
            //  _uniqueBookingIds kind of acts like an array



            List<Equipment> _equipmentsListRows = new List<Equipment>(); 
            //used to store the datas of equipmentid 
            List<Employee> _employeesListRows  = new List<Employee>(); 
            // used to store the datas of employeeid


            //  Now we loop through each unique ids
            foreach (var _uniqueBookingId in _uniqueBookingIds)
            {   
                
                var _datasOfCurrentUniqueId = result.Where(o => o.BookingId == _uniqueBookingId );
                // Now we select rows with the current _uniqueBookingId
                
                // Now we loop through each rows with the current uniqueId
                foreach (var _filteredItem in _datasOfCurrentUniqueId)
                {     
                    Equipment _filteredEquipment= new Equipment();
                    _filteredEquipment.Id=_filteredItem.EquipmentsId;
                    _filteredEquipment.BookingId=_uniqueBookingId;
                    _filteredEquipment.Name=_filteredItem.EquipmentName;
                    // Fetches the data of Equipment from each rows 


                    Employee _filteredEmployee= new Employee();
                    _filteredEmployee.EmployeeId = _filteredItem.EmployeeId ;
                    _filteredEmployee.BookingId = _uniqueBookingId ;
                    _filteredEmployee.EmployeeFirstName = _filteredItem.EmployeeFirstName ;
                    _filteredEmployee.EmployeeMiddleName = _filteredItem.EmployeeMiddleName ;
                    _filteredEmployee.EmployeeLastName = _filteredItem.EmployeeLastName ;
                    _filteredEmployee.EmployeeDepartmentID = _filteredItem.EmployeeDepartmentID ;
                    _filteredEmployee.EmployeeCategoryId = _filteredItem.EmployeeCategoryId ;
                    _filteredEmployee.EmployeeDepartmentName = _filteredItem.EmployeeDepartmentName ;
                    // Fetches the data of Employees from each rows 


                    if(_filteredEquipment.Id!=null)
                    {
                        // if equipmentId is not null we save the data ,
                        // else there is no data so we dont need to store that
                        _equipmentsListRows.Add(_filteredEquipment );
                        
                    }

                    if(_filteredEmployee.EmployeeId!=null){
                        _employeesListRows.Add(_filteredEmployee);
                    }
                    

                }


                
            }

            _equipmentsList = _equipmentsListRows.Select(o => o).DistinctBy(o => o.Id).ToList();
            _employeesList  = _employeesListRows.Select(o => o).DistinctBy(o => o.EmployeeId).ToList();
            //  The above _equipmentsListRows and _employeesListRows may contain some of the ids repeated
            //  So we need to filter that , 
            // To do that we select the unique values from both lists.








            // Now we need to push these two lists to the corresponding booking id
            //START ===== Insert into response

            List<BookingResponse> _bookingResponelist = new List<BookingResponse>();  
            //  Stroes the final response list 

            foreach (var _id in _uniqueBookingIds)
            {
                // Loops through all the uniqu ids 

                var _datasOfId = result.First(o => o.BookingId == _id );
                // gets the first data with the first id 
                // if we use result.Select instead of result.First it will select all rows from result with the same id 
                //  we dont want duplicate data here so we only select the first row that has returned with the id so that 
                // we can the common datas for all rows 
                // now bleow lines stores those data into a response model that weve created  
                
                BookingResponse _bookingResponse = new BookingResponse();      
                _bookingResponse.BookingId = _datasOfId.BookingId ;
                _bookingResponse.OperationTheatreId = _datasOfId.OperationTheatreId ;
                _bookingResponse.IsBookedBy = _datasOfId.IsBookedBy ;
                _bookingResponse.AnaesthetistId = _datasOfId.AnaesthetistId ;
                _bookingResponse.BookingStatusId = _datasOfId.BookingStatusId ;
                _bookingResponse.AnaesthesiaTypeId = _datasOfId.AnaesthesiaTypeId ;
                _bookingResponse.SurgeryId = _datasOfId.SurgeryId ;
                _bookingResponse.PatientRegistrationNo = _datasOfId.PatientRegistrationNo ;
                _bookingResponse.OperationStartDate = _datasOfId.OperationStartDate ;
                _bookingResponse.OperationEndDate = _datasOfId.OperationEndDate ;
                _bookingResponse.OperationDuration = _datasOfId.OperationDuration ;
                _bookingResponse.InstructionToNurse = _datasOfId.InstructionToNurse ;
                _bookingResponse.InstructionToAnaesthetist = _datasOfId.InstructionToAnaesthetist ;
                _bookingResponse.InstructionToOperationTeatrePersons = _datasOfId.InstructionToOperationTeatrePersons ;
                _bookingResponse.RequestForSpecialMeterial = _datasOfId.RequestForSpecialMeterial ;
                _bookingResponse.ModifiedBy = _datasOfId.ModifiedBy ;
                _bookingResponse.IsDeleted = _datasOfId.IsDeleted ;
                _bookingResponse.TheatreName = _datasOfId.TheatreName ;
                _bookingResponse.TheatreLocation = _datasOfId.TheatreLocation ;
                _bookingResponse.TheatreType = _datasOfId.TheatreType ;
                _bookingResponse.TheatreDefaultDepartment = _datasOfId.TheatreDefaultDepartment ;
                _bookingResponse.TheatreCleaningTime  = _datasOfId.TheatreCleaningTime  ;
                _bookingResponse.AnaesthetistFirstName = _datasOfId.AnaesthetistFirstName ;
                _bookingResponse.AnaesthetistLastName = _datasOfId.AnaesthetistLastName ;
                _bookingResponse.AnaesthetistMiddleName = _datasOfId.AnaesthetistMiddleName ;
                _bookingResponse.AnaesthetistDepartmentId = _datasOfId.AnaesthetistDepartmentId ;
                _bookingResponse.AnaesthetistPositionId = _datasOfId.AnaesthetistPositionId ;
                _bookingResponse.AnaesthetistJobId = _datasOfId.AnaesthetistJobId ;
                _bookingResponse.AnaesthetistGender = _datasOfId.AnaesthetistGender ;
                _bookingResponse.AnaesthetistEmployeeCurrentStatus = _datasOfId.AnaesthetistEmployeeCurrentStatus ;
                _bookingResponse.AnaesthetistEmployeeIsActive = _datasOfId.AnaesthetistEmployeeIsActive ;
                _bookingResponse.AnaesthesiaType = _datasOfId.AnaesthesiaType ;
                _bookingResponse.StatusCode  = _datasOfId.StatusCode  ;
                _bookingResponse.StatusName  = _datasOfId.StatusName  ;
                //above lines Stores common details to BookingResponse model


                _bookingResponse.EquipmentsMapping = _equipmentsList.Where(o => o.BookingId==_datasOfId.BookingId).ToList();
                // this line loads the Equipment mapping List to the object

                _bookingResponse.EmployeesMapping = _employeesList.Where(o => o.BookingId==_datasOfId.BookingId).ToList();
                // this line loads the Employee mapping List to the object
                    
                _bookingResponelist.Add(_bookingResponse);
                //  Attaches it to a list To store all the booking datas 
                
            }
            
            IEnumerable<BookingResponse> _response = _bookingResponelist;
            // Converting the output booking list to IEnumerables and passes to the next layer as response
            //END ===== Insert into response


            return _response;
        }
        #endregion


        public async Task<IEnumerable<PostBookingModel>> PostBooking(PostBookingModel _booking)
        {
            // START - VALIDATION SECTION   
            var _OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(_booking.OperationTheatreId, _booking.StartDate, _booking.EndDate);
            if(_OTAllocationStatus<1)
            {
                throw new InvalidOperationException("the ot "+_booking.OperationTheatreId +" is not allocated");
            }


            var _OTBlockStatus = await _bookingDataAccess.IsOperationTheatreBloked(_booking.OperationTheatreId, _booking.StartDate, _booking.EndDate);
            if(_OTBlockStatus>0)
            {
                throw new InvalidOperationException("the ot "+_booking.OperationTheatreId +" is blocked");
            }


            var _OTBookingStatus    = await _bookingDataAccess.IsOperationTheatreBooked(_booking.OperationTheatreId, _booking.StartDate, _booking.EndDate);
            if(_OTBookingStatus>0)
            {
                throw new InvalidOperationException("the ot "
                                                    +_booking.OperationTheatreId 
                                                    +" is already booked for the slot "
                                                    +_booking.StartDate+" to "+_booking.EndDate);
            }

            // END - VALIDATION SECTION




            var result = await _bookingDataAccess.PostBooking(_booking);
            return result;
        }


        public async Task<IEnumerable<PostBookingModel>> UpdateBooking(PostBookingModel _booking)
        {
            //? is validation needed here?
            var result = await _bookingDataAccess.UpdateBooking(_booking);
            return result;
        }
    }
}
