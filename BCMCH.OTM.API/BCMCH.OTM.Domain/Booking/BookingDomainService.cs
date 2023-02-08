using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Domain.Contract.Booking;
using BCMCH.OTM.Infrastucture.Generic;

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



        
        public async Task<IEnumerable<Bookings>> GetBookingList(int departmentId, string? fromDate,string? toDate)
        {
            var result = await _bookingDataAccess.GetBookingList(departmentId, fromDate, toDate);
            // result = result.Where(booking=>booking.OperationTheatreId==operationTheatreId);
            // filters the bookings with given otid 
            return result;
        }
        public async Task<EventFields> GetEventEquipmentsAndEmployees(int bookingId)
        {
            var equipments = await _bookingDataAccess.GetEventEquipments(bookingId);
            var employees  = await _bookingDataAccess.GetEventEmployees(bookingId);
            var departmentIds = employees.Select(o => o.DepartmentID).Distinct();

            // Console.WriteLine(departmentIds.Contains(11));

            var allDepartments = await _bookingDataAccess.GetDepartments();
            var filteredDepartments = allDepartments.Where(o=>  departmentIds.Contains(o.Id));
            
            var fields = new EventFields();
            fields.Equipments = equipments;
            fields.Surgeons = employees;
            fields.Departments = filteredDepartments;
            
            return fields;
        }
        // public async Task<IEnumerable<Employee>> GetEventEmployees(int bookingId)
        // {
        //     var result = await _bookingDataAccess.GetEventEmployees(bookingId);
        //     return result;
        // }

        public async Task<IEnumerable<Bookings>> DeleteBooking(string IdArray="")
        {
            var result = await _bookingDataAccess.DeleteBooking("["+IdArray+"]");
            return result;
        }

        public async Task<Envelope<IEnumerable<int>>> AddBooking(PostBookingModel booking)
        {

            // convertTimeTwelveToTwentyFour(booking.EndDate);
            #region VALIDATION
            // START - VALIDATION SECTION   
            var OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(booking.OperationTheatreId, booking.DepartmentId, booking.StartDate, booking.EndDate);
            if (OTAllocationStatus < 1)
            {
                return new Envelope<IEnumerable<int>>(false, $"OT {booking.OperationTheatreId} is not allocated forthis time");
            }

            var OTBlockStatus = await _bookingDataAccess.IsOperationTheatreBloked(booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if (OTBlockStatus > 0)
            {
                return new Envelope<IEnumerable<int>>(false, $"Operation Theatre {booking.OperationTheatreId} is blocked");
            }

            var OTBookingStatus = await _bookingDataAccess.IsOperationTheatreBooked(0, booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if (OTBookingStatus > 0)
            {
                return new Envelope<IEnumerable<int>>(false, $"Operation Theatre {booking.OperationTheatreId} is already booked for the slot ${booking.StartDate} to ${booking.EndDate}");
            }


            // END - VALIDATION SECTION
            #endregion        

            booking.EmployeeIdArray="["+booking.EmployeeIdArray+"]";
            booking.EquipmentsIdArray="["+booking.EquipmentsIdArray+"]";
            var result = await _bookingDataAccess.AddBooking(booking);
            return new Envelope<IEnumerable<int>>(true, "booking created", result); ;
        }


        public async Task<Envelope<IEnumerable<UpdateBookingModel>>> UpdateBooking(UpdateBookingModel booking)
        {
            
            
            #region VALIDATION  
            var OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(booking.OperationTheatreId, booking.DepartmentId, booking.StartDate, booking.EndDate);
            if(OTAllocationStatus < 1)
            {
               return new Envelope<IEnumerable<UpdateBookingModel>>(false,$"OT {booking.OperationTheatreId} is not allocated for this time");
            }

            var OTBlockStatus = await _bookingDataAccess.IsOperationTheatreBloked(booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if(OTBlockStatus > 0)
            {
                return new Envelope<IEnumerable<UpdateBookingModel>>(false, $"Operation Theatre {booking.OperationTheatreId} is blocked");
            }

            var OTBookingStatus    = await _bookingDataAccess.IsOperationTheatreBooked(booking.Id, booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if(OTBookingStatus > 0)
            {
                return new Envelope<IEnumerable<UpdateBookingModel>>(false, $"Operation Theatre {booking.OperationTheatreId} is already booked for the slot ${booking.StartDate} to ${booking.EndDate}");
            }
            #endregion


            booking.EmployeeIdArray="["+booking.EmployeeIdArray+"]";
            booking.EquipmentsIdArray="["+booking.EquipmentsIdArray+"]";
            var result = await _bookingDataAccess.UpdateBooking(booking);

            return new Envelope<IEnumerable<UpdateBookingModel>>(true,"data-update-success", result); ;
        }

        public async Task<IEnumerable<Blocking>> AddBlocking(Blocking blocking)
        {
            var result = await _bookingDataAccess.AddBlocking(blocking);
            return result;
        }

        private DateTime convertTimeTwelveToTwentyFour( string _datetime )
        {
             DateTime dateTime =  DateTime.ParseExact( _datetime,  
                                                        "yyyy/MM/dd hh:mm:ss tt", 
                                                        System.Globalization.CultureInfo.InvariantCulture);
            // Console.WriteLine();
            // Console.Write("12 hour datetime : ");
            // Console.Write(_datetime);
            // Console.WriteLine();

            // Console.WriteLine();
            // Console.Write("24 hour datetime : ");
            // Console.Write(dateTime);
            // Console.WriteLine();

            
            return dateTime;
        }

        
        public async Task<BookingsAndAllocations> SelectBookingsAndAllocations(int departmentId,int operationTheatreId , string? fromDate,string? toDate)
        {
            // selects bookings and allocations in accordance with given 
            // departmentId
            // operationTheatreId
            // fromDate
            // toDate
            var bookings = await _bookingDataAccess.GetBookingList(departmentId, fromDate, toDate);
            bookings = bookings.Where(booking=>booking.OperationTheatreId==operationTheatreId);
            // filters the bookings with given otid 

            // var result = await _bookingDataAccess.GetBookingList(departmentId, fromDate, toDate);
            // result = result.Where(booking=>booking.OperationTheatreId==operationTheatreId);

            var allocations = await _bookingDataAccess.GetAllocation(departmentId, fromDate, toDate);
            // fetches allocation details of the given departments
            var filteredAllocations = allocations.Where( o=> operationTheatreId==o.OperationTheatreId);
            // filter allocations for given operation theatre id 
            var result = new BookingsAndAllocations();
            result.Bookings = bookings;
            result.Allocations = filteredAllocations;
            return result;
        }
        public async Task<IEnumerable<int?>> SelectAllocatedTheatres(int departmentId, string? fromDate,string? toDate)
        {
            // sellects the unique operation theatre id those are allocated for the given departments
            // in accordance with given startDate and endDate
            var allocations = await _bookingDataAccess.GetAllocation(departmentId, fromDate, toDate);
            var allocatedOperationtheatres = allocations.Select(o => o.OperationTheatreId).Distinct();
            // above line selects unique OperationTheatreId from the allocations 
            // used to show drop down in frontend             
            var result = allocatedOperationtheatres;
            return result;
        }

        public async Task<IEnumerable<Patient>> GetPatientData(string registrationNo)
        {
            var result = await _bookingDataAccess.GetPatientData(registrationNo);
            return result;
        }
    }
}
