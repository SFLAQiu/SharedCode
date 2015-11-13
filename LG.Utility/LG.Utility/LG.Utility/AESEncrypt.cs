using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LG.Utility {
    public class AESEncrypt {
        /// <summary>
        /// AES加/解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secretKey">秘钥(32位)</param>
        /// <param name="vectorStr">向量(16位)</param>
        /// <param name="doType">操作类型(1=加密，2=解密)</param>
        /// <returns></returns>
        private static string Aes( string data, string secretKey = "CG6G89MVGJKUDB6PSI3HS935X45C6YF6", string vectorStr = "C6G89MB6PI3X5YF6", AesDoType? doType = null) {
            if (!doType.HasValue) return string.Empty;
            byte[] result = null;
            try {
                byte[] key = Encoding.UTF8.GetBytes(secretKey);
                byte[] vector = Encoding.UTF8.GetBytes(vectorStr);
                byte[] dataBytes = null;
                if (doType.Value == AesDoType.Encrypt) {
                    dataBytes = Encoding.UTF8.GetBytes(data);
                } else if (doType.Value == AesDoType.Decrypt) {
                    dataBytes = Convert.FromBase64String(data);
                }
                if (dataBytes == null) return string.Empty;
                Rijndael aes = Rijndael.Create();
                using (MemoryStream Memory = new MemoryStream()) {
                    ICryptoTransform ctf = null;
                    if (doType.Value == AesDoType.Encrypt) {
                        ctf = aes.CreateEncryptor(key, vector);
                    } else if (doType.Value == AesDoType.Decrypt) {
                        ctf = aes.CreateDecryptor(key, vector);
                    }
                    if (ctf == null) return string.Empty;
                    using (CryptoStream cryptoStream = new CryptoStream(Memory, ctf, CryptoStreamMode.Write)) {
                        cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        result = Memory.ToArray();
                    }
                }
            } catch {
                result = null;
            }
            return result == null ? string.Empty : (AesDoType.Encrypt == doType.Value ? Convert.ToBase64String(result) : Encoding.UTF8.GetString(result));
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data">明文或密文</param>
        /// <param name="secretKey">秘钥(32位)</param>
        /// <param name="vectorStr">向量(16位)</param>
        public static string AesEncrypt(string data, string secretKey = "CG6G89MVGJKUDB6PSI3HS935X45C6YF6", string vectorStr = "C6G89MB6PI3X5YF6") {
            return Aes(data, doType: AesDoType.Encrypt, secretKey: secretKey, vectorStr: vectorStr);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="data">明文或密文</param>
        /// <param name="secretKey">秘钥(32位)</param>
        /// <param name="vectorStr">向量(16位)</param>
        public static string AesDecrypt(string data, string secretKey = "CG6G89MVGJKUDB6PSI3HS935X45C6YF6", string vectorStr = "C6G89MB6PI3X5YF6") {
            return Aes(data, doType: AesDoType.Decrypt, secretKey: secretKey, vectorStr: vectorStr);
        }

    }
    public enum AesDoType {
        /// <summary>
        /// 加密
        /// </summary>
        Encrypt = 1,
        /// <summary>
        /// 解密
        /// </summary>
        Decrypt = 2
    }
}
