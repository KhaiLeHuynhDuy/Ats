using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    public enum LOAN_PERIOD
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "3 Tháng")]
        THREE_MONTH = 1,
        [Display(Name = "6 Tháng")]
        SIX_MONTH = 2,
        [Display(Name = "9 Tháng")]
        NINE_MONTH = 3,
        [Display(Name = "12 Tháng")]
        TWELVE_MONTH = 4,
        [Display(Name = "Trên 12 Tháng")]
        OVER_TWELVE_MONTH = 5,
    }
    public enum LOAN_PURPOSE
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Vay Tiền Tín Chấp")]
        UNSECURED_LOAN = 1,
        [Display(Name = "Vay Tiền Thế Chấp")]
        MORTGAGE_LOAN = 2,
        [Display(Name = "Cần Mở Thẻ Vay")]
        APPLY_LOAN_CARD = 3,
        [Display(Name = "Cần Cầm Đồ")]
        PAWN = 4,
        [Display(Name = "Cần Mở Thẻ Tín Dụng")]
        APPLY_CREDIT_CARD = 5,
        [Display(Name = "Cần Mua Bảo Hiểm Nhân Thọ")]
        PURCHASE_LIFE_INSURANCE = 6,
    }    

    public enum INCOME_RECEIVED_METHOD
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Tiền Mặt")]
        CASH = 1,
        [Display(Name = "Chuyển Khoản")]
        BANK_TRANSFER = 2,
        [Display(Name = "Chuyển Khoản và Tiền Mặt")]
        BANK_TRANSFER_CASH = 3,
    }

    public enum VEHICLE_REGISTRATION_CERTIFICATE
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Không Có Đứng Tên")]
        NONE = 1,
        [Display(Name = "Có Cavet Xe Máy")]
        MOTORCYCLE = 2,
        [Display(Name = "Có Cavet Xe Ôtô")]
        AUTOMOBILE = 3,
        [Display(Name = "Có Cavet Xe Tải")]
        TRUCK = 4,
    }
    public enum HEALTH_INSURANCE
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Không Có Mua")]
        NONE = 1,
        [Display(Name = "Nơi Làm Việc Tự Mua")]
        FULLY_INSURED = 2,
        [Display(Name = "Cá Nhân Tự Mua")]
        SELF_INSURED = 3,
    }
    public enum LIFE_INSURANCE
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Không Có Mua")]
        NONE = 1,
        [Display(Name = "Đã Có Mua")]
        HAS_BOUGHT = 2,
        [Display(Name = "Đang Cần Mua")]
        NEED_TO_BUY = 3,
    }
    public enum ELECTRICITY_BILL
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Không Có")]
        NONE = 1,
        [Display(Name = "Có Đứng Tên")]
        IN_OWN_NAME = 2,
        [Display(Name = "Cha/Mẹ Đứng Tên")]
        IN_PARENT_NAME = 3,
    }
    public enum INTERNET_BILL
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Không Có")]
        NONE = 1,
        [Display(Name = "Có Đứng Tên")]
        IN_OWN_NAME = 2,
    }
    public enum BANK_ACCOUNT
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Không Có")]
        NONE = 1,
        [Display(Name = "Có Thẻ ATM")]
        DEBIT_CARD = 2,
        [Display(Name = "Có Thẻ Tín Dụng")]
        CREDIT_CARD = 3,
        [Display(Name = "Có Thẻ ATM và Thẻ Tín Dụng")]
        DEBIT_AND_CREDIT = 4,
    }  
    public enum SIM_CARD
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Thuê Bao Trả Trước")]
        PRE_PAID = 1,
        [Display(Name = "Thuê Bao Trả Sau")]
        POST_PAID = 2,
    }       

    public enum Register_Channel
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Đăng ký thất bại")]
        Failed = 1,
        [Display(Name = "Đăng ký thành công")]
        successful = 2,
    }
    public enum Register_Store
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Đăng ký thất bại")]
        Failed = 1,
        [Display(Name = "Đăng ký thành công")]
        successful = 2,
    }
    public enum Register_Employee
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Đăng ký thất bại")]
        Failed = 1,
        [Display(Name = "Đăng ký thành công")]
        successful = 2,
    }
    public enum LeadGroup
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Upload")]
        SINGLE = 1,
        [Display(Name = "Target")]
        MARRIED = 2,
        [Display(Name = "Tag")]
        DIVORCED = 3,
        [Display(Name = "Group")]
        WIDOWED = 4,
    }
}
