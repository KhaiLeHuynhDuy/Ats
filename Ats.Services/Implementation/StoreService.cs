using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Models.Store;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Ats.Models.ResponeResult;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Ats.Domain.Member.Models;
using Ats.Commons;
using System.Globalization;

namespace Ats.Services.Implementation
{
    public class StoreService : BaseService, IStoreService
    {
        private IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private int pageSize;

        public StoreService(IUnitOfWork unitOfWork, IConfiguration config, IHostingEnvironment environment, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            _hostingEnvironment = environment;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<StoreViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.StoreRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                              x.Phone != null && x.Phone.ToLower().Contains(searchText.ToLower()) ||
                              x.StoreCode != null && x.StoreCode.ToLower().Contains(searchText.ToLower()) ||
                              x.Email != null && x.Email.ToLower().Contains(searchText.ToLower()) ||
                              x.StoreName != null && x.StoreName.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.Active) : query.OrderByDescending(x => x.Active);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.StoreName) : query.OrderByDescending(x => x.StoreName);
                        break;
                    case "code":
                        query = IsAscOrder ? query.OrderBy(x => x.StoreCode) : query.OrderByDescending(x => x.StoreCode);
                        break;
                    case "phone":
                        query = IsAscOrder ? query.OrderBy(x => x.Phone) : query.OrderByDescending(x => x.Phone);
                        break;
                    case "email":
                        query = IsAscOrder ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email);
                        break;
                    case "address":
                        query = IsAscOrder ? query.OrderBy(x => x.Address) : query.OrderByDescending(x => x.Address);
                        break;
                    case "city":
                        query = IsAscOrder ? query.OrderBy(x => x.City) : query.OrderByDescending(x => x.City);
                        break;
                    case "team":
                        query = IsAscOrder ? query.OrderBy(x => x.Team.Name) : query.OrderByDescending(x => x.Team.Name);
                        break;
                }
            }

            var data = query.Select(x => new StoreViewModel()
            {
                Id = x.Id,
                StoreName = x.StoreName,
                StoreCode = x.StoreCode,
                TeamName = x.Team != null ? x.Team.Name : null,
                Phone = x.Phone,
                Address = x.Address,
                Email = x.Email,
                City = x.City,
                Desc = x.Desc,
                Active = x.Active
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<StoreViewModel> GetStores()
        {
            _logger.LogDebug($"Get all Staore");
            var stores = UnitOfWork.StoreRepo.GetAll().Where(x => x.Active).Select(x => new StoreViewModel()
            {
                Id = x.Id,
                StoreName = x.StoreName,
                StoreCode = x.StoreCode,
                Phone = x.Phone,
                Email = x.Email,
                City = x.City,
                Address = x.Address,
                Desc = x.Desc,
                Active = x.Active
            }).OrderBy(x => x.StoreName).ToList();

            return stores;
        }

        public StoreViewModel GetStore(int id)
        {
            _logger.LogDebug($"Store Detail service (Id: {id})");
            var entity = UnitOfWork.StoreRepo.GetById(id);
            if (entity != null)
            {
                var model = new StoreViewModel()
                {
                    Id = entity.Id,
                    StoreName = entity.StoreName,
                    StoreCode = entity.StoreCode,
                    TeamId = entity.TeamId,
                    Phone = entity.Phone,
                    Email = entity.Email,
                    Address = entity.Address,
                    City = entity.City,
                    Desc = entity.Desc,
                    Active = entity.Active
                };

                return model;
            }
            return null;
        }
        public void CreateStore(StoreViewModel model)
        {
            _logger.LogDebug($"Create Store (Name: {model.StoreName})");
            var entity = new Ats.Domain.Store.Models.Store
            {
                Id = model.Id,
                StoreName = model.StoreName,
                StoreCode = model.StoreCode,
                TeamId = model.TeamId,
                Phone = model.Phone,
                Address = model.Address,
                Email = model.Email,
                City = model.City,
                Desc = model.Desc,
                Active = model.Active
            };
            if (model.StoreCodeAutomatically)
            {
                entity.StoreCode = this.GenerateStoreCode();
                _logger.LogDebug($"Creating Store Code generated: {model.StoreCode}");
            }
            UnitOfWork.StoreRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateStore(StoreViewModel model)
        {
            _logger.LogDebug($"Edit Store (Id: {model.Id})");
            var entity = UnitOfWork.StoreRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.StoreName = model.StoreName;
                entity.TeamId = model.TeamId;
                entity.City = model.City;
                entity.Email = model.Email;
                entity.Address = model.Address;
                entity.Phone = model.Phone;
                entity.Desc = model.Desc;
                entity.Active = model.Active;
                UnitOfWork.StoreRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteStore(int id)
        {
            _logger.LogDebug($"Delete Store (Id: {id})");
            var entity = UnitOfWork.StoreRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.StoreRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public List<Store> GetStoreCategories()
        {
            _logger.LogDebug($"Get Store Categories Enter.");
            var channelCategories = UnitOfWork.StoreRepo.GetAll().Where(x => x.Active).Select(x => new Store()
            {
                Id = x.Id,
                StoreName = x.StoreName
            }).OrderBy(x => x.StoreName).ToList();

            return channelCategories;
        }

        public async Task<object> ImportPurchasedTransCSV(PurchasedTransColumnMappingViewModel purchasedTrans, IFormFile formFile, Guid userId)
        {
            int totalRow = 0;
            int batchCount = 0;
            int totalSuccess = 0;
            int batchCountToSave = 0;
            int.TryParse(_config.GetValue<string>("CSVPurchasedTransImportBatchSave"), out batchCountToSave);
            char csvDelimited = ';';
            char.TryParse(_config.GetValue<string>("CSVDelimited"), out csvDelimited);

            if (batchCountToSave != 0) batchCountToSave = 100;

            _logger.LogDebug($"Start import PurchasedTrans from CSV ({csvDelimited}) {formFile.FileName}. UserId: {userId}");
            Stopwatch sw = new Stopwatch();
            sw.Start();


            try
            {
                //Save file csv
                string csvFileUrl = SaveFile(formFile, userId);
                List<PurchasedTransExcelViewModel> excelPurchasedTrans = new List<PurchasedTransExcelViewModel>();
                var csvFilePath = Path.Combine(_hostingEnvironment.WebRootPath, csvFileUrl);
                FileInfo newFile = new FileInfo(csvFilePath);

                _logger.LogDebug($"Loading CSV file {csvFilePath}.");

                List<String[]> rows = Ats.Commons.CSVParser.Import(csvFilePath, csvDelimited, true, false);
                totalRow = rows.Count;

                _logger.LogDebug($"Loaded CSV file {csvFilePath}. Total rows: {totalRow}");

                Dictionary<PURCHASEDTRANS_IMPORT, int> order = new Dictionary<PURCHASEDTRANS_IMPORT, int>();
                foreach (PurchasedTransItemViewModel mapping in purchasedTrans.PurchasedTransColumnMap)
                {
                    if (mapping.Col.HasValue)
                        order.Add(mapping.FieldCode, mapping.Col.Value);
                }

              

                _logger.LogDebug($"Start extracting data {csvFilePath}. Batch Save: {batchCountToSave} items/save");

                List<PurchasedTransExcelViewModel> purchasedTransExcels = new List<PurchasedTransExcelViewModel>();
                foreach (String[] row in rows)
                {

                    PurchasedTransExcelViewModel purchasedTran = new PurchasedTransExcelViewModel();

                    // INVOICE_NO
                    if (order.ContainsKey(PURCHASEDTRANS_IMPORT.INVOICE_NO))
                    {
                        purchasedTran.InvoiceNo = row[order[PURCHASEDTRANS_IMPORT.INVOICE_NO]];
                    }
                    else // generate InvoiceNo
                    {
                        purchasedTran.InvoiceNo = this.GenerateStoreCode();
                    }

                    // REF_ID
                    if (order.ContainsKey(PURCHASEDTRANS_IMPORT.REF_ID))
                    {
                        purchasedTran.RefId = row[order[PURCHASEDTRANS_IMPORT.REF_ID]];
                    }

                    // AMOUNT
                    if (order.ContainsKey(PURCHASEDTRANS_IMPORT.AMOUNT))
                    {
                        purchasedTran.Amount = row[order[PURCHASEDTRANS_IMPORT.AMOUNT]];
                    }

                    // PURCHASED_DATE
                    if (order.ContainsKey(PURCHASEDTRANS_IMPORT.PURCHASED_DATE))
                    {
                        purchasedTran.PurchasedDates = row[order[PURCHASEDTRANS_IMPORT.PURCHASED_DATE]];
                    }

                    // CURRENCY
                    if (order.ContainsKey(PURCHASEDTRANS_IMPORT.CURRENCY))
                    {
                        purchasedTran.Currency = row[order[PURCHASEDTRANS_IMPORT.CURRENCY]];
                    }

                    // validation
                    if (String.IsNullOrEmpty(purchasedTran.InvoiceNo))
                    {
                        // not qualify to import
                        continue;
                    }

                    // MEMBER_REFERENCE
                    if (order.ContainsKey(PURCHASEDTRANS_IMPORT.MEMBER_REFERENCE))
                    {
                        purchasedTran.MemberReference = row[order[PURCHASEDTRANS_IMPORT.MEMBER_REFERENCE]];

                    }

                    // REMARK
                    if (order.ContainsKey(PURCHASEDTRANS_IMPORT.REMARK))
                    {
                        purchasedTran.Remark = row[order[PURCHASEDTRANS_IMPORT.REMARK]];
                    }

                   purchasedTransExcels.Add(purchasedTran);

                    if (purchasedTransExcels.Count % batchCountToSave == 0)
                    {
                        _logger.LogDebug($"Import Member Extracting data {csvFilePath}. Batch Count: {batchCount}");
                        SavePurchasedTrans(purchasedTransExcels);
                        purchasedTransExcels.Clear();
                        _logger.LogDebug($"Imported Member data {csvFilePath}. Batch Count: {batchCount}");
                        batchCount++;
                    }
                };

                if (purchasedTransExcels.Count != 0)
                {
                    _logger.LogDebug($"Import Member Extract data {csvFilePath}. Batch Count: {batchCount}");
                    SavePurchasedTrans(purchasedTransExcels);
                    totalSuccess += purchasedTransExcels.Count;
                    purchasedTransExcels.Clear();
                    _logger.LogDebug($"Imported Member data {csvFilePath}. Batch Count: {batchCount}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"CSV PurchasedTrans import error: {ex.Message}");
                _logger.LogError($"CSV PurchasedTrans import exception: {ex}");
            }
            finally
            {
                sw.Stop();
                totalSuccess += batchCount * batchCountToSave;
                _logger.LogDebug($"CSV PurchasedTrans imported '{formFile.FileName}' completed {totalSuccess}/{totalRow} - Time (hh:mm:ss): {sw.Elapsed.Minutes}:{sw.Elapsed.Seconds}:{sw.Elapsed.Milliseconds}");
            }

            return new ResponseData(true, 200, "Success", new { TotalRow = totalRow, Success = totalSuccess });
        }
        private void SavePurchasedTrans(List<PurchasedTransExcelViewModel> purchasedTrans)
        {
            try
            {
                this.UnitOfWork.BeginTransaction();

                foreach (PurchasedTransExcelViewModel model in purchasedTrans)
                {
                    this.createPurchasedTransEntity(model);
                }

                this.UnitOfWork.SaveChanges();
                this.UnitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Save import Purchased Trans error:{ex.Message}");
                _logger.LogError($"Save import Purchased Trans exception:{ex}");

                this.UnitOfWork.RollbackTransaction();

                throw ex;
            }
        }

        private Guid createPurchasedTransEntity(PurchasedTransExcelViewModel model)
        {
            PurchasedTransaction purchased = new PurchasedTransaction();


            purchased.Id = Guid.NewGuid();
            purchased.InvoiceNo = model.InvoiceNo;
            purchased.RefId = model.RefId;
            purchased.Amount = Convert.ToDouble(model.Amount);
            purchased.PurchasedDate = DateTime.Now;
            purchased.Currency = model.Currency;
            purchased.Remark = model.Remark;
            purchased.MemberReference = model.MemberReference;
            purchased.ProcessDate = DateTime.UtcNow;
            

            UnitOfWork.PurchasedTransactionRepo.Insert(purchased);
            return purchased.Id;
        }
        private string SaveFile(IFormFile file, Guid userId)
        {
            var path = $"{_config.GetValue<string>("FolderImportUploadTemp")}";
            var serverPath = Path.Combine(_hostingEnvironment.WebRootPath, path);

            DirectoryInfo directory = new DirectoryInfo(serverPath);
            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now.ToString("ddMMyyyyhhmmss")}{extension}";
            var filePath = Path.Combine(serverPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            string url = $"{path}/{fileName}";
            return url;
        }
        private string GenerateStoreCode()
        {
            var generator = new RandomGenerator();
            return generator.RandomCode();
        }
    }
}
