using System;
using System.Runtime.Serialization;

namespace Model
{
    /// <summary>
    /// Response code.
    /// </summary>
    [DataContract, Serializable]
    public enum ResponseCode
    {
        /// <summary>
        /// İşlem başarılı.
        /// </summary>
        Success = 0,
        /// <summary>
        /// Limit aşımı.
        /// </summary>
        RateLimit = 1,
        /// <summary>
        /// İçerik yok.
        /// </summary>
        NoContent = 2,
        /// <summary>
        /// İçeriği tekrar al.
        /// </summary>
        ResetContent = 3,
        /// <summary>
        /// Hatalı istek.
        /// </summary>
        InvalidRequest = 4,
        /// <summary>
        /// Erişim izni yok.
        /// </summary>
        AccessDenied = 5,
        /// <summary>
        /// Giriş bilgisi yok | Giriş yapılamadı.
        /// </summary>
        Unauthorized = 6,
        /// <summary>
        /// Method bulunamadı.
        /// </summary>
        NotFound = 7,
        /// <summary>
        /// Bu methoda erişim izni verilemedi.
        /// </summary>
        MethodNotAllowed = 8,
        /// <summary>
        /// Header bilgisi doğrulanamadı.
        /// </summary>
        InvalidHeader = 9,
        /// <summary>
        /// Hizmet şuan için aktif değil.
        /// </summary>
        ServiceUnavailable = 10,
        /// <summary>
        /// Eksik parametre.
        /// </summary>
        MissingProperty = 11,
        /// <summary>
        /// Zaten varolan data.
        /// </summary>
        Conflict = 12,
        /// <summary>
        /// Model doğrulanamadı
        /// </summary>
        ValidationError = 13,
        /// <summary>
        /// Engellenmiş kullanıcı
        /// </summary>
        BannedUser = 99,
    }

    /// <summary>
    /// Authentication type.
    /// </summary>
    [DataContract, Serializable]
    public enum AuthenticationType
    {
        /// <summary>
        /// Herkese açık
        /// </summary>
        Anonymous = 0,
        /// <summary>
        /// Kullanıcı girişi zorunlu
        /// </summary>
        User = 1
    }
}