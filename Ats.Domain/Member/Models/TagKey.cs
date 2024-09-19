using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    public enum TagGroup
    {
        [Display(Name = "")]
        NULL = 0,
        [Display(Name = "Perfonal")]
        PERSONAL = 1,
        [Display(Name = "Brand")]
        BRAND = 2,
        [Display(Name = "Loyalty")]
        LOYALTY = 3,
        [Display(Name = "Referral")]
        REFERRAL = 4,
        [Display(Name = "Transaction")]
        TRANSACTION = 5,
        [Display(Name = "LifeCycle")]
        LIFE_CYCLE = 6,
        [Display(Name = "Communication")]
        COMMUNICATION = 7,
        [Display(Name = "Other")]
        OTHER = 8,
    }

    public enum DataFormat
    {
        [Display(Name = "Text")]
        TEXT = 0,
        [Display(Name = "Number")]
        NUMBER = 1,
        [Display(Name = "Check")]
        CHECK = 2,
        [Display(Name = "SingleSelection")]
        SINGLE_SELECTION = 3,
        [Display(Name = "Checklist")]
        CHECK_LIST = 4,
        [Display(Name = "MultiSelection")]
        MULTI_SELECTION = 5,
        [Display(Name = "Date")]
        DATE = 6,
        [Display(Name = "Range")]
        RANGE = 7        
    }

    [Table("tagkeys")]
    public class TagKey : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public TagGroup? TagGroup { get; set; }
        public DataFormat? DataFormat { get; set; }
        public string DefaultValue { get; set; }
        public string Remark { get; set; }
    }
}
