using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Domain.Member.Models
{

    public enum MEMBER_IMPORT
    {
        [Display(Name = "")]
        NOT_MAP = 0,
        [Display(Name = "Code")]
        MEMBERCODE = 1,
        [Display(Name = "FirstName")]
        FIRSTNAME = 2,
        [Display(Name = "LastName")]
        LASTNAME = 3,
        [Display(Name = "Gender")]
        GENDER = 4,
        [Display(Name = "Phone")]
        PHONE = 5,
        [Display(Name = "Email")]
        EMAIL = 6,
        [Display(Name = "Address")]
        ADDRESS1 = 7,
        [Display(Name = "Marritial")]
        MARITAL_STATUS = 8,
        [Display(Name = "Channel")]
        REGISTERCHANNEL = 9,
        [Display(Name = "Store")]
        REGISTERSTORE = 10,
        [Display(Name = "JobTitle")]
        JOBTITLE = 11,
        [Display(Name = "Province")]
        PROVINCE = 12,
        [Display(Name = "District")]
        DISTRICT = 13,
        [Display(Name = "YOB")]
        YOB = 14,
        [Display(Name = "DOB")]
        DOB = 15,
    }
}
