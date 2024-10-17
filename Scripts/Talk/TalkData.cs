using develop_common;
using develop_timeline;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    [Serializable]
    public class TalkData
    {
        public List<StringEventHandle> StringEvent;
        [Multiline]
        public string JapaneseTalkText;
        [Multiline]
        public string EnglishTalkText;
    }
}