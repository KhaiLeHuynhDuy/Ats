using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Gift.Models;
using Ats.Models;
using Ats.Models.Gift;
using Ats.Models.Member;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface IMemberTagCategoryService
    {
        List<MemberTagCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        MemberTagCategoryViewModel Get(int id);
        List<MemberTagCategoryViewModel> Get();
        void Create(MemberTagCategoryViewModel model);
        void Update(MemberTagCategoryViewModel model);
        void Delete(int id);
        
    }
}
