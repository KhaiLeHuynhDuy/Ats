using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("borrowers")]
    public class Borrower : AuditBase, IEntity<Guid>
    {
        public Borrower()
        {
            Documents = new HashSet<Document>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int BorrowerId { get; set; }
        public int LeadId { get; set; }
        public Guid? ReferenceId { get; set; }
        public Guid? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
        public int? JobTitleId { get; set; }
        [ForeignKey("JobTitleId")]
        public virtual JobTitle JobTitle { get; set; }
        public int? OccupationId { get; set; }
        [ForeignKey("OccupationId")]
        public virtual Occupation Occupation { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string RegisterNumber { get; set; }
        public string Remark { get; set; }
        public Guid? OrgId { get; set; }
        [ForeignKey("OrgId")]
        public virtual Organization.Models.Organization Organization { get; set; }
        public double Amount { get; set; }
        public double MonthlyIncome { get; set; }
        public int? LoanProductId { get; set; }
        [ForeignKey("LoanProductId")]
        public virtual Lead.Models.LoanPackage LoanProduct { get; set; }
        public int? OriginalFileId { get; set; }
        [ForeignKey("OriginalFileId")]
        public virtual OriginalFile OriginalFile { get; set; }
        public int? OriginalFileAdditionId { get; set; }
        [ForeignKey("OriginalFileAdditionId")]
        public virtual OriginalFileAddition OriginalAdditionFile { get; set; }
        public BORROWER_SOURCE? BorrowerSource { get; set; }
        public LOAN_PERIOD? LoanPeriod { get; set; }
        public LOAN_PURPOSE? LoanPurpose { get; set; }
        public INCOME_RECEIVED_METHOD? IncomeReceivedMethod { get; set; }
        public HEALTH_INSURANCE? HealthInsurance { get; set; }
        public ELECTRICITY_BILL? ElectricityBill { get; set; }
        public VEHICLE_REGISTRATION_CERTIFICATE? VehicleRegistrationCertificate { get; set; }
        public LIFE_INSURANCE? LifeInsurance { get; set; }
        public BANK_ACCOUNT? BankAccount { get; set; }
        public INTERNET_BILL? InternetBill { get; set; }
        public SIM_CARD? SimCard { get; set; }
        public MARITAL_STATUS? MaritalStatus { get; set; }
        public DateTime BorrowerDate { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Document> Documents { get; set; }
    }
    public enum BORROWER_SOURCE
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Nhập dữ liệu")]
        IMPORT = 1,
        [Display(Name = "Đối tác")]
        PARTNER = 2,
        [Display(Name = "Nội bộ")]
        INTERNAL = 3,
    }
}
