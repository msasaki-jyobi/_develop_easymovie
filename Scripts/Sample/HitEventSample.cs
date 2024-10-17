using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    [RequireComponent(typeof(BoxCollider))]

    public class HitEventSample : MonoBehaviour
    {
        public string TargetName = "Player";
        public List<TalkData> Talks = new List<TalkData>();

        public List<StringEventHandle> EnterEvent = new List<StringEventHandle>();
        public List<StringEventHandle> ExitEvent = new List<StringEventHandle>();

        private TalkOptionSample TalkOption;
        void Start()
        {
            TalkOption = TalkOptionSample.Instance;
            if(TalkOption != null)
            {
                //TalkManager.Instance.TalkStartEvent += OnTalkStartEvent;
                //TalkManager.Instance.TalkUpdateEvent += OnTalkEventHandle;
                //TalkManager.Instance.TalkFinishEvent += OnTalkFinishEvent;
            }
            
        }

        void Update()
        {

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
                if (TalkOption != null)
                    //Debug.Log($"Enter hit:{hit}, eventName:{EnterEvent.EventName}, eventValue:{EnterEvent.EventValue}");
                    foreach (var ev in EnterEvent)
                        TalkOption.OnEnterEventInvoke(gameObject, hit, ev.EventName, ev.EventValue);
            }
        }

        private void OnExit(GameObject hit)
        {
            if (hit.name == TargetName)
            {
                if (TalkOption != null)
                    //Debug.Log($"Exit hit:{hit}, eventName:{ExitEvent.EventName}, eventValue:{ExitEvent.EventValue}");
                    foreach (var ev in ExitEvent)
                        TalkOption.OnEnterEventInvoke(gameObject, hit, ev.EventName, ev.EventValue);
            }
        }

        ///// <summary>
        ///// トーク開始時、このオブジェクトが行いたい処理
        ///// </summary>
        //private void OnTalkStartEvent()
        //{
        //    Debug.Log("Start!!");
        //}
        ///// <summary>
        ///// トーク中に、このオブジェクトが行いたい処理
        ///// </summary>
        ///// <param name="eventName"></param>
        ///// <param name="eventValue"></param>
        //private void OnTalkEventHandle(string eventName, string eventValue)
        //{
        //    Debug.Log($"eventName: {eventName}, eventValue:{eventValue}");

        //    switch (eventName)
        //    {
        //        case "Camera":
        //            CameraManager.Instance.OnSelectChangeCamera(eventValue);
        //            break;
        //    }
        //}
        ///// <summary>
        ///// トーク終了時にこのオブジェクトが行いたい処理
        ///// </summary>
        //private void OnTalkFinishEvent()
        //{
        //    CameraManager.Instance.OnSelectChangeCamera("DefaultCamera");
        //}
    }
}
