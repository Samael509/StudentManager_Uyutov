using System.Windows;
using System.Windows.Controls;

namespace LoginDemo.Views
{
    public partial class UsersControl : UserControl
    {
        public UsersControl()
        {
            InitializeComponent();
        }

        private void Button_click(object sender, RoutedEventArgs e) {
            (Window.GetWindow(this) as MainWindow)?.SwitchToLogin();
        }
    }
}
