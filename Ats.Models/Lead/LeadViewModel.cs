using Ats.Domain;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Ats.Commons.Constants;

namespace Ats.Models.Lead
{
    public class LeadViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int YOB { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateTime? DOB { get; set; }
        public bool Gender { get; set; }
        public string Phone { get; set; }
        public string PersonalId { get; set; }
        public string PersonalIdRegisteredPlace { get; set; }
        public DateTime? PersonalIdRegisteredDate { get; set; }
        public int? OccupationId { get; set; }
        public int? JobTitleId { get; set; }
        public string JobTitleName { get; set; }
        public string Address1 { get; set; }
        public string AdditionalAddress { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? CountryId { get; set; }

        public int? AdditionalProvinceId { get; set; }
        public int? AdditionalDistrictId { get; set; }
        public string ProfileAddress { get; set; }
        public string CompanyName { get; set; }
        public int? LoanProductId { get; set; }
        public double Amount { get; set; }

        public int? OriginalFileId { get; set; }
        public int? OriginalFileAdditionId { get; set; }
        public int? IncomeStreamId { get; set; }
        public int? IncomeAmountId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public LOAN_PERIOD? LoanPeroid { get; set; }
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
        public string LoadProductName { get; set; }
        public string LoanPeroidName { get; set; }
    }
}
