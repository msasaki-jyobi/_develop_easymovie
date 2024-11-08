using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace develop_easymovie
{
    public class TalkOption : SingletonMonoBehaviour<TalkOption>
    {

        [SerializeField] private bool IsTabDebug;
        public List<TalkData> DebugTalks = new List<TalkData>();

        public UnityEvent TalkStartUnityEvent;
        public UnityEvent TalkFinishUnityEvent;

        void Start()
        {
            TalkManager.Instance.TalkStartEvent += OnTalkStartEvent;
            TalkManager.Instance.TalkUpdateEvent += OnTalkEventHandle;
            TalkManager.Instance.TalkFinishEvent += OnTalkFinishEvent;
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Tab))
                if (IsTabDebug)
                    TalkManager.Instance.StartTyping(DebugTalks);
        }

        /// <summary>
        /// トーク開始時に実行したい処理
        /// </summary>
        private void OnTalkStartEvent()
        {
            Debug.Log("Start!!");
            TalkStartUnityEvent?.Invoke();
        }
        /// <summary>
        /// トーク終了時に呼びだしたい処理
        /// </summary>
        private void OnTalkFinishEvent()
        {
            //CameraManager.Instance.OnSelectChangeCamera("DefaultCamera");
            TalkFinishUnityEvent?.Invoke();
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

        /// <summary>
        /// トークを持つオブジェクトに触れたときに呼び出したい処理
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventValue"></param>
        public void OnEnterEventInvoke(GameObject self, GameObject hit, string eventName, string eventValue)
        {
            Debug.Log($"Enter: " +
                $"self:{self.gameObject.name}," +
                $"hit:{hit.gameObject.name}" +
                $"eventName:{eventName}," +
                $"eventValue:{eventValue}");

            // 該当オブジェクトを光らせる


            // イベントの実行
            switch (eventName)
            {
                case "GetA":
                    GameObject a = new GameObject();
                    a.name = "aaa";
                    break;
            }
        }
        /// <summary>
        /// トークを持つオブジェクトから離れたときに呼び出したい処理
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventValue"></param>
        public void OnExitEventInvoke(GameObject self, GameObject hit, string eventName, string eventValue)
        {
            Debug.Log($"Exit: " +
                $"self:{self.gameObject.name}," +
                $"hit:{hit.gameObject.name}" +
                $"eventName:{eventName}," +
                $"eventValue:{eventValue}");

            // 該当オブジェクトを消灯する
        }
    }
}
