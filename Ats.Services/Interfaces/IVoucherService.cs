using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Channel.Models;
using Ats.Domain.Voucher.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Models.Voucher;

namespace Ats.Services.Interfaces
{
    public interface IVoucherService
    {
        List<VoucherViewModel> Search(string searchText, int? voucherCateid, int? channelid, bool? voucherType, int? stockFrom, int? stockTo, string orderField, bool IsAscOrder, int page, int size, out int total);
        VoucherViewModel GetVoucher(Guid id);
        void CreateVoucher(VoucherViewModel model);
        void UpdateVoucher(VoucherViewModel model);
        void DeleteVoucher(Guid id);
        List<VoucherViewModel> GetVouchers();
        List<VoucherCategory> GetVoucherCategories();
        void Send(VoucherViewModel model);

    }
}
