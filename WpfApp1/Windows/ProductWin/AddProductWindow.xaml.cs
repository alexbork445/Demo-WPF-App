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
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private readonly string _projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private string? _imageName = null;
        private BitmapImage _selectImage;
        private ExampleDbContext _context;
        public AddProductWindow(ExampleDbContext context)
        {
            InitializeComponent();
            _context = context;

            _selectImage = new BitmapImage(new Uri(Path.Combine(_projPath, "Images", "Defaults", "picture.png")));
            BoxImage.Source = _selectImage;
            BoxCategory.ItemsSource = _context.ProductType.ToList();
            BoxManufacturer.ItemsSource = _context.Manufacturer.ToList();
            BoxSupplier.ItemsSource = _context.Supplier.ToList();
        }

        private void ButtonAddProduct(object sender, RoutedEventArgs e)
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
                if (int.Parse(BoxPrice.Text) < 0) throw new Exception("Цена не может быть отрицательной");
                if (int.Parse(BoxCount.Text) < 0) throw new Exception("Количество не может быть отрицательным");
                Product newProduct = new Product()
                {
                    Article = BoxName.Text,
                    ProductType = BoxCategory.SelectedItem as ProductType,
                    Description = BoxDescription.Text,
                    Manufacturer = BoxManufacturer.SelectedItem as Manufacturer,
                    Supplier = BoxSupplier.SelectedItem as Supplier,
                    Price = int.Parse(BoxPrice.Text),
                    UnitOfMeasure = BoxUnit.Text,
                    Amount = int.Parse(BoxCount.Text),
                    Discount = int.Parse(BoxDiscount.Text),
                    Photo = _imageName,
                };
                _context.Product.Add(newProduct);
                _context.SaveChanges();

                DialogResult = true;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не верный формат ввода:\n{ex.Message}", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            return;
        }

        private void ButtonLoadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == true)
            {
                Uri uri = new Uri(openFile.FileName);

                BitmapImage select = new(uri);
                if (select.Width > 400 || select.Height > 300)
                {
                    MessageBox.Show("Размеры изображения имеют не верный формат", "Ошибка изображения", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _selectImage = select;
                _imageName = openFile.SafeFileName;
                BoxImage.Source = _selectImage;
            }
        }
    }
}
