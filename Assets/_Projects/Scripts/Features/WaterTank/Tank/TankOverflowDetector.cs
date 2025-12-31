using App.Events;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace App.Features.WaterTank.Tank
{
    /// <summary>
    /// 水槽からのこぼれを検知するコンポーネント
    /// 水槽の下部に配置したTriggerで水オブジェクトを検知する
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TankOverflowDetector : MonoBehaviour
    {
        [Header("検知設定")]
        [Tooltip("検知対象のタグ")]
        [SerializeField] private string _waterTag = "Water";

        private IPublisher<GameOverEvent> _gameOverPublisher;

        [Inject]
        public void Construct(IPublisher<GameOverEvent> gameOverPublisher)
        {
            _gameOverPublisher = gameOverPublisher;
        }

        private void Awake()
        {
            // Colliderがトリガーであることを確認
            var collider = GetComponent<Collider2D>();
            if (!collider.isTrigger)
            {
                Debug.LogWarning("TankOverflowDetector: Colliderをトリガーに設定してください");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // 水オブジェクトがこぼれたことを検知
            if (other.CompareTag(_waterTag))
            {
                Debug.Log("水がこぼれました！ゲームオーバー");

                // MessagePipeでゲームオーバーイベントを発行
                if (_gameOverPublisher != null)
                {
                    _gameOverPublisher.Publish(new GameOverEvent(0)); // スコアは後で設定
                }
                else
                {
                    Debug.LogError("TankOverflowDetector: GameOverPublisher is null. Make sure VContainer is properly configured.");
                }
            }
        }
    }
}
