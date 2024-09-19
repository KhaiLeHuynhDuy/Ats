using Ats.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IDocumentService
    {
        List<DocumentViewModel> GetListDocuments(Guid leadId);
        void CreateDocument(DocumentViewModel model);
        void DeleteDocument(Guid id);
        List<DocumentViewModel> GetListBorrowerDocuments(Guid leadId);
        void CreateBorrowerDocument(DocumentViewModel model);
        void DeleteBorrowerDocument(Guid id);
    }
}
