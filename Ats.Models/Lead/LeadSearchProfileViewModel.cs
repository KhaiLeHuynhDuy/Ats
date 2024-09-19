using Ats.Domain;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Models.Lead
{
    public class LeadSearchProfileViewModel
    {
        public Guid Id { get; set; }
        public int LeadId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int YOB { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public DateTime? DOB { get; set; }
        public bool Gender { get; set; }
        public string Phone { get; set; }
        public string PersonalId { get; set; }
        public string Occupation { get; set; }
        public string JobTitle { get; set; }
        public string Address1 { get; set; }
        public string AdditionalAddress { get; set; }
        public string AdditionalProvince { get; set; }
        public string AdditionalDistrict { get; set; }
        public string FullAddress { get; set; }
        public string HalfAddress { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string FullAdditionalAddress { get; set; }
        public double MonthyIncome { get; set; }
        public string PersonalIdRegisterPlace { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PersonalIdRegisteredDate { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string ResigterNumber { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string Remark { get; set; }
        public string LoanProduct { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public double Amount { get; set; }
        public LOAN_PERIOD? LoanPeriod { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]

        public string OriginalFile { get; set; }
        public string OriginalFileAddition { get; set; }
        //public int? IncomeStreamId { get; set; }
        //public int? IncomeAmountId { get; set; }

        public LEAD_SOURCE? LeadSource { get; set; }
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
        public bool IsActive { get; set; }
    }
}
