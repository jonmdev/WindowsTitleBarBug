using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace WindowsTitleBarBug {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


#if WINDOWS
            //https://stackoverflow.com/questions/71806578/maui-how-to-remove-the-title-bar-and-fix-the-window-size //temporary solution, if make full screen no mismatch pixels
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(lifeCycleBuilder =>
                {
                    lifeCycleBuilder.OnWindowCreated(w =>
                    {
                        w.ExtendsContentIntoTitleBar = false;
                        IntPtr wHandle = WinRT.Interop.WindowNative.GetWindowHandle(w);
                        WindowId windowId = Win32Interop.GetWindowIdFromWindow(wHandle);
                        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
                        var titleBar = appWindow.TitleBar;         
                        titleBar.BackgroundColor = Microsoft.Maui.Graphics.Colors.GreenYellow.ToWindowsColor();
                        titleBar.ButtonBackgroundColor = Microsoft.Maui.Graphics.Colors.GreenYellow.ToWindowsColor();                                             
                        titleBar.InactiveBackgroundColor = Microsoft.Maui.Graphics.Colors.LightGrey.ToWindowsColor();    
                        titleBar.ButtonInactiveBackgroundColor = Microsoft.Maui.Graphics.Colors.DarkGrey.ToWindowsColor();
                        //appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);  // TO SET THE APP INTO FULL SCREEN
                        appWindow.SetPresenter(AppWindowPresenterKind.Default);  // TO NOT SET THE APP INTO FULL SCREEN
                        appWindow.Resize (new SizeInt32(1500, 700));
                        appWindow.Move(new PointInt32(0, 0));
                    });
                    
                });
            });

#endif
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
