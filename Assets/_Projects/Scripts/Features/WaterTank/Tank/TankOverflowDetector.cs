using UnityEngine;
using UnityEngine.Events;

namespace App.Features.WaterTank.Tank
{
    /// <summary>
    /// 水槽からのこぼれを検知するコンポーネント
    /// 水槽の下部に配置したTriggerで水オブジェクトを検知する
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TankOverflowDetector : MonoBehaviour
    {
        [Header("検知設定")]
        [Tooltip("検知対象のタグ")]
        [SerializeField] private string _waterTag = "Water";
        
        [Header("イベント")]
        public UnityEvent OnOverflow;
        
        private void Awake()
        {
            // Colliderがトリガーであることを確認
            var collider = GetComponent<Collider>();
            if (!collider.isTrigger)
            {
                Debug.LogWarning("TankOverflowDetector: Colliderをトリガーに設定してください");
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // 水オブジェクトがこぼれたことを検知
            if (other.CompareTag(_waterTag))
            {
                Debug.Log("水がこぼれました！ゲームオーバー");
                OnOverflow?.Invoke();
            }
        }
    }
}
