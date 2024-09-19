using Ats.Models.Comment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface ICommentService
    {
        List<CommentViewModel> GetListComments(Guid leadId, int page, int size, out int total);
        List<CommentViewModel> GetListComments(Guid leadId);
        CommentViewModel GetComment(Guid id);
        void CreateComment(CommentViewModel model);
        void UpdateComment(CommentViewModel model);
        void DeleteComment(Guid id);
        List<CommentViewModel> GetListBorrowerComments(Guid borrowerId);
    }
}
