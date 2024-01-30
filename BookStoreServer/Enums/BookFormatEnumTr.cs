using System.ComponentModel.DataAnnotations;

namespace BookStoreServer.Enums;

public enum BookFormatEnumTr
{
    [Display(ShortName = "Ciltli Kapak")]
    Ciltli,
    [Display(GroupName = "Sesli Kitap")]
    SesliKitap,
    [Display(Name = "E-Kitap Kapak")]
    EKitap
}
