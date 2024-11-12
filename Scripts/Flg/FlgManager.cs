using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace develop_easymovie
{
    public class FlgManager : SingletonMonoBehaviour<FlgManager>
    {
        public List<StringEventHandle> FlgItems = new List<StringEventHandle>();
        /// <summary>
        /// フラグイベントを新規追加・加算する
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <param name="value"></param>
        public void AddFlg(string eventFlgName, int value)
        {
            for (int i = 0; i < FlgItems.Count; i++)
            {
                if (FlgItems[i].EventName == eventFlgName)
                {
                    // 加算処理
                    int valueA = int.Parse(FlgItems[i].EventValue);
                    int valueB = value;
                    int total = valueA + valueB;

                    FlgItems[i].EventValue = total.ToString();
                    Debug.Log($"{eventFlgName} 加算数：{value}, 残り：{total}");
                    return;
                }
            }

            // 存在しない場合
            var flg = new StringEventHandle();
            flg.EventName = eventFlgName;
            flg.EventValue = value.ToString();
            FlgItems.Add(flg);
        }

        /// <summary>
        /// フラグイベントが存在するかチェック
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <returns></returns>
        public bool CheckSelectNameFlg(string eventFlgName)
        {
            for (int i = 0; i < FlgItems.Count; i++)
            {
                if (FlgItems[i].EventName == eventFlgName)
                {
                    return true;
                }
            }

            // 存在しない場合
            return false;
        }
        /// <summary>
        /// フラグイベントの個数を取得する
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <returns></returns>
        public int GetSelectNameFlgValue(string eventFlgName)
        {
            for (int i = 0; i < FlgItems.Count; i++)
            {
                if (FlgItems[i].EventName == eventFlgName)
                {
                    // 加算処理
                    int result = int.Parse(FlgItems[i].EventValue);

                    return result;
                }
            }
            // 存在しない場合
            return 0;
        }

    }
}
