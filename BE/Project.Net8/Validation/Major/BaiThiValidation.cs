using FluentValidation;
using Project.Net8.Constants;
using Project.Net8.Models.Core;
using Project.Net8.Models.Major;
using Project.Net8.Models.Permission;

public class BaiThiValidation : AbstractValidator<BaiThiModel>
{
    public BaiThiValidation()
    {
        RuleFor(model => model.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .NotNull().WithMessage("Tên không được để null.")
            .OverridePropertyName(x => x.Name);

        //RuleFor(model => model.SDT)
        //    .NotNull().WithMessage("Số điện thoại không được bỏ trống");

        //.Must(x =>
        //{
            
        //    if (x == null || x.Equals("")) return false;
        //    if (Validation.Is_Number(x))
        //    {
        //        return true;
        //    }
        //    return false;
        //}).WithMessage("Ngày cấp lần đầu không hợp lệ hoặc sai định dạng !")
        //    .OverridePropertyName(x => x.SDT);

        //RuleFor(model => model.NgayKyHopDong)
        //    .Must(x =>
        //    {
        //        if (x == null || x.Equals("")) 
        //            return false;
        //        if( Validation.IsDateValid(x))
        //        {
        //            return true;
        //        }
        //        return false;
                  
        //    }).WithMessage("huioy")
        //    .OverridePropertyName( x =>x.NgayKyHopDong);
            

    }
}
