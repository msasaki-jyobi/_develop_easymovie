using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace develop_easymovie
{
    public class HitLoadUnityEvent : MonoBehaviour
    {
        public HitEvent HitEvent;

        public UnityEvent EnterEvent;
        public UnityEvent ExitEvent;

        private void Start()
        {
            HitEvent.HitEnterEvent += OnHitEnterHandle;
            HitEvent.HitExitEvent += OnHitExitHandle;
        }

        private void OnHitEnterHandle(StringEventHandle enter)
        {
            if (enter.EventName == "Enter")
                EnterEvent?.Invoke();

        }
        private void OnHitExitHandle(StringEventHandle exit)
        {
            if (exit.EventName == "Exit")
                ExitEvent?.Invoke();

        }
    }

}
