using BaiThi.Models.Major;
using DTC.DefaultRepository.Constants;
using DTC.DefaultRepository.Models.Base;
using DTC.T;
using MongoDB.Bson.Serialization.Attributes;
using Project.Net8.Models.Core;
using CoreModel = DTC.DefaultRepository.Models.Core.CoreModel;

namespace Project.Net8.Models.Major;

public class BaiThiModel: Audit, TEntity<string>
{

    public int DienTich { get; set; }
    public int GiaTriNen { get; set; } 
    public CommonModelShort TrangThai { get; set; }

    public string DiaChi { get; set; }
    public string GhiChu { get; set; }
    public string TenNguoiMua { get; set; }
    public string CMND { get; set; }
    public DateTime? NgayKyHopDong { get; set; }

    [BsonIgnore]
    public string NgayKyHopDongShow
    {
        get { return NgayKyHopDong.HasValue ? NgayKyHopDong.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE_CORE) : ""; }
    }

    public string SDT { get; set; }

}