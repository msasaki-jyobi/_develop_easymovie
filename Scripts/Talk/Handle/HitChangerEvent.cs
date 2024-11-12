using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    public class HitChangerEvent : MonoBehaviour
    {
        [Header("�����ւ����s��Talk�I�u�W�F�N�g�iHitEvent�j")]
        public TalkEvent HitTalkEvent;
        [Header("�����ւ�����������EventName(Enter, Exit)")]
        public string ChangeFlgName;
        [Header("�����ւ������b���e")]
        public List<TalkData> Talks = new List<TalkData>();
        [Header("�����ւ�����EnterEvent")]
        public List<StringEventHandle> TalkStartEvent = new List<StringEventHandle>();
        [Header("�����ւ�����ExitEvent")]
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
                Debug.Log($"{HitTalkEvent.name} �̍��ڂ����ꂼ�� {this.name} �ō����ւ��܂���");
            }
        }
    }
}

