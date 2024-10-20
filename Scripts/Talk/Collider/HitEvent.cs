using Cysharp.Threading.Tasks;
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
        public bool IsHiddenCheckFlg;
        public string HiddenFlgName = "";
        public bool IsPlayingCheckFlg;
        public string PlayingFlgName = "";
        [Space(10)]
        public string TargetName = "Player";
        public List<TalkData> Talks = new List<TalkData>();

        public List<StringEventHandle> EnterEvent = new List<StringEventHandle>();
        public List<StringEventHandle> ExitEvent = new List<StringEventHandle>();

        private TalkManager _talkManager;

        private bool _isHit;
        private bool _isPlaying;

        public event Action<StringEventHandle> HitEnterEvent;
        public event Action<StringEventHandle> HitExitEvent;

        public bool IsHitAutoPlay;
        public bool OnePlayHide;

        private bool _isHidden;

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
            if (Input.GetKeyDown(KeyCode.E))
                if (_isHit && !IsHitAutoPlay)
                {
                    TalkPlay();
                }
        }

        public void TalkPlay()
        {
            if (!_isHidden)
                if (!_isPlaying)
                {
                    _isPlaying = true;
                    TalkManager.Instance.StartTyping(Talks);

                    if(OnePlayHide)
                        _isHidden = true;
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

        private async void OnHit(GameObject hit)
        {
            if (hit.name == TargetName)
            {
                if (!PlayFlgCheck()) // フラグが持ってない場合Return
                    return;
                if (HiddenFlgCheck()) // 非表示フラグがあったらReturn
                    return;

                await UniTask.Delay(10);


                _isHit = true;
                if (_talkManager != null)
                    foreach (var enter in EnterEvent) // このオブジェクトに登録されているイベントを呼び出す
                        HitEnterEvent?.Invoke(enter);
                //Debug.Log($"Enter hit:{hit}, eventName:{EnterEvent.}, eventValue:{EnterEvent.EventValue}");

                if (IsHitAutoPlay)
                    TalkPlay();
            }
        }

        private async void OnExit(GameObject hit)
        {
            if (hit.name == TargetName)
            {
                if (PlayFlgCheck()) // フラグが持ってない場合Return
                    return;
                if (!HiddenFlgCheck()) // 非表示フラグがあったらReturn
                    return;

                await UniTask.Delay(10);


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
        private async void OnTalkFinishEvent()
        {
            await UniTask.Delay(100);
            _isPlaying = false;
        }
        /// <summary>
        /// true = 実行可能です
        /// </summary>
        /// <returns></returns>
        public bool PlayFlgCheck() 
        {
            if(!IsPlayingCheckFlg) return true;

            return FlgManager.Instance.CheckSelectNameFlg(PlayingFlgName); // フラグが"存在する" 場合 true 
        }
        /// <summary>
        /// true = 実行不可能です
        /// </summary>
        /// <returns></returns>
        public bool HiddenFlgCheck()
        {
            if (!IsHiddenCheckFlg) return false;

            return FlgManager.Instance.CheckSelectNameFlg(HiddenFlgName); // 非表示フラグが"存在する" 場合 true
        }
    }
}
