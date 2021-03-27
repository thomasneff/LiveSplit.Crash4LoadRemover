using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CrashNSaneLoadDetector
{
  class DLLImportStuff
  {


    public enum TernaryRasterOperations : uint
    {
      SRCCOPY = 0x00CC0020,
      SRCPAINT = 0x00EE0086,
      SRCAND = 0x008800C6,
      SRCINVERT = 0x00660046,
      SRCERASE = 0x00440328,
      NOTSRCCOPY = 0x00330008,
      NOTSRCERASE = 0x001100A6,
      MERGECOPY = 0x00C000CA,
      MERGEPAINT = 0x00BB0226,
      PATCOPY = 0x00F00021,
      PATPAINT = 0x00FB0A09,
      PATINVERT = 0x005A0049,
      DSTINVERT = 0x00550009,
      BLACKNESS = 0x00000042,
      WHITENESS = 0x00FF0062,
      CAPTUREBLT = 0x40000000
    }

    private struct WINDOWPLACEMENT
    {
      public int length;
      public int flags;
      public int showCmd;
      public System.Drawing.Point ptMinPosition;
      public System.Drawing.Point ptMaxPosition;
      public System.Drawing.Rectangle rcNormalPosition;
    }

    enum GetAncestorFlags
    {
      // Retrieves the parent window. This does not include the owner, as it does with the GetParent function.
      GetParent = 1,
      // Retrieves the root window by walking the chain of parent windows.
      GetRoot = 2,
      // Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
      GetRootOwner = 3
    }

    public enum GWL
    {
      GWL_WNDPROC = (-4),
      GWL_HINSTANCE = (-6),
      GWL_HWNDPARENT = (-8),
      GWL_STYLE = (-16),
      GWL_EXSTYLE = (-20),
      GWL_USERDATA = (-21),
      GWL_ID = (-12)
    }

    [Flags]
    private enum WindowStyles : uint
    {
      WS_BORDER = 0x800000,
      WS_CAPTION = 0xc00000,
      WS_CHILD = 0x40000000,
      WS_CLIPCHILDREN = 0x2000000,
      WS_CLIPSIBLINGS = 0x4000000,
      WS_DISABLED = 0x8000000,
      WS_DLGFRAME = 0x400000,
      WS_GROUP = 0x20000,
      WS_HSCROLL = 0x100000,
      WS_MAXIMIZE = 0x1000000,
      WS_MAXIMIZEBOX = 0x10000,
      WS_MINIMIZE = 0x20000000,
      WS_MINIMIZEBOX = 0x20000,
      WS_OVERLAPPED = 0x0,
      WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
      WS_POPUP = 0x80000000u,
      WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
      WS_SIZEFRAME = 0x40000,
      WS_SYSMENU = 0x80000,
      WS_TABSTOP = 0x10000,
      WS_VISIBLE = 0x10000000,
      WS_VSCROLL = 0x200000
    }

    enum DWMWINDOWATTRIBUTE : uint
    {
      NCRenderingEnabled = 1,
      NCRenderingPolicy,
      TransitionsForceDisabled,
      AllowNCPaint,
      CaptionButtonBounds,
      NonClientRtlLayout,
      ForceIconicRepresentation,
      Flip3DPolicy,
      ExtendedFrameBounds,
      HasIconicBitmap,
      DisallowPeek,
      ExcludedFromPeek,
      Cloak,
      Cloaked,
      FreezeRepresentation
    }

    [DllImport("gdi32.dll")]
    public static extern bool BitBlt(IntPtr DC, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr SrcDC, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hDC);

    [DllImport("user32.dll")]
    public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

    [DllImport("gdi32.dll", SetLastError = true)]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll")]
    static extern IntPtr GetShellWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", ExactSpelling = true)]
    static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

    [DllImport("dwmapi.dll")]
    static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);

    // This static method is required because Win32 does not support
    // GetWindowLongPtr directly.
    // http://pinvoke.net/default.aspx/user32/GetWindowLong.html
    static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
    {
      if (IntPtr.Size == 8)
        return GetWindowLongPtr64(hWnd, nIndex);
      else
        return GetWindowLongPtr32(hWnd, nIndex);
    }

    public static bool IsWindowValidForCapture(IntPtr hwnd)
    {
      if (hwnd.ToInt32() == 0)
      {
        return false;
      }

      if (hwnd == GetShellWindow())
      {
        return false;
      }

      if (!IsWindowVisible(hwnd))
      {
        return false;
      }

      if (GetAncestor(hwnd, GetAncestorFlags.GetRoot) != hwnd)
      {
        return false;
      }
      /*
      var style = (WindowStyles)GetWindowLongPtr(hwnd, (int)GWL.GWL_STYLE).ToInt32();
      if (style.HasFlag(WindowStyles.WS_DISABLED))
      {
        return false;
      }*/

      var cloaked = false;
      bool test = false;
      var hrTemp = DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.Cloaked, out cloaked, Marshal.SizeOf(test));
      if (hrTemp == 0 && cloaked)
      {
        return false;
      }

      WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
      GetWindowPlacement(hwnd, ref placement);

      // minimized
      if (placement.showCmd == 2)
        return false;

      return true;
    }
  }
}

