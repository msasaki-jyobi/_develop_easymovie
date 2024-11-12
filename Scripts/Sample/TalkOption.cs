using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace develop_easymovie
{
    // 会話処理中の全体補助
    public class TalkOption : SingletonMonoBehaviour<TalkOption>
    {
        void Start()
        {
            TalkManager.Instance.TalkStartEvent += OnTalkEventHandle;
            TalkManager.Instance.TalkUpdateEvent += OnTalkEventHandle;
            TalkManager.Instance.TalkFinishEvent += OnTalkEventHandle;
        }
        /// <summary>
        /// トーク中に呼ばれたイベントに応じて呼び出したい処理
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventValue"></param>
        private void OnTalkEventHandle(string eventName, string eventValue)
        {
            Debug.Log($"eventName: {eventName}, eventValue:{eventValue}");

            switch (eventName)
            {
                case "Camera":
                    CameraManager.Instance.OnSelectChangeCamera(eventValue);
                    break;
                case "AddItem":
                    FlgManager.Instance.AddFlg(eventValue, 1); // アイテム名を渡して1追加する
                    break;
                case "CostItem":
                    FlgManager.Instance.AddFlg(eventValue, -1); // アイテム名を渡して1減らす
                    break;
                case "QuestSelect": // クエスト選択画面を出す
                    develop_common.UIStateManager.Instance.OnChangeStateAndButtons("QuestSelect");
                    break;


            }
        }
    }
}
