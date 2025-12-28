using UnityEngine;

namespace DesktopOjisan.Core
{
    /// <summary>
    /// キャラクターを画面内で直線移動させ、端に達したら反射するコンポーネント
    /// NOTE: このスクリプトはスフィア等のキャラクターオブジェクトにアタッチする
    /// </summary>
    public class CharacterMover : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _speed = 100f;
        [SerializeField] private Vector2 _initialDirection = new Vector2(1f, 1f);

        [Header("Bounds")]
        [SerializeField] private float _padding = 50f;

        private Vector2 _direction;
        private Vector2 _screenSize;
        private RectTransform _rectTransform;
        private Camera _mainCamera;

        private void Awake()
        {
            _direction = _initialDirection.normalized;
            _rectTransform = GetComponent<RectTransform>();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            UpdateScreenSize();
        }

        private void Update()
        {
            Move();
            CheckBoundsAndReflect();
        }

        /// <summary>
        /// 画面サイズを更新する
        /// </summary>
        public void UpdateScreenSize()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            var size = WindowController.GetScreenDimensions();
            _screenSize = new Vector2(size.x, size.y);
#else
            _screenSize = new Vector2(Screen.width, Screen.height);
#endif
        }

        private void Move()
        {
            if (_rectTransform != null)
            {
                // UI要素の場合
                _rectTransform.anchoredPosition += _direction * _speed * Time.deltaTime;
            }
            else
            {
                // 3Dオブジェクトの場合（ワールド座標で移動）
                Vector3 movement = new Vector3(_direction.x, _direction.y, 0f) * _speed * Time.deltaTime;
                transform.position += movement;
            }
        }

        private void CheckBoundsAndReflect()
        {
            Vector2 position;
            Vector2 size;

            if (_rectTransform != null)
            {
                position = _rectTransform.anchoredPosition;
                size = _rectTransform.sizeDelta / 2f;
            }
            else
            {
                // 3Dオブジェクトの場合はビューポート座標を使用
                Vector3 viewportPos = _mainCamera.WorldToScreenPoint(transform.position);
                position = new Vector2(viewportPos.x, viewportPos.y);
                size = Vector2.one * 50f; // 仮のサイズ
            }

            bool reflected = false;

            // 左右の境界チェック
            if (position.x - size.x - _padding < 0)
            {
                _direction.x = Mathf.Abs(_direction.x);
                reflected = true;
            }
            else if (position.x + size.x + _padding > _screenSize.x)
            {
                _direction.x = -Mathf.Abs(_direction.x);
                reflected = true;
            }

            // 上下の境界チェック
            if (position.y - size.y - _padding < 0)
            {
                _direction.y = Mathf.Abs(_direction.y);
                reflected = true;
            }
            else if (position.y + size.y + _padding > _screenSize.y)
            {
                _direction.y = -Mathf.Abs(_direction.y);
                reflected = true;
            }

            if (reflected)
            {
                _direction = _direction.normalized;
            }
        }

        /// <summary>
        /// 移動速度を設定
        /// </summary>
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        /// <summary>
        /// 移動方向を設定
        /// </summary>
        public void SetDirection(Vector2 direction)
        {
            _direction = direction.normalized;
        }
    }
}
