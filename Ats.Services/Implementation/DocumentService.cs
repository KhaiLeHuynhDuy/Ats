using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Lead.Models;
using Ats.Models.Document;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class DocumentService : BaseService, IDocumentService    
    {
        private IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private int pageSize;

        public DocumentService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger, IHostingEnvironment hostingEnvironment) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
            _hostingEnvironment = hostingEnvironment;
        }
        #region Lead
        public List<DocumentViewModel> GetListDocuments(Guid leadId)
        {
            _logger.LogDebug($"Get all lead Documents service (leadId: ${leadId})");
            var query = UnitOfWork.DocumentRepo.GetAll().Where(x => x.LeadId == leadId);

            var data = query.Select(x => new DocumentViewModel()
            {
                Id = x.Id,
                DocumentName = x.DocumentName,
                FileName = x.FileName,
                Path = x.Path,
                Url = x.Url,
                Date = x.Date,
                ValidTo = x.ValidTo,
                DocumentType = x.DocumentType,
                Extension = x.Extension,
                Title = x.Title,
                Remark = x.Remark
            }).ToList();

            return data;
        }
        public void CreateDocument(DocumentViewModel model)
        {
            _logger.LogDebug($"Create Document service (Name: {model.DocumentName})");
            if (model.File != null)
            {
                string uploadFolder = $"documents/lead-{model.LeadNum}";
                if (!Directory.Exists(Path.Combine(_hostingEnvironment.WebRootPath, uploadFolder)))
                {
                    Directory.CreateDirectory(Path.Combine(_hostingEnvironment.WebRootPath, uploadFolder));
                }
                string fileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, uploadFolder, fileName);
                string fileUrl = "/" + uploadFolder + "/" + fileName;
                string fileExtension = Path.GetExtension(model.File.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.File.CopyTo(fileStream);
                }

                var document = new Document
                {
                    Id = Guid.NewGuid(),
                    DocumentName = model.DocumentName,
                    FileName = fileName,
                    Url = fileUrl,
                    DocumentType = model.DocumentType,
                    Date = DateTime.UtcNow,
                    ValidTo = model.ValidTo,
                    Extension = fileExtension,
                    Title = model.Title,
                    Remark = model.Remark,
                    LeadId = model.LeadId,
                };
                UnitOfWork.DocumentRepo.Insert(document);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteDocument(Guid id)
        {
            _logger.LogDebug($"Delete Document service (Id: {id})");
            var document = UnitOfWork.DocumentRepo.GetById(id);
            var lead = UnitOfWork.LeadRepo.GetById(document.LeadId.Value);
            if (document != null)
            {
                UnitOfWork.DocumentRepo.Delete(document);
                UnitOfWork.SaveChanges();
                File.Delete(Path.Combine(@$"wwwroot/documents/lead-{lead.LeadId}", document.FileName));
            }
        }
        #endregion

        #region Borrower
        public List<DocumentViewModel> GetListBorrowerDocuments(Guid borrowerId)
        {
            _logger.LogDebug($"Get all Borrower Documents service (borrowerId: ${borrowerId})");
            var query = UnitOfWork.DocumentRepo.GetAll().Where(x => x.BorrowerId == borrowerId);

            var data = query.Select(x => new DocumentViewModel()
            {
                Id = x.Id,
                DocumentName = x.DocumentName,
                FileName = x.FileName,
                Path = x.Path,
                Url = x.Url,
                Date = x.Date,
                ValidTo = x.ValidTo,
                DocumentType = x.DocumentType,
                Extension = x.Extension,
                Title = x.Title,
                Remark = x.Remark
            }).ToList();

            return data;
        }
        public void CreateBorrowerDocument(DocumentViewModel model)
        {
            _logger.LogDebug($"Create Document service (Name: {model.DocumentName})");
            if (model.File != null)
            {
                string uploadFolder = $"documents/borrower-{model.BorrowerNum}";
                if (!Directory.Exists(Path.Combine(_hostingEnvironment.WebRootPath, uploadFolder)))
                {
                    Directory.CreateDirectory(Path.Combine(_hostingEnvironment.WebRootPath, uploadFolder));
                }
                string fileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, uploadFolder, fileName);
                string fileUrl = "/" + uploadFolder + "/" + fileName;
                string fileExtension = Path.GetExtension(model.File.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.File.CopyTo(fileStream);
                }

                var document = new Document
                {
                    Id = Guid.NewGuid(),
                    DocumentName = model.DocumentName,
                    FileName = fileName,
                    Url = fileUrl,
                    DocumentType = model.DocumentType,
                    Date = DateTime.UtcNow,
                    ValidTo = model.ValidTo,
                    Extension = fileExtension,
                    Title = model.Title,
                    Remark = model.Remark,
                    LeadId = model.LeadId,
                    BorrowerId = model.BorrowerId,
                };
                UnitOfWork.DocumentRepo.Insert(document);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteBorrowerDocument(Guid id)
        {
            _logger.LogDebug($"Delete Document service (Id: {id})");
            var document = UnitOfWork.DocumentRepo.GetById(id);
            var borrower = UnitOfWork.BorrowerRepo.GetById(document.BorrowerId.Value);
            if (document != null)
            {
                UnitOfWork.DocumentRepo.Delete(document);
                UnitOfWork.SaveChanges();
                File.Delete(Path.Combine(@$"wwwroot/documents/borrower-{borrower.BorrowerId}", document.FileName));
            }
        }
        #endregion
    }
}
