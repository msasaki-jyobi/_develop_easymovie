using Cinemachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace develop_easymovie
{
    public class CameraManager : SingletonMonoBehaviour<CameraManager>
    {
        // デフォルトのカメラ
        [Header("追加機能：JKey 切り替え")]
        public CinemachineBrain Brain;

        [Header("追加機能：JKey 切り替え")]
        public List<CinemachineVirtualCamera> DefaultVcams;
        public CinemachineFreeLook DefaultFreeLook;

        [Header("自動取得")]
        public List<CinemachineVirtualCamera> VCams = new List<CinemachineVirtualCamera>();
        public List<CinemachineFreeLook> FreeLooks = new List<CinemachineFreeLook>();

        // 現在アクティブのカメラ
        public CinemachineVirtualCamera ActiveVcam;
        private TalkManager _talkManager;
        public ReactiveProperty<int> CameraSlotNum = new ReactiveProperty<int>();

        private void Start()
        {
            foreach (var cam in FindObjectsOfType<CinemachineVirtualCamera>())
                VCams.Add(cam);
            foreach (var freeCam in FindObjectsOfType<CinemachineFreeLook>())
                FreeLooks.Add(freeCam);

            _talkManager = TalkManager.Instance;
            if (_talkManager != null)
            {
                _talkManager.TalkUpdateEvent += OnTalkUpdateHandle;
            }

            CameraSlotNum
                .Subscribe((x) =>
                {
                    if (CameraSlotNum.Value == 100) return;

                    var max = DefaultVcams.Count; // FreeLookのカメラNum

                    if (CameraSlotNum.Value == max)
                        ChangeFreeLookCamera(DefaultFreeLook);
                    else
                    {
                        ChangeActiveCamera(DefaultVcams[CameraSlotNum.Value]);

                    }
                });
        }

        private void Update()
        {
            // カメラ切り替え
            if (Input.GetKeyDown(KeyCode.J))
                SetDefaultCamera();
        }

        /// <summary>
        /// 渡されたカメラに切り替える（アクティブカメラを切り替える）
        /// </summary>
        /// <param name="vcam"></param>
        public async void ChangeActiveCamera(CinemachineVirtualCamera vcam, float blendTime = -1, Transform lookTarget = null, Transform followTarget = null, Vector3 offset = default, bool freeCameraReset = true)
        {
            if (blendTime >= 0)
                Brain.m_DefaultBlend.m_Time = blendTime;

            if (ActiveVcam != null)
                ActiveVcam.m_Priority = 0;
            if (freeCameraReset)
                foreach (var freeCam in FreeLooks)
                {
                    if (freeCam == null) continue;
                    freeCam.m_Priority = 0;
                }
            foreach (var cam in VCams)
            {
                if (cam == null) continue;
                cam.m_Priority = 0;
            }
            if (lookTarget != null)
                vcam.m_LookAt = lookTarget;
            if (followTarget != null)
                vcam.m_Follow = followTarget;
            if (offset != default)
            {
                var x = Random.Range(-offset.x, offset.x);
                var y = Random.Range(-offset.y, offset.y);
                var z = Random.Range(-offset.z, offset.z);
                var transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
                if (transposer != null)
                {
                    transposer.m_FollowOffset.x = x;
                    transposer.m_FollowOffset.y = y;
                    transposer.m_FollowOffset.z = z;
                }
            }


            await UniTask.Delay(1);

            ActiveVcam = vcam;
            ActiveVcam.m_Priority = 30;
            Debug.Log($"ActiveCamera　カメラを切り替えました:{ActiveVcam.name}");
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
                {
                    ChangeActiveCamera(VCams[i]);
                }
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
        /// <summary>
        /// 指定されたFreeCameraに切り替える
        /// </summary>
        /// <param name="cinemachineFreeLook"></param>
        public async void ChangeFreeLookCamera(CinemachineFreeLook cinemachineFreeLook)
        {
            if (cinemachineFreeLook == null)
                return;

            foreach (var cam in VCams)
                cam.m_Priority = 0;
            foreach (var freeCam in FreeLooks)
                freeCam.m_Priority = 0;

            await UniTask.Delay(100);
            cinemachineFreeLook.m_Priority = 50;
        }

        ///// <summary>
        ///// 一定時間引数のカメラに切り替え、時間経過後元のカメラに戻す
        ///// </summary>
        ///// <param name="vcam"></param>
        ///// <param name="timer"></param>
        //public async void ChangeTimeCamera(CinemachineVirtualCamera vcam, float timer = 1f)
        //{
        //    var oldCamera = _activeVcam;
        //    ChangeActiveCamera(vcam);
        //    await UniTask.Delay((int)(1000 * timer));
        //    ChangeActiveCamera(oldCamera);
        //}

        /// <summary>
        /// ランダムなUnitInstanceカメラに切り替え、LookAt対象をTargetにする
        /// </summary>
        public void ChangeRandomCamera(Transform Target, CinemachineFreeLook freeLook)
        {
            if (freeLook == null) return;

            freeLook.Follow = Target;
            freeLook.LookAt = Target;

            // パラメータも設定
            freeLook.m_Orbits[0].m_Radius = Random.Range(0.1f, 1);
            freeLook.m_Orbits[1].m_Radius = Random.Range(0.1f, 1);
            freeLook.m_Orbits[2].m_Radius = Random.Range(0.1f, 1);

            freeLook.m_Orbits[0].m_Height = Random.Range(0.5f, 1);
            freeLook.m_Orbits[1].m_Height = Random.Range(0.1f, 1);
            freeLook.m_Orbits[2].m_Height = Random.Range(-0f, -1);

            //freeLook.m_YAxis.Value = 0;
            //freeLook.m_XAxis.Value = 0;


            ChangeFreeLookCamera(freeLook);
        }
        /// <summary>
        /// デフォルトカメラを切り替える
        /// </summary>
        public async void SetDefaultCamera(bool isChange = true)
        {
            if (DefaultVcams == null) return;
            if (DefaultVcams.Count == 0) return;
            if (DefaultFreeLook == null) return;

            if (isChange) // 次のカメラに切り替える
            {
                var num = CameraSlotNum.Value;
                CameraSlotNum.Value = num >= DefaultVcams.Count ? 0 : num + 1;
            }
            else // 切り替え前の状態に戻す
            {
                //if (CameraSlotNum.Value >= DefaultVcams.Count)
                //    ChangeFreeLookCamera(DefaultFreeLook);
                //else
                ChangeActiveCamera(DefaultVcams[0]);
            }
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
            SetDefaultCamera(false);
        }
    }
}
