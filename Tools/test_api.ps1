# デスクトップおじさん API テストスクリプト
# PowerShell で実行

$baseUrl = "http://localhost:8080"

function Test-Ping {
    Write-Host "=== Ping テスト ===" -ForegroundColor Cyan
    try {
        $response = Invoke-RestMethod -Uri "$baseUrl/api/ping" -Method Get
        Write-Host "結果: $($response | ConvertTo-Json)" -ForegroundColor Green
    }
    catch {
        Write-Host "エラー: $_" -ForegroundColor Red
    }
}

function Test-Status {
    Write-Host "`n=== Status テスト ===" -ForegroundColor Cyan
    try {
        $response = Invoke-RestMethod -Uri "$baseUrl/api/status" -Method Get
        Write-Host "結果: $($response | ConvertTo-Json)" -ForegroundColor Green
    }
    catch {
        Write-Host "エラー: $_" -ForegroundColor Red
    }
}

function Test-Message {
    param([string]$message = "テストメッセージだよ！")

    Write-Host "`n=== Message テスト ===" -ForegroundColor Cyan
    try {
        $body = @{ message = $message } | ConvertTo-Json
        $response = Invoke-RestMethod -Uri "$baseUrl/api/message" -Method Post -Body $body -ContentType "application/json"
        Write-Host "結果: $($response | ConvertTo-Json)" -ForegroundColor Green
    }
    catch {
        Write-Host "エラー: $_" -ForegroundColor Red
    }
}

function Test-Command {
    param([string]$type = "greet", [string]$data = "")

    Write-Host "`n=== Command テスト ($type) ===" -ForegroundColor Cyan
    try {
        $body = @{ type = $type; data = $data } | ConvertTo-Json
        $response = Invoke-RestMethod -Uri "$baseUrl/api/command" -Method Post -Body $body -ContentType "application/json"
        Write-Host "結果: $($response | ConvertTo-Json)" -ForegroundColor Green
    }
    catch {
        Write-Host "エラー: $_" -ForegroundColor Red
    }
}

# 全テスト実行
Write-Host "デスクトップおじさん API テスト開始" -ForegroundColor Yellow
Write-Host "================================`n"

Test-Ping
Test-Status
Test-Message -message "こんにちは、おじさん！"
Test-Command -type "greet"
Test-Command -type "talk"
Test-Command -type "status"

Write-Host "`n================================"
Write-Host "テスト完了!" -ForegroundColor Yellow
