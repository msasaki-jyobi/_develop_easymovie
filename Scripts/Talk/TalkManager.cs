using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;  // UniTaskを使うために必要
using develop_common;
using DG.Tweening;             // DoTweenを使うために必要
using TMPro;                   // TextMeshProを使うために必要
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

        public TMP_Text TextComponent;      // 表示するTextMeshProコンポーネント
        public develop_common.ClipData TypingSound;        // 効果音
        public float TypingSpeed = 0.05f;   // 文字が表示されるスピード(秒)
        public float FastTypingSpeed = 0.01f; // スペースキーが押された時の高速スピード

        private CancellationTokenSource _cts;  // キャンセル用のトークン
        private bool _isTyping = false;        // タイピング中かどうか
        private bool _isSkipping = false;      // スキップ中かどうか
        private bool _isTextFullyDisplayed = false; // 全ての文字が表示されたかどうか
        private float _currentTypingSpeed;     // 現在のタイピングスピード

        public event Action<string, string> TalkStartEvent;  // 一つのトークがはじまるときのイベント
        public event Action<string, string> TalkUpdateEvent;  // 一つのトークが読み込むときのイベント
        public event Action<string, string> TalkFinishEvent;  // 一つのトークがおわるときのイベント

        // 会話を開始する関数 (複数の会話文を処理する)
        // hitEvent：EnterEvent、FinishEvent実行用
        public void StartTyping(List<TalkData> talkDatas, TalkEvent talkEvent = null)
        {
            // もしすでにタイピング中なら前のタスクをキャンセル
            if (_isTyping)
            {
                _cts?.Cancel();
            }

            // 新しいCancellationTokenSourceを作成
            _cts = new CancellationTokenSource();

            // StartEvent
            if (talkEvent != null)
                foreach (var ev in talkEvent.TalkStartEvents)
                    TalkStartEvent?.Invoke(ev.EventName, ev.EventValue);

            // 新たにタイピングエフェクトを開始 (List<string>を順に処理)
            TypeTextSequence(talkDatas, _cts.Token, talkEvent).Forget();  // UniTaskを使用
        }

        // 文字列リストを順番に表示するタスク
        private async UniTaskVoid TypeTextSequence(List<TalkData> talkDatas, CancellationToken token, TalkEvent talkEvent = null)
        {
            _isTyping = true;  // タイピング中フラグをオン



            foreach (var text in talkDatas)
            {
                if (token.IsCancellationRequested)  // キャンセルされた場合
                {
                    return;
                }

                // イベント
                foreach (var ev in text.StringEvent)
                    TalkUpdateEvent?.Invoke(ev.EventName, ev.EventValue);

                string loadText = GameLanguage == ELanguage.Japanese ? text.JapaneseTalkText : text.EnglishTalkText;

                // 各テキストを順番に表示
                await TypeText(loadText, token);

                // スペースキー待ち
                await WaitForSpaceKeyPress(token);
            }

            _isTyping = false;  // タイピング終了フラグをオフ

            TextComponent.text = "";  // テキストをクリア

            // FinishEvent
            if (talkEvent != null)
            {
                foreach (var ev in talkEvent.TalkFinishEvents)
                    TalkFinishEvent?.Invoke(ev.EventName, ev.EventValue);

                await UniTask.Delay(100);

                talkEvent.IsPlaying = false;
            }

            Debug.Log("会話終了？");
        }

        // 文字列を一文字ずつ表示するタスク
        private async UniTask TypeText(string text, CancellationToken token)
        {
            TextComponent.text = "";  // テキストをクリア
            _isSkipping = false;      // スキップフラグをリセット
            _isTextFullyDisplayed = false; // 全ての文字が表示されていない状態にリセット

            _currentTypingSpeed = TypingSpeed;  // 現在のスピードを通常スピードに設定

            foreach (char letter in text.ToCharArray())
            {
                if (token.IsCancellationRequested)  // キャンセルされた場合
                {
                    return;
                }

                if (_isSkipping)
                {
                    TextComponent.text = text;  // 残りのテキストを一気に表示
                    break;
                }

                TextComponent.text += letter;  // 文字を追加

                if (TypingSound != null)
                {
                    AudioManager.Instance.PlayOneShotClipData(TypingSound);
                }

                // 文字を表示してから指定の時間待機
                await UniTask.Delay(TimeSpan.FromSeconds(_currentTypingSpeed), cancellationToken: token);
            }

            _isTextFullyDisplayed = true; // 全ての文字が表示されたことを記録

            // 1つのテキストが終わったら短い待機時間を設ける（例えば0.02秒）
            await UniTask.Delay(TimeSpan.FromSeconds(0.02f), cancellationToken: token);

            // スピードを元に戻す
            _currentTypingSpeed = TypingSpeed;


        }

        // スペースキーが押されるのを待つタスク
        private async UniTask WaitForSpaceKeyPress(CancellationToken token)
        {
            // スペースキーが押されるのを待機
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
                        break;  // スキップ中または全てのテキストが表示されていれば次へ進む
                    }
                    else
                    {
                        // 現在のテキストを高速で表示する
                        _currentTypingSpeed = FastTypingSpeed;
                        _isSkipping = true;  // スキップフラグを設定
                    }
                }

                await UniTask.Yield(token);  // 次のフレームまで待機
            }
        }

        // 文字の表示速度を変更する関数
        public void SetTypingSpeed(float newSpeed)
        {
            TypingSpeed = newSpeed;
        }

        public void ForceStopTyping()
        {
            // もしタイピング中なら、現在のタスクをキャンセル
            if (_isTyping)
            {
                _cts?.Cancel();  // 現在のタスクをキャンセル
                _isTyping = false;  // タイピングフラグをオフにする

                // 必要に応じて状態をリセット
                _isSkipping = false;
                _isTextFullyDisplayed = false;

                // テキスト表示をクリアする
                TextComponent.text = "";
            }
        }
    }
}
