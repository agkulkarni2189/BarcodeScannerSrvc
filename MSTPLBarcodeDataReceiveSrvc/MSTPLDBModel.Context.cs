﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSTPLBarcodeDataReceiveSrvc
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MSTPLFixedBarcodeReaderDBEntities2 : DbContext
    {
        public MSTPLFixedBarcodeReaderDBEntities2()
            : base("name=MSTPLFixedBarcodeReaderDBEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BarcodeReaderMaster> BarcodeReaderMasters { get; set; }
        public virtual DbSet<DeviceMaster> DeviceMasters { get; set; }
        public virtual DbSet<ErrorCodeMaster> ErrorCodeMasters { get; set; }
        public virtual DbSet<LaminatorBarcodeReaderMappingMaster> LaminatorBarcodeReaderMappingMasters { get; set; }
        public virtual DbSet<LaminatorMaster> LaminatorMasters { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Transaction_Table> Transaction_Table { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
    
        public virtual int InsertNewuser(string userName, string password, string firstName, string lastName, string email)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertNewuser", userNameParameter, passwordParameter, firstNameParameter, lastNameParameter, emailParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_GetAllBarcodeReaderDetails_Result> sp_GetAllBarcodeReaderDetails(string deviceIP)
        {
            var deviceIPParameter = deviceIP != null ?
                new ObjectParameter("DeviceIP", deviceIP) :
                new ObjectParameter("DeviceIP", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAllBarcodeReaderDetails_Result>("sp_GetAllBarcodeReaderDetails", deviceIPParameter);
        }
    
        public virtual ObjectResult<sp_GetAllBarcodeTransactions_Result> sp_GetAllBarcodeTransactions(string module_Serial_Number, string barcode_Reader_Serial_Number, string laminator_Number, Nullable<System.DateTime> barcode_Scan_Date, Nullable<bool> isSearchOp, Nullable<long> barcodeTransID, Nullable<int> startLaminatorID, Nullable<int> endLaminatorID, string deviceIP)
        {
            var module_Serial_NumberParameter = module_Serial_Number != null ?
                new ObjectParameter("Module_Serial_Number", module_Serial_Number) :
                new ObjectParameter("Module_Serial_Number", typeof(string));
    
            var barcode_Reader_Serial_NumberParameter = barcode_Reader_Serial_Number != null ?
                new ObjectParameter("Barcode_Reader_Serial_Number", barcode_Reader_Serial_Number) :
                new ObjectParameter("Barcode_Reader_Serial_Number", typeof(string));
    
            var laminator_NumberParameter = laminator_Number != null ?
                new ObjectParameter("Laminator_Number", laminator_Number) :
                new ObjectParameter("Laminator_Number", typeof(string));
    
            var barcode_Scan_DateParameter = barcode_Scan_Date.HasValue ?
                new ObjectParameter("Barcode_Scan_Date", barcode_Scan_Date) :
                new ObjectParameter("Barcode_Scan_Date", typeof(System.DateTime));
    
            var isSearchOpParameter = isSearchOp.HasValue ?
                new ObjectParameter("IsSearchOp", isSearchOp) :
                new ObjectParameter("IsSearchOp", typeof(bool));
    
            var barcodeTransIDParameter = barcodeTransID.HasValue ?
                new ObjectParameter("BarcodeTransID", barcodeTransID) :
                new ObjectParameter("BarcodeTransID", typeof(long));
    
            var startLaminatorIDParameter = startLaminatorID.HasValue ?
                new ObjectParameter("StartLaminatorID", startLaminatorID) :
                new ObjectParameter("StartLaminatorID", typeof(int));
    
            var endLaminatorIDParameter = endLaminatorID.HasValue ?
                new ObjectParameter("EndLaminatorID", endLaminatorID) :
                new ObjectParameter("EndLaminatorID", typeof(int));
    
            var deviceIPParameter = deviceIP != null ?
                new ObjectParameter("DeviceIP", deviceIP) :
                new ObjectParameter("DeviceIP", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAllBarcodeTransactions_Result>("sp_GetAllBarcodeTransactions", module_Serial_NumberParameter, barcode_Reader_Serial_NumberParameter, laminator_NumberParameter, barcode_Scan_DateParameter, isSearchOpParameter, barcodeTransIDParameter, startLaminatorIDParameter, endLaminatorIDParameter, deviceIPParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_GetAllDuplicateUsers(string userName)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_GetAllDuplicateUsers", userNameParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_GetAllDuplicateUsersByEmail(string userEmail)
        {
            var userEmailParameter = userEmail != null ?
                new ObjectParameter("UserEmail", userEmail) :
                new ObjectParameter("UserEmail", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_GetAllDuplicateUsersByEmail", userEmailParameter);
        }
    
        public virtual ObjectResult<sp_GetBarcodeReaderDetailsByReaderID_Result> sp_GetBarcodeReaderDetailsByReaderID(Nullable<int> barcodeReaderID)
        {
            var barcodeReaderIDParameter = barcodeReaderID.HasValue ?
                new ObjectParameter("BarcodeReaderID", barcodeReaderID) :
                new ObjectParameter("BarcodeReaderID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetBarcodeReaderDetailsByReaderID_Result>("sp_GetBarcodeReaderDetailsByReaderID", barcodeReaderIDParameter);
        }
    
        public virtual ObjectResult<sp_GetDeviceDetailsByLaminatorBarcodeReaderMappingID_Result> sp_GetDeviceDetailsByLaminatorBarcodeReaderMappingID(Nullable<int> laminatorBarcodeReaderMappingID)
        {
            var laminatorBarcodeReaderMappingIDParameter = laminatorBarcodeReaderMappingID.HasValue ?
                new ObjectParameter("LaminatorBarcodeReaderMappingID", laminatorBarcodeReaderMappingID) :
                new ObjectParameter("LaminatorBarcodeReaderMappingID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetDeviceDetailsByLaminatorBarcodeReaderMappingID_Result>("sp_GetDeviceDetailsByLaminatorBarcodeReaderMappingID", laminatorBarcodeReaderMappingIDParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_GetLaminatorBarcodeReaderMappingIDByBarcodeReaderID(Nullable<int> barcodeReaderID)
        {
            var barcodeReaderIDParameter = barcodeReaderID.HasValue ?
                new ObjectParameter("BarcodeReaderID", barcodeReaderID) :
                new ObjectParameter("BarcodeReaderID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_GetLaminatorBarcodeReaderMappingIDByBarcodeReaderID", barcodeReaderIDParameter);
        }
    
        public virtual ObjectResult<sp_GetLaminatorDetailsbyLaminatorBarcodeMappingID_Result> sp_GetLaminatorDetailsbyLaminatorBarcodeMappingID(Nullable<int> laminatorBarcodeMappingID)
        {
            var laminatorBarcodeMappingIDParameter = laminatorBarcodeMappingID.HasValue ?
                new ObjectParameter("LaminatorBarcodeMappingID", laminatorBarcodeMappingID) :
                new ObjectParameter("LaminatorBarcodeMappingID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetLaminatorDetailsbyLaminatorBarcodeMappingID_Result>("sp_GetLaminatorDetailsbyLaminatorBarcodeMappingID", laminatorBarcodeMappingIDParameter);
        }
    
        public virtual ObjectResult<sp_GetUserDetails_Result> sp_GetUserDetails(string userName, string password)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetUserDetails_Result>("sp_GetUserDetails", userNameParameter, passwordParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_InsertNewuser(string userName, string password, string firstName, string lastName, string email)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_InsertNewuser", userNameParameter, passwordParameter, firstNameParameter, lastNameParameter, emailParameter);
        }
    
        public virtual int sp_MarkBarcodeTransactionsDisplayed()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_MarkBarcodeTransactionsDisplayed");
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_ReturnLoggedInUserNums(Nullable<int> currentUserId)
        {
            var currentUserIdParameter = currentUserId.HasValue ?
                new ObjectParameter("CurrentUserId", currentUserId) :
                new ObjectParameter("CurrentUserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_ReturnLoggedInUserNums", currentUserIdParameter);
        }
    
        public virtual ObjectResult<Nullable<long>> sp_SaveBarcodeTransaction(Nullable<long> barcodeTransactionID, string modSerNum, Nullable<System.DateTime> creationTime, Nullable<int> laminatorBarcodeReaderMappingID, Nullable<int> errorID, Nullable<bool> isErrorResolved)
        {
            var barcodeTransactionIDParameter = barcodeTransactionID.HasValue ?
                new ObjectParameter("BarcodeTransactionID", barcodeTransactionID) :
                new ObjectParameter("BarcodeTransactionID", typeof(long));
    
            var modSerNumParameter = modSerNum != null ?
                new ObjectParameter("ModSerNum", modSerNum) :
                new ObjectParameter("ModSerNum", typeof(string));
    
            var creationTimeParameter = creationTime.HasValue ?
                new ObjectParameter("CreationTime", creationTime) :
                new ObjectParameter("CreationTime", typeof(System.DateTime));
    
            var laminatorBarcodeReaderMappingIDParameter = laminatorBarcodeReaderMappingID.HasValue ?
                new ObjectParameter("LaminatorBarcodeReaderMappingID", laminatorBarcodeReaderMappingID) :
                new ObjectParameter("LaminatorBarcodeReaderMappingID", typeof(int));
    
            var errorIDParameter = errorID.HasValue ?
                new ObjectParameter("ErrorID", errorID) :
                new ObjectParameter("ErrorID", typeof(int));
    
            var isErrorResolvedParameter = isErrorResolved.HasValue ?
                new ObjectParameter("IsErrorResolved", isErrorResolved) :
                new ObjectParameter("IsErrorResolved", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<long>>("sp_SaveBarcodeTransaction", barcodeTransactionIDParameter, modSerNumParameter, creationTimeParameter, laminatorBarcodeReaderMappingIDParameter, errorIDParameter, isErrorResolvedParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}