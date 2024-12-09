using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegIN_Fadeev.Pages
{
    /// <summary>
    /// Логика взаимодействия для PinCode.xaml
    /// </summary>
    public partial class PinCode : Page
    {
        private bool isNotificationSet = false;
        private Confirmation.TypeConfirmation typeConfirmation;

        public PinCode(Confirmation.TypeConfirmation _typeConfirmation)
        {
            InitializeComponent();
            typeConfirmation = _typeConfirmation;
            ChangeLabel();
        }

        void ChangeLabel()
        {
            if (typeConfirmation == Confirmation.TypeConfirmation.Login)
            {
                if (MainWindow.mainWindow.UserLogin.PinCode != String.Empty) { TextPinCode.Content = "Enter your pin code."; BtnCreatePinCode.Visibility = Visibility.Hidden; LbSkip.Visibility = Visibility.Hidden; }
                else TextPinCode.Content = "Create a pin code for faster authorization.";
            }
            else if (typeConfirmation == Confirmation.TypeConfirmation.Regin)
            {
                TextPinCode.Content = "Create a pin code for faster authorization.";
            }
        }

        private void SetText(object sender, RoutedEventArgs e)
        {
            if (isNotificationSet)
            {
                SetNotification("", Brushes.Black);
                isNotificationSet = false;
            }
        }

        private void SetPinCode(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SetPinCode();
            else if (isNotificationSet)
            {
                SetNotification("", Brushes.Black);
                isNotificationSet = false;
            }
        }

        void SetPinCode()
        {
            Regex regex = new Regex(@"\d{4}");
            if (regex.IsMatch(TbPinCode.Text))
            {
                if (typeConfirmation == Confirmation.TypeConfirmation.Login)
                {
                    if (MainWindow.mainWindow.UserLogin.PinCode != String.Empty && MainWindow.mainWindow.UserLogin.PinCode == TbPinCode.Text) MessageBox.Show("Successful pin code entry, welcome!");
                    else if (MainWindow.mainWindow.UserLogin.PinCode != String.Empty && MainWindow.mainWindow.UserLogin.PinCode != TbPinCode.Text)
                    {
                        SetNotification("Incorrect pin-code", Brushes.Red);
                        isNotificationSet = true;
                    }
                    else if (MainWindow.mainWindow.UserLogin.PinCode == String.Empty)
                    {
                        MainWindow.mainWindow.UserLogin.AddPinCode(TbPinCode.Text);
                        MessageBox.Show("Successful pin code entry, welcome!");
                    }
                }
                else if (typeConfirmation == Confirmation.TypeConfirmation.Regin)
                {
                    MainWindow.mainWindow.UserLogin.AddPinCode(TbPinCode.Text);
                    MessageBox.Show("Successful pin code entry, welcome!");
                }
            }
            else
            {
                SetNotification("Invalid pin-code", Brushes.Red);
                isNotificationSet = true;
            }
        }

        private void AddPincode(object sender, RoutedEventArgs e) => SetPinCode();

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LbPinCode.Content = Message;
            LbPinCode.Foreground = _Color;
        }

        private void OpenMain(object sender, MouseButtonEventArgs e) => MessageBox.Show("Welcome!");

        private void BackPage(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Login());
    }
}
