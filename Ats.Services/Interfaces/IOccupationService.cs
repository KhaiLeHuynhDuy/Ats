using Ats.Models.Occupation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IOccupationService
    {
        List<OccupationViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        OccupationViewModel GetOccupation(int id);
        void CreateOccupation(OccupationViewModel model);
        void UpdateOccupation(OccupationViewModel model);
        void DeleteOccupation(int id);        
        List<OccupationViewModel> GetOccupations();
    }
}
