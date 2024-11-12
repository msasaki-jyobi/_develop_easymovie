using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    public class HitChangerEvent : MonoBehaviour
    {
        [Header("差し替えを行うTalkオブジェクト（HitEvent）")]
        public TalkEvent HitTalkEvent;
        [Header("差し替えが発生するEventName(Enter, Exit)")]
        public string ChangeFlgName;
        [Header("差し替えられる会話内容")]
        public List<TalkData> Talks = new List<TalkData>();
        [Header("差し替えられるEnterEvent")]
        public List<StringEventHandle> TalkStartEvent = new List<StringEventHandle>();
        [Header("差し替えられるExitEvent")]
        public List<StringEventHandle> TalkFinishEvent = new List<StringEventHandle>();

        private void Start()
        {
            TalkManager.Instance.TalkStartEvent += CheckFlgHandle;
            TalkManager.Instance.TalkUpdateEvent += CheckFlgHandle;
            TalkManager.Instance.TalkFinishEvent += CheckFlgHandle;
        }

        private void CheckFlgHandle(string eventName, string eventValue)
        {
            if(eventName == ChangeFlgName)
            {
                HitTalkEvent.Talks = Talks;
                HitTalkEvent.TalkStartEvents = TalkStartEvent;
                HitTalkEvent.TalkFinishEvents = TalkFinishEvent;
                Debug.Log($"{HitTalkEvent.name} の項目をそれぞれ {this.name} で差し替えました");
            }
        }
    }
}

