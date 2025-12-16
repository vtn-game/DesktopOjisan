#include <windows.h>

extern "C" {
    // Get the Unity window handle
    __declspec(dllexport) HWND GetUnityWindowHandle() {
        return GetActiveWindow();
    }

    // Remove window frame (borderless window)
    __declspec(dllexport) void RemoveWindowFrame(HWND hwnd) {
        if (hwnd == NULL) {
            hwnd = GetActiveWindow();
        }

        // Get current window style
        LONG style = GetWindowLong(hwnd, GWL_STYLE);

        // Remove title bar and border
        style &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);

        SetWindowLong(hwnd, GWL_STYLE, style);

        // Apply changes
        SetWindowPos(hwnd, NULL, 0, 0, 0, 0,
            SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER);
    }

    // Restore window frame
    __declspec(dllexport) void RestoreWindowFrame(HWND hwnd) {
        if (hwnd == NULL) {
            hwnd = GetActiveWindow();
        }

        // Restore standard window style
        LONG style = GetWindowLong(hwnd, GWL_STYLE);
        style |= (WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);

        SetWindowLong(hwnd, GWL_STYLE, style);

        // Apply changes
        SetWindowPos(hwnd, NULL, 0, 0, 0, 0,
            SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER);
    }

    // Set window to be always on top
    __declspec(dllexport) void SetWindowTopMost(HWND hwnd, bool topMost) {
        if (hwnd == NULL) {
            hwnd = GetActiveWindow();
        }

        SetWindowPos(hwnd, topMost ? HWND_TOPMOST : HWND_NOTOPMOST,
            0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
    }

    // Set window position
    __declspec(dllexport) void SetWindowPosition(HWND hwnd, int x, int y) {
        if (hwnd == NULL) {
            hwnd = GetActiveWindow();
        }

        SetWindowPos(hwnd, NULL, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
    }

    // Set window size
    __declspec(dllexport) void SetWindowSize(HWND hwnd, int width, int height) {
        if (hwnd == NULL) {
            hwnd = GetActiveWindow();
        }

        SetWindowPos(hwnd, NULL, 0, 0, width, height, SWP_NOMOVE | SWP_NOZORDER);
    }

    // Enable click-through (transparent to mouse clicks)
    __declspec(dllexport) void SetClickThrough(HWND hwnd, bool enable) {
        if (hwnd == NULL) {
            hwnd = GetActiveWindow();
        }

        LONG exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

        if (enable) {
            exStyle |= (WS_EX_LAYERED | WS_EX_TRANSPARENT);
        } else {
            exStyle &= ~WS_EX_TRANSPARENT;
        }

        SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
    }

    // Set window transparency (0-255, 255 = opaque)
    __declspec(dllexport) void SetWindowTransparency(HWND hwnd, BYTE alpha) {
        if (hwnd == NULL) {
            hwnd = GetActiveWindow();
        }

        LONG exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        exStyle |= WS_EX_LAYERED;
        SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);

        SetLayeredWindowAttributes(hwnd, 0, alpha, LWA_ALPHA);
    }
}
