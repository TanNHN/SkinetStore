using System;

namespace Core.Entities;

public class Address : BaseEntity
{
    public required string StreetAddress { get; set; }      // Số nhà, tên đường, hẻm/ngõ
    public required string Ward { get; set; }              // Phường / Xã / Thị trấn
    public required string District { get; set; }          // Quận / Huyện / Thị xã / TP thuộc tỉnh
    public required string Province { get; set; }          // Tỉnh / Thành phố trực thuộc TW
    public string? PostalCode { get; set; }        // Mã bưu chính (nếu dùng)
    public required string Country { get; set; } = "Vietnam";  // Quốc gia
}
