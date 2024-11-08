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
        /// �g�[�N�J�n���Ɏ��s����������
        /// </summary>
        private void OnTalkStartEvent()
        {
            Debug.Log("Start!!");
            TalkStartUnityEvent?.Invoke();
        }
        /// <summary>
        /// �g�[�N�I�����ɌĂт�����������
        /// </summary>
        private void OnTalkFinishEvent()
        {
            //CameraManager.Instance.OnSelectChangeCamera("DefaultCamera");
            TalkFinishUnityEvent?.Invoke();
        }
        /// <summary>
        /// �g�[�N���ɌĂ΂ꂽ�C�x���g�ɉ����ČĂяo����������
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
                    FlgManager.Instance.AddFlg(eventValue, 1); // �A�C�e������n����1�ǉ�����
                    break;
                case "CostItem":
                    FlgManager.Instance.AddFlg(eventValue, -1); // �A�C�e������n����1���炷
                    break;
                case "QuestSelect": // �N�G�X�g�I����ʂ��o��
                    develop_common.UIStateManager.Instance.OnChangeStateAndButtons("QuestSelect");
                    break;


            }
        }

        /// <summary>
        /// �g�[�N�����I�u�W�F�N�g�ɐG�ꂽ�Ƃ��ɌĂяo����������
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

            // �Y���I�u�W�F�N�g�����点��


            // �C�x���g�̎��s
            switch (eventName)
            {
                case "GetA":
                    GameObject a = new GameObject();
                    a.name = "aaa";
                    break;
            }
        }
        /// <summary>
        /// �g�[�N�����I�u�W�F�N�g���痣�ꂽ�Ƃ��ɌĂяo����������
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

            // �Y���I�u�W�F�N�g����������
        }
    }
}
