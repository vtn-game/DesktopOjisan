using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DesktopOjisan.UI
{
    /// <summary>
    /// おじさんの会話文を表示するUI
    /// </summary>
    public class OjisanDialogUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private TextMeshProUGUI dialogText;
        [SerializeField] private Image ojisanIcon;

        [Header("Settings")]
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private float autoHideDelay = 5f;
        [SerializeField] private bool useTypingEffect = true;

        private Coroutine _typingCoroutine;
        private Coroutine _autoHideCoroutine;
        private string _currentMessage;

        public event Action OnDialogComplete;
        public event Action OnDialogHidden;

        public bool IsShowing => dialogPanel != null && dialogPanel.activeSelf;

        private void Awake()
        {
            if (dialogPanel != null)
            {
                dialogPanel.SetActive(false);
            }
        }

        /// <summary>
        /// メッセージを表示
        /// </summary>
        public void ShowMessage(string message, bool autoHide = true)
        {
            if (dialogPanel == null || dialogText == null) return;

            _currentMessage = message;
            dialogPanel.SetActive(true);

            // 既存のコルーチンを停止
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }
            if (_autoHideCoroutine != null)
            {
                StopCoroutine(_autoHideCoroutine);
            }

            if (useTypingEffect)
            {
                _typingCoroutine = StartCoroutine(TypeText(message, autoHide));
            }
            else
            {
                dialogText.text = message;
                OnDialogComplete?.Invoke();

                if (autoHide)
                {
                    _autoHideCoroutine = StartCoroutine(AutoHide());
                }
            }
        }

        /// <summary>
        /// タイピングエフェクト付きでテキスト表示
        /// </summary>
        private IEnumerator TypeText(string message, bool autoHide)
        {
            dialogText.text = "";

            foreach (var c in message)
            {
                dialogText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            _typingCoroutine = null;
            OnDialogComplete?.Invoke();

            if (autoHide)
            {
                _autoHideCoroutine = StartCoroutine(AutoHide());
            }
        }

        /// <summary>
        /// 自動で非表示
        /// </summary>
        private IEnumerator AutoHide()
        {
            yield return new WaitForSeconds(autoHideDelay);
            Hide();
        }

        /// <summary>
        /// ダイアログを非表示
        /// </summary>
        public void Hide()
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }
            if (_autoHideCoroutine != null)
            {
                StopCoroutine(_autoHideCoroutine);
                _autoHideCoroutine = null;
            }

            if (dialogPanel != null)
            {
                dialogPanel.SetActive(false);
            }

            OnDialogHidden?.Invoke();
        }

        /// <summary>
        /// タイピングをスキップして全文表示
        /// </summary>
        public void SkipTyping()
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;

                if (dialogText != null && !string.IsNullOrEmpty(_currentMessage))
                {
                    dialogText.text = _currentMessage;
                }

                OnDialogComplete?.Invoke();

                _autoHideCoroutine = StartCoroutine(AutoHide());
            }
        }

        /// <summary>
        /// おじさんアイコンを設定
        /// </summary>
        public void SetOjisanIcon(Sprite icon)
        {
            if (ojisanIcon != null)
            {
                ojisanIcon.sprite = icon;
            }
        }
    }
}
