namespace Infrastructure.ValueObjects;

/// <summary> 地址信息 </summary>
public class Address
{
    /// <summary> 国家编码 </summary>
    public string? CountryCode { get; set; }

    /// <summary> 国家 </summary>
    public string? Country { get; set; }

    /// <summary> 省编码 </summary>
    public string? ProvinceCode { get; set; }

    /// <summary> 省 </summary>
    public string? Province { get; set; }

    /// <summary> 市编码 </summary>
    public string? CityCode { get; set; }

    /// <summary> 市 </summary>
    public string? City { get; set; }

    /// <summary> 区编码 </summary>
    public string? AreaCode { get; set; }

    /// <summary> 区 </summary>
    public string? Area { get; set; }

    /// <summary> 街道编码 </summary>
    public string? StreetCode { get; set; }

    /// <summary> 街道 </summary>
    public string? Street { get; set; }

    /// <summary> 乡/镇编码 </summary>
    public string? VillageCode { get; set; }

    /// <summary> 乡镇 </summary>
    public string? Village { get; set; }

    /// <summary> 详细地址 </summary>
    public string? Detailed { get; set; }

    /// <summary> 经度 </summary>
    public double Longitudes { get; set; }

    /// <summary> 纬度 </summary>
    public double Latitude { get; set; }
}