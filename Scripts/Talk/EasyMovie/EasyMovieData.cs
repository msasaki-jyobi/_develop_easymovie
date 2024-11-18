using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    [CreateAssetMenu(fileName = "EasyMovieData", menuName = "develop_easymovie / EasyMovieData")]
    public class EasyMovieData : ScriptableObject
    {
        public string UnitName; // ユニット名
        public GameObject ActionPrefab; // アクション
        public string CameraName; // カメラ名
        public float BlendTime; // 切り替え時間
        public Vector3 WorldPos; // 座標
        public Vector3 WorldRot; // 回転
    }

    //public class EasyMovieInfo
    //{
    //    public string UnitName; // ユニット名
    //    public GameObject ActionPrefab; // アクション
    //    public string CameraName; // カメラ名
    //    public float BlendTime; // 切り替え時間
    //    public Vector3 WorldPos; // 座標
    //    public Vector3 WorldRot; // 回転

    //    //// 対応コマンド一覧
    //    //public List<GameObject> UIActions = new List<GameObject>();
    //    //// データ一覧から購入フラグ立ってるものだけできる
    //}
}