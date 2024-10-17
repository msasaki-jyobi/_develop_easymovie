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
                    foreach (var enter in EnterEvent) // ���̃I�u�W�F�N�g�ɓo�^����Ă���C�x���g���Ăяo��
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
                    foreach (var exit in ExitEvent) // ���̃I�u�W�F�N�g�ɓo�^����Ă���C�x���g���Ăяo��
                        HitExitEvent?.Invoke(exit);
                //Debug.Log($"Exit hit:{hit}, eventName:{ExitEvent.EventName}, eventValue:{ExitEvent.EventValue}");

            }
        }

        /// <summary>
        /// �g�[�N�J�n���A���̃I�u�W�F�N�g���s����������
        /// </summary>
        private void OnTalkStartEvent()
        {
            Debug.Log("Start!!");
        }
        /// <summary>
        /// �g�[�N���ɁA���̃I�u�W�F�N�g���s����������
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
        /// �g�[�N�I�����ɂ��̃I�u�W�F�N�g���s����������
        /// </summary>
        private void OnTalkFinishEvent()
        {
            _isPlaying = false;
        }
    }
}
