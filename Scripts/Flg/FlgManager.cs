using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace develop_easymovie
{
    public class FlgManager : SingletonMonoBehaviour<FlgManager>
    {
        public List<StringEventHandle> FlgEvents = new List<StringEventHandle>();

        public event Action<string, string> CheckFlgEvent;
        // Fっていうフラグがたったら、このオブジェクトをこれで上書きするね　を実行するタイミング
        private void Start()
        {
            foreach (StringEventHandle flg in FlgEvents)
                CheckFlgEvent?.Invoke(flg.EventName, flg.EventValue);
        }

        /// <summary>
        /// フラグイベントを新規追加・加算する
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <param name="value"></param>
        public void AddFlg(string eventFlgName, int value)
        {
            for (int i = 0; i < FlgEvents.Count; i++)
            {
                if (FlgEvents[i].EventName == eventFlgName)
                {
                    // 加算処理
                    int valueA = int.Parse(FlgEvents[i].EventValue);
                    int valueB = value;
                    int total = valueA + valueB;

                    FlgEvents[i].EventValue = total.ToString();
                    CheckFlgEvent?.Invoke(eventFlgName, total.ToString());

                    Debug.Log($"{eventFlgName} 加算数：{value}, 残り：{total}");
                    return;
                }
            }

            // 存在しない場合
            var flg = new StringEventHandle();
            flg.EventName = eventFlgName;
            flg.EventValue = value.ToString();
            FlgEvents.Add(flg);
        }

        /// <summary>
        /// フラグイベントが存在するかチェック
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <returns></returns>
        public bool CheckSelectNameFlg(string eventFlgName)
        {
            for (int i = 0; i < FlgEvents.Count; i++)
            {
                if (FlgEvents[i].EventName == eventFlgName)
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
            for (int i = 0; i < FlgEvents.Count; i++)
            {
                if (FlgEvents[i].EventName == eventFlgName)
                {
                    // 加算処理
                    int result = int.Parse(FlgEvents[i].EventValue);

                    return result;
                }
            }
            // 存在しない場合
            return 0;
        }
    }
}
