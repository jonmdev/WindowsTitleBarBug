
using System.Diagnostics;

namespace WindowsTitleBarBug {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            //BASIC LAYOUT
            ContentPage contentPage = new ContentPage();
            MainPage = contentPage;

            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            absoluteLayout.BackgroundColor = Colors.Red;
            contentPage.Content = absoluteLayout;

            Image image = new Image();
            Size imgSize = new Size(800, 526);
            image.Source = "beach.jpg";
            absoluteLayout.Add(image);

            AbsoluteLayout topBar = new AbsoluteLayout();
            topBar.BackgroundColor = Colors.White;
            topBar.HeightRequest = 40;
            absoluteLayout.Add(topBar);

            //RESIZER
            contentPage.SizeChanged += delegate {
                if (contentPage.Height > 0) {
                    absoluteLayout.HeightRequest = contentPage.Height;
                    absoluteLayout.WidthRequest = contentPage.Width;

                    image.WidthRequest = contentPage.Width;
                    image.HeightRequest = (imgSize.Height / imgSize.Width) * image.WidthRequest;

                    topBar.WidthRequest = contentPage.Width;
                }
            };

            //OSCILLATOR
            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1 / 30.0);
            DateTime lastDateTime = DateTime.Now;
            double timerTime = 0;
            timer.Tick += delegate {
                image.TranslationY = Math.Sin(Math.Tau * 0.3 * timerTime) * 300;
                timerTime += (DateTime.Now - lastDateTime).TotalSeconds;
                lastDateTime = DateTime.Now;
            };
            timer.Start();


        }
        protected override Window CreateWindow(IActivationState activationState) {
            
            Window window = base.CreateWindow(activationState);
            window.Title = "HELLO APPLICATION";
            return window;

        }

    }
}
