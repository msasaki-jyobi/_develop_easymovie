using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace develop_easymovie
{

    public class EasyMoviePlayer : MonoBehaviour
    {
        public List<EasyMovieInfo> infos = new List<EasyMovieInfo>();
        public PlayableDirector SubmitPlayableDirector;

        [SerializeField] private bool _isDebugX;

        void Update()
        {
            if (_isDebugX)
                if (Input.GetKeyDown(KeyCode.X))
                    EasyMovieManager.Instance.PlayMovie(this);
        }
        /// <summary>
        /// カメラを切り替えてもらう
        /// </summary>
        /// <param name="vcamName"></param>
        public void OnSetChangeCamera(string vcamName)
        {
            CameraManager.Instance.OnSelectChangeCamera(vcamName);
        }
        /// <summary>
        /// メッセージを切り替えてもらう
        /// </summary>
        /// <param name="vcamName"></param>
        public void OnSetMessage(string message)
        {
            EasyMovieManager.Instance.SetMessageText(message);
        }
        public void OnChangeTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
        public void OnChangeUIState(string changeState)
        {
            // ステートを設定
            develop_common.UIStateManager.Instance.OnChangeStateAndButtons(changeState);
        }
        public void OnSelectUIState(string timelineName)
        {
            // タイムラインを用意？ timelineName
            // はいを押したら実行されるタイムライン名をセットしておく

            // ステートを設定
            develop_common.UIStateManager.Instance.OnChangeStateAndButtons("Select");
        }
        public void OnBookingTimeline()
        {

        }
        public void OnChangeCameraBlendTime(float time)
        {
            EasyMovieManager.Instance.ChangeCameraBlendTime(time);
        }
    }
}
