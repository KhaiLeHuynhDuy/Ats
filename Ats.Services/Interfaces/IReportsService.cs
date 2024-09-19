using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ats.Domain.Member.Models;
using Ats.Models;
using Ats.Models.Brand;
using Ats.Models.Member;
using Ats.Models.Product;
using Ats.Models.Reports;

namespace Ats.Services.Interfaces
{
    public interface IReportsService
    {
        List<MemberReportsViewModel>SearchMemberReport(string searchText, string dateFrom, string dateTo, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<ProductQrReportsViewModel> SearchProductQrReport(string searchText, string[] productId, string dateFrom, string dateTo, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<RedemptionReportsViewModel> SearchRedemptionReport(string searchText, Guid? collectionId, string orderField, bool IsAscOrder, int page, int size, out int total);

        List<ProductViewModel> GetProducts();
        List<ProductCollectionViewModel> GetCollections();

        Task<List<MemberReportsViewModel>> ExportMemberReport( string dateFrom, string dateTo);
        Task<List<ProductQrReportsViewModel>> ExportProductQrReport(string[] productId, string dateFrom, string dateTo);
        Task<List<RedemptionReportsViewModel>> ExportRedemptionReport(Guid? collectionId);

    }
}
