using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    [RequireComponent(typeof(BoxCollider))]

    public class HitEvent : MonoBehaviour
    {
        public string TargetName = "Player";
        public List<TalkData> Talks = new List<TalkData>();

        public List<StringEventHandle> EnterEvent = new List<StringEventHandle>();
        public List<StringEventHandle> ExitEvent = new List<StringEventHandle>();

        private TalkManager _talkManager;

        private bool _isHit;
        private bool _isPlaying;

        public event Action<StringEventHandle> HitEnterEvent;
        public event Action<StringEventHandle> HitExitEvent;

        void Start()
        {
            _talkManager = TalkManager.Instance;
            if (_talkManager != null)
            {
                TalkManager.Instance.TalkStartEvent += OnTalkStartEvent;
                TalkManager.Instance.TalkUpdateEvent += OnTalkEventHandle;
                TalkManager.Instance.TalkFinishEvent += OnTalkFinishEvent;
            }

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
                if (_isHit && !_isPlaying)
                {
                    _isPlaying = true;
                    TalkManager.Instance.StartTyping(Talks);
                }
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnHit(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit(other.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnExit(collision.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(other.gameObject);
        }

        private void OnHit(GameObject hit)
        {
            if (hit.name == TargetName)
            {
                _isHit = true;
                if (_talkManager != null)
                    foreach (var enter in EnterEvent) // このオブジェクトに登録されているイベントを呼び出す
                        HitEnterEvent?.Invoke(enter);
                //Debug.Log($"Enter hit:{hit}, eventName:{EnterEvent.}, eventValue:{EnterEvent.EventValue}");
            }
        }

        private void OnExit(GameObject hit)
        {
            if (hit.name == TargetName)
            {
                _isHit = false;
                if (_talkManager != null)
                    foreach (var exit in ExitEvent) // このオブジェクトに登録されているイベントを呼び出す
                        HitExitEvent?.Invoke(exit);
                //Debug.Log($"Exit hit:{hit}, eventName:{ExitEvent.EventName}, eventValue:{ExitEvent.EventValue}");

            }
        }

        /// <summary>
        /// トーク開始時、このオブジェクトが行いたい処理
        /// </summary>
        private void OnTalkStartEvent()
        {
            Debug.Log("Start!!");
        }
        /// <summary>
        /// トーク中に、このオブジェクトが行いたい処理
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
            }
        }
        /// <summary>
        /// トーク終了時にこのオブジェクトが行いたい処理
        /// </summary>
        private void OnTalkFinishEvent()
        {
            _isPlaying = false;
        }
    }
}
