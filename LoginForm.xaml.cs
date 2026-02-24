using System.Windows;

namespace SportsStoreApp
{
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Функция входа будет реализована позже", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция регистрации будет реализована позже", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}