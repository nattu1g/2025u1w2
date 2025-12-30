using UnityEngine;
using System.Collections.Generic;
using App.Features.Assembly;
using App.Settings;

namespace App.Features.WaterTank.Water
{
    /// <summary>
    /// Waterオブジェクトの生成・管理を行うクラス
    /// </summary>
    public class WaterSpawner
    {
        private readonly GlobalAssetAssembly _assetAssembly;
        private readonly List<GameObject> _spawnedWaters = new();

        public WaterSpawner(GlobalAssetAssembly assetAssembly)
        {
            _assetAssembly = assetAssembly;
        }

        /// <summary>
        /// Waterをランダムに生成
        /// </summary>
        public void SpawnWaters()
        {
            ClearAllWaters();

            // CircleWaterを生成
            int circleCount = Random.Range(GameConstants.MinCircleWaterCount, GameConstants.MaxCircleWaterCount + 1);
            SpawnWaterType(_assetAssembly.CircleWaterPrefab, circleCount, GameConstants.CircleWaterMinDistance);

            // EllipseWaterを生成
            int ellipseCount = Random.Range(GameConstants.MinEllipseWaterCount, GameConstants.MaxEllipseWaterCount + 1);
            SpawnWaterType(_assetAssembly.EllipseWaterPrefab, ellipseCount, GameConstants.LargeWaterMinDistance);

            // SquareWaterを生成
            int squareCount = Random.Range(GameConstants.MinSquareWaterCount, GameConstants.MaxSquareWaterCount + 1);
            SpawnWaterType(_assetAssembly.SquareWaterPrefab, squareCount, GameConstants.LargeWaterMinDistance);

            Debug.Log($"WaterSpawner: Spawned {circleCount} circles, {ellipseCount} ellipses, {squareCount} squares");
        }

        /// <summary>
        /// 指定されたタイプのWaterを生成
        /// </summary>
        private void SpawnWaterType(GameObject prefab, int count, float minDistance)
        {
            if (prefab == null)
            {
                Debug.LogWarning($"WaterSpawner: Prefab is null, skipping spawn");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 position = FindValidPosition(minDistance);
                GameObject water = Object.Instantiate(prefab, position, Quaternion.identity);
                water.tag = "Water"; // タグを設定
                _spawnedWaters.Add(water);
            }
        }

        /// <summary>
        /// 既存のWaterと重複しない位置を探す
        /// </summary>
        private Vector3 FindValidPosition(float minDistance)
        {
            // GameConstantsから生成範囲を取得
            // 最大100回試行して、見つからなければ強制配置
            for (int attempt = 0; attempt < 100; attempt++)
            {
                float x = Random.Range(GameConstants.WaterSpawnMinX, GameConstants.WaterSpawnMaxX);
                Vector3 position = new Vector3(x, 0f, 0f);

                bool isValid = true;
                foreach (var water in _spawnedWaters)
                {
                    if (water != null && Vector3.Distance(position, water.transform.position) < minDistance)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid) return position;
            }

            // 見つからなかった場合は強制配置
            Debug.LogWarning($"WaterSpawner: Could not find valid position after 100 attempts, forcing placement");
            return new Vector3(Random.Range(GameConstants.WaterSpawnMinX, GameConstants.WaterSpawnMaxX), 0f, 0f);
        }

        /// <summary>
        /// 全てのWaterを削除
        /// </summary>
        public void ClearAllWaters()
        {
            foreach (var water in _spawnedWaters)
            {
                if (water != null)
                {
                    Object.Destroy(water);
                }
            }
            _spawnedWaters.Clear();
            Debug.Log("WaterSpawner: Cleared all waters");
        }
    }
}
