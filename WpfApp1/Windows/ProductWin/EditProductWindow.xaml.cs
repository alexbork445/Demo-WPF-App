using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1.Windows.ProductWin
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private readonly string _projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private ExampleDbContext _context;
        private Product _product;
        private BitmapImage _selectImage;
        private string? _imageName = null;
        public EditProductWindow(ExampleDbContext context, Product product)
        {
            _context = context;
            InitializeComponent();

            _product = product;
            Load();
        }

        private void Load()
        {
            BoxCategory.ItemsSource = _context.ProductType.ToList();
            BoxCategory.SelectedItem = _product.ProductType;
            BoxSupplier.ItemsSource = _context.Supplier.ToList();
            BoxSupplier.SelectedItem = _product.Supplier;
            BoxManufacturer.ItemsSource = _context.Manufacturer.ToList();
            BoxManufacturer.SelectedItem = _product.Manufacturer;
            BoxName.Text = _product.Article;
            BoxDescription.Text = _product.Description;
            BoxDiscount.Text = _product.Discount.ToString();
            BoxPrice.Text = _product.Price.ToString();
            BoxUnit.Text = _product.UnitOfMeasure.ToString();
            BoxCount.Text = _product.Amount.ToString();
        }

        private void ButtonSaveProduct(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BoxDescription.Text) ||
                string.IsNullOrWhiteSpace(BoxDiscount.Text) ||
                string.IsNullOrWhiteSpace(BoxName.Text) ||
                string.IsNullOrWhiteSpace(BoxPrice.Text) ||
                string.IsNullOrWhiteSpace(BoxUnit.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                _product.Article = BoxName.Text;
                _product.ProductType = BoxCategory.SelectedItem as ProductType;
                _product.Description = BoxDescription.Text;
                _product.Manufacturer = BoxManufacturer.SelectedItem as Manufacturer;
                _product.Supplier = BoxSupplier.SelectedItem as Supplier;
                _product.UnitOfMeasure = BoxUnit.Text;
                _product.Price = int.Parse(BoxPrice.Text.Trim());
                _product.Discount = int.Parse(BoxDiscount.Text);
                _product.Amount = int.Parse(BoxCount.Text.Trim());
                if (_imageName != null)
                {
                    _product.Photo = _imageName;
                }

                _context.Entry(_product).State = EntityState.Modified;
                _context.SaveChanges();

                DialogResult = true;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"не верный формат ввода {ex.Message}", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonLoadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == true)
            {
                Uri uri = new Uri(openFile.FileName);

                BitmapImage select = new(uri);
                if (select.Width > 512 || select.Height > 512)
                {
                    MessageBox.Show("Размеры изображения имеют не верный формат", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    File.Copy(openFile.FileName, Path.Combine(_projPath, "Images", Path.GetFileName(openFile.FileName)));
                }
                catch(Exception ex)
                {
                    
                }

                _selectImage = select;
                _imageName = openFile.SafeFileName;
                BoxImage.Source = _selectImage;
            }
        }

    }
}
