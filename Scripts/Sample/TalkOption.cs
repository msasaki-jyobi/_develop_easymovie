using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace develop_easymovie
{
    // ��b�������̑S�̕⏕
    public class TalkOption : SingletonMonoBehaviour<TalkOption>
    {
        void Start()
        {
            TalkManager.Instance.TalkStartEvent += OnTalkEventHandle;
            TalkManager.Instance.TalkUpdateEvent += OnTalkEventHandle;
            TalkManager.Instance.TalkFinishEvent += OnTalkEventHandle;
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
    }
}
