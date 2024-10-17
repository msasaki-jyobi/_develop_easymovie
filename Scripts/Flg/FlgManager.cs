using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace develop_easymovie
{
    public class FlgManager : SingletonMonoBehaviour<FlgManager>
    {
        public List<StringEventHandle> FlgEvents = new List<StringEventHandle>();

        public event Action<string, string> CheckFlgEvent;
        // F���Ă����t���O����������A���̃I�u�W�F�N�g������ŏ㏑������ˁ@�����s����^�C�~���O
        private void Start()
        {
            foreach (StringEventHandle flg in FlgEvents)
                CheckFlgEvent?.Invoke(flg.EventName, flg.EventValue);
        }

        /// <summary>
        /// �t���O�C�x���g��V�K�ǉ��E���Z����
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <param name="value"></param>
        public void AddFlg(string eventFlgName, string value)
        {
            for (int i = 0; i < FlgEvents.Count; i++)
            {
                if (FlgEvents[i].EventName == eventFlgName)
                {
                    // ���Z����
                    var valueA = FlgEvents[i].EventValue;
                    var valueB = value;
                    int total = int.Parse(valueA + valueB);

                    FlgEvents[i].EventValue += total.ToString();
                    CheckFlgEvent?.Invoke(eventFlgName, total.ToString());

                    Debug.Log($"{eventFlgName} ���Z���F{value}, �c��F{total}");
                    return;
                }
            }

            // ���݂��Ȃ��ꍇ
            var flg = new StringEventHandle();
            flg.EventName = eventFlgName;
            flg.EventValue = value;
            FlgEvents.Add(flg);
        }

        /// <summary>
        /// �t���O�C�x���g�����݂��邩�`�F�b�N
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <returns></returns>
        public bool CheckSelectNameFlg(string eventFlgName)
        {
            for (int i = 0; i < FlgEvents.Count; i++)
            {
                if (FlgEvents[i].EventName == eventFlgName)
                {
                    return true;
                }
            }

            // ���݂��Ȃ��ꍇ
            return false;
        }
        /// <summary>
        /// �t���O�C�x���g�̌����擾����
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <returns></returns>
        public int GetSelectNameFlgValue(string eventFlgName)
        {
            for (int i = 0; i < FlgEvents.Count; i++)
            {
                if (FlgEvents[i].EventName == eventFlgName)
                {
                    // ���Z����
                    var valueA = FlgEvents[i].EventValue;
                    int result = int.Parse(valueA);

                    return result;
                }
            }
            // ���݂��Ȃ��ꍇ
            return 0;
        }
    }
}