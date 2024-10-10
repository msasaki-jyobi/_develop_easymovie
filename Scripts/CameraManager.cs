using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{
    public class CameraManager : SingletonMonoBehaviour<CameraManager>
    {
        [Header("自動取得")]
        public List<CinemachineVirtualCamera> VCams = new List<CinemachineVirtualCamera>();

        // 現在アクティブのカメラ
        private CinemachineVirtualCamera _activeVcam;

        private void Start()
        {
            foreach (var cam in FindObjectsOfType<CinemachineVirtualCamera>())
                VCams.Add(cam);
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
    }
}
