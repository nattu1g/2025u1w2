using UnityEngine;

namespace Common.Features.Save
{
    public class SaveValue<T1, T2> : ISaveValue
    {
        public T1 Val1;
        public T2 Val2;

        public SaveValue(T1 val1, T2 val2)
        {
            this.Val1 = val1;
            this.Val2 = val2;
        }


        /// <summary>
        /// JSONに変換
        /// </summary>
        /// <returns>JSON</returns>
        public string ToJson()
        {
            return JsonUtility.ToJson(this, true);
        }
        /// <summary>
        /// JSONから変換
        /// </summary>
        /// <param name="jsonStr">JSON</param>
        public void FromJson(string jsonStr)
        {
            JsonUtility.FromJsonOverwrite(jsonStr, this);
        }


        /// <summary>
        /// 値をセット
        /// </summary>
        /// <param name="val1">val1 の値</param>
        /// <param name="val2">val2 の値</param>
        public void SetValue(T1 val1, T2 val2)
        {
            this.Val1 = val1;
            this.Val2 = val2;
        }

    }

}

