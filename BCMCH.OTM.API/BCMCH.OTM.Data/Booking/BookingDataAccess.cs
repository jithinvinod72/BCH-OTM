using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Dapper;
using BCMCH.OTM.API.Shared.Booking;
using System.Data;
using System.Data.Common;
using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;

namespace BCMCH.OTM.Data.Booking
{
    public class BookingDataAccess : IBookingDataAccess
    {
        #region PRIVATE
        private readonly ISqlDbHelper _sqlHelper;
        #endregion

        #region CONSTRUCTOR
        public BookingDataAccess(ISqlDbHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        #endregion

        public async Task<IEnumerable<Bookings>> GetBookingList(string? fromDate, string? toDate)
        {
            const string StoredProcedure = "[OTM].[SelectBookings]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@FromDate", fromDate);
            SqlParameters.Add("@ToDate", toDate);
            var result = await _sqlHelper.QueryAsync<Bookings>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<int>> AddBooking(PostBookingModel booking)
        {
            const string StoredProcedure = "[OTM].[InsertBooking]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@OperationTheatreId", booking.OperationTheatreId);
            SqlParameters.Add("@DoctorId", booking.DoctorId);
            SqlParameters.Add("@AnaesthetistId", booking.AnaesthetistId);

            SqlParameters.Add("@StatusId", booking.StatusId);
            SqlParameters.Add("@AnaesthesiaTypeId", booking.AnaesthesiaTypeId);
            SqlParameters.Add("@SurgeryId", booking.SurgeryId);
            SqlParameters.Add("@OtherSurgery", booking.OtherSurgery);

            SqlParameters.Add("@RegistrationNo", booking.RegistrationNo);
            SqlParameters.Add("@StartDate", booking.StartDate);
            SqlParameters.Add("@EndDate", booking.EndDate);
            SqlParameters.Add("@Duration", booking.Duration);
            SqlParameters.Add("@InstructionToNurse", booking.InstructionToNurse);
            SqlParameters.Add("@InstructionToAnaesthetist", booking.InstructionToAnaesthetist);
            SqlParameters.Add("@InstructionToOperationTeatrePersons", booking.InstructionToOperationTeatrePersons);
            SqlParameters.Add("@RequestForSpecialMeterial", booking.RequestForSpecialMeterial);
            SqlParameters.Add("@DepartmentId", booking.DepartmentId);
            // SqlParameters.Add("@Type", booking.Type );
            SqlParameters.Add("@EmployeeIdArray", booking.EmployeeIdArray);
            SqlParameters.Add("@EquipmentsIdArray", booking.EquipmentsIdArray);
            SqlParameters.Add("@SurgeriesIdArray", booking.SurgeriesIdArray);
            SqlParameters.Add("@MaterialsIdArray", booking.MaterialsIdArray);
            SqlParameters.Add("@MedicineIdArray", booking.MedicineIdArray);

            var result = await _sqlHelper.QueryAsync<int>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<GetAllocation>> GetAllocation(string startDate, string endDate)
        {
            const string StoredProcedure = "[OTM].[SelectAllocation]";
            var SqlParameters = new DynamicParameters();
            // SqlParameters.Add("@DeartmentId"    , departmentId );
            SqlParameters.Add("@StartDate", startDate);
            SqlParameters.Add("@EndDate", endDate);
            var result = await _sqlHelper.QueryAsync<GetAllocation>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<Equipments>> GetEventEquipments(int bookingId)
        {
            string Query = @"
                            SELECT 
                                [EquipmentsMaster].Id ,
                                [Name],
                                [Description]
                            FROM [OTM].[EquipmentsMapping]
                            LEFT JOIN  
                                [OTM].[EquipmentsMaster] ON [EquipmentId] =[OTM].[EquipmentsMaster].[Id]
                            WHERE 
                                BookingId=@bookingId 
                                AND 
                                [OTM].[EquipmentsMapping].[EquipmentId] IS NOT NULL
                            ";

            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@bookingId", bookingId);
            var result = await _sqlHelper.QueryAsync<Equipments>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<Equipments>> GetEventMedicines(int bookingId)
        {
            string Query = @"                            
                            SELECT
                                [EquipmentsMaster].Id ,
                                [Name],
                                [Description]
                            FROM [OTM].[EquipmentsMapping]
                                LEFT JOIN
                                [OTM].[EquipmentsMaster] ON [MedicineId] =[OTM].[EquipmentsMaster].[Id]
                            WHERE 
                                BookingId=@bookingId
                                AND
                                [OTM].[EquipmentsMapping].[MedicineId] IS NOT NULL
                            ";

            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@bookingId", bookingId);
            var result = await _sqlHelper.QueryAsync<Equipments>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<Equipments>> GetEventMaterials(int bookingId)
        {
            string Query = @"
                            SELECT
                                [EquipmentsMaster].Id ,
                                [Name],
                                [Description]
                            FROM [OTM].[EquipmentsMapping]
                                LEFT JOIN
                                [OTM].[EquipmentsMaster] ON [MaterialId] =[OTM].[EquipmentsMaster].[Id]
                            WHERE 
                                BookingId=@bookingId
                                AND
                                [OTM].[EquipmentsMapping].[MaterialId] IS NOT NULL
                            ";

            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@bookingId", bookingId);
            var result = await _sqlHelper.QueryAsync<Equipments>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<Employee>> GetEventEmployees(int bookingId)
        {
            // used to fetch the employees with their booking Id
            string Query = @"
                                SELECT 
                                    EmployeeId,
                                    FirstName,
                                    MiddleName,
                                    LastName,
                                    DepartmentID ,
                                    [EmployeeCategories].[Id]       AS EmployeeCategoryId,
                                    [EmployeeCategories].[Name]     AS CategoryName
                                FROM  
                                    [OTM].[EmployeeMapping]
                                LEFT JOIN 
                                    HR.[Employees] ON OTM.EmployeeMapping.EmployeeId =HR.[Employees].Id
                                LEFT JOIN 
                                    [HR].[EmployeeCategories] AS EmployeeCategories ON [HR].[Employees].CategoryID = EmployeeCategories.[Id]
                                WHERE 
                                    BookingId=@bookingId
                            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@bookingId", bookingId.ToString());
            var result = await _sqlHelper.QueryAsync<Employee>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<Surgeries>> GetEventSurgeries(int bookingId)
        {
            string Query = @"
                                SELECT
                                    SurgeryMapping.[SurgeryId]              AS Id,
                                    SurgeryDetails.[Name]                   AS Name,
                                    SurgeryDetails.[PrintName]              AS PrintName,
                                    SurgeryDetails.[AliasName]              AS AliasName,
                                    SurgeryDetails.[InstructionsToPatient]  AS InstructionsToPatient
                                FROM 
                                    [behive-dev-otm].[OTM].[SurgeriesMapping] AS SurgeryMapping
                                LEFT JOIN 
                                    [behive-dev-otm].[dbo].[View_SurgeryServices] AS SurgeryDetails ON SurgeryMapping.SurgeryId = SurgeryDetails.Id
                                WHERE BookingId=@bookingId
                            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@bookingId", bookingId.ToString());
            var result = await _sqlHelper.QueryAsync<Surgeries>(Query, SqlParameters, CommandType.Text);
            return result;
        }


        public async Task<IEnumerable<Departments>> GetDepartments()
        {
            string Query = @"
                                SELECT 
                                    [Id],
                                    [Code],
                                    [DivisionId],
                                    [TypeCode],
                                    [Name]
                                FROM 
                                    dbo.Departments
                            ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<Departments>(Query, SqlParameters, CommandType.Text);
            return result;
        }


        public async Task<IEnumerable<UpdateBookingModel>> UpdateBooking(UpdateBookingModel booking)
        {
            const string StoredProcedure = "[OTM].[UpdateBooking]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@Id", booking.Id);
            SqlParameters.Add("@OperationTheatreId", booking.OperationTheatreId);
            SqlParameters.Add("@DoctorId", booking.DoctorId);
            SqlParameters.Add("@AnaesthetistId", booking.AnaesthetistId);

            SqlParameters.Add("@StatusId", booking.StatusId);
            SqlParameters.Add("@AnaesthesiaTypeId", booking.AnaesthesiaTypeId);
            SqlParameters.Add("@SurgeryId", booking.SurgeryId);
            SqlParameters.Add("@OtherSurgery", booking.OtherSurgery);

            SqlParameters.Add("@RegistrationNo", booking.RegistrationNo);
            SqlParameters.Add("@StartDate", booking.StartDate);
            SqlParameters.Add("@EndDate", booking.EndDate);
            SqlParameters.Add("@Duration", booking.Duration);
            SqlParameters.Add("@InstructionToNurse", booking.InstructionToNurse);
            SqlParameters.Add("@InstructionToAnaesthetist", booking.InstructionToAnaesthetist);
            SqlParameters.Add("@InstructionToOperationTeatrePersons", booking.InstructionToOperationTeatrePersons);

            SqlParameters.Add("@RequestForSpecialMeterial", booking.RequestForSpecialMeterial);
            SqlParameters.Add("@DepartmentId", booking.DepartmentId);
            SqlParameters.Add("@EmployeeIdArray", booking.EmployeeIdArray);
            SqlParameters.Add("@EquipmentsIdArray", booking.EquipmentsIdArray);
            SqlParameters.Add("@SurgeriesIdArray", booking.SurgeriesIdArray);

            SqlParameters.Add("@MaterialsIdArray", booking.MaterialsIdArray);
            SqlParameters.Add("@MedicineIdArray", booking.MedicineIdArray);

            var result = await _sqlHelper.QueryAsync<UpdateBookingModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }


        public async Task<int> IsOperationTheatreAllocated(int operationTheatreId, int departmentId, string startDate, string endDate)
        {
            const string StoredProcedure = "[OTM].[IsOperationTheatreAllocated]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@StartDateToSearch", startDate);
            SqlParameters.Add("@EndDateToSearch", endDate);
            SqlParameters.Add("@operationTheatreId", operationTheatreId);
            SqlParameters.Add("@departmentId", departmentId);
            var result = await _sqlHelper.QueryAsync<UpdateBookingModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            // the result has Ienumerable type 
            // what we do is we simply return the count/length of the ienumerable 
            // if the count is less than 0 then the ot is not allocated for the department 
            // if the count is greater than 0 then the ot is allocated for the department 

            return result.Count();
        }
        public async Task<int> IsOperationTheatreBloked(int operationTheatreId, string startDate, string endDate)
        {
            const string StoredProcedure = "[OTM].[IsOperationTheatreBloked]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@operationTheatreId", operationTheatreId);
            SqlParameters.Add("@StartDateToSearch", startDate);
            SqlParameters.Add("@EndDateToSearch", endDate);
            var result = await _sqlHelper.ExecuteAsync(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<int> IsOperationTheatreBooked(int bookingIdToExcludeFromSearch, int operationTheatreId, string startDate, string endDate)
        {
            // bookingIdToExcludeFromSearch is used in updation , 
            // for updation we dont have to check the time slots with current operation id 
            // for inserting bookingIdToExcludeFromSearch is 0
            const string StoredProcedure = "[OTM].[IsOperationTheatreBooked]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@BookingIdToExclude", bookingIdToExcludeFromSearch);
            SqlParameters.Add("@operationTheatreId", operationTheatreId);
            SqlParameters.Add("@StartDateToSearch", startDate);
            SqlParameters.Add("@EndDateToSearch", endDate);
            var result = await _sqlHelper.ExecuteAsync(StoredProcedure, SqlParameters, CommandType.StoredProcedure);

            return result;
        }


        public async Task<IEnumerable<Bookings>> DeleteBooking(string IdArray)
        {
            string Query = @"
                                UPDATE 
                                    [OTM].[Bookings]
                                SET 
                                    [IsDeleted] = 1
                                WHERE 
                                    Id IN (SELECT value FROM OPENJSON(@IdArray))
                            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@IdArray", IdArray);
            var result = await _sqlHelper.QueryAsync<Bookings>(Query, SqlParameters, CommandType.Text);
            return result;
        }



        #region BLOCKING
        public async Task<IEnumerable<Blocking>> AddBlocking(Blocking blocking)
        {
            string Query = @"
                            INSERT INTO [OTM].[Bookings]
                            (
                                [OperationTheatreId],
                                [StatusId],
                                [StartDate],
                                [EndDate],
                                [Duration],
                                [ModifiedBy],
                                [IsDeleted]
                            )
                            VALUES
                            (
                                @OperationTheatreId,
                                @StatusId,
                                @StartDate,
                                @EndDate,
                                @Duration,
                                @ModifiedBy,
                                0
                            )
                           ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@OperationTheatreId", blocking.OperationTheatreId);
            SqlParameters.Add("@StatusId", 3);
            SqlParameters.Add("@StartDate", blocking.StartDate);
            SqlParameters.Add("@EndDate", blocking.EndDate);
            SqlParameters.Add("@Duration", blocking.Duration);
            SqlParameters.Add("@ModifiedBy", blocking.ModifiedBy);

            var result = await _sqlHelper.QueryAsync<Blocking>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<Blocking>> EditBlocking(Blocking blocking)
        {
            string Query = @"
                            UPDATE [OTM].[Bookings]
                            SET
                                [OperationTheatreId] = @OperationTheatreId,
                                [StatusId] = @StatusId,
                                [StartDate] = @StartDate,
                                [EndDate] = @EndDate,
                                [Duration] = @Duration,
                                [ModifiedBy] = @ModifiedBy
                            WHERE Id=@Id
                           ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@OperationTheatreId", blocking.OperationTheatreId);
            SqlParameters.Add("@StatusId", 3);
            SqlParameters.Add("@StartDate", blocking.StartDate);
            SqlParameters.Add("@EndDate", blocking.EndDate);
            SqlParameters.Add("@Duration", blocking.Duration);
            SqlParameters.Add("@ModifiedBy", blocking.ModifiedBy);
            SqlParameters.Add("@Id", blocking.Id);
            var result = await _sqlHelper.QueryAsync<Blocking>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<Patient>> GetPatientData(string registrationNo)
        {
            const string StoredProcedure = "[OTM].[SelectPatientDetails]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@registrationNo", registrationNo);
            var result = await _sqlHelper.QueryAsync<Patient>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        #endregion




        // PATHOLOGY START
        #region PATHOLOGY
        public async Task<IEnumerable<Pathology>> GetPathology()
        {
            string Query = @"
                            SELECT 
                                Pathology.[Id],
                                Pathology.[RegistrationNo],
                                Pathology.[datetime]                AS Datetime,
                                Pathology.[status]                  AS Status,
                                Pathology.[IsDeleted],
                                Pathology.[PostedBy],
                                PatientMaster.FirstName             AS PatientFirstName,
                                PatientMaster.MiddleName            AS PatientMiddleName,
                                PatientMaster.LastName              AS PatientLastName,
                                EmployeeTable.[DepartmentID]        AS BookedDepartment,
                                SurgeonTable.[FullName]             AS BookedByName,
                                DepartmentTable.Name                AS DepartmentName
                            FROM 
                                [behive-dev-otm].[OTM].[Pathology] AS Pathology
                            LEFT JOIN 
                                [behive-dev-otm].dbo.PatientMaster 
                                AS PatientMaster ON 
                                Pathology.RegistrationNo = PatientMaster.RegistrationNo
                            LEFT JOIN 
                                [dbo].[Users] AS SurgeonTable 
                                ON 
                                Pathology.PostedBy = [SurgeonTable].EmployeeId 
                            LEFT JOIN 
                                [HR].[Employees] AS EmployeeTable 
                                ON 
                                Pathology.[PostedBy] = [EmployeeTable].Id 
                            LEFT JOIN 
                                [dbo].[Departments] AS DepartmentTable 
                                ON 
                                EmployeeTable.[DepartmentID] = DepartmentTable.Id
                            WHERE 
                                Pathology.[IsDeleted]=0
                                ;
                           ";
            var SqlParameters = new DynamicParameters();
            // SqlParameters.Add("@RegNo", Pathology.RegistrationNo);
            var result = await _sqlHelper.QueryAsync<Pathology>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<PathologySample>> GetPathologyDataWithId(int id)
        {
            string Query = @"
                SELECT TOP (1000) [Id]
                    ,[PathologyId]
                    ,[ProcedureId]
                    ,[HistopathologyId]
                    ,[SpecimenNature]
                    ,[BiposySite]
                FROM 
                    [behive-dev-otm].[OTM].[PathologySamples]
                WHERE 
                    PathologyId=@PathologyId
            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@PathologyId", id);
            var result = await _sqlHelper.QueryAsync<PathologySample>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<int>> PostPathology(Pathology Pathology)
        {
            string Query = @"
                            INSERT INTO [OTM].[Pathology]
                                (
                                    [RegistrationNo] ,
                                    [status] ,
                                    [PostedBy],
                                    [IsDeleted],
                                    [DateTime]
                                )
                            VALUES
                                (
                                    @RegNo,
                                    @status,
                                    @PostedBy,
                                    @IsDeleted,
                                    @DateTime
                                )

                            DECLARE @PathologyId  AS BIGINT;
                            SET @PathologyId = SCOPE_IDENTITY();

                            INSERT INTO [OTM].[PathologySamples]
                                (
                                    [PathologyId] ,
                                    [ProcedureId] ,
                                    [HistopathologyId] ,
                                    [SpecimenNature] ,
                                    [BiposySite]
                                )
                            SELECT
                                @PathologyId        ,
                                ProcedureId         ,
                                HistopathologyId    ,
                                natureOfSpecimen    ,
                                siteOfBiopsy
                            FROM OPENJSON(@nestedData)
                            WITH (
                                ProcedureId INT,
                                HistopathologyId INT,
                                natureOfSpecimen NVARCHAR(50),
                                siteOfBiopsy NVARCHAR(50)
                            );

                            SELECT @PathologyId
                           ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@RegNo", Pathology.RegistrationNo);
            SqlParameters.Add("@PostedBy", Pathology.PostedBy);
            SqlParameters.Add("@nestedData", Pathology.NestedData);
            SqlParameters.Add("@status", 1);
            SqlParameters.Add("@IsDeleted", 0);
            SqlParameters.Add("@DateTime", Pathology.Datetime);

            var result = await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;

        }



        public async Task<IEnumerable<int>> PatchPathology(Pathology pathology)
        {
            string Query = @"
                            UPDATE [OTM].[Pathology]
                            SET
                                [RegistrationNo]    = @RegNo    ,
                                [status]            = @status    ,
                                [PostedBy]          = @PostedBy    ,
                                [IsDeleted]         = @IsDeleted    ,
                                [DateTime]          = @DateTime

                            WHERE
                                Id=@PathologyId

                            DELETE FROM [OTM].[PathologySamples]
                            WHERE PathologyId=@PathologyId


                            INSERT INTO [OTM].[PathologySamples]
                                (
                                    [PathologyId] ,
                                    [ProcedureId] ,
                                    [HistopathologyId] ,
                                    [SpecimenNature] ,
                                    [BiposySite]
                                )
                            SELECT
                                @PathologyId        ,
                                ProcedureId         ,
                                HistopathologyId    ,
                                natureOfSpecimen    ,
                                siteOfBiopsy
                            FROM OPENJSON(@nestedData)
                            WITH (
                                ProcedureId INT,
                                HistopathologyId INT,
                                natureOfSpecimen NVARCHAR(50),
                                siteOfBiopsy NVARCHAR(50)
                            );
                            SELECT @PathologyId
                           ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@PathologyId", pathology.Id);
            SqlParameters.Add("@RegNo", pathology.RegistrationNo);
            SqlParameters.Add("@PostedBy", pathology.PostedBy);
            SqlParameters.Add("@nestedData", pathology.NestedData);
            SqlParameters.Add("@DateTime", pathology.Datetime);
            SqlParameters.Add("@status", 1);
            SqlParameters.Add("@IsDeleted", 0);


            var result = await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;

        }

        public async Task<IEnumerable<int>> DeletePathology(String idArray)
        {
            string Query = @"
                            UPDATE [OTM].[Pathology]
                            SET
                                [IsDeleted] = 1
                            WHERE
                                Id IN (SELECT value FROM OPENJSON(@IdArray))
                           ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@IdArray", idArray);
            var result = await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;

        }
        #endregion
        // PATHOLOGY END


        // Removable Devices START
        #region RemovableDevices
        public async Task<IEnumerable<int>> PostRemovableDevices(RemovableDevicesMain removableDevicesMain)
        {
            string Query = @"
                            INSERT INTO [OTM].[RemovableDevicesMain]
                            (
                                [RegistrationNo],
                                [status],
                                [IsDeleted],
                                [PostedBy],
                                [DateTime]
                            )
                            VALUES
                            (
                                @RegistrationNo,
                                @status,
                                @IsDeleted,
                                @PostedBy,
                                @DateTime
                            )

                            DECLARE @RemovableDevicesMainId  AS BIGINT;
                            SET @RemovableDevicesMainId = SCOPE_IDENTITY();


                            INSERT INTO [OTM].[RemovableDevicesSelected]
                            (
                                [RemovableDeviceMainId],
                                [RemovableDeviceId],
                                [RemovableDeviceName],
                                [Notes],
                                [PlacedIn],
                                [PlacedDate],
                                [DateToRemove],
                                [IsRemoved]
                            )
                            SELECT
                                @RemovableDevicesMainId ,
                                deviceId                ,
                                deviceName              ,
                                notes                   ,
                                devicePlaced            ,
                                placedDate              ,
                                removedDate             ,
                                0
                            FROM OPENJSON(@nestedData)
                            WITH (
                                deviceId INT                    ,
                                deviceName NVARCHAR(800)        ,
                                notes NVARCHAR(800)             ,
                                devicePlaced NVARCHAR(800)      ,
                                removedDate DATETIME            ,
                                placedDate DATETIME     
                            );
                            SELECT @RemovableDevicesMainId
                            
                           ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@RegistrationNo", removableDevicesMain.RegistrationNo);
            SqlParameters.Add("@status", removableDevicesMain.Status);
            SqlParameters.Add("@IsDeleted", 0);
            SqlParameters.Add("@PostedBy", removableDevicesMain.PostedBy);
            SqlParameters.Add("@DateTime", removableDevicesMain.Datetime);
            SqlParameters.Add("@nestedData", removableDevicesMain.NestedData);


            var result = await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<int>> EditRemovableDevices(RemovableDevicesMain removableDevicesMain)
        {
            string Query = @"
                            UPDATE [OTM].[RemovableDevicesMain]
                            SET
                                [RegistrationNo] = @RegistrationNo,
                                [status] = @status,
                                [IsDeleted] = @IsDeleted,
                                [PostedBy] = @PostedBy,
                                [DateTime] = @DateTime
                            WHERE Id=@RemovableDevicesMainId;

                            DELETE FROM [OTM].[RemovableDevicesSelected] 
                            WHERE RemovableDeviceMainId=@RemovableDevicesMainId;

                            INSERT INTO [OTM].[RemovableDevicesSelected]
                            (
                                [RemovableDeviceMainId],
                                [RemovableDeviceId],
                                [RemovableDeviceName],
                                [Notes],
                                [PlacedIn],
                                [PlacedDate],
                                [DateToRemove],
                                [IsRemoved]
                            )
                            SELECT
                                @RemovableDevicesMainId ,
                                deviceId                ,
                                deviceName              ,
                                notes                   ,
                                devicePlaced            ,
                                placedDate              ,
                                removedDate             ,
                                0
                            FROM OPENJSON(@nestedData)
                            WITH (
                                deviceId INT                    ,
                                deviceName NVARCHAR(800)        ,
                                notes NVARCHAR(800)             ,
                                devicePlaced NVARCHAR(800)      ,
                                removedDate DATETIME            ,
                                placedDate DATETIME     
                            );
                            SELECT @RemovableDevicesMainId
                            
                           ";
            var SqlParameters = new DynamicParameters();

            SqlParameters.Add("@RemovableDevicesMainId", removableDevicesMain.Id);
            SqlParameters.Add("@RegistrationNo", removableDevicesMain.RegistrationNo);
            SqlParameters.Add("@status", removableDevicesMain.Status);
            SqlParameters.Add("@IsDeleted", 0);
            SqlParameters.Add("@PostedBy", removableDevicesMain.PostedBy);
            SqlParameters.Add("@DateTime", removableDevicesMain.Datetime);
            SqlParameters.Add("@nestedData", removableDevicesMain.NestedData);


            var result = await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<int>> DeleteRemovableDeviceMain(string idArray)
        {
            string Query = @"
                            UPDATE [OTM].[RemovableDevicesMain]
                            SET
                                [IsDeleted] = 1
                            WHERE
                                Id IN (SELECT value FROM OPENJSON(@IdArray))
                           ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@IdArray", idArray);
            var result = await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;

        }

        public async Task<IEnumerable<RemovableDevicesMain>> GetRemovableDevices()
        {
            string Query = @"
                            SELECT
                                RemovableDevices.[Id],
                                RemovableDevices.[RegistrationNo],
                                RemovableDevices.[DateTime]                AS Datetime,
                                RemovableDevices.[status]                  AS Status,
                                RemovableDevices.[IsDeleted],
                                RemovableDevices.[PostedBy],
                                PatientMaster.FirstName             AS PatientFirstName,
                                PatientMaster.MiddleName            AS PatientMiddleName,
                                PatientMaster.LastName              AS PatientLastName,
                                EmployeeTable.[DepartmentID]        AS BookedDepartment,
                                SurgeonTable.[FullName]             AS BookedByName,
                                DepartmentTable.Name                AS DepartmentName
                            FROM
                                [behive-dev-otm].[OTM].[RemovableDevicesMain] AS RemovableDevices
                                LEFT JOIN
                                    [behive-dev-otm].dbo.PatientMaster 
                                AS PatientMaster ON 
                                    RemovableDevices.RegistrationNo = PatientMaster.RegistrationNo

                                LEFT JOIN
                                [dbo].[Users] AS SurgeonTable ON 
                                RemovableDevices.PostedBy = [SurgeonTable].EmployeeId

                                LEFT JOIN
                                [HR].[Employees] AS EmployeeTable ON 
                                RemovableDevices.[PostedBy] = [EmployeeTable].Id

                                LEFT JOIN
                                [dbo].[Departments] AS DepartmentTable ON 
                                EmployeeTable.[DepartmentID] = DepartmentTable.Id
                            WHERE 
                                RemovableDevices.[IsDeleted]=0;
                           ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<RemovableDevicesMain>(Query, SqlParameters, CommandType.Text);
            return result;
        }


        public async Task<IEnumerable<RemovableDevicesSelcted>> GetRemovableDevicesSelected(int id)
        {
            string Query = @"
                            SELECT TOP (1000) 
                                [Id]
                                ,[RemovableDeviceMainId]
                                ,[RemovableDeviceId]
                                ,[RemovableDeviceName]
                                ,[Notes]
                                ,[PlacedIn]
                                ,[PlacedDate]
                                ,[DateToRemove]
                                ,[IsRemoved]
                            FROM [behive-dev-otm].[OTM].[RemovableDevicesSelected]
                            WHERE RemovableDeviceMainId=@RemovableDeviceId
                           ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@RemovableDeviceId", id);
            var result = await _sqlHelper.QueryAsync<RemovableDevicesSelcted>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        #endregion
        // Removable Devices END





        // NON OP 
        public async Task<IEnumerable<NonOP>> AddNonOPRequest(NonOP nonOP)
        {
            try
            {
                string Query = @"
                INSERT INTO [OTM].[NonOP]
                    (
                    [PatientUHID],
                    [PatientName],
                    [PatientAge],
                    [AdmittedLocation],
                    [ProcedureToPerform],
                    [PriorityLevel],
                    [ProvisionalDiagnosis],
                    [Comments],
                    [Status],
                    [DateToBePerformed],
                    [PostedDateTime]
                    )
                VALUES
                    (
                        @PatientUHID,
                        @PatientName,
                        @PatientAge,
                        @AdmittedLocation,
                        @ProcedureToPerform,
                        @PriorityLevel,
                        @ProvisionalDiagnosis,
                        @Comments,
                        @Status,
                        @DateToBePerformed,
                        GETDATE()
                )
                ";
                var SqlParameters = new DynamicParameters();
                
                
                // const string StoredProcedure = "[OTM].[InsertNonOP]";
                // var SqlParameters = new DynamicParameters();
                SqlParameters.Add("@PatientUHID", nonOP.PatientUHID);
                SqlParameters.Add("@PatientName", nonOP.PatientName);
                SqlParameters.Add("@PatientAge", nonOP.PatientAge);
                SqlParameters.Add("@PriorityLevel", nonOP.PriorityLevel);
                SqlParameters.Add("@DateToBePerformed", nonOP.DateToBePerformed);
                SqlParameters.Add("@Comments", nonOP.Comments);
                SqlParameters.Add("@Status", nonOP.Status);
                SqlParameters.Add("@ProcedureToPerform", nonOP.ProcedureToPerform);
                SqlParameters.Add("@ProvisionalDiagnosis", nonOP.ProvisionalDiagnosis);
                SqlParameters.Add("@AdmittedLocation", nonOP.AdmittedLocation);
                
                var result = await _sqlHelper.QueryAsync<NonOP>(Query, SqlParameters, CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }

        }

        public async Task<IEnumerable<NonOP>> GetNonOPRequests()
        {
            const string Query = @"
                                    SELECT 
                                         [Id]
                                        ,[PatientUHID]
                                        ,[PatientName]
                                        ,[PatientAge]
                                        ,[AdmittedLocation]
                                        ,[ProcedureToPerform]
                                        ,[PriorityLevel]
                                        ,[ProvisionalDiagnosis]
                                        ,[Comments]
                                        ,[DateToBePerformed]
                                        ,[PostedDateTime]
                                        ,[status]
                                    FROM [OTM].[NonOP]
                                 ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<NonOP>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<NonOP>> EditNonOPRequests(NonOP nonOP)
        {
            const string StoredProcedure = "[OTM].[UpdateNonOP]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@PatientUHID", nonOP.PatientUHID);
            SqlParameters.Add("@PatientName", nonOP.PatientName);
            SqlParameters.Add("@PatientAge", nonOP.PatientAge);
            SqlParameters.Add("@PriorityLevel", nonOP.PriorityLevel);
            SqlParameters.Add("@DateToBePerformed", nonOP.DateToBePerformed);
            SqlParameters.Add("@Comments", nonOP.Comments);
            SqlParameters.Add("@ProcedureToPerform", nonOP.ProcedureToPerform);
            SqlParameters.Add("@ProvisionalDiagnosis", nonOP.ProvisionalDiagnosis);
            SqlParameters.Add("@AdmittedLocation", nonOP.AdmittedLocation);
            SqlParameters.Add("@Id", nonOP.Id);
            var result = await _sqlHelper.QueryAsync<NonOP>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }


        // Time Updates 
        public async Task<IEnumerable<BookingTime>> PostOTTimings(BookingTime bookingTime)
        {
            string Query = @"
                            UPDATE [OTM].[Bookings]
                            SET
                                OtComplexEntry      = @OtComplexEntry     , 
                                PreOpEntryTime      = @PreOpEntryTime     ,
                                PreOpExitTime       = @PreOpExitTime      ,
                                OtEntryTime         = @OtEntryTime        ,
                                OtExitTime          = @OtExitTime         ,
                                PostOpEntryTime     = @PostOpEntryTime    ,
                                PostOpExitTime      = @PostOpExitTime     ,
                                AverageSurgeryTime  = @AverageSurgeryTime  
                            WHERE  
                                [Bookings].[Id]=@BookingId
                           ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add( "@BookingId"         , bookingTime.BookingId ) ;
            SqlParameters.Add( "@OtComplexEntry"    , bookingTime.OtComplexEntry ) ;
            SqlParameters.Add( "@PreOpEntryTime"    , bookingTime.PreOpEntryTime ) ;
            SqlParameters.Add( "@PreOpExitTime"     , bookingTime.PreOpExitTime ) ;
            SqlParameters.Add( "@OtEntryTime"       , bookingTime.OtEntryTime ) ;
            SqlParameters.Add( "@OtExitTime"        , bookingTime.OtExitTime ) ;
            SqlParameters.Add( "@PostOpEntryTime"   , bookingTime.PostOpEntryTime ) ;
            SqlParameters.Add( "@PostOpExitTime"    , bookingTime.PostOpExitTime ) ;
            SqlParameters.Add( "@AverageSurgeryTime", bookingTime.AverageSurgeryTime ) ;
            

            var result = await _sqlHelper.QueryAsync<BookingTime>(Query, SqlParameters, CommandType.Text);
            return result;
        }




        // DASHBOARD 
        #region DASHBOARD-TODAY
        public async Task<IEnumerable<DashboardOperation>> GetTodaysOtStatuses()
        {
            string Query = @"
                            SELECT 
                                Bookings.[Id]                    AS operationId,
                                Bookings.[OperationTheatreId]    AS OperationTheatreId,
                                OperationTheatre.Name            AS OperationTheatreName,
                                Bookings.[RegistrationNo]        AS RegistrationNo,
                                Patients.FirstName               AS PatientFirstName,
                                Patients.MiddleName              AS PatientMiddleName,
                                Patients.LastName                AS PatientLastName,
                                Bookings.[StartDate]             AS StartDate,
                                Bookings.[EndDate]               AS EndDate,
                                Bookings.[DepartmentId]          AS DepartmentId,
                                Bookings.[PreOpEntryTime]        AS PreOpEntryTime,
                                Bookings.[PreOpExitTime]         AS PreOpExitTime,
                                Bookings.[OtEntryTime]           AS OtEntryTime,
                                Bookings.[OtExitTime]            AS OtExitTime,
                                Bookings.[PostOpEntryTime]       AS PostOpEntryTime,
                                Bookings.[PostOpExitTime]        AS PostOpExitTime,
                                Bookings.[AverageSurgeryTime]    AS AverageSurgeryTime,
                                Bookings.[OtComplexEntry]        AS OtComplexEntry
                            FROM [OTM].[Bookings] AS Bookings
                            LEFT JOIN 
                                [OTM].[OperationTheatreMaster] AS OperationTheatre ON Bookings.OperationTheatreId = OperationTheatre.Id
                            LEFT JOIN [dbo].[PatientMaster] AS Patients ON Bookings.RegistrationNo = [Patients].[RegistrationNo]
                            WHERE CONVERT(DATE, Bookings.[StartDate]) = CONVERT(DATE, GETDATE());
                            
                           ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<DashboardOperation>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        #endregion
        // DASHBOARD REGION END

    }

}
