using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ats.Commons.Resource;

namespace Ats.Domain
{
    public enum DATA_TYPE
    {
        [Display(Name = "Text")]
        TEXT = 0,
        [Display(Name = "Number")]
        NUMBER = 1,
        [Display(Name = "Boolean")]
        BOOLEAN = 2,
    }

    public enum GENDER
    {
        [Display(Name = "Unspecified")]
        UNSPECIFIED = 0,
        [Display(Name = "Male")]
        MALE = 1,
        [Display(Name = "Female")]
        FAMALE = 2,
    }

    public enum MARITAL_STATUS
    {
        [Display(Name = "")]
        NULL = 0,
        [Display(Name = "Độc Thân")]
        SINGLE = 1,
        [Display(Name = "Đã Kết Hôn")]
        MARRIED = 2,
        [Display(Name = "Ly Dị")]
        DIVORCED = 3,
        [Display(Name = "Ly Thân")]
        WIDOWED = 4,
    }
    //public enum TAG_KEY

    public enum TIER_LEVEL
    {
        [Display(Name = "Silver")]
        SILVER = 1,
        [Display(Name = "Gold")]
        GOLD = 2,
        [Display(Name = "Diamond")]
        DIAMOND = 3,
    }
    public enum TAG_KEY
    {

        NOT_MAP=0,
        [Display(Name = "Gender")]
        GENDER = 1,
        [Display(Name = "Age")]
        AGE = 2,
        [Display(Name = "Birthday")]
        BIRTHDAY = 3,
        [Display(Name = "Birthday Month")]
        BIRTHDAY_MONTH = 4,
        [Display(Name = "Country")]
        COUNTRY = 5,
        [Display(Name = "State")]
        STATE = 6,
        [Display(Name = "City")]
        CITY = 7,
        [Display(Name = "Wedding Aniversaty")]
        WEDDING_ANIVERSATY = 8,
        [Display(Name = "Merried Years")]
        MARRIED_YEARS = 9,
        [Display(Name = "Wedding Month")]
        WEDDING_MONTH = 34,
        [Display(Name = "Married Status")]
        MARRIRD_STATUS = 10,
        [Display(Name = "Preferred Brand")]
        PREFERRED_BRAND = 11,
        [Display(Name = "Preferred Store")]
        PREFERRED_STORE = 12,
        [Display(Name = "Preferrer Product")]
        PREFERRER_PRODUCT = 13,
        [Display(Name = "Tier")]
        TIER = 14,
        [Display(Name = "Register Date")]
        REGISTER_DATE = 15,
        [Display(Name = "Available Point")]
        AVAILABLE_POINT = 16,
        [Display(Name = "Register Store")]
        REGISTER_STORE = 17,
        [Display(Name = "Register Channel")]
        REGISTER_CHANNEL = 18,
        [Display(Name = "Referrer")]
        REFERRER = 19,
        [Display(Name = "Referee")]
        REFEREE = 20,
        [Display(Name = "Referrers")]
        REFERRERS = 21,
        [Display(Name = "First Transaction Date")]
        FIRST_TRANSACTION= 22,
        [Display(Name = "Last Transaction Date")]
        LAST_TRANSACTION= 23,
        [Display(Name = "Number Of Transaction")]
        NUMBER_TRANSACTION = 24,
        [Display(Name = "Amount Of Transaction")]
        AMOUNT_TRANSACTION = 25,
        [Display(Name = "Average Basket")]
        AVERAGE_BASKET = 26,
        [Display(Name = "Average Items")]
        AVERAGE_ITEMS = 27,
        [Display(Name = "Purchased Brand")]
        PURCHASED_BRAND = 28,
        [Display(Name = "Purchased Category")]
        PURCHASED_CATEGORY = 29,
        [Display(Name = "Purchased Store")]
        PURCHASED_STORE = 30,
        [Display(Name = "Purchased Product")]
        PURCHASED_PRODUCT = 31,
        [Display(Name = "Member LifeCycle")]
        MEMBER_LIFECYCLE = 32,
        [Display(Name = "Communication preferences")]
        COMMUNICATION_PREFERENCES = 33,      
    }

    public enum MEMBER_LIFECYCLE
    {
        [Display(Name = "Unspecified")]
        UNSPECIFIED = 0,
        [Display(Name = "New Prospects")]
        NEW_PROSPECTS = 1,
        [Display(Name = "Sleeping Prospects")]
        SLEEPING_PROSPECTS = 2,
        [Display(Name = "First Purchasers")]
        FIRST_PURCHASERS = 3,
        [Display(Name = "Repeat Purchasers")]
        REPEAT_PURCHASERS = 4,
        [Display(Name = "Re-purchasers")]
        REPURCHASERS = 5,
        [Display(Name = "Loyal Purchasers")]
        LOYAL_PURCHASERS = 6,
        [Display(Name = "Sleeping Purchasers")]
        SLEEPING_PURCHASERS = 7,
        [Display(Name = "Lapsed Purchasers")]
        LAPSED_PURCHASERS = 8,
    }
    public enum NameMonths
    {
        [Display(Name  ="Month(s)")]
        Month = 0,
        [Display(Name = "Tháng 1")]
        JAN = 1,
        [Display(Name = "Tháng 2")]
        FEB = 2,
        [Display(Name = "Tháng 3")]
        MAR = 3,
        [Display(Name = "Tháng 4")]
        APR = 4,
        [Display(Name = "Tháng 5")]
        MAY = 5,
        [Display(Name = "Tháng 6")]
        JUN = 6,
        [Display(Name = "Tháng 7")]
        JUL = 7,
        [Display(Name = "Tháng 8")]
        AUG = 8,
        [Display(Name = "Tháng 9")]
        SEP = 9,
        [Display(Name = "Tháng 10")]
        OCT = 10,
        [Display(Name = "Tháng 11")]
        NOV = 11,
        [Display(Name = "Tháng 12")]
        DEC = 12,
    }
   
    public enum EXPIRY_COUPON
    {  
        NOT_MAP = 0,
        [Display(Name = "Expired")]
        EXPIRED = 1,
        [Display(Name = "Out of đate")]
        OUT_OF_DATE = 2,
    }

    public enum COMMUNICATION_CHANNEL
    {
        [Display(Name = "Panda Loyalty")]
        PANDA_LOYALTY = 1,
        [Display(Name = "SMS")]
        SMS = 2,
        [Display(Name = "Email")]
        EMAIL = 3,
        [Display(Name = "Zalo")]
        ZALO = 4,
    }

    public enum MEMBER_CHANNEL
    {
        [Display(Name = "Panda Loyalty")]
        PANDA_LOYALTY = 1,
        [Display(Name = "Store-POS")]
        STORE_POS = 2,
        [Display(Name = "Corporate Web")]
        CORPORATE_WEB = 3,
        [Display(Name = "Ecommerce Web")]
        ECOMMERCE_WEB = 4,
        [Display(Name = "Zalo")]
        ZALO = 5,
    }
}
