using UnityEngine;

namespace DesktopOjisan.Core
{
    /// <summary>
    /// ウィンドウのフォーカス状態に応じてFPSを制御するコンポーネント
    /// NOTE: 最前面では高FPS、背面では低FPSにすることで省電力化を図る
    /// </summary>
    public class FrameRateLimiter : MonoBehaviour
    {
        [Header("FPS Settings")]
        [SerializeField] private int _foregroundFPS = 120;
        [SerializeField] private int _backgroundFPS = 15;

        [Header("References")]
        [SerializeField] private WindowController _windowController;

        private bool _isForeground = true;
        private float _checkInterval = 0.5f;
        private float _lastCheckTime;

        private void Start()
        {
            // 初期状態は最前面とみなす
            SetForegroundFPS();
        }

        private void Update()
        {
            // 定期的にフォーカス状態をチェック
            if (Time.unscaledTime - _lastCheckTime > _checkInterval)
            {
                _lastCheckTime = Time.unscaledTime;
                CheckFocusState();
            }
        }

        private void CheckFocusState()
        {
            bool isForeground = CheckIsForeground();

            if (isForeground != _isForeground)
            {
                _isForeground = isForeground;

                if (_isForeground)
                {
                    SetForegroundFPS();
                }
                else
                {
                    SetBackgroundFPS();
                }
            }
        }

        private bool CheckIsForeground()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            if (_windowController != null)
            {
                return _windowController.IsForeground();
            }
            return Application.isFocused;
#else
            return Application.isFocused;
#endif
        }

        private void SetForegroundFPS()
        {
            Application.targetFrameRate = _foregroundFPS;
            QualitySettings.vSyncCount = 0;
            Debug.Log($"[FrameRateLimiter] Foreground mode: {_foregroundFPS} FPS");
        }

        private void SetBackgroundFPS()
        {
            Application.targetFrameRate = _backgroundFPS;
            QualitySettings.vSyncCount = 0;
            Debug.Log($"[FrameRateLimiter] Background mode: {_backgroundFPS} FPS");
        }

        /// <summary>
        /// 最前面FPSを設定
        /// </summary>
        public void SetForegroundTargetFPS(int fps)
        {
            _foregroundFPS = fps;
            if (_isForeground)
            {
                SetForegroundFPS();
            }
        }

        /// <summary>
        /// 背面FPSを設定
        /// </summary>
        public void SetBackgroundTargetFPS(int fps)
        {
            _backgroundFPS = fps;
            if (!_isForeground)
            {
                SetBackgroundFPS();
            }
        }

        /// <summary>
        /// 現在のフォーカス状態を取得
        /// </summary>
        public bool IsForeground => _isForeground;

        /// <summary>
        /// 現在の目標FPSを取得
        /// </summary>
        public int CurrentTargetFPS => _isForeground ? _foregroundFPS : _backgroundFPS;
    }
}
