using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;  // UniTask���g�����߂ɕK�v
using develop_common;
using DG.Tweening;             // DoTween���g�����߂ɕK�v
using TMPro;                   // TextMeshPro���g�����߂ɕK�v
using UnityEngine;
using UnityEngine.Timeline;
namespace develop_easymovie
{
    public enum ELanguage
    {
        Japanese,
        English,
    }

    public class TalkManager : SingletonMonoBehaviour<TalkManager>
    {
        public ELanguage GameLanguage;

        public TMP_Text TextComponent;      // �\������TextMeshPro�R���|�[�l���g
        public develop_common.ClipData TypingSound;        // ���ʉ�
        public float TypingSpeed = 0.05f;   // �������\�������X�s�[�h(�b)
        public float FastTypingSpeed = 0.01f; // �X�y�[�X�L�[�������ꂽ���̍����X�s�[�h

        private CancellationTokenSource _cts;  // �L�����Z���p�̃g�[�N��
        private bool _isTyping = false;        // �^�C�s���O�����ǂ���
        private bool _isSkipping = false;      // �X�L�b�v�����ǂ���
        private bool _isTextFullyDisplayed = false; // �S�Ă̕������\�����ꂽ���ǂ���
        private float _currentTypingSpeed;     // ���݂̃^�C�s���O�X�s�[�h

        public event Action<string, string> TalkStartEvent;  // ��̃g�[�N���͂��܂�Ƃ��̃C�x���g
        public event Action<string, string> TalkUpdateEvent;  // ��̃g�[�N���ǂݍ��ނƂ��̃C�x���g
        public event Action<string, string> TalkFinishEvent;  // ��̃g�[�N�������Ƃ��̃C�x���g

        // ��b���J�n����֐� (�����̉�b������������)
        // hitEvent�FEnterEvent�AFinishEvent���s�p
        public void StartTyping(List<TalkData> talkDatas, TalkEvent talkEvent = null)
        {
            // �������łɃ^�C�s���O���Ȃ�O�̃^�X�N���L�����Z��
            if (_isTyping)
            {
                _cts?.Cancel();
            }

            // �V����CancellationTokenSource���쐬
            _cts = new CancellationTokenSource();

            // StartEvent
            if (talkEvent != null)
                foreach (var ev in talkEvent.TalkStartEvents)
                    TalkStartEvent?.Invoke(ev.EventName, ev.EventValue);

            // �V���Ƀ^�C�s���O�G�t�F�N�g���J�n (List<string>�����ɏ���)
            TypeTextSequence(talkDatas, _cts.Token, talkEvent).Forget();  // UniTask���g�p
        }

        // �����񃊃X�g�����Ԃɕ\������^�X�N
        private async UniTaskVoid TypeTextSequence(List<TalkData> talkDatas, CancellationToken token, TalkEvent talkEvent = null)
        {
            _isTyping = true;  // �^�C�s���O���t���O���I��



            foreach (var text in talkDatas)
            {
                if (token.IsCancellationRequested)  // �L�����Z�����ꂽ�ꍇ
                {
                    return;
                }

                // �C�x���g
                foreach (var ev in text.StringEvent)
                    TalkUpdateEvent?.Invoke(ev.EventName, ev.EventValue);

                string loadText = GameLanguage == ELanguage.Japanese ? text.JapaneseTalkText : text.EnglishTalkText;

                // �e�e�L�X�g�����Ԃɕ\��
                await TypeText(loadText, token);

                // �X�y�[�X�L�[�҂�
                await WaitForSpaceKeyPress(token);
            }

            _isTyping = false;  // �^�C�s���O�I���t���O���I�t

            TextComponent.text = "";  // �e�L�X�g���N���A

            // FinishEvent
            if (talkEvent != null)
            {
                foreach (var ev in talkEvent.TalkFinishEvents)
                    TalkFinishEvent?.Invoke(ev.EventName, ev.EventValue);

                await UniTask.Delay(100);

                talkEvent.IsPlaying = false;
            }

            Debug.Log("��b�I���H");
        }

        // ��������ꕶ�����\������^�X�N
        private async UniTask TypeText(string text, CancellationToken token)
        {
            TextComponent.text = "";  // �e�L�X�g���N���A
            _isSkipping = false;      // �X�L�b�v�t���O�����Z�b�g
            _isTextFullyDisplayed = false; // �S�Ă̕������\������Ă��Ȃ���ԂɃ��Z�b�g

            _currentTypingSpeed = TypingSpeed;  // ���݂̃X�s�[�h��ʏ�X�s�[�h�ɐݒ�

            foreach (char letter in text.ToCharArray())
            {
                if (token.IsCancellationRequested)  // �L�����Z�����ꂽ�ꍇ
                {
                    return;
                }

                if (_isSkipping)
                {
                    TextComponent.text = text;  // �c��̃e�L�X�g����C�ɕ\��
                    break;
                }

                TextComponent.text += letter;  // ������ǉ�

                if (TypingSound != null)
                {
                    AudioManager.Instance.PlayOneShotClipData(TypingSound);
                }

                // ������\�����Ă���w��̎��ԑҋ@
                await UniTask.Delay(TimeSpan.FromSeconds(_currentTypingSpeed), cancellationToken: token);
            }

            _isTextFullyDisplayed = true; // �S�Ă̕������\�����ꂽ���Ƃ��L�^

            // 1�̃e�L�X�g���I�������Z���ҋ@���Ԃ�݂���i�Ⴆ��0.02�b�j
            await UniTask.Delay(TimeSpan.FromSeconds(0.02f), cancellationToken: token);

            // �X�s�[�h�����ɖ߂�
            _currentTypingSpeed = TypingSpeed;


        }

        // �X�y�[�X�L�[���������̂�҂^�X�N
        private async UniTask WaitForSpaceKeyPress(CancellationToken token)
        {
            // �X�y�[�X�L�[���������̂�ҋ@
            while (!token.IsCancellationRequested)
            {
                //bool check = true;
                //var currentState = develop_common.UIStateManager.Instance.GetCurrentStateName();
                //check = check && currentState != "QuestSelect" && currentState != "QuestSubmit";
                //if (check)

                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab))
                {
                    if (_isSkipping || _isTextFullyDisplayed)
                    {
                        break;  // �X�L�b�v���܂��͑S�Ẵe�L�X�g���\������Ă���Ύ��֐i��
                    }
                    else
                    {
                        // ���݂̃e�L�X�g�������ŕ\������
                        _currentTypingSpeed = FastTypingSpeed;
                        _isSkipping = true;  // �X�L�b�v�t���O��ݒ�
                    }
                }

                await UniTask.Yield(token);  // ���̃t���[���܂őҋ@
            }
        }

        // �����̕\�����x��ύX����֐�
        public void SetTypingSpeed(float newSpeed)
        {
            TypingSpeed = newSpeed;
        }

        public void ForceStopTyping()
        {
            // �����^�C�s���O���Ȃ�A���݂̃^�X�N���L�����Z��
            if (_isTyping)
            {
                _cts?.Cancel();  // ���݂̃^�X�N���L�����Z��
                _isTyping = false;  // �^�C�s���O�t���O���I�t�ɂ���

                // �K�v�ɉ����ď�Ԃ����Z�b�g
                _isSkipping = false;
                _isTextFullyDisplayed = false;

                // �e�L�X�g�\�����N���A����
                TextComponent.text = "";
            }
        }
    }
}
