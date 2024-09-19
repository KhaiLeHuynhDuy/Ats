using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    [Table("leads")]
    public class Lead : AuditBase, IEntity<Guid>
    {
        public Lead()
        {
            LeadUsers = new HashSet<LeadUser>();
            LeadTeams = new HashSet<LeadTeam>();
            LeadAccessLevelHistories = new HashSet<LeadAccessLevelHistory>();
            LeadStatusHistories = new HashSet<LeadStatusHistory>();
            LeadExchangeRequests = new HashSet<LeadExchangeRequest>();
            LeadLevelHistories = new HashSet<LeadLevelHistory>();
            LoanApplications = new HashSet<LoanApplication>();
            LoanNotes = new HashSet<LoanNote>();
            Documents = new HashSet<Document>();
            Debts = new HashSet<Debt>();
            Incomes = new HashSet<Income>();
            Comments = new HashSet<Comment>();
            LoanBooks = new HashSet<LoanBook>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
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

        public LEAD_STATUS? LeadStatus { get; set; }
        public LEAD_SOURCE? LeadSource { get; set; }
        public LEAD_LEVEL? LeadLevel { get; set; }
        public LEAD_ACCESS_LEVEL? LeadAccessLevel { get; set; }

        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string RegisterNumber { get; set; }
        public string Remark { get; set; }
        public Guid? OrgId { get; set; }
        [ForeignKey("OrgId")]
        public virtual Organization.Models.Organization Organization { get; set; }

        public int? LoanProductId { get; set; }
        [ForeignKey("LoanProductId")]
        public virtual LoanPackage LoanProduct { get; set; }

        public double Amount { get; set; }
        public double MonthlyIncome { get; set; }
        public LOAN_PERIOD? LoanPeriod { get; set; }
        public int? IncomeStreamId { get; set; }
        [ForeignKey("IncomeStreamId")]
        public virtual IncomeStream IncomeStream { get; set; }
        public int? IncomeAmountId { get; set; }
        [ForeignKey("IncomeAmountId")]
        public virtual IncomeAmount IncomeAmount { get; set; }
        public int? OriginalFileId { get; set; }
        [ForeignKey("OriginalFileId")]
        public virtual OriginalFile OriginalFile { get; set; }
        public int? OriginalFileAdditionId { get; set; }
        [ForeignKey("OriginalFileAdditionId")]
        public virtual OriginalFileAddition OriginalAdditionFile { get; set; }
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
        public DateTime LeadDate { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<LeadUser> LeadUsers { get; set; }
        public virtual ICollection<LeadTeam> LeadTeams { get; set; }
        public virtual ICollection<LeadAccessLevelHistory> LeadAccessLevelHistories { get; set; }
        public virtual ICollection<LeadStatusHistory> LeadStatusHistories { get; set; }
        public virtual ICollection<LeadExchangeRequest> LeadExchangeRequests { get; set; }
        public virtual ICollection<LeadLevelHistory> LeadLevelHistories { get; set; }
        public virtual ICollection<LoanApplication> LoanApplications { get; set; }
        public virtual ICollection<LoanNote> LoanNotes { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Debt> Debts { get; set; }
        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LoanBook> LoanBooks { get; set; }
    }
}
