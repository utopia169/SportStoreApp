using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SportsStoreApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();

            // Или открыть после загрузки главного окна
            this.Loaded += (s, e) => {
                Edit editWindow = new Edit();
                editWindow.Show();
            };
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Здесь будет логика поиска
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика поиска
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Здесь будет логика при выборе товара
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика переключения страниц
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика переключения страниц
        }

        private void btnPage_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика переключения страниц
        }
    }
}