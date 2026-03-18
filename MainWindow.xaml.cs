using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SportsStoreApp
{
    public partial class MainWindow : Window
    {
        private bbbEntities1 db = new bbbEntities1();

        private Users currentUser;
        private bool isLoaded;

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        public MainWindow(Users user) : this()

        {
            currentUser = user;
            //txtUserInfo.Text = $"{user.LastName} {user.FirstName}";
        }

        private void LoadProducts()
        {
            try
            {
                var products = db.Products
                    .Include("Category")
                    .Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Category = p.Categories.Name,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        Status = p.Quantity > 0 ? "В наличии" : "Нет в наличии",
                        AddedDate = p.AddedDate
                    })
                    .ToList();
                dgProducts.ItemsSource = products;
                txtTotalItems.Text = products.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
 
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try 
            {
                string searchText = txtSearch.Text.ToLower();

                if (String.IsNullOrWhiteSpace(searchText) || searchText == "Поиск товаров...")
                {
                    LoadProducts();
                    return;
                }

                var filteredProducts = db.Products
                    .Include("Categories")
                    .ToList()
                    .Where(p => p.Name.ToLower().Contains(searchText) ||
                                p.Categories.Name.ToLower().Contains(searchText))
                    .Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Category = p.Categories.Name,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        Status = p.Quantity > 0 ? "В наличии" : "Нет в наличии",
                        AddedDate = p.AddedDate
                    })
                    .ToList();
                if (!isLoaded) return;
                dgProducts.ItemsSource = filteredProducts;
                txtTotalItems.Text = filteredProducts.Count.ToString();
            }
            catch (Exception)
            {
                LoadProducts();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем категории
                if (!db.Categories.Any())
                {
                    MessageBox.Show("В базе нет ни одной категории! Сначала добавьте категорию.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var firstCategory = db.Categories.First();

                int nextId = db.Products.Any() ? db.Products.Max(p => p.Id) + 1 : 1;

                var newProduct = new Products
                {
                    Name = $"Товар {nextId}",
                    CategoryId = firstCategory.Id,
                    Price = 0,
                    Quantity = 0,
                    StatusId = 1,
                    AddedDate = DateTime.Now
                };

                db.Products.Add(newProduct);
                db.SaveChanges();

                LoadProducts();
                MessageBox.Show("Товар успешно добавлен", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Ошибка валидации Entity Framework
                string errors = "";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errors += $"Поле: {validationError.PropertyName}, Ошибка: {validationError.ErrorMessage}\n";
                    }
                }
                MessageBox.Show($"Ошибка валидации:\n{errors}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                // Ошибка обновления БД - смотрим внутреннее исключение
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nВнутреннее исключение: {ex.InnerException.Message}";
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += $"\n\nГлубокое исключение: {ex.InnerException.InnerException.Message}";
                    }
                }
                MessageBox.Show($"Ошибка БД:\n{errorMessage}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Общая ошибка: {ex.Message}\n\nСтек: {ex.StackTrace}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                var selectedProduct = dgProducts.SelectedItem as dynamic;
                if (selectedProduct == null)
                {
                    MessageBox.Show("Выберите товар для редактирования",
                        "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var productId = selectedProduct.Id;
                var products = db.Products.Find(productId);

                if (products != null)
                {
                    Edit editWindow = new Edit(products);
                    editWindow.ShowDialog();

                    MessageBox.Show($"Редактирование товара: {products.Name}",
                        "Редактирование", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка редактирования: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic selectedProduct = dgProducts.SelectedItem;
                if (selectedProduct == null)
                {
                    MessageBox.Show("Выберите товар для удаления",
                        "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var result = MessageBox.Show("Вы действительно хотите удалить выбранный товар?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int productId = selectedProduct.Id;
                    var product = db.Products.Find(productId);

                    if (product != null)
                    {
                        db.Products.Remove(product);
                        db.SaveChanges();
                        LoadProducts();

                        MessageBox.Show("Товар был успешно удален", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти из системы?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) 
            {
                LoginForm loginWindow = new LoginForm();
                loginWindow.Show();

                this.Close();
            }
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnPage1_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }
        private void btnPage2_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }
        private void btnPage3_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }
        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dgProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnEdit_Click(sender, null);
        }

        protected override void OnClosed(EventArgs e)
        {
            db?.Dispose();
            base.OnClosed(e);
        }
    }
}