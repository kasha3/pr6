using RegIN_Fadeev.Classes;
using System.Windows;
using System.Windows.Controls;

namespace RegIN_Fadeev
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow;
        public User UserLogin = new User();
        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
            OpenPage(new Pages.Login());
        }

        public void OpenPage(Page page) => Animation.FrameDoubleAnimation(frame, page);
    }
}
