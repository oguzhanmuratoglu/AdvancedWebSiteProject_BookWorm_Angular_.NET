using System.ComponentModel.DataAnnotations;

namespace BookStoreServer.Enums;

public enum OrderStatusEnumTr
{
    [Display(Name = "Onay Bekleniyor")]
    OnayBekleniyor,
    Hazırlanıyor,
    Yolda,
    [Display(Name = "Teslim Edildi")]
    TeslimEdildi,
    Reddedildi,
    [Display(Name = "İade Edildi")]
    IadeEdildi
}
