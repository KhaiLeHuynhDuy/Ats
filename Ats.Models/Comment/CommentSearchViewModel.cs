using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Comment
{
    public class CommentSearchViewModel : BaseSearchViewModel
    {
        public List<CommentViewModel> Comments { get; set; }
        public CommentViewModel Comment { get; set; }
    }
}
