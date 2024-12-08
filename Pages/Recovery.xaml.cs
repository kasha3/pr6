using RegIN_Fadeev.Classes;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RegIN_Fadeev.Pages
{
    /// <summary>
    /// Логика взаимодействия для Recovery.xaml
    /// </summary>
    public partial class Recovery : Page
    {
        string OldLogin;
        bool IsCapture = false;
        public Recovery()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogin.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogin.HandlerInCorrectLogin += InCorrectLogin;
            Capture.HandlerCorrectCapture += CorrectCapture;
        }

        private void CorrectLogin()
        {
            if (OldLogin != TbLogin.Text)
            {
                SetNotification("Hi, " + MainWindow.mainWindow.UserLogin.Name, Brushes.Black);
                try
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(MainWindow.mainWindow.UserLogin.Image);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    ImageSource imgSrc = biImg;
                    //Рефакторинг
                    Animation.ImageDoubleAnimation(IUser, imgSrc);
                }
                catch (Exception exp)
                {
                    Debug.WriteLine(exp.Message);
                };
                OldLogin = TbLogin.Text;
                SendNewPassword();
            }
        }

        private void InCorrectLogin()
        {
            if (LNameUser.Content.ToString() != "")
            {
                LNameUser.Content = "";
                //Рефакторинг
                Animation.ImageDoubleAnimation(IUser, new BitmapImage(new Uri("pack://application:,,,/Images/empty_logo.png")));
            }
            if (TbLogin.Text.Length > 0)
            {
                SetNotification("Login is incorrect", Brushes.Red);
            }
        }
        private void CorrectCapture()
        {
            Capture.IsEnabled = false;
            IsCapture = true;
            SendNewPassword();
        }

        public void SendNewPassword()
        {
            if (IsCapture)
            {
                if (MainWindow.mainWindow.UserLogin.Password != String.Empty)
                {
                    //Рефакторинг
                    Animation.ImageDoubleAnimation(IUser, new BitmapImage(new Uri("pack://application:,,,/Images/empty_logo.png")));
                    SetNotification("An email has been sent to your email.", Brushes.Black);
                    MainWindow.mainWindow.UserLogin.CreateNewPassword();
                }
            }
        }

        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);
            }
        }

        private void SetLogin(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);
        }

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}
