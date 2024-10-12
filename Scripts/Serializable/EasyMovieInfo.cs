using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace develop_easymovie
{
    [Serializable]
    public class EasyMovieInfo
    {
        public float NextDelayTime = 1f;
        public bool DelayGameStop;

        [Space(5)]
        public bool IsChangeCamera;
        public string SetChangeCameraName;
        [Space(5)]
        public bool IsSetMessage;
        [Multiline] public string SetMessage;
        [Space(5)]
        public bool IsChangeBlendTime;
        public float SetBlendTime;
        [Space(5)]
        public bool IsTimeScale;
        public float SetTimeScale = 1;
        [Space(5)]
        public bool IsActiveSelectUI;
        [Space(5)]
        public UnityEvent StartUnityEvent;
    }
}