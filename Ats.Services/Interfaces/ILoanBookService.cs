using Ats.Models.LoanBook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface ILoanBookService
    {
        List<LoanBookViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        LoanBookViewModel GetLoanBook(Guid id);
        void CreateLoanBook(LoanBookViewModel model);
        void UpdateLoanBook(LoanBookViewModel model);
        void DeleteLoanBook(Guid id);
        List<LoanBookViewModel> GetLoanBooks();
    }
}
