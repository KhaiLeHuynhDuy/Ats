using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models;
using Ats.Models.Product;
using Ats.Models.Voucher;

namespace Ats.Services.Interfaces
{
    public interface IVoucherCategoryService
    {
        List<VoucherCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        VoucherCategoryViewModel GetVoucherCategory(int id);
        void CreateVoucherCategory(VoucherCategoryViewModel model);
        void UpdateVoucherCategory(VoucherCategoryViewModel model);
        void DeleteVoucherCategory(int id);
        List<VoucherCategoryViewModel> GetVoucherCategorys();
    }
}
