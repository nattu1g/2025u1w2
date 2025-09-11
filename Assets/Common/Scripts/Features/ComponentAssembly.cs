using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Common.Features
{
    /// <summary>
    /// Vcontainerが参照しにくいコンポーネントの登録と、MonoBehaviourに依存する関数を使用できるように定義
    /// </summary>
    public class ComponentAssembly : MonoBehaviour
    {
        [Header("AssemblySceneName")][SerializeField] private string _assemblySceneName;
        public string AssemblySceneName => _assemblySceneName;
        [Header("AudioMixer")][SerializeField] private AudioMixer _audioMixer;
        public AudioMixer AudioMixer => _audioMixer;
        [LabelText("ステータスカードPrefab")]
        [SerializeField] private GameObject _statusCardPrefab;
        public GameObject StatusCardPrefab => _statusCardPrefab;

        /// <summary>
        /// コンポーネントアセンブリのシーン名を設定する
        /// </summary>
        /// <param name="assemblySceneName">シーン名</param>
        public void SetAssemblySceneName(string assemblySceneName)
        {
            _assemblySceneName = assemblySceneName;
        }

        /// <summary>
        /// MonoBehaviourをインスタンス化する
        /// </summary>
        /// <param name="gameObject">インスタンス化するオブジェクト</param>
        /// <param name="parent">親オブジェクト</param>
        /// <returns></returns>
        public GameObject MakeGameObject(GameObject gameObject, Transform parent)
        {
            return Instantiate(gameObject, parent);
        }

        /// <summary>
        /// オブジェクトを破棄する
        /// </summary>
        /// <param name="gameObject">破棄するオブジェクト</param>
        public void DestoroyObject(GameObject gameObject)
        {
            Destroy(gameObject);
        }
    }
}