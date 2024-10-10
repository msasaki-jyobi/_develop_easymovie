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
        public UnityEvent StartUnityEvent;
    }
}