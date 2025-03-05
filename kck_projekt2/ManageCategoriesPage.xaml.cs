using kck_api.Controller;
using kck_api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kck_projekt2
{
    /// <summary>
    /// Interaction logic for ManageCategoriesPage.xaml
    /// </summary>
    public partial class ManageCategoriesPage : UserControl
    {
        private readonly CategoryController _categoryController;
        private MainWindow _mainWindow;
        public ManageCategoriesPage(MainWindow mainW)
        {
            InitializeComponent();
            _categoryController = CategoryController.GetInstance();
            _mainWindow = mainW;
            LoadCategories();
        }

        private async void LoadCategories()
        {
            List<CategoryModel> categories = await _categoryController.GetAllCategoriesAsync();
            if (categories.Count == 0)
            {
                InformationEmpty.Text = (string)Application.Current.Resources["EmptyCategoryListStr"];
                InformationEmpty.Visibility = Visibility.Visible;
                CategoriesListBox.ItemsSource = categories;
            }
            else
            {
                InformationEmpty.Visibility = Visibility.Collapsed;
                CategoriesListBox.ItemsSource = categories;
            }
        }

        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                await _categoryController.GetOrCreateCategoryIdAsync(CategoryNameTextBox.Text);
                CategoryNameTextBox.Clear();
                LoadCategories();
            }
        }

        private async void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            string categoryListText = (string)Application.Current.Resources["NewNameCatPopStr"];
            string editCategoryText = (string)Application.Current.Resources["EditCatStr"];
            if (CategoriesListBox.SelectedItem is CategoryModel selectedCategory)
            {
                string newName = CategoryNameTextBox.Text;
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    await _categoryController.EditCategoryAsync(selectedCategory.Id, newName);
                    LoadCategories();
                }
            }
        }

        private async void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesListBox.SelectedItem is CategoryModel selectedCategory)
            {
                await _categoryController.RemoveCategoryAsync(selectedCategory.Id);
                LoadCategories();
            }
        }
        private void CategoriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesListBox.SelectedItem is CategoryModel selectedCategory)
            {
                CategoryNameTextBox.Text = selectedCategory.Name;
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.contentControl.Content = new ActionMenuPage(_mainWindow);
        }
    }
}
