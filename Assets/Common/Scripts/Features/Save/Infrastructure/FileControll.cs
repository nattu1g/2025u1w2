using System;
using System.IO;
using UnityEngine;

namespace Common.Features.Save
{
    /// <summary>
    /// ファイルにセーブ、ロードするクラス
    /// </summary>
    public static class FileControll
    {
        /// <summary>
        /// ファイルにセーブ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="json">セーブデータ</param>
        /// <returns>true:成功 false:失敗</returns>
        public static bool WriteFile(string filePath, string cjson)
        {
#if UNITY_EDITOR
            // セーブデータをそのまま保存
            var json = cjson;
#else
            // セーブデータを暗号化して保存
            var json = AESCipher.Encrypt(cjson);
#endif

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.Write(json);
                    sw.Flush();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("セーブに失敗しました" + e);
                return false;
            }
        }
        /// <summary>
        /// ファイルからロード
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>ロードデータ</returns>
        public static string ReadFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))//�f�[�^�����݂���ꍇ�͕Ԃ�
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
#if UNITY_EDITOR
                        return sr.ReadToEnd();
#else
                        return AESCipher.Decrypt(sr.ReadToEnd());
#endif
                    }
                }
                //ファイルがない場合はデフォルト値を返す
                return default;
            }
            catch (Exception)
            {
                Debug.LogError("ロードに失敗しました");
                return "";
            }

        }
    }
}
