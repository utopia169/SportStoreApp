using System;
using System.Linq;
using System.Windows;

namespace SportsStoreApp
{
    public partial class LoginForm : Window
    {
        private bbbEntities1 db = new bbbEntities1();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Введите email");
                return;
            }

            if (txtPassword.Password == "")
            {
                ShowError("Введите пароль");
                return;
            }

            try
            {
                var user = db.Users.FirstOrDefault(u => u.Email == txtUsername.Text);

                if (user == null)
                {
                    ShowError("Пользователь не найден");
                    return;
                }

                if (user.PasswordHash == txtPassword.Password)
                {
                    MessageBox.Show($"Добро пожаловать, {user.Email}!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow mainWindow = new MainWindow();

                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ShowError("Неверный пароль");
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка подключения к БД: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}