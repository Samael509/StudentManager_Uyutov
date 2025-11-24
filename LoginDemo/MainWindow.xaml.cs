using System.Security.Cryptography.X509Certificates;
using System.Windows;
using LoginDemo.Views;

namespace LoginDemo {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            MainContent.Content = new LoginControl();
        }

        public void SwitchToAdmin() {
            MainContent.Content = new AdminControl();
        }

        public void SwitchToUser() {
            MainContent.Content = new UsersControl();
        }

        public void SwitchToLogin() {
            MainContent.Content = new LoginControl();
        }
    }
}