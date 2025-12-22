@echo off
chcp 65001 >nul
echo デスクトップおじさん API テスト
echo ================================

echo.
echo [Ping テスト]
curl -s http://localhost:8080/api/ping

echo.
echo.
echo [Status テスト]
curl -s http://localhost:8080/api/status

echo.
echo.
echo [Message テスト]
curl -s -X POST -H "Content-Type: application/json" -d "{\"message\":\"こんにちは、おじさん！\"}" http://localhost:8080/api/message

echo.
echo.
echo [Command テスト - greet]
curl -s -X POST -H "Content-Type: application/json" -d "{\"type\":\"greet\",\"data\":\"\"}" http://localhost:8080/api/command

echo.
echo.
echo ================================
echo テスト完了!
pause
