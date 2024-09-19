using Ats.Models.Lead;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

namespace Ats.Services.Interfaces
{
    public interface ILeadService
    {
        List<LeadViewModel> Search(string searchText, int? province, int? occupation, int? loanProduct, int? loanPeriod, int? loanAmount, Gender? gender, string orderField, bool IsAscOrder, int page, int size, out int total);
        LeadViewModel GetLead(Guid id);
        LeadProfileViewModel GetLeadProfile(Guid id);
        void CreateLead(LeadViewModel model);
        void DeleteLead(Guid id);
        string ImportLeadExcel(LeadImportViewModel model, Guid userId, out int totalRowSuccess, out int totalRowFail, out bool result);
        Task<object> ImportLeadExcel(LeadsViewModel leads, IFormFile formFile, Guid userId);
        void UpdateLeadProfile(LeadProfileViewModel model);
        void UpdateLeadCompanyInfo(LeadProfileViewModel model);
        void UpdateLeadOtherInfo(LeadProfileViewModel model);
        void UpdateLeadLoanInfo(LeadProfileViewModel model);
        void UpdateLeadRemarkInfo(LeadProfileViewModel model);
        Task<object> LinkDownloadImportLeadExcel(string urlExcel);
    }
}
