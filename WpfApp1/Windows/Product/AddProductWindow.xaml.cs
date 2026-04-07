using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.IO;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1.Windows.Product
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private readonly string projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private string? imageName = null;
        private BitmapImage selectImage;
        private PaulDbBorkAsContext _context;
        public AddProductWindow(PaulDbBorkAsContext context)
        {
            InitializeComponent();
            _context = context;

            selectImage = new BitmapImage(new Uri(Path.Combine(projPath, "Images", "Defaults", "picture.png")));
            BoxImage.Source = selectImage;
            BoxCategory.ItemsSource = _context.EquipmentTypes.ToList();
            BoxManufacturer.ItemsSource = _context.Manufacturers.ToList();
            BoxSupplier.ItemsSource = _context.Suppliers.ToList();
        }

        private void ButtonAddProduct(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BoxDescription.Text) ||
                string.IsNullOrWhiteSpace(BoxDiscount.Text) ||
                string.IsNullOrWhiteSpace(BoxName.Text) ||
                string.IsNullOrWhiteSpace(BoxPrice.Text) ||
                string.IsNullOrWhiteSpace(BoxUnit.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            try
            {
                Equipment newProduct = new()
                {
                    Name = BoxName.Text.Trim(),
                    Type = BoxCategory.SelectedItem as EquipmentType,
                    Description = BoxDescription.Text.Trim(),
                    Manufacturer = BoxManufacturer.SelectedItem as Manufacturer,
                    Supplier = BoxSupplier.SelectedItem as Supplier,
                    RentalCost = int.Parse(BoxPrice.Text),
                    RentalUnit = BoxUnit.Text,
                    AvailableQuantity = int.Parse(BoxCount.Text),
                    Discount = int.Parse(BoxDiscount.Text),
                    Photo = imageName,
                };
                _context.Equipment.Add(newProduct);
                _context.SaveChanges();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"не верный формат ввода {ex.Message}");
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
                if (select.Width > 400 || select.Height > 300)
                {
                    MessageBox.Show("Размеры изображения имеют не верный формат");
                    return;
                }

                selectImage = select;
                imageName = openFile.SafeFileName;
                BoxImage.Source = selectImage;
            }
        }
    }
}
