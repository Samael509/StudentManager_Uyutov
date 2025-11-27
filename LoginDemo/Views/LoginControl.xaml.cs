using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;


namespace LoginDemo.Views
{
    public partial class LoginControl : UserControl
    {
        private string dbPath = @"Data Source=C:\SOFT\authdemo.db";
        public LoginControl()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = InputLogin.Text;
                string password = InputPassword.Password;
                using var connection = new SqliteConnection(dbPath);
                connection.Open();
                using var command = new SqliteCommand("SELECT Role FROM Users WHERE Login = $l AND Password = $p", connection);
                command.Parameters.AddWithValue("$l", login);
                command.Parameters.AddWithValue("$p", password);
                var role = command.ExecuteScalar()?.ToString();
                if (role == null)
                {
                    InfoText.Text = "неверный логин или пароль";
                    return;
                }
                if (role == "Admin")
                {
                    (Window.GetWindow(this) as MainWindow)?.SwitchToAdmin();
                }
                else
                {
                    (Window.GetWindow(this) as MainWindow)?.SwitchToUser();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message);
            }
        }
    }
}

