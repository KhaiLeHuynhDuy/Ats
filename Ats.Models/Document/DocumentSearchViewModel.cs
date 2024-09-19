using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Document
{
    public class DocumentSearchViewModel : BaseSearchViewModel
    {
        public List<DocumentViewModel> Documents { get; set; }
        public DocumentViewModel Document { get; set; }
    }
}
