using BaiThi.Models.Major;
using DTC.DefaultRepository.FromBodyModels;


namespace Project.Net8.Interface.Major
{
    public interface INguoiMuaService
    {
        Task<dynamic> Create(NguoiMuaModel model);
        Task<dynamic> Update(NguoiMuaModel model);

    }
}