using System;

namespace Core.Entities;

public class Coupon
{
    public required string? Id { get; set; }
    public long? AmountOff { get; set; }
    public decimal? PercentOff { get; set; }
    public string? Duration { get; set; }
    public long? DurationInMonths { get; set; }
    public long? MaxRedemptions { get; set; }
    public string? Name { get; set; }
    public bool? Valid { get; set; }
}
