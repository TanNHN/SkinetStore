using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class AddressDTO
{
    [Required]
    public string StreetAddress { get; set; } = string.Empty;      // Số nhà, tên đường, hẻm/ngõ
    [Required]
    public string Ward { get; set; } = string.Empty;              // Phường / Xã / Thị trấn
    [Required]
    public string District { get; set; } = string.Empty;          // Quận / Huyện / Thị xã / TP thuộc tỉnh
    [Required]
    public string Province { get; set; } = string.Empty;          // Tỉnh / Thành phố trực thuộc TW
    public string? PostalCode { get; set; }        // Mã bưu chính (nếu dùng)
    [Required]
    public string Country { get; set; } = "Vietnam";  // Quốc gia
}
