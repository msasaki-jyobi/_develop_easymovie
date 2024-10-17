using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    public class CameraManager : SingletonMonoBehaviour<CameraManager>
    {
        [Header("自動取得")]
        public List<CinemachineVirtualCamera> VCams = new List<CinemachineVirtualCamera>();
        public List<CinemachineFreeLook> FreeLooks = new List<CinemachineFreeLook>();

        // 現在アクティブのカメラ
        private CinemachineVirtualCamera _activeVcam;
        private TalkManager _talkManager;

        private void Start()
        {
            foreach (var cam in FindObjectsOfType<CinemachineVirtualCamera>())
                VCams.Add(cam);
            foreach (var freeCam in FindObjectsOfType<CinemachineFreeLook>())
                FreeLooks.Add(freeCam);

            _talkManager = TalkManager.Instance;
            if (_talkManager != null)
            {

                _talkManager.TalkStartEvent += OnTalkStartHandle;
                _talkManager.TalkUpdateEvent += OnTalkUpdateHandle;
                _talkManager.TalkFinishEvent += OnTalkFinishHandle;
            }

        }

        /// <summary>
        /// 渡されたカメラに切り替える（アクティブカメラを切り替える）
        /// </summary>
        /// <param name="vcam"></param>
        public void ChangeActiveCamera(CinemachineVirtualCamera vcam)
        {
            if (_activeVcam != null)
                _activeVcam.m_Priority = 0;

            _activeVcam = vcam;
            _activeVcam.m_Priority = 30;
            Debug.Log($"ActiveCamera:{_activeVcam.name}");
        }
        /// <summary>
        /// オブジェクト名と一致するカメラが合ったら、一致した名前の切り替える
        /// </summary>
        /// <param name="cameraName"></param>
        public void OnSelectChangeCamera(string cameraName)
        {
            for (int i = 0; i < VCams.Count; i++)
            {
                if (VCams[i].name == cameraName)
                    ChangeActiveCamera(VCams[i]);
            }
        }
        /// <summary>
        /// 名前が一致したカメラを取得する
        /// </summary>
        /// <param name="cameraName"></param>
        /// <returns></returns>
        public CinemachineVirtualCamera OnGetCamera(string cameraName)
        {
            for (int i = 0; i < VCams.Count; i++)
            {
                if (VCams[i].name == cameraName)
                    return VCams[i];
            }
            return null;
        }
        public async void ChangeFreeLookCamera(CinemachineFreeLook cinemachineFreeLook)
        {
            foreach (var cam in VCams)
                cam.m_Priority = 0;
            foreach (var freeCam in FreeLooks)
                freeCam.m_Priority = 0;

            await UniTask.Delay(100);
            cinemachineFreeLook.m_Priority = 30;
        }

        private void OnTalkStartHandle()
        {

        }

        private void OnTalkUpdateHandle(string eventName, string eventValue)
        {
            if (eventName == "Camera")
                OnSelectChangeCamera(eventValue);
        }
        private void OnTalkFinishHandle()
        {
            _activeVcam.m_Priority = 0;
        }
    }
}
