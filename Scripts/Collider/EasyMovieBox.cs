using Cysharp.Threading.Tasks;
using develop_easymovie;
using develop_timeline;
using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class EasyMovieBox : MonoBehaviour
    {
        private string _hitObjectName = "Player";

        public bool IsFlgCheck;
        public string CheckFlgName;

        public DirectorPlayer EnterAutoPlayDirectorPlayer;
        public EasyMoviePlayer EasyMoviePlayer;
        public string EnterMessage;
        public string EnterAddFlg;
        public string EnterTrigger;

        public bool PlayingColliderHidden; // 実行後に非表示（本来はTriggerで隠す

        private void OnTriggerEnter(Collider other)
        {
            Play(other.gameObject);
        }


        private async void Play(GameObject other)
        {
            if (other.name != _hitObjectName) return;

            bool check = true;
            if (IsFlgCheck) check = check && FlgManager.Instance.CheckFlg(CheckFlgName);

            if (!check) return;

            if (PlayingColliderHidden)
                if (TryGetComponent<BoxCollider>(out var collider)) collider.enabled = false;

            if (EnterAutoPlayDirectorPlayer != null)
            {
                EnterAutoPlayDirectorPlayer.Play();
                gameObject.SetActive(false);
            }
            if (EasyMoviePlayer != null)
                EasyMovieManager.Instance.PlayMovie(EasyMoviePlayer);
            else
            {
                TextFadeController.Instance.UpdateMessageText(EnterMessage);
                if (EnterTrigger != "")
                {
                    // 数秒後に実行する
                    await UniTask.Delay(3000);
                    FlgManager.Instance.LoadTrigger(EnterTrigger, true);
                    if (EnterAddFlg != "")
                    {
                        FlgManager.Instance.AddFlg(EnterAddFlg);
                    }
                }
            }

        }
    }



}