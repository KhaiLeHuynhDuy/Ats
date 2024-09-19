using System.ComponentModel.DataAnnotations;
using System.Configuration;


namespace Ats.Commons
{
    public static class Constants
    {
        public enum Month
        {
            [Display(Name = "01")]
            JAN = 1,
            [Display(Name = "02")]
            FEB = 2,
            [Display(Name = "03")]
            MAR = 3,
            [Display(Name = "04")]
            APR = 4,
            [Display(Name = "05")]
            MAY = 5,
            [Display(Name = "06")]
            JUN = 6,
            [Display(Name = "07")]
            JUL = 7,
            [Display(Name = "08")]
            AUG = 8,
            [Display(Name = "09")]
            SEP = 9,
            [Display(Name = "10")]
            OCT = 10,
            [Display(Name = "11")]
            NOV = 11,
            [Display(Name = "12")]
            DEC = 12,
        }

        //public enum Month
        //{
        //    NotSet = 0,
        //    January = 1,
        //    February = 2,
        //    March = 3,
        //    April = 4,
        //    May = 5,
        //    June = 6,
        //    July = 7,
        //    August = 8,
        //    September = 9,
        //    October = 10,
        //    November = 11,
        //    December = 12
        //}

        public enum Claims
        {
            User = 0,
            Administrator = 1,
            Staff = 2,
            Manager = 3
        };
        public enum SumOrder
        {
            Name_az = 0,
            Name_za = 1,
            Add_newest = 2,
            Add_oldest = 3,
            Update_newest = 4,
            Update_oldest = 5,
            Code_az = 6,
            Code_za = 7,
            Email_az = 8,
            Email_za = 9
        }
        public enum Gender
        {
            [Display(Name = "Male")]
            MALE = 0,
            [Display(Name = "Female")]
            FEMALE = 1,
            [Display(Name = "Other")]
            OTHER = 2
        }
        //public const string SYS_USER_GROUP_UNAUTHORIZED_USERS = "Unauthorized Users";
        //public const string SYS_USER_GROUP_STAFFS = "Staffs";
        //public const string SYS_USER_GROUP_USERS = "User";
        //public const string SYS_USER_GROUP_PROJECT_MANAGERS = "Project Managers";

        public const string SYS_USER_GROUP_MEMBER = "Member";
        public const string SYS_USER_GROUP_SYSTEM_ADMINISTRATORS = "System Admin";  
        public const string SYS_USER_GROUP_MANAGERS = "Managers";
        public const string SYS_USER_GROUP_MARKETING = "Marketing";
        public const string SYS_USER_GROUP_EMPLOYEE = "Employee";
        

        public const string SYS_ROLE_SYSTEM_ADMINISTRATION = "system_administration";
        public const string SYS_ROLE_USER_MANAGEMENT = "user_management";

        public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL = "department_management_all";
        public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_VIEW = "department_management_view";
        public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT = "department_management_edit";
        public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_DELETE = "department_management_delete";
        public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_REPORT = "department_management_report";

        public const string ENTITY_FRAMEWORK_CONNECTION_STRING = "LpnDbContext";

        public static string COOKIE_PATH = "";
        public static string CONFIGURATION_AUDIENCE_SECRET = ConfigurationManager.AppSettings["as:AudienceSecret"];
        public static string CONFIGURATION_AUDIENCE_ID = ConfigurationManager.AppSettings["as:AudienceId"];
        public static int ACCESSTOKEN_EXPIRE_TIMESPAN_MINUTES = 0;
        public static int TOKEN_LIFESPAN_MINUTES = 60;
        public static int PASSWORD_MIN_LENGHT = 8;

        public const string DateFormat = "dd/MM/yyyy";
        public const string NumberFormat = "{0:#,#}";

        public static class RoleName
        {
            //public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL = "department_management_all";
            //public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_VIEW = "department_management_view";
            //public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT = "department_management_edit";
            //public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_DELETE = "department_management_delete";
            //public const string SYS_ROLE_DEPARTMENT_MANAGEMENT_REPORT = "department_management_report";

            #region Members
            public const string SYS_ROLE_MEMBER_MANAGEMENT_VIEW = "member_management_view"; 
            public const string SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE = "member_management_create_edit_delete";

            //public const string SYS_ROLE_MEMBER_MANAGEMENT_INDEX = "member_management_index";
            //public const string SYS_ROLE_MEMBER_MANAGEMENT_CREATE = "member_management_create";
            //public const string SYS_ROLE_MEMBER_MANAGEMENT_EDIT = "member_management_edit";
            //public const string SYS_ROLE_MEMBER_MANAGEMENT_DELETE = "member_management_delete";
            //public const string SYS_ROLE_MEMBER_MANAGEMENT_REPORT = "member_management_report";

            public const string SYS_ROLE_MEMBERTAG_MANAGEMENT_VIEW = "membertag_management_view";
            public const string SYS_ROLE_MEMBERTAG_MANAGEMENT_CREATE_EDIT_DELETE = "membertag_management_create_edit_delete";

            //public const string SYS_ROLE_MEMBERTAG_MANAGEMENT_INDEX = "membertag_management_index";
            //public const string SYS_ROLE_MEMBERTAG_MANAGEMENT_EDIT = "membertag_management_edit";
            //public const string SYS_ROLE_MEMBERTAG_MANAGEMENT_DELETE = "membertag_management_delete";
            //public const string SYS_ROLE_MEMBERTAG_MANAGEMENT_REPORT = "membertag_management_report";

            
            public const string SYS_ROLE_MEMBERGROUP_MANAGEMENT_VIEW = "membergroup_management_view";
            public const string SYS_ROLE_MEMBERGROUP_MANAGEMENT_CREATE_EDIT_DELETE = "membergroup_management_create_edit_delete";

            //public const string SYS_ROLE_MEMBERGROUP_MANAGEMENT_INDEX = "membergroup_management_index";
            //public const string SYS_ROLE_MEMBERGROUP_MANAGEMENT_EDIT = "membergroup_management_edit";
            //public const string SYS_ROLE_MEMBERGROUP_MANAGEMENT_DELETE = "membergroup_management_delete";
            //public const string SYS_ROLE_MEMBERGROUP_MANAGEMENT_REPORT = "membergroup_management_report";

            
            public const string SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_VIEW = "membersegment_management_view";
            public const string SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_CREATE_EDIT_DELETE = "membersegment_management_create_edit_delete";

            //public const string SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_INDEX = "membersegment_management_index";
            //public const string SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_EDIT = "membersegment_management_edit";
            //public const string SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_DELETE = "membersegment_management_delete";
            //public const string SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_REPORT = "membersegment_management_report";

            
            public const string SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_VIEW = "memberlifecycle_management_view";
            public const string SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_CREATE_EDIT_DELETE = "memberlifecycle_management_create_edit_delete";

            //public const string SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_INDEX = "memberlifecycle_management_index";
            //public const string SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_EDIT = "memberlifecycle_management_edit";
            //public const string SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_DELETE = "memberlifecycle_management_delete";
            //public const string SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_REPORT = "memberlifecycle_management_report";
            #endregion Members

            #region Loyalty

            public const string SYS_ROLE_LOYALTY_MANAGEMENT_VIEW = "loyalty_management_view";
            public const string SYS_ROLE_LOYALTY_MANAGEMENT_CREATE_EDIT_DELETE = "loyalty_management_create_edit_delete";

            public const string SYS_ROLE_TIERRULE_MANAGEMENT_VIEW = "tierrule_management_view";
            public const string SYS_ROLE_TIERRULE_MANAGEMENT_CREATE_EDIT_DELETE = "tierrule_management_create_edit_delete";


            //public const string SYS_ROLE_TIERRULE_MANAGEMENT_INDEX = "tierrule_management_index"; 
            //public const string SYS_ROLE_TIERRULE_MANAGEMENT_EDIT = "tierrule_management_edit";
            //public const string SYS_ROLE_TIERRULE_MANAGEMENT_DELETE = "tierrule_management_delete";
            //public const string SYS_ROLE_TIERRULE_MANAGEMENT_REPORT = "tierrulet_management_report";

            
            public const string SYS_ROLE_REFERRAL_MANAGEMENT_VIEW = "referral_management_view";
            public const string SYS_ROLE_REFERRAL_MANAGEMENT_CREATE_EDIT_DELETE = "referral_management_create_edit_delete";

            //public const string SYS_ROLE_REFERRAL_MANAGEMENT_INDEX = "referral_management_index";
            //public const string SYS_ROLE_REFERRAL_MANAGEMENT_EDIT = "referral_management_edit";
            //public const string SYS_ROLE_REFERRAL_MANAGEMENT_DELETE = "referralt_management_delete";
            //public const string SYS_ROLE_REFERRAL_MANAGEMENT_REPORT = "referral_management_report";

            
            public const string SYS_ROLE_POINTRULE_MANAGEMENT_VIEW = "pointrule_management_view";
            public const string SYS_ROLE_POINTRULE_MANAGEMENT_CREATE_EDIT_DELETE = "pointrule_management_create_edit_delete";

            //public const string SYS_ROLE_POINTRULE_MANAGEMENT_INDEX = "pointrule_management_index";
            //public const string SYS_ROLE_POINTRULE_MANAGEMENT_EDIT = "pointrule_management_edit";
            //public const string SYS_ROLE_POINTRULE_MANAGEMENT_CREATE = "pointrule_management_create";
            //public const string SYS_ROLE_POINTRULE_MANAGEMENT_DELETE = "pointrule_management_delete";
            //public const string SYS_ROLE_POINTRULE_MANAGEMENT_REPORT = "pointrule_management_report";

           
            public const string SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_VIEW = "pointrulestandard_management_view";
            public const string SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_CREATE_EDIT_DELETE = "pointrulestandard_management_create_edit_delete";

            //public const string SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_INDEX = "pointrulestandard_management_index";
            //public const string SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_EDIT = "pointrulestandard_management_edit";
            //public const string SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_DELETE = "pointrulestandard_management_delete";
            //public const string SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_REPORT = "pointrulestandard_management_report";
            #endregion Loyalty

            #region Campaign
            
            public const string SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_VIEW = "spotcampaign_management_view";
            public const string SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_CREATE_EDIT_DELETE = "spotcampaign_management_create_edit_delete";
            public const string SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_ANALYSH = "spotcampaign_management_analysh";

            //public const string SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_INDEX = "spotcampaign_management_index";
            //public const string SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_EDIT = "spotcampaign_management_edit";
            //public const string SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_CREATE = "spotcampaign_management_create";
            //public const string SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_DELETE = "spotcampaign_management_delete"; 
            //public const string SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_REPORT = "spotcampaign_management_report";

            
            public const string SYS_ROLE_RECURRINGCAMPAIGN_MANAGEMENT_VIEW = "recurringcampaign_management_view";
            public const string SYS_ROLE_RECURRINGCAMPAIGN_MANAGEMENT_CREATE_EDIT_DELETE = "recurringcampaign_management_create_edit_delete";

            //public const string SYS_ROLE_RECURRINGCAMPAIGN_MANAGEMENT_INDEX = "recurringcampaign_management_index";
            //public const string SYS_ROLE_RECURRINGCAMPAIGN_MANAGEMENT_EDIT = "recurringcampaign_management_edit";
            //public const string SYS_ROLE_RECURRINGCAMPAIGN_MANAGEMENT_CREATE = "recurringcampaign_management_create";
            //public const string SYS_ROLE_RECURRINGCAMPAIGN_MANAGEMENT_DELETE = "recurringcampaign_management_delete";
            //public const string SYS_ROLE_RECURRINGCAMPAIGN_MANAGEMENT_REPORT = "recurringcampaign_management_report";

            
            public const string SYS_ROLE_TRIGGERCAMPAIGN_MANAGEMENT_VIEW = "triggercampaign_management_view";
            public const string SYS_ROLE_TRIGGERCAMPAIGN_MANAGEMENT_CREATE_EDIT_DELETE = "triggercampaign_management_create_edit_delete";

            //public const string SYS_ROLE_TRIGGERCAMPAIGN_MANAGEMENT_INDEX = "triggercampaign_management_index";
            //public const string SYS_ROLE_TRIGGERCAMPAIGN_MANAGEMENT_EDIT = "triggercampaign_management_edit";
            //public const string SYS_ROLE_TRIGGERCAMPAIGN_MANAGEMENT_CREATE = "triggercampaign_management_create";
            //public const string SYS_ROLE_TRIGGERCAMPAIGN_MANAGEMENT_DELETE = "triggercampaign_management_delete";
            //public const string SYS_ROLE_TRIGGERCAMPAIGN_MANAGEMENT_REPORT = "triggercampaign_management_report";

           
            public const string SYS_ROLE_LIFECYCLECAMPAIGN_MANAGEMENT_VIEW = "lifecyclecampaign_management_view";
            public const string SYS_ROLE_LIFECYCLECAMPAIGN_MANAGEMENT_CREATE_EDIT_DELETE = "lifecyclecampaign_management_create_edit_delete";

            //public const string SYS_ROLE_LIFECYCLECAMPAIGN_MANAGEMENT_INDEX = "lifecyclecampaign_management_index";
            //public const string SYS_ROLE_LIFECYCLECAMPAIGN_MANAGEMENT_EDIT = "lifecyclecampaign_management_edit";
            //public const string SYS_ROLE_LIFECYCLECAMPAIGN_MANAGEMENT_CREATE = "lifecyclecampaign_management_create";
            //public const string SYS_ROLE_LIFECYCLECAMPAIGN_MANAGEMENT_DELETE = "lifecyclecampaign_management_delete";
            //public const string SYS_ROLE_LIFECYCLECAMPAIGN_MANAGEMENT_REPORT = "lifecyclecampaign_management_report";

            
            public const string SYS_ROLE_COUPON_MANAGEMENT_VIEW = "coupon_management_view";
            public const string SYS_ROLE_COUPON_MANAGEMENT_CREATE_EDIT_DELETE = "coupon_management_create_edit_delete";

            //public const string SYS_ROLE_COUPON_MANAGEMENT_INDEX = "coupon_management_index";
            //public const string SYS_ROLE_COUPON_MANAGEMENT_EDIT = "coupon_management_edit";
            //public const string SYS_ROLE_COUPON_MANAGEMENT_CREATE = "coupon_management_create";
            //public const string SYS_ROLE_COUPON_MANAGEMENT_DELETE = "coupon_management_delete";
            //public const string SYS_ROLE_COUPON_MANAGEMENT_REPORT = "coupon_management_report";

            
            public const string SYS_ROLE_COLLECTION_MANAGEMENT_VIEW = "collection_management_view";
            public const string SYS_ROLE_COLLECTION_MANAGEMENT_CREATE_EDIT_DELETE = "collection_management_create_edit_delete";

            //public const string SYS_ROLE_COLLECTION_MANAGEMENT_INDEX = "collection_management_index";
            //public const string SYS_ROLE_COLLECTION_MANAGEMENT_EDIT = "collection_management_edit";
            //public const string SYS_ROLE_COLLECTION_MANAGEMENT_CREATE = "collection_management_create";
            //public const string SYS_ROLE_COLLECTION_MANAGEMENT_DELETE = "collection_management_delete";
            //public const string SYS_ROLE_COLLECTION_MANAGEMENT_REPORT = "collection_management_report";

            
            
            public const string SYS_ROLE_ORDER_MANAGEMENT_VIEW = "order_management_view";
            public const string SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE = "order_management_create_edit_delete";

            public const string SYS_ROLE_ORDER_MANAGEMENT_INDEXINTRNSIT = "order_management_indexintransit";
            public const string SYS_ROLE_ORDER_MANAGEMENT_INDEXCOMPLETE = "order_management_indexcomplete";
            public const string SYS_ROLE_ORDER_MANAGEMENT_INDEXCANCEL = "order_management_indexcancel";
            //public const string SYS_ROLE_ORDER_MANAGEMENT_INDEX = "order_management_index";
            //public const string SYS_ROLE_ORDER_MANAGEMENT_EDIT = "order_management_edit";
            //public const string SYS_ROLE_ORDER_MANAGEMENT_CREATE = "order_management_create";
            //public const string SYS_ROLE_ORDER_MANAGEMENT_DELETE = "order_management_delete";
            //public const string SYS_ROLE_ORDER_MANAGEMENT_REPORT = "order_management_report";
            #endregion Campaign

            #region Report
            public const string SYS_ROLE_REPORT_MANAGEMENT_VIEW = "report_management_view";

            public const string SYS_ROLE_MEMBERREPORT_MANAGEMENT_VIEW = "memberreport_management_view";
            public const string SYS_ROLE_MEMBERREPORT_MANAGEMENT_CREATE_EDIT_DELETE = "memberreport_management_create_edit_delete";

            //public const string SYS_ROLE_MEMBERREPORT_MANAGEMENT_INDEX = "memberreport_management_index";
            //public const string SYS_ROLE_MEMBERREPORT_MANAGEMENT_EDIT = "memberreport_management_edit";
            //public const string SYS_ROLE_MEMBERREPORT_MANAGEMENT_DELETE = "memberreport_management_delete";
            //public const string SYS_ROLE_MEMBERREPORT_MANAGEMENT_REPORT = "memberreport_management_report";

            
            public const string SYS_ROLE_REWARDREPORD_MANAGEMENT_VIEW = "rewardreport_management_view";
            public const string SYS_ROLE_REWARDREPORD_MANAGEMENT_CREATE_EDIT_DELETE = "rewardreport_management_create_edit_delete";

            //public const string SYS_ROLE_REWARDREPORD_MANAGEMENT_INDEX = "rewardreport_management_index";
            //public const string SYS_ROLE_REWARDREPORD_MANAGEMENT_EDIT = "rewardreport_management_edit";
            //public const string SYS_ROLE_REWARDREPORD_MANAGEMENT_DELETE = "rewardreport_management_delete";
            //public const string SYS_ROLE_REWARDREPORD_MANAGEMENT_REPORT = "rewardreport_management_report";

            
            public const string SYS_ROLE_PRODUCTQRSCANREPROT_MANAGEMENT_VIEW = "productqrscanreport_management_view";
            public const string SYS_ROLE_PRODUCTQRSCANREPROT_MANAGEMENT_CREATE_EDIT_DELETE = "productqrscanreport_management_create_edit_delete";

            //public const string SYS_ROLE_PRODUCTQRSCANREPROT_MANAGEMENT_INDEX = "productqrscanreport_management_index";
            //public const string SYS_ROLE_PRODUCTQRSCANREPROT_MANAGEMENT_EDIT = "productqrscanreport_management_edit";
            //public const string SYS_ROLE_PRODUCTQRSCANREPROT_MANAGEMENT_DELETE = "productqrscanreport_management_delete";
            //public const string SYS_ROLE_PRODUCTQRSCANREPROT_MANAGEMENT_REPORT = "productqrscanreport_management_report";
            #endregion Report

            #region Tools
            public const string SYS_ROLE_IMPORT_MANAGEMENT_INDEX = "import_management_index";
            //public const string SYS_ROLE_MEMBERIMPORT_MANAGEMENT_VIEW = "memberimport_management_view";
            //public const string SYS_ROLE_MEMBERIMPORT_MANAGEMENT_EDIT = "memberimport_management_edit";
            //public const string SYS_ROLE_MEMBERIMPORT_MANAGEMENT_DELETE = "memberimport_management_delete";
            //public const string SYS_ROLE_MEMBERIMPORT_MANAGEMENT_REPORT = "memberimport_management_report";

            //public const string SYS_ROLE_FILEIMPORT_MANAGEMENT_VIEW = "fileimport_management_view";
            //public const string SYS_ROLE_FILEIMPORT_MANAGEMENT_EDIT = "fileimport_management_edit";
            //public const string SYS_ROLE_FILEIMPORT_MANAGEMENT_DELETE = "fileimport_management_delete";
            //public const string SYS_ROLE_FILEIMPORT_MANAGEMENT_REPORT = "fileimport_management_report";

            //public const string SYS_ROLE_TRANSACTIONS_MANAGEMENT_VIEW = "transactions_management_view";
            //public const string SYS_ROLE_TRANSACTIONS_MANAGEMENT_EDIT = "transactions_management_edit";
            //public const string SYS_ROLE_TRANSACTIONS_MANAGEMENT_DELETE = "transactions_management_delete";
            //public const string SYS_ROLE_TRANSACTIONS_MANAGEMENT_REPORT = "transactions_management_report";
            #endregion Tools

            #region Wallets
            public const string SYS_ROLE_WALLETS_MANAGEMENT_INDEX = "wallets_management_index";
    
            public const string SYS_ROLE_WALLETS_MANAGEMENT_DELETE = "wallets_management_delete";
            //public const string SYS_ROLE_WALLETS_MANAGEMENT_EDIT = "wallets_management_edit";
            //public const string SYS_ROLE_WALLETS_MANAGEMENT_REPORT = "wallets_management_report";
            #endregion Wallets

            #region Configuration

            #region Organization

            public const string SYS_ROLE_ORGANIZATION_MANAGEMENT_VIEW = "organization_management_view";
            public const string SYS_ROLE_ORGANIZATION_MANAGEMENT_CREATE_EDIT_DELETE = "organization_management_create_edit_delete";

            //public const string SYS_ROLE_ORGANIZATION_MANAGEMENT_INDEX = "organization_management_index";
            //public const string SYS_ROLE_ORGANIZATION_MANAGEMENT_CREATE = "organization_management_create";
            //public const string SYS_ROLE_ORGANIZATION_MANAGEMENT_EDIT = "organization_management_edit";
            //public const string SYS_ROLE_ORGANIZATION_MANAGEMENT_DELETE = "organization_management_delete";
            //public const string SYS_ROLE_ORGANIZATION_MANAGEMENT_REPORT = "organization_management_report";
            #endregion Organization

            #region Store 
            
            public const string SYS_ROLE_STORE_MANAGEMENT_VIEW = "store_management_view";
            public const string SYS_ROLE_STORE_MANAGEMENT_CREATE_EDIT_DELETE = "store_management_create_edit_delete";

            //public const string SYS_ROLE_STORE_MANAGEMENT_INDEX = "store_management_index";
            //public const string SYS_ROLE_STORE_MANAGEMENT_EDIT = "store_management_edit";
            //public const string SYS_ROLE_STORE_MANAGEMENT_CREATE = "store_management_create";
            //public const string SYS_ROLE_STORE_MANAGEMENT_DELETE = "store_management_delete";
            //public const string SYS_ROLE_STORE_MANAGEMENT_REPORT = "store_management_report";
            #endregion Store
     


            #region Users
           
            public const string SYS_ROLE_USERS_MANAGEMENT_VIEW = "users_management_view";
            public const string SYS_ROLE_USERS_MANAGEMENT_CREATE_EDIT_DELETE = "users_management_create_edit_delete";
            public const string SYS_ROLE_USERS_MANAGEMENT_RESETPASSWORD = "users_management_reset_password";

            //public const string SYS_ROLE_USERS_MANAGEMENT_INDEX = "users_management_index";
            //public const string SYS_ROLE_USERS_MANAGEMENT_EDIT = "users_management_edit";
            //public const string SYS_ROLE_USERS_MANAGEMENT_CREATE = "users_management_create";
            //public const string SYS_ROLE_USERS_MANAGEMENT_DELETE = "users_management_delete";
            //public const string SYS_ROLE_USERS_MANAGEMENT_REPORT = "users_management_report";
            #endregion Users

            #region Role Groups
            
            public const string SYS_ROLE_ROLEGROUPS_MANAGEMENT_VIEW = "rolegroups_management_view";
            public const string SYS_ROLE_ROLEGROUPS_MANAGEMENT_CREATE_EDIT_DELETE = "rolegroups_management_create_edit_delete";

            //public const string SYS_ROLE_ROLEGROUPS_MANAGEMENT_INDEX = "rolegroups_management_index";
            //public const string SYS_ROLE_ROLEGROUPS_MANAGEMENT_EDIT = "rolegroups_management_edit";
            //public const string SYS_ROLE_ROLEGROUPS_MANAGEMENT_CREATE = "rolegroups_management_create";
            //public const string SYS_ROLE_ROLEGROUPS_MANAGEMENT_DELETE = "rolegroups_management_delete";
            //public const string SYS_ROLE_ROLEGROUPS_MANAGEMENT_REPORT = "rolegroups_management_report";
            #endregion Role Groups

            #region Team
            
            public const string SYS_ROLE_TEAM_MANAGEMENT_VIEW = "team_management_view";
            public const string SYS_ROLE_TEAM_MANAGEMENT_CREATE_EDIT_DELETE = "team_management_create_edit_delete";

            //public const string SYS_ROLE_TEAM_MANAGEMENT_INDEX = "team_management_index";
            //public const string SYS_ROLE_TEAM_MANAGEMENT_CREATE = "team_management_create";
            //public const string SYS_ROLE_TEAM_MANAGEMENT_EDIT = "team_management_edit";
            //public const string SYS_ROLE_TEAM_MANAGEMENT_DELETE = "team_management_delete";
            //public const string SYS_ROLE_TEAM_MANAGEMENT_REPORT = "team_management_report";

            public const string SYS_ROLE_TEAMUSER_MANAGEMENT_CREATE = "teamuser_management_create";
            public const string SYS_ROLE_TEAMUSER_MANAGEMENT_EDIT = "teamuser_management_edit";
            public const string SYS_ROLE_TEAMUSER_MANAGEMENT_DELETE = "teamuser_management_delete";
            #endregion Team

            #region Departments
           
            public const string SYS_ROLE_DEPARTMENTS_MANAGEMENT_VIEW = "departments_management_view";
            public const string SYS_ROLE_DEPARTMENTS_MANAGEMENT_CREATE_EDIT_DELETE = "departments_management_create_edit_delete";

            //public const string SYS_ROLE_DEPARTMENTS_MANAGEMENT_INDEX = "departments_management_index";
            //public const string SYS_ROLE_DEPARTMENTS_MANAGEMENT_CREATE = "departments_management_create";
            //public const string SYS_ROLE_DEPARTMENTS_MANAGEMENT_EDIT = "departments_management_edit";
            //public const string SYS_ROLE_DEPARTMENTS_MANAGEMENT_DELETE = "departments_management_delete";
            //public const string SYS_ROLE_DEPARTMENTS_MANAGEMENT_REPORT = "departments_management_report";

            public const string SYS_ROLE_DEPARTMENTSUSER_MANAGEMENT_CREATE = "departmentsuser_management_create";
            public const string SYS_ROLE_DEPARTMENTSUSER_MANAGEMENT_EDIT = "departmentsuser_management_edit";
            public const string SYS_ROLE_DEPARTMENTSUSER_MANAGEMENT_DELETE = "departmentsuser_management_delete";
            #endregion Departments        

         

            #region Loyalty Tier
         

            //public const string SYS_ROLE_LOYALTYTIER_MANAGEMENT_INDEX = "loyaltytier_management_index";
            //public const string SYS_ROLE_LOYALTYTIER_MANAGEMENT_EDIT = "loyaltytier_management_edit";
            //public const string SYS_ROLE_LOYALTYTIER_MANAGEMENT_CREATE = "loyaltytier_management_create";
            //public const string SYS_ROLE_LOYALTYTIER_MANAGEMENT_DELETE = "loyaltytier_management_delete";
            //public const string SYS_ROLE_LOYALTYTIERS_MANAGEMENT_REPORT = "loyaltytier_management_report";
            #endregion Loyalty Tier

            #region Loyalty Point Type
           
         
            //public const string SYS_ROLE_LOYALTYPOINTTYPE_MANAGEMENT_INDEX = "loyaltypointtype_management_index";
            //public const string SYS_ROLE_LOYALTYPOINTTYPE_MANAGEMENT_EDIT = "loyaltypointtype_management_edit";
            //public const string SYS_ROLE_LOYALTYPOINTTYPE_MANAGEMENT_CREATE = "loyaltypointtype_management_create";
            //public const string SYS_ROLE_LOYALTYPOINTTYPE_MANAGEMENT_DELETE = "loyaltypointtype_management_delete";
            //public const string SYS_ROLE_LOYALTYPOINTTYPE_MANAGEMENT_REPORT = "loyaltypointtypes_management_report";
            #endregion Loyalty Point Type

        


      

          

            #region Unit
            public const string SYS_PRODUCT_MANAGEMENT_VIEW = "product_management_view";
            public const string SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE = "product_management_function";


            public const string SYS_CATEGORY_MANAGEMENT_VIEW = "category_management_view";
            public const string SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE = "category_management_function";
            //public const string SYS_ROLE_UNIT_MANAGEMENT_INDEX = "unit_management_index";
            //public const string SYS_ROLE_UNIT_MANAGEMENT_EDIT = "unit_management_edit";
            //public const string SYS_ROLE_UNIT_MANAGEMENT_CREATE = "unit_management_create";
            //public const string SYS_ROLE_UNIT_MANAGEMENT_DELETE = "unit_management_delete";
            //public const string SYS_ROLE_UNIT_MANAGEMENT_REPORT = "unit_management_report";
            #endregion Unit

            #endregion Configuration

        }
        public enum keyClaims
        {
            membershipRegistratiom = 1,
        }
        public enum GroupRoles
        {
            Member = 0,
            SystemAdmin = 1,
            Manager = 2,
            Marketing = 3,
            Employee = 4 
        };
        public static class GroupName
        {

        }
        public static class LoyaltyPointTypeNames
        {
            public const string RedemptionPoints = "Redemption Points";
        }

        public static class Claimskey
        {
            public const string SYS_CLAIMS_KEY = "MEMBERSHIP_REGISTRATION_LIMIT";

        };
    }
}
