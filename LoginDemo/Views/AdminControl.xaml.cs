using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using System.Data;

namespace LoginDemo.Views
{
    public partial class AdminControl : UserControl
    {
        private string dbPath = @"Data Source=C:\SOFT\DB Uyutov\authdemo.db";
        public AdminControl()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers() {
            using var connection = new SqliteConnection(dbPath);
            connection.Open();
            using var command = new SqliteCommand("SELECT * FROM Users", connection);
            using var reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            UsersGrid.ItemsSource = dt.DefaultView;
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e) {
            using var connection = new SqliteConnection(dbPath);
            if (sender is Button btn && btn.DataContext is DataRowView row) {
                string login = row["Login"].ToString();
                string password = row["Password"].ToString();
                int id = Convert.ToInt32(row["Id"]);

                if(login == "admin" && password == "admin") {
                    MessageBox.Show("гл. админа нельзя удалить");
                    return;
                }
                if (MessageBox.Show($"удалить пользователя ID={id}?", "удаление",
                    MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    return;
                using var command = new SqliteCommand(
                    "DELETE FROM Users WHERE Id = $id", connection);
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
                LoadUsers();
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e) {
            string login = InputLogin.Text;
            string password = InputPassword.Text;
            string role = (InputRole.SelectedItem as ComboBoxItem)?.Content.ToString();

            if(string.IsNullOrWhiteSpace(login)||
                string.IsNullOrWhiteSpace(password)||
                string.IsNullOrWhiteSpace(role)) {
                MessageBox.Show("заполните все поля");
            }
            using var connection = new SqliteConnection(dbPath);
            connection.Open();
            using var command = new SqliteCommand("INSERT INTO Users (Login, Password, Role) VALUES ($l, $p, $r", connection);
            command.Parameters.AddWithValue("$l", login);
            command.Parameters.AddWithValue("$p", password);
            command.Parameters.AddWithValue("$r", role);
            command.ExecuteNonQuery();
            InputLogin.Clear();
            InputPassword.Clear();
            InputRole.SelectedIndex = -1;
            LoadUsers();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            (Window.GetWindow(this) as MainWindow)?.SwitchToLogin();
        }

        private void UsersGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is DataRowView rowView)
            {
                int id = Convert.ToInt32(rowView["Id"]);
                string login = rowView["Login"].ToString();
                string password = rowView["Password"].ToString();
                string role = rowView["Role"].ToString();

                using var connection = new SqliteConnection(dbPath);
                connection.Open();
                string sql = "UPDATE Users SET Login = $login, Password = $password, Role = $role WHERE Id = $id";

                using var command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("$login", login);
                command.Parameters.AddWithValue("$password", password);
                command.Parameters.AddWithValue("$role", role);
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
