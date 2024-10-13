using Cysharp.Threading.Tasks;
using develop_common;
using develop_timeline;
using Kogane;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace develop_easymovie
{
    public class EasyMoviePlayer : MonoBehaviour
    {
        public List<EasyMovieInfo> infos = new List<EasyMovieInfo>();
        public DirectorPlayer SubmitDirectorPlayer;

        public string SubmitPlayableFinishTrigger = "";
        public string SubmitPlayableFinishAddFlgName = "";
        public string SubmitPlayableFinishMessage = "＜Triggerがない場合再生＞";



        public DictionaryStringString FinishTable = new DictionaryStringString();

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

        /// <summary>
        /// YES が選択されてタイムラインが終了した後の呼び出し
        /// </summary>
        public async void SubmitFinishMovie()
        {
            // Debug.Log($"{_playingEasyMoviePlayer} FinishText:{_playingEasyMoviePlayer.FinishText}");

            // フラグ追加
            if (SubmitPlayableFinishAddFlgName != "")
                FlgManager.Instance.AddFlg(SubmitPlayableFinishAddFlgName);
            // クエスト更新 // フラグ更新 この時点で数秒後にメッセージが表示される・・・。
            if (SubmitPlayableFinishTrigger != "")
            {
                FlgManager.Instance.LoadTrigger(SubmitPlayableFinishTrigger);
            }
            else if (SubmitPlayableFinishMessage != "") // Triggerがない場合はシンプルにメッセージを再生
            {
                TextFadeController.Instance.UpdateMessageText(SubmitPlayableFinishMessage);
            }


            // その他追加実行欄
            foreach (var table in FinishTable)
            {
                // フラグの追加
                if (table.Key == "Flg")
                    FlgManager.Instance.AddFlg(table.Value);
                // アイテム入手
                // ダメージ＋GameOver
            }
        }
    }
}
