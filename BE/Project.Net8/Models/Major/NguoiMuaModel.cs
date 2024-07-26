using DTC.DefaultRepository.Models.Base;
using DTC.T;

namespace BaiThi.Models.Major
{

    public class NguoiMuaModel : Audit, TEntity<string>
    {
        public string TenNguoiMua { get; set; }
        public string CMND { get; set; }
        public DateTime? NgayKyHopDong { get; set; }
        public string SDT { get; set; }
    }
    public class NguoiMuaModelShort
    {
        public string TenNguoiMua { get; set; }
        public string CMND { get; set; }
        public DateTime? NgayKyHopDong { get; set; }
        public string SDT { get; set; }
    }

}
