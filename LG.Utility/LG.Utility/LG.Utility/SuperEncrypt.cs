using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LG.Utility {
    /// <summary>
    /// 超级混淆加密AES加密+随机6位秘钥+BASE64
    /// <para>
    /// 密串=BASE64("【秘钥】前3位"+AES加密(MD5【随机6位+可选附加码】32位，向量)+"【秘钥】后3位").Replace("+", "_").Replace("/", "-").Replace("=", "*");
    /// </para>
    /// </summary>
    public class SuperEncrypt {
        /// <summary>
        /// 秘钥的附加码(默认秘钥是6随机数)
        /// </summary>
        private string _KeyAppendCode { get; set; }
        /// <summary>
        /// 16位向量(AES加密的时候使用)
        /// </summary>
        private string _AESVector { get; set; }
        /// <summary>
        /// AES加密向量
        /// </summary>
        /// <param name="AESVector">16位向量</param>
        /// <param name="keyAppendCode">秘钥的附加码</param>
        public SuperEncrypt(string AESVector, string keyAppendCode = "") {
            _AESVector = AESVector;
            _KeyAppendCode = keyAppendCode;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="secretKey">秘钥</param>
        /// <returns></returns>
        public string Encrypt(string plaintext, out string secretKey) {
            secretKey = string.Empty;
            if (plaintext.IsNullOrWhiteSpace()) return string.Empty;
            secretKey = Rand.Str(6);//"CWII7J";
            if (secretKey.IsNullOrWhiteSpace() || secretKey.Count() != 6) return string.Empty;
            var leftStr = secretKey.Substring(0, 3);
            var rightStr = secretKey.Substring(3, 3);
            //随机6位密码+附加码
            var aesKey = "{0}{1}".FormatStr(secretKey, _KeyAppendCode).MD5().ToLower();
            //加密操作
            var ciphertext = AESEncrypt.AesEncrypt(plaintext, secretKey: aesKey, vectorStr: _AESVector).Replace("+", "_").Replace("/", "-").Replace("=", "*");
            return "{0}{1}{2}".FormatStr(leftStr, ciphertext, rightStr);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str">密串</param>
        /// <returns></returns>
        public string Decrypt(string str) {
            var  parseKey= string.Empty;
            var ciphertext = GetParse(str, out parseKey);
            if (parseKey.IsNullOrWhiteSpace() || ciphertext.IsNullOrWhiteSpace()) return string.Empty;
            ciphertext = ciphertext.Replace("_", "+").Replace("-", "/").Replace("*", "=");
            //随机6位密码+附加码
            var aesKey = "{0}{1}".FormatStr(parseKey, _KeyAppendCode).MD5().ToLower();
            //解密操作
            var plaintext = AESEncrypt.AesDecrypt(ciphertext, secretKey: aesKey, vectorStr: _AESVector);
            return plaintext;
        }
       /// <summary>
        /// 获取密文
       /// </summary>
       /// <param name="str">解析串</param>
       /// <param name="secretKey">秘钥</param>
       /// <returns>密文</returns>
        public static string GetParse(string str, out string secretKey) {
            secretKey = string.Empty;
            var ciphertext = string.Empty;
            if (str.IsNullOrWhiteSpace() || str.Count() <= 6) return string.Empty;
            var length = str.Count();
            var leftKey = str.Substring(0, 3);
            ciphertext = str.Substring(3, length - 6);
            var rightKey = str.Substring(length - 3, 3);
            secretKey = "{0}{1}".FormatStr(leftKey, rightKey);
            return ciphertext;
        }
        /// <summary>
        /// 获取秘钥
        /// </summary>
        /// <param name="str">解析串</param>
        /// <returns>秘钥</returns>
        public static string GetSecretKey(string str) {
            string secretKey = string.Empty;
            GetParse(str, out secretKey);
            return secretKey;
        }
    }
}
