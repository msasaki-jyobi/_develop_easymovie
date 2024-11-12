using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace develop_easymovie
{
    public class FlgManager : SingletonMonoBehaviour<FlgManager>
    {
        public List<StringEventHandle> FlgItems = new List<StringEventHandle>();
        /// <summary>
        /// �t���O�C�x���g��V�K�ǉ��E���Z����
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <param name="value"></param>
        public void AddFlg(string eventFlgName, int value)
        {
            for (int i = 0; i < FlgItems.Count; i++)
            {
                if (FlgItems[i].EventName == eventFlgName)
                {
                    // ���Z����
                    int valueA = int.Parse(FlgItems[i].EventValue);
                    int valueB = value;
                    int total = valueA + valueB;

                    FlgItems[i].EventValue = total.ToString();
                    Debug.Log($"{eventFlgName} ���Z���F{value}, �c��F{total}");
                    return;
                }
            }

            // ���݂��Ȃ��ꍇ
            var flg = new StringEventHandle();
            flg.EventName = eventFlgName;
            flg.EventValue = value.ToString();
            FlgItems.Add(flg);
        }

        /// <summary>
        /// �t���O�C�x���g�����݂��邩�`�F�b�N
        /// </summary>
        /// <param name="eventFlgName"></param>
        /// <returns></returns>
        public bool CheckSelectNameFlg(string eventFlgName)
        {
            for (int i = 0; i < FlgItems.Count; i++)
            {
                if (FlgItems[i].EventName == eventFlgName)
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
            for (int i = 0; i < FlgItems.Count; i++)
            {
                if (FlgItems[i].EventName == eventFlgName)
                {
                    // ���Z����
                    int result = int.Parse(FlgItems[i].EventValue);

                    return result;
                }
            }
            // ���݂��Ȃ��ꍇ
            return 0;
        }

    }
}
