using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace DesktopOjisan.Server
{
    /// <summary>
    /// 外部からのコマンドを受け付けるHTTPサーバ
    /// </summary>
    public class ApiServer : MonoBehaviour
    {
        [SerializeField] private int port = 8080;
        [SerializeField] private bool autoStart = true;

        private HttpListener _listener;
        private Thread _listenerThread;
        private bool _isRunning;

        // メインスレッドで処理するためのキュー
        private readonly ConcurrentQueue<ApiCommand> _commandQueue = new();

        public event Action<ApiCommand> OnCommandReceived;

        public int Port => port;
        public bool IsRunning => _isRunning;

        private void Start()
        {
            if (autoStart)
            {
                StartServer();
            }
        }

        private void OnDestroy()
        {
            StopServer();
        }

        private void Update()
        {
            // メインスレッドでコマンドを処理
            while (_commandQueue.TryDequeue(out var command))
            {
                OnCommandReceived?.Invoke(command);
            }
        }

        public void StartServer()
        {
            if (_isRunning) return;

            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add($"http://localhost:{port}/");
                _listener.Start();
                _isRunning = true;

                _listenerThread = new Thread(ListenLoop)
                {
                    IsBackground = true
                };
                _listenerThread.Start();

                Debug.Log($"[ApiServer] サーバ起動: http://localhost:{port}/");
            }
            catch (Exception e)
            {
                Debug.LogError($"[ApiServer] サーバ起動失敗: {e.Message}");
            }
        }

        public void StopServer()
        {
            _isRunning = false;

            if (_listener != null)
            {
                _listener.Stop();
                _listener.Close();
                _listener = null;
            }

            if (_listenerThread != null)
            {
                _listenerThread.Join(1000);
                _listenerThread = null;
            }

            Debug.Log("[ApiServer] サーバ停止");
        }

        private void ListenLoop()
        {
            while (_isRunning)
            {
                try
                {
                    var context = _listener.GetContext();
                    ProcessRequest(context);
                }
                catch (HttpListenerException)
                {
                    // サーバ停止時の例外は無視
                }
                catch (Exception e)
                {
                    Debug.LogError($"[ApiServer] リクエスト処理エラー: {e.Message}");
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string responseString;
            int statusCode = 200;

            try
            {
                var path = request.Url.AbsolutePath.ToLower();

                switch (path)
                {
                    case "/api/command":
                        responseString = HandleCommand(request);
                        break;
                    case "/api/message":
                        responseString = HandleMessage(request);
                        break;
                    case "/api/status":
                        responseString = HandleStatus();
                        break;
                    case "/api/ping":
                        responseString = "{\"status\":\"ok\",\"message\":\"pong\"}";
                        break;
                    default:
                        responseString = "{\"error\":\"Not found\"}";
                        statusCode = 404;
                        break;
                }
            }
            catch (Exception e)
            {
                responseString = $"{{\"error\":\"{e.Message}\"}}";
                statusCode = 500;
            }

            // レスポンス送信
            response.StatusCode = statusCode;
            response.ContentType = "application/json; charset=utf-8";
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        private string HandleCommand(HttpListenerRequest request)
        {
            if (request.HttpMethod != "POST")
            {
                return "{\"error\":\"POST method required\"}";
            }

            using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
            var body = reader.ReadToEnd();

            var command = JsonUtility.FromJson<ApiCommand>(body);
            if (command == null || string.IsNullOrEmpty(command.type))
            {
                return "{\"error\":\"Invalid command format\"}";
            }

            _commandQueue.Enqueue(command);
            Debug.Log($"[ApiServer] コマンド受信: {command.type}");

            return $"{{\"status\":\"ok\",\"command\":\"{command.type}\"}}";
        }

        private string HandleMessage(HttpListenerRequest request)
        {
            if (request.HttpMethod != "POST")
            {
                return "{\"error\":\"POST method required\"}";
            }

            using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
            var body = reader.ReadToEnd();

            var messageData = JsonUtility.FromJson<MessageCommand>(body);
            if (messageData == null || string.IsNullOrEmpty(messageData.message))
            {
                return "{\"error\":\"Invalid message format\"}";
            }

            var command = new ApiCommand
            {
                type = "message",
                data = messageData.message
            };
            _commandQueue.Enqueue(command);

            Debug.Log($"[ApiServer] メッセージ受信: {messageData.message}");
            return "{\"status\":\"ok\"}";
        }

        private string HandleStatus()
        {
            return $"{{\"status\":\"running\",\"port\":{port}}}";
        }
    }

    [Serializable]
    public class ApiCommand
    {
        public string type;
        public string data;
    }

    [Serializable]
    public class MessageCommand
    {
        public string message;
    }
}
