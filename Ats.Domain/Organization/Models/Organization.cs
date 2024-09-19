using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Organization.Models
{
    [Table("organizations")]
    public class Organization : AuditBase, IEntity<Guid>
    {
        public Organization()
        {
            Debts = new HashSet<Debt>();
            Incomes = new HashSet<Income>();
            Warehouses = new HashSet<Warehouse>();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ORGANIZATION_TYPE OrganizationType { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Debt> Debts { get; set; }
        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<Warehouse> Warehouses { get; set; }
    }
    public enum ORGANIZATION_TYPE
    {
        [Display(Name = "Công Ty")]
        COMPANY = 1,
        [Display(Name = "Ngân Hàng")]
        BANK = 2,
        [Display(Name = "Công Ty Tài Chính")]
        FINANCE_COMPANY = 3,
        [Display(Name = "Bên Cho Vay")]
        LENDER = 4,
        [Display(Name = "Tập Đoàn")]
        CORPORATION = 5,
        [Display(Name = "Chi Nhánh")]
        BRANCH = 6,
        [Display(Name = "Phòng Nội Bộ")]
        INTERNAL_DEPARTMENT = 7,
    }
}
