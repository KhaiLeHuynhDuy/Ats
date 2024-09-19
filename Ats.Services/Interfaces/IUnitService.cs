using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models.Unit;

namespace Ats.Services.Interfaces
{
   public interface IUnitService
    {
        List<UnitViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        UnitViewModel GetUnit(int id);
        void CreateUnit(UnitViewModel model);
        void UpdateUnit(UnitViewModel model);
        void DeleteUnity(int id);
        List<UnitViewModel> GetUnits();
    }
}
