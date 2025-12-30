using UnityEngine;
using R3;
using System;
using App.Features.WaterTank.Baseline;

namespace App.Features.WaterTank.Water
{
    /// <summary>
    /// 水位が基準線を超えたか判定するコンポーネント
    /// </summary>
    public class WaterLevelChecker : MonoBehaviour
    {
        private BaselineDisplay _baselineDisplay;
        private readonly Subject<bool> _baselineReachedSubject = new();
        private bool _isAboveBaseline = false;
        private bool _wasAboveBaseline = false;

        /// <summary>
        /// 基準線到達時のイベント（true: 到達, false: 未到達）
        /// </summary>
        public Observable<bool> OnBaselineReachedAsObservable() => _baselineReachedSubject;

        /// <summary>
        /// 現在水位が基準線を超えているか
        /// </summary>
        public bool IsAboveBaseline => _isAboveBaseline;

        /// <summary>
        /// BaselineDisplayを注入（VContainer経由）
        /// </summary>
        public void Initialize(BaselineDisplay baselineDisplay)
        {
            _baselineDisplay = baselineDisplay;
            Debug.Log("WaterLevelChecker: BaselineDisplay injected");
        }

        private void LateUpdate()
        {
            if (_baselineDisplay == null)
            {
                return;
            }

            // 全てのWaterオブジェクトを取得（タグで検索）
            GameObject[] waterObjects = GameObject.FindGameObjectsWithTag("Water");

            if (waterObjects.Length == 0)
            {
                // タグがない場合は、Waterという名前のオブジェクトを検索
                waterObjects = GameObject.FindObjectsOfType<GameObject>();
                waterObjects = System.Array.FindAll(waterObjects, obj => obj.name.Contains("Water") && obj.GetComponent<Rigidbody2D>() != null);
            }

            if (waterObjects.Length == 0)
            {
                return;
            }

            // 全てのWaterの中で最も高い位置を取得
            float maxWaterTopY = float.MinValue;
            foreach (GameObject waterObj in waterObjects)
            {
                Transform waterTransform = waterObj.transform;
                float waterTopY = waterTransform.position.y + (waterTransform.localScale.y / 2f);
                if (waterTopY > maxWaterTopY)
                {
                    maxWaterTopY = waterTopY;
                }
            }

            // 基準線のY座標を取得
            float baselineY = _baselineDisplay.CurrentHeight;

            // 基準線を超えているか判定
            _isAboveBaseline = maxWaterTopY >= baselineY;

            // 状態が変わったらイベント発行
            if (_isAboveBaseline != _wasAboveBaseline)
            {
                _baselineReachedSubject.OnNext(_isAboveBaseline);
                Debug.Log($"WaterLevelChecker: Baseline reached = {_isAboveBaseline} (Max water: {maxWaterTopY:F2}, Baseline: {baselineY:F2})");
                _wasAboveBaseline = _isAboveBaseline;
            }
        }

        private void OnDestroy()
        {
            _baselineReachedSubject.Dispose();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_baselineDisplay == null) return;

            // 全てのWaterオブジェクトを取得
            GameObject[] waterObjects = GameObject.FindGameObjectsWithTag("Water");
            if (waterObjects.Length == 0)
            {
                waterObjects = GameObject.FindObjectsOfType<GameObject>();
                waterObjects = System.Array.FindAll(waterObjects, obj => obj.name.Contains("Water") && obj.GetComponent<Rigidbody2D>() != null);
            }

            // 最も高いWaterの上端を可視化
            float maxWaterTopY = float.MinValue;
            Vector3 maxWaterPos = Vector3.zero;
            foreach (GameObject waterObj in waterObjects)
            {
                Transform waterTransform = waterObj.transform;
                float waterTopY = waterTransform.position.y + (waterTransform.localScale.y / 2f);
                if (waterTopY > maxWaterTopY)
                {
                    maxWaterTopY = waterTopY;
                    maxWaterPos = new Vector3(waterTransform.position.x, waterTopY, waterTransform.position.z);
                }
            }

            if (maxWaterTopY > float.MinValue)
            {
                Gizmos.color = _isAboveBaseline ? Color.green : Color.yellow;
                Gizmos.DrawWireSphere(maxWaterPos, 0.2f);
            }
        }
#endif
    }
}
