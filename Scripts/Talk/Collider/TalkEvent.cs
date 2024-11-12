using Cysharp.Threading.Tasks;
using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    [RequireComponent(typeof(BoxCollider))]

    public class TalkEvent : MonoBehaviour
    {
        public bool IsHiddenCheckFlg;
        public string HiddenFlgName = "";
        public bool IsPlayingCheckFlg;
        public string PlayingFlgName = "";
        [Space(10)]
        public string TargetName = "Player";
        public List<TalkData> Talks = new List<TalkData>();

        public List<StringEventHandle> TalkStartEvents = new List<StringEventHandle>();
        public List<StringEventHandle> TalkFinishEvents = new List<StringEventHandle>();

        private bool _isHit;
        public bool IsPlaying;

        public bool IsHitAutoPlay;
        public bool OnePlayHide;

        private bool _isHidden;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                if (_isHit && !IsHitAutoPlay) // �G��Ă�Ƃ�
                {
                    TalkPlay();
                }
        }

        public void TalkPlay()
        {
            if (!_isHidden)
                if (!IsPlaying)
                {
                    IsPlaying = true;
                    TalkManager.Instance.StartTyping(Talks, this);

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
                if (!PlayFlgCheck()) // �t���O�������ĂȂ��ꍇReturn
                    return;
                if (HiddenFlgCheck()) // ��\���t���O����������Return
                    return;

                await UniTask.Delay(10);

                if (IsHitAutoPlay)
                    TalkPlay();
                else
                    _isHit = true;
            }
        }

        private async void OnExit(GameObject hit)
        {
            if (hit.name == TargetName)
            {
                //if (PlayFlgCheck()) // �t���O�������ĂȂ��ꍇReturn
                //    return;
                //if (!HiddenFlgCheck()) // ��\���t���O����������Return
                //    return;

                //await UniTask.Delay(10);

                _isHit = false;
            }
        }

    
        /// <summary>
        /// true = ���s�\�ł�
        /// </summary>
        /// <returns></returns>
        public bool PlayFlgCheck() 
        {
            if(!IsPlayingCheckFlg) return true;

            return FlgManager.Instance.CheckSelectNameFlg(PlayingFlgName); // �t���O��"���݂���" �ꍇ true 
        }
        /// <summary>
        /// true = ���s�s�\�ł�
        /// </summary>
        /// <returns></returns>
        public bool HiddenFlgCheck()
        {
            if (!IsHiddenCheckFlg) return false;

            return FlgManager.Instance.CheckSelectNameFlg(HiddenFlgName); // ��\���t���O��"���݂���" �ꍇ true
        }
    }
}
