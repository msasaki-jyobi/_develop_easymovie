using Cinemachine;
using Cysharp.Threading.Tasks;
using develop_common;
using develop_timeline;
using develop_tps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace develop_easymovie
{
    public class EasyMovieManager : SingletonMonoBehaviour<EasyMovieManager>
    {
        public CinemachineVirtualCamera DefaultVCam;
        public CinemachineBrain Brain;
        public TextMeshProUGUI MessageTextUGUI;
        public UnitActionLoader UnitActionLoader;
        public Rigidbody UnitRigidBody;

        private bool _isPlaying;
        private EasyMoviePlayer _playingEasyMoviePlayer;

        public event Action MovieFinishEvent;


        private void Start()
        {
            DirectorManager.Instance.FinishEvent += OnTimelineFinish;
        }

        public async void PlayMovie(EasyMoviePlayer easyPlayer)
        {
            if (_isPlaying) return;
            _isPlaying = true;

            _playingEasyMoviePlayer = easyPlayer;
            foreach (EasyMovieInfo info in easyPlayer.infos)
            {
                LoadMovie(info);
                await UniTask.Delay(1000 * (int)info.NextDelayTime, ignoreTimeScale: !info.DelayGameStop);

            }
            EndEasyMovie();
        }
        private void LoadMovie(EasyMovieInfo info)
        {
            if (info.IsChangeCamera)
                SetChangeCamera(info.SetChangeCameraName);

            if (info.IsSetMessage)
                SetMessageText(info.SetMessage);
            ;
            if (info.IsChangeBlendTime)
                ChangeCameraBlendTime(info.SetBlendTime);

            if (info.IsActiveSelectUI)
                develop_common.UIStateManager.Instance.OnChangeStateAndButtons("Select");

            if (info.IsTimeScale)
                Time.timeScale = info.SetTimeScale;




            info.StartUnityEvent?.Invoke();
        }
        /// <summary>
        /// FinishMovie�̍Ō�̗v�f���ǂݍ��܂ꂽ��̏����iYES�ENo���ʁj
        /// </summary>
        /// <returns></returns>
        private async UniTask EndEasyMovie()
        {
            await UniTask.Delay(100);

            if (DefaultVCam != null)
                CameraManager.Instance.ChangeActiveCamera(DefaultVCam);

            // UI��Close
            develop_common.UIStateManager.Instance.ChangeUIState("Close");
            ChangeCameraBlendTime(0);

            // Text������ۂ�
            MessageTextUGUI.text = "";

            _isPlaying = false;
            //_playingEasyMoviePlayer = null;
        }

        public void SetChangeCamera(string cameraName)
        {
            CameraManager.Instance.OnSelectChangeCamera(cameraName);
        }
        public void SetMessageText(string message)
        {
            var color = MessageTextUGUI.color;
            color.a = 1;
            MessageTextUGUI.color = color;

            //develop_common.UIStateManager.Instance.ChangeUIState("Message");
            MessageTextUGUI.text = message;
        }
        public void ChangeCameraBlendTime(float time)
        {
            Brain.m_DefaultBlend.m_Time = time;
        }
        /// <summary>
        /// YES����������
        /// </summary>
        public void OnSubmitMovie()
        {
            Time.timeScale = 1;
            UnitRigidBody.isKinematic = true;
            UnitActionLoader.ChangeStatus(EUnitStatus.Executing);
            //develop_common.UIStateManager.Instance.OnChangeStateAndButtons("Close");
            if (_playingEasyMoviePlayer.SubmitDirectorPlayer != null)
            {
                DirectorManager.Instance.PlayEasyMovie(_playingEasyMoviePlayer);
            }
        }
        /// <summary>
        /// Timeline���[�r�[���Đ�����������
        /// </summary>
        private void OnTimelineFinish()
        {
            // Movie�f�[�^����Finish����
            if (_playingEasyMoviePlayer != null)
                _playingEasyMoviePlayer.SubmitFinishMovie();

            _playingEasyMoviePlayer = null;
            UnitRigidBody.isKinematic = false;
            //UnitActionLoader.ChangeStatus(EUnitStatus.Ready);

            // �o�^���ꂽEM��Finish�����s
            MovieFinishEvent?.Invoke();
        }

    }
}
