using Cinemachine;
using Cysharp.Threading.Tasks;
using develop_common;
using develop_timeline;
using develop_tps;
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
            FinishEasyMovie();
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
        private async UniTask FinishEasyMovie()
        {
            await UniTask.Delay(100);

            if (DefaultVCam != null)
                CameraManager.Instance.ChangeActiveCamera(DefaultVCam);


            // UI‚ðClose
            develop_common.UIStateManager.Instance.ChangeUIState("Close");
            ChangeCameraBlendTime(0);

            _isPlaying = false;
            _playingEasyMoviePlayer = null;
        }

        public void SetChangeCamera(string cameraName)
        {
            CameraManager.Instance.OnSelectChangeCamera(cameraName);
        }
        public void SetMessageText(string message)
        {
            develop_common.UIStateManager.Instance.ChangeUIState("Message");
            MessageTextUGUI.text = message;
        }
        public void ChangeCameraBlendTime(float time)
        {
            Brain.m_DefaultBlend.m_Time = time;
        }

        public void OnSubmitMovie()
        {
            Time.timeScale = 1;
            UnitRigidBody.isKinematic = true;
            UnitActionLoader.ChangeStatus(EUnitStatus.Executing);
            //develop_common.UIStateManager.Instance.OnChangeStateAndButtons("Close");
            if (_playingEasyMoviePlayer.SubmitPlayableDirector != null)
            {
                _playingEasyMoviePlayer.SubmitPlayableDirector.Play();
                DirectorManager.Instance.SetPlayDirector(_playingEasyMoviePlayer.SubmitPlayableDirector);
            }
        }

        private void OnTimelineFinish(string eventName, string value)
        {
            UnitRigidBody.isKinematic = false;
            UnitActionLoader.ChangeStatus(EUnitStatus.Ready);
        }

    }
}
