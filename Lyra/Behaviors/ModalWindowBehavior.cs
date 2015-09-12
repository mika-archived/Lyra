using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;

using MetroRadiance.Core.Win32;

namespace Lyra.Behaviors
{
    /// <summary>
    /// Window Style を適用します。
    /// </summary>
    public class ModalWindowBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SourceInitialized += AssociatedObjectOnSourceInitialized;
        }

        private void AssociatedObjectOnSourceInitialized(object sender, EventArgs eventArgs)
        {
            var hwnd = new WindowInteropHelper(this.AssociatedObject).Handle;
            var windowStyle = hwnd.GetWindowLong();
            windowStyle &= ~(WS.MAXIMIZEBOX | WS.MINIMIZEBOX);
            hwnd.SetWindowLong(windowStyle);

            // http://stackoverflow.com/questions/18580430/hide-the-icon-from-a-wpf-window
            var extendedStyle = hwnd.GetWindowLongEx();
            hwnd.SetWindowLongEx(extendedStyle | WSEX.DLGMODALFRAME);
            NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.NOMOVE | SWP.NOSIZE | SWP.NOZORDER | SWP.FRAMECHANGED);
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.SourceInitialized -= AssociatedObjectOnSourceInitialized;
            base.OnDetaching();
        }
    }
}