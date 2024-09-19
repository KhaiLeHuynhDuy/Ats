using Ats.Models.OriginalFile;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IOriginalFileService
    {
        //Original File
        List<OriginalFileViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        OriginalFileViewModel GetOriginalFile(int id);
        void CreateOriginalFile(OriginalFileViewModel model);
        void UpdateOriginalFile(OriginalFileViewModel model);
        void DeleteOriginalFile(int id);

        //Original File Addition
        List<OriginalFileViewModel> SearchAddition(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        OriginalFileViewModel GetOriginalFileAddition(int id);
        void CreateOriginalFileAddition(OriginalFileViewModel model);
        void UpdateOriginalFileAddition(OriginalFileViewModel model);
        void DeleteOriginalFileAddition(int id);
    }
}
