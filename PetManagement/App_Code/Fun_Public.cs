using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PetManagement.App_Code
{
    public class Fun_Public
    {
        #region SHA512
        public string SHA512(string UserNo)
        {
            //建立一個SHA512
            SHA512 EncryptionSHA = new SHA512CryptoServiceProvider();
            //將字串轉成byte[]
            byte[] source = Encoding.Default.GetBytes(UserNo);
            //進行SHA512加密
            byte[] crypo = EncryptionSHA.ComputeHash(source);

            return BitConverter.ToString(crypo).Replace("-", string.Empty).ToLower();
        }
        #endregion
    }
}