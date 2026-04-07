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
namespace WpfApp1.Windows.Product
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private readonly string projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        PaulDbBorkAsContext _context;
        private Equipment _product;
        private BitmapImage selectImage;
        private string? imageName = null;

        public EditProductWindow(PaulDbBorkAsContext context, Equipment product)
        {
            InitializeComponent();

            _context = context;
            _product = product;
            Load();
        }

        private void Load()
        {
            BoxCategory.ItemsSource = _context.EquipmentTypes.ToList();
            BoxCategory.SelectedItem = _product.Type;
            BoxSupplier.ItemsSource = _context.Suppliers.ToList();
            BoxSupplier.SelectedItem = _product.Supplier;
            BoxManufacturer.ItemsSource = _context.Manufacturers.ToList();
            BoxManufacturer.SelectedItem = _product.Manufacturer;
            BoxName.Text = _product.Name;
            BoxDescription.Text = _product.Description;
            BoxDiscount.Text = _product.Discount.ToString();
            BoxPrice.Text = _product.RentalCost.ToString();
            BoxUnit.Text = _product.RentalUnit.ToString();
            BoxCount.Text = _product.AvailableQuantity.ToString();
        }

        private void ButtonSaveProduct(object sender, RoutedEventArgs e)
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
                _product.Name = BoxName.Text.Trim();
                _product.Type = BoxCategory.SelectedItem as EquipmentType;
                _product.Description = BoxDescription.Text.Trim();
                _product.Manufacturer = BoxManufacturer.SelectedItem as Manufacturer;
                _product.Supplier = BoxSupplier.SelectedItem as Supplier;
                _product.RentalCost = int.Parse(BoxPrice.Text);
                _product.RentalUnit = BoxUnit.Text;
                _product.AvailableQuantity = int.Parse(BoxCount.Text);
                _product.Discount = int.Parse(BoxDiscount.Text);
                if (imageName != null)
                {
                    _product.Photo = imageName;
                }

                _context.Entry(_product).State = EntityState.Modified;
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

                File.Copy(Path.Combine(projPath, "Images"), openFile.FileName);

                selectImage = select;
                imageName = openFile.SafeFileName;
                BoxImage.Source = selectImage;
            }
        }
    }
}
