using Ats.Models.Borrower;
using System;
using System.Collections.Generic;
using System.Text;
using static Ats.Commons.Constants;

namespace Ats.Services.Interfaces
{
    public interface IBorrowerService
    {
        List<BorrowerViewModel> Search(string searchText, int? province, int? occupation, int? loanProduct, int? loanPeriod, int? loanAmount, Gender? gender, string orderField, bool IsAscOrder, int page, int size, out int total);
        BorrowerViewModel GetBorrower(Guid id);
        BorrowerProfileViewModel GetBorrowerProfile(Guid id);
        void CreateBorrower(BorrowerViewModel model);
        void DeleteBorrower(Guid id);
        void UpdateBorrowerProfile(BorrowerProfileViewModel model);
        void UpdateBorrowerCompanyInfo(BorrowerProfileViewModel model);
        void UpdateBorrowerOtherInfo(BorrowerProfileViewModel model);
        void UpdateBorrowerLoanInfo(BorrowerProfileViewModel model);
        void UpdateBorrowerRemarkInfo(BorrowerProfileViewModel model);
    }
}
