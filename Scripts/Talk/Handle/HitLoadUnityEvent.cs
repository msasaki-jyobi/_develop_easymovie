using Cysharp.Threading.Tasks;
using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace develop_easymovie
{
    public class HitLoadUnityEvent : MonoBehaviour
    {
        public HitEvent FlgCheckHitEvent;

        [SerializeField] private string TargetTag = "Unit";
        [SerializeField] private string TargetName = "Player";

        public UnityEvent EnterEvent;
        public UnityEvent ExitEvent;

        private void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnHit(collision.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(other.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnExit(collision.gameObject);
        }

        private async void OnHit(GameObject hit)
        {
            if(FlgCheckHitEvent != null)
            {
                if (!FlgCheckHitEvent.PlayFlgCheck()) // フラグが持ってない場合Return
                    return;
                if (FlgCheckHitEvent.HiddenFlgCheck()) // 非表示フラグがあったらReturn
                    return;

                await UniTask.Delay(10);
            }

            if (hit.CompareTag(TargetTag) || hit.name == TargetName)
                EnterEvent?.Invoke();
        }
        private void OnExit(GameObject hit)
        {
            if (hit.CompareTag(TargetTag) || hit.name == TargetName)
                ExitEvent?.Invoke();
        }
    }

}
