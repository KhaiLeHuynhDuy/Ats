using Ats.Commons.Models;
using Ats.Domain.Identity.Models;
using Ats.Domain.Member.Models;
using Ats.Models;
using Ats.Models.Account;
using Ats.Models.Campaign;
using Ats.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ats.Services.Interfaces
{
    public interface ICampaignService : IBaseService
    {
        List<CampaignViewModel> Draft(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<ScheduledViewModel> Scheduled(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<OnGoingViewModel> OnGoing(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<PastViewModel> Past(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<ScheduledViewModel> Scheduled();
        List<OnGoingViewModel> OnGoing();
        List<PastViewModel> Past();



        CampaignViewModel GetCampaign(Guid Id);
        void CreateCampaign(CampaignViewModel model);
        void DeleteCampaign(Guid id);
        void Update(CampaignViewModel model);
        int Send(Guid id);

        List<CampaignViewModel> FatestFourCampaigns();

    }
}