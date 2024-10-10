using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace develop_easymovie
{
    public class EasyMovieManager : SingletonMonoBehaviour<EasyMovieManager>
    {
        public CinemachineVirtualCamera DefaultVCam;
        public TextMeshProUGUI MessageTextUGUI;

        public async void PlayMovie(EasyMoviePlayer easyPlayer)
        {
            foreach(EasyMovieInfo info in easyPlayer.infos) 
            { 
                LoadMovie(info);
                await UniTask.Delay(1000 * (int)info.NextDelayTime);
            }
            FinishEasyMovie();
        }

        private void LoadMovie(EasyMovieInfo info)
        {
            info.StartUnityEvent?.Invoke();
        }

        public void SetMessageText(string message)
        {
            MessageTextUGUI.text = message;
        }

        public void FinishEasyMovie()
        {
            if (DefaultVCam != null)
                CameraManager.Instance.ChangeActiveCamera(DefaultVCam);
        }
    }
}
