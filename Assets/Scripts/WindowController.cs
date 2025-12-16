using System;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Windows API wrapper for controlling window appearance.
/// Used to create borderless/transparent desktop mascot window.
/// </summary>
public class WindowController : MonoBehaviour
{
    #region Native Plugin Imports

    private const string DLL_NAME = "WindowController";

    [DllImport(DLL_NAME)]
    private static extern IntPtr GetUnityWindowHandle();

    [DllImport(DLL_NAME)]
    private static extern void RemoveWindowFrame(IntPtr hwnd);

    [DllImport(DLL_NAME)]
    private static extern void RestoreWindowFrame(IntPtr hwnd);

    [DllImport(DLL_NAME)]
    private static extern void SetWindowTopMost(IntPtr hwnd, bool topMost);

    [DllImport(DLL_NAME)]
    private static extern void SetWindowPosition(IntPtr hwnd, int x, int y);

    [DllImport(DLL_NAME)]
    private static extern void SetWindowSize(IntPtr hwnd, int width, int height);

    [DllImport(DLL_NAME)]
    private static extern void SetClickThrough(IntPtr hwnd, bool enable);

    [DllImport(DLL_NAME)]
    private static extern void SetWindowTransparency(IntPtr hwnd, byte alpha);

    #endregion

    #region Fields

    private IntPtr _windowHandle = IntPtr.Zero;

    [Header("Window Settings")]
    [SerializeField] private bool _removeBorderOnStart = true;
    [SerializeField] private bool _alwaysOnTop = true;
    [SerializeField] private bool _clickThrough = false;
    [SerializeField, Range(0, 255)] private byte _transparency = 255;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the window handle.
    /// </summary>
    public IntPtr WindowHandle => _windowHandle;

    /// <summary>
    /// Gets whether the window is borderless.
    /// </summary>
    public bool IsBorderless { get; private set; }

    #endregion

    #region Unity Lifecycle

    private void Start()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        InitializeWindow();
#else
        Debug.Log("WindowController: Native plugin only works in Windows standalone build.");
#endif
    }

    private void OnDestroy()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        // Restore window frame when destroyed
        if (_windowHandle != IntPtr.Zero && IsBorderless)
        {
            RestoreWindowFrame(_windowHandle);
        }
#endif
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Removes the window frame (borderless mode).
    /// </summary>
    public void RemoveBorder()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (_windowHandle == IntPtr.Zero) return;

        RemoveWindowFrame(_windowHandle);
        IsBorderless = true;
        Debug.Log("WindowController: Border removed.");
#endif
    }

    /// <summary>
    /// Restores the window frame.
    /// </summary>
    public void RestoreBorder()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (_windowHandle == IntPtr.Zero) return;

        RestoreWindowFrame(_windowHandle);
        IsBorderless = false;
        Debug.Log("WindowController: Border restored.");
#endif
    }

    /// <summary>
    /// Sets whether the window should always be on top.
    /// </summary>
    public void SetAlwaysOnTop(bool enabled)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (_windowHandle == IntPtr.Zero) return;

        SetWindowTopMost(_windowHandle, enabled);
        Debug.Log($"WindowController: Always on top = {enabled}");
#endif
    }

    /// <summary>
    /// Moves the window to the specified position.
    /// </summary>
    public void MoveWindow(int x, int y)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (_windowHandle == IntPtr.Zero) return;

        SetWindowPosition(_windowHandle, x, y);
#endif
    }

    /// <summary>
    /// Resizes the window.
    /// </summary>
    public void ResizeWindow(int width, int height)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (_windowHandle == IntPtr.Zero) return;

        SetWindowSize(_windowHandle, width, height);
#endif
    }

    /// <summary>
    /// Sets click-through mode (mouse events pass through).
    /// </summary>
    public void SetClickThroughMode(bool enabled)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (_windowHandle == IntPtr.Zero) return;

        SetClickThrough(_windowHandle, enabled);
        Debug.Log($"WindowController: Click-through = {enabled}");
#endif
    }

    /// <summary>
    /// Sets window transparency (0 = fully transparent, 255 = fully opaque).
    /// </summary>
    public void SetTransparency(byte alpha)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (_windowHandle == IntPtr.Zero) return;

        SetWindowTransparency(_windowHandle, alpha);
#endif
    }

    #endregion

    #region Private Methods

    private void InitializeWindow()
    {
        _windowHandle = GetUnityWindowHandle();

        if (_windowHandle == IntPtr.Zero)
        {
            Debug.LogError("WindowController: Failed to get window handle.");
            return;
        }

        Debug.Log($"WindowController: Window handle acquired: {_windowHandle}");

        if (_removeBorderOnStart)
        {
            RemoveBorder();
        }

        if (_alwaysOnTop)
        {
            SetAlwaysOnTop(true);
        }

        if (_clickThrough)
        {
            SetClickThroughMode(true);
        }

        if (_transparency < 255)
        {
            SetTransparency(_transparency);
        }
    }

    #endregion
}
