using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    public class HitChangerEvent : MonoBehaviour
    {
        public HitEvent HitTalkEvent;
        public string ChangeFlgName;
        public List<TalkData> Talks = new List<TalkData>();
        public List<StringEventHandle> EnterEvent = new List<StringEventHandle>();
        public List<StringEventHandle> ExitEvent = new List<StringEventHandle>();

        private void Start()
        {
            FlgManager.Instance.CheckFlgEvent += CheckFlgHandle;
        }

        private void CheckFlgHandle(string eventName, string eventValue)
        {
            if(eventName == ChangeFlgName)
            {
                HitTalkEvent.Talks = Talks;
                HitTalkEvent.EnterEvent = EnterEvent;
                HitTalkEvent.ExitEvent = ExitEvent;
                Debug.Log($"{HitTalkEvent.name} ÇÃçÄñ⁄ÇÇªÇÍÇºÇÍ {this.name} Ç≈ç∑Çµë÷Ç¶Ç‹ÇµÇΩ");
            }
        }
    }
}

