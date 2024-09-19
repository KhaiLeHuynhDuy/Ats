using Ats.Models.JobTitle;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IJobTitleService
    {
        List<JobTitleViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        JobTitleViewModel GetJobTitle(int id);
        List<JobTitleViewModel> GetJobTitles();
        void CreateJobTitle(JobTitleViewModel model);
        void UpdateJobTitle(JobTitleViewModel model);
        void DeleteJobTitle(int id);
    }
}
