using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Component
{
    /// <summary>
    /// プレイヤーの生徒データと先生データをまとめて保存するためのクラス
    /// </summary>
    [System.Serializable]
    public class PlayerAndTeacherSaveData
    {
        // GenericOwnedCollectionSaveData<CardData> を使用して生徒と先生の所持データを保持
        // public GenericOwnedCollectionSaveData<CardData> Students;
        // public GenericOwnedCollectionSaveData<CardData> Teachers;
        // public GenericOwnedCollectionSaveData<CardData> EventItems;
        // public GenericOwnedCollectionSaveData<CardData> ClubItems;

        public PlayerAndTeacherSaveData()
        {
            // デフォルトコンストラクタで各コレクションを初期化
            // Students = new GenericOwnedCollectionSaveData<CardData>();
            // Teachers = new GenericOwnedCollectionSaveData<CardData>();
            // EventItems = new GenericOwnedCollectionSaveData<CardData>();
            // ClubItems = new GenericOwnedCollectionSaveData<CardData>();

        }
    }
}