using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Common.Features.Save
{
    /// <summary>
    /// 暗号化、復号化クラス
    /// </summary>
    public static class AESCipher
    {

        // 初期化ベクトル
        private const string AES_IV_256 = @"mER5Ve6jZ/F8CY%~";
        // 暗号化キー
        private const string AES_Key_256 = @"kxvuA&k|WDRkzgG47yAsuhwFzkQZMNf3";

        /// <summary>
        /// 暗号化
        /// </summary>
        /// <param name="text">暗号化データ</param>
        /// <param name="iv">初期化ベクトル</param>
        /// <param name="key">暗号化キー</param>
        /// <returns>暗号化データ</returns>
        public static string Encrypt(string text)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            // ブロックサイズ
            myRijndael.BlockSize = 128;
            // 暗号化アルゴリズム
            myRijndael.KeySize = 256;
            // 暗号化モード
            myRijndael.Mode = CipherMode.CBC;
            // パディング
            myRijndael.Padding = PaddingMode.PKCS7;

            myRijndael.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            myRijndael.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            // 暗号化
            ICryptoTransform encryptor = myRijndael.CreateEncryptor(myRijndael.Key, myRijndael.IV);

            byte[] encrypted;
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream ctStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(ctStream))
                    {
                        sw.Write(text);
                    }//using
                    encrypted = mStream.ToArray();
                }//using
            }//using
            return (System.Convert.ToBase64String(encrypted));
        }//Encrypt

        /// <summary>
        /// 復号化
        /// </summary>
        /// <param name="cipher">暗号化データ</param>
        /// <param name="iv">初期化ベクトル</param>
        /// <param name="key">暗号化キー</param>
        /// <returns>復号化データ</returns>
        public static string Decrypt(string cipher)
        {
            RijndaelManaged rijndael = new RijndaelManaged();
            // ブロックサイズ
            rijndael.BlockSize = 128;
            // 暗号化アルゴリズム
            rijndael.KeySize = 256;
            // 暗号化モード
            rijndael.Mode = CipherMode.CBC;
            // パディング
            rijndael.Padding = PaddingMode.PKCS7;

            rijndael.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            rijndael.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

            string plain = string.Empty;
            using (MemoryStream mStream = new MemoryStream(System.Convert.FromBase64String(cipher)))
            {
                using (CryptoStream ctStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(ctStream))
                    {
                        plain = sr.ReadLine();
                    }//using
                }//using
            }//using
            return plain;
        }//Decrypt

    }//AESCipher
}
