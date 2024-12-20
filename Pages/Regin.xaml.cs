﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Imaging = Aspose.Imaging;
using System.Windows.Media.Imaging;
using RegIN_Fadeev.Classes;

namespace RegIN_Fadeev.Pages
{
    /// <summary>
    /// Логика взаимодействия для Regin.xaml
    /// </summary>
    public partial class Regin : Page
    {
        OpenFileDialog FileDialogImage = new OpenFileDialog();
        bool BCorrectLogin = false;
        bool BCorrectPassword = false;
        bool BCorrectConfirmPassword = false;
        bool BSetImages = false;
        public Regin()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogin.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogin.HandlerInCorrectLogin += InCorrectLogin;
            FileDialogImage.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg";
            FileDialogImage.RestoreDirectory = true;
            FileDialogImage.Title = "Choose a photo for your avatar";
        }
        
        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetLogin();
            }
        } 

        private void SetLogin(object sender, RoutedEventArgs e)
        {
            SetLogin();
        }

        public void SetLogin()
        {
            Regex regex = new Regex(@"([a-zA-Z0-9._-]{4,}@[a-zA-Z0-9._-]{2,}\.[a-zA-Z0-9]{2,})");
            BCorrectLogin = regex.IsMatch(TbLogin.Text);
            if (regex.IsMatch(TbLogin.Text))
            {
                SetNotification("", Brushes.Black);
                MainWindow.mainWindow.UserLogin.GetUserLogin(TbLogin.Text);
            }
            else
            {
                SetNotification("Invalid login \nexample: user@example.com", Brushes.Red);
            }
            OnRegin();
        }

        #region SetPassword
        private void SetPassword(object sender, RoutedEventArgs e)
        {
            SetPassword();
        }

        private void SetPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetPassword();
            }
        }

        public void SetPassword()
        {
            Regex regex = new Regex(@"(?=.*[0-9])(?=.*[!@#$%^&*\-_=])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*\-_=]{10,}");
            BCorrectPassword = regex.IsMatch(TbPassword.Password);
            if (regex.IsMatch(TbPassword.Password))
            {
                SetNotification("", Brushes.Black);
                if (TbConfirmPassword.Password.Length > 0)
                {
                    ConfirmPassword(true);
                }
                OnRegin();
            }
            else
            {
                SetNotification("Inavlid password \nexample: Asdfg1234*", Brushes.Red);
            }
        }
        #endregion

        #region SetConfirmPassword
        private void ConfirmPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfirmPassword();
            }
        }
        
        private void ConfirmPassword(object sender, RoutedEventArgs e)
        {
            ConfirmPassword();
        }

        public void ConfirmPassword(bool Pass = false)
        {
            BCorrectConfirmPassword = TbConfirmPassword.Password == TbPassword.Password;
            if (TbPassword.Password != TbConfirmPassword.Password)
            {
                SetNotification("Passwords do not match.", Brushes.Red);
            }
            else
            {
                SetNotification("", Brushes.Black);
                if (!Pass)
                {
                    SetPassword();
                }
            }
        }

        #endregion

        void OnRegin()
        {
            if (!BCorrectLogin)
                return;
            if (TbName.Text.Length == 0)
                return;
            if (!BCorrectPassword)
                return;
            if (!BCorrectConfirmPassword)
                return;
            MainWindow.mainWindow.UserLogin.Login = TbLogin.Text;
            MainWindow.mainWindow.UserLogin.Password = TbPassword.Password;
            MainWindow.mainWindow.UserLogin.Name = TbName.Text;
            if (BSetImages)
            {
                MainWindow.mainWindow.UserLogin.Image = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\IUser.jpg");
            }
            MainWindow.mainWindow.UserLogin.DateUpdate = DateTime.Now;
            MainWindow.mainWindow.UserLogin.DateCreate = DateTime.Now;
            MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Regin));
        }

        private void SetName(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsLetter(e.Text, 0));
        }

        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            try
            {
                File.Delete(Directory.GetCurrentDirectory() + @"\IUser.jpg");
                if (FileDialogImage.ShowDialog() == true)
                {
                    using (Imaging.Image image = Imaging.Image.Load(FileDialogImage.FileName))
                    {
                        int NewWidth = 0;
                        int NewHeight = 0;
                        if (image.Width > image.Height)
                        {
                            NewWidth = (int)(image.Width * (256f / image.Height));
                            NewHeight = 256;
                        }
                        else
                        {
                            NewWidth = 256;
                            NewHeight = (int)(image.Height * (256f / image.Width));
                        }
                        image.Resize(NewWidth, NewHeight);
                        image.Save("IUser.jpg");
                    }
                    using (Imaging.RasterImage rasterImage = (Imaging.RasterImage)Imaging.Image.Load("IUser.jpg"))
                    {
                        if (!rasterImage.IsCached)
                        {
                            rasterImage.CacheData();
                        }
                        int X = 0;
                        int Width = 256;
                        int Y = 0;
                        int Height = 256;
                        if (rasterImage.Width > rasterImage.Height)
                        {
                            X = (int)((rasterImage.Width - 256f) / 2);
                        }
                        else
                        {
                            Y = (int)((rasterImage.Height - 256f) / 2);
                        }
                        Imaging.Rectangle rectangle = new Imaging.Rectangle(X, Y, Width, Height);
                        rasterImage.Crop(rectangle);
                        rasterImage.Save("IUser.jpg");
                    }
                    Animation.ImageDoubleAnimation(IUser, new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\IUser.jpg")));
                    BSetImages = true;
                }
                else
                {
                    BSetImages = false;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }

        private void CorrectLogin()
        {
            SetNotification("Login already in use", Brushes.Red);
            BCorrectLogin = false;
        }

        private void InCorrectLogin()
        {
            SetNotification("", Brushes.Black);
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}
