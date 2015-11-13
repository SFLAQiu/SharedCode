using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace LG.Utility {
    /// <summary>
    /// DES加解密
    /// </summary>
    public class EncryptDes {
        Byte[] FIV;
        Byte[] FKEY;
        /// <summary>
        /// 构造函数
        /// </summary>
        public EncryptDes()
        {
            Byte[] IV = new Byte[] { 155, 102, 247, 3, 26, 199, 204, 36 };
            Byte[] key = new Byte[] { 149, 11, 93, 156, 78, 6, 118, 192 };
            FIV = IV;
            FKEY = key;
        }

        /// <summary>
        /// 采用标准 64位 DES 算法 加密 
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public string ToEncrypt(string strText)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(FKEY, FIV), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cs);
            sw.Write(strText);
            sw.Flush();
            cs.FlushFinalBlock();
            ms.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
        }

        /// <summary>
        /// 采用标准64位3DES 算法 解密
        /// </summary>
        /// <param name="EnctyptStr"></param>
        /// <returns></returns>
        public string ReturnEncrypt(string EnctyptStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = Convert.FromBase64String(EnctyptStr);
            MemoryStream ms = new MemoryStream(inputByteArray);
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(FKEY, FIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
