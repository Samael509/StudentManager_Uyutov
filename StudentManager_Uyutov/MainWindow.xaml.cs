using System.Data;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace StudentManager_Uyutov;

public partial class MainWindow : Window {

    private const string ConnectionString = @"Data Source=C:\SOFT\students.db";
    public MainWindow() {
        InitializeComponent();
        try {
            LoadData();
        }
        catch (Exception ex) {
            MessageBox.Show($"ошибка при загрузке данных: {ex.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }

    private void LoadData() {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = new SqliteCommand("SELECT Id, Name FROM Students ORDER BY Id", connection);
        using var reader = command.ExecuteReader();
        var dt = new DataTable();
        dt.Load(reader);
        DataGridPeople.ItemsSource = dt.DefaultView;
    }

    private void Add_Click(object sender, RoutedEventArgs e) {
        var name = InputName.Text?.Trim();
        if (string.IsNullOrEmpty(name)) {
            MessageBox.Show("введите имя перед добавлением", "внимание",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        try {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = new SqliteCommand($"INSERT INTO Students (Name) VALUES ($name);", connection);
            command.Parameters.AddWithValue("$name", name);
            command.ExecuteNonQuery();
        }
        catch (Exception ex) {
            MessageBox.Show($"ошибка при добавлении: {ex.Message}", "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        LoadData();
        InputName.Clear();
    }

    private void Delete_Click(object sender, RoutedEventArgs e) {
        if (DataGridPeople.SelectedItem is not DataRowView row) {
            MessageBox.Show(
                "выберите запись",
                "внимание",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        long idLong;
        try {
            idLong = Convert.ToInt64(row["Id"]);
        }
        catch {
            MessageBox.Show("не удалось прочитать Id", "ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        var answer = MessageBox.Show($"удалить запись с Id = {idLong}?", "подтвердите удаление",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (answer != MessageBoxResult.Yes) return;

        try {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = new SqliteCommand($"DELETE FROM Students WHERE Id = $id;", connection);
            command.Parameters.AddWithValue("$id", idLong);
            command.ExecuteNonQuery();
        }
        catch (Exception ex) {
            MessageBox.Show($"ошибка при удалении записи: {ex.Message}", "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        LoadData();
    }

    private void DataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
    {
        e.Row.Header = (e.Row.GetIndex() + 1).ToString();
    }
}