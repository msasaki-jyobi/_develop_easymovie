using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_easymovie
{

    public class EasyMoviePlayer : MonoBehaviour
    {
        public List<EasyMovieInfo> infos = new List<EasyMovieInfo>();

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
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
    }
}
