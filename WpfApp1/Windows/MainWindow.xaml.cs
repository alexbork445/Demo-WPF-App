using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using WpfApp1.Data;
using WpfApp1.Models;
using WpfApp1.UserControllers;
using WpfApp1.Windows;
using WpfApp1.Windows.OrderWin;
using WpfApp1.Windows.ProductWin;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _currentUser;
        private List<Product> _products;
        private readonly string _projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private string _sortParam = "по возрастанию";
        private string _filtParam = "все поставщики";

        private ExampleDbContext _context = new();

        public MainWindow()
        {
            Authorization authorization = new Authorization(_context);
            if (authorization.ShowDialog() != true)
            {
                Application.Current.Shutdown();
            }

            InitializeComponent();
            _products = _context.Product.ToList();

            BoxUserName.Text = "гость";
            PanelFind.Visibility = Visibility.Collapsed;
            PanelBottomButton.Visibility = Visibility.Collapsed;
            Sort();

            if (Cookies.LoggedUser != null)
            {
                BoxUserName.Text = Cookies.LoggedUser.Fullname;
                _currentUser = Cookies.LoggedUser;
                DrawSuppliers();

                if (Cookies.LoggedUser.Role.RoleName == "Администратор")
                {
                    BoxProduct.MouseDoubleClick += BoxProduct_MouseDoubleClick;
                    PanelFind.Visibility = Visibility.Visible;
                    PanelBottomButton.Visibility = Visibility.Visible;
                }
                else
                {
                    PanelBottomAdmin.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Button_exit_user(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
            
        }

        public void DrawSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>()
            {
                new Supplier()
                {
                    Id = -1,
                    SupplierName = "все поставщики",
                }
            };
            suppliers.AddRange(_context.Supplier.ToList());
            ComboBoxItem.ItemsSource = suppliers;
        }

        private void DrawProductItem(List<Product> product)
        {
            if (BoxProduct != null)
            {
                BoxProduct.Items.Clear();
                foreach (Product item in product)
                {
                    if (item != null)
                    {
                        ItemEquipment xml = new ItemEquipment(item);

                        BoxProduct.Items.Add(xml);
                    }
                }
            }
        }

        private void BoxProduct_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox list = sender as ListBox;
            ItemEquipment controller = list.SelectedItem as ItemEquipment;
            Product product = controller.DataContext as Product;
            EditProductWindow edit = new(_context, product);

            if (edit.ShowDialog() == true)
            {
                Sort();
            }
        }

        private void Button_add_product(object sender, RoutedEventArgs e)
        {
            AddProductWindow add = new AddProductWindow(_context);
            if (add.ShowDialog() == true)
            {
                Sort();
            }
        }

        private void Button_request(object sender, RoutedEventArgs e)
        {
            OrderWin request = new OrderWin(_context);
            if (request.ShowDialog() == true)
            {

            }
        }

        private void BoxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();
        }

        private void RadioUpp_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;

            if (radio.Content.ToString() == "по возрастанию")
            {
                _sortParam = "по возрастанию";
            }
            else if (radio.Content.ToString() == "по убыванию")
            {
                _sortParam = "по убыванию";
            }
            Sort();
        }

        private void ComboSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.SelectedItem != null)
            {
                _filtParam = box.SelectedItem.ToString();
            }
            Sort();
        }

        public void Sort()
        {
            _products = _context.Product
                .Include(q => q.Supplier)
                .Include(q => q.Manufacturer)
                .Include(q => q.ProductType)
                .ToList();

            _products = _products.Where(q =>
                (q.Description?.Contains(BoxFind.Text.Trim()) ?? false)
                || (q.Article?.Contains(BoxFind.Text.Trim()) ?? false)
                || (q.Manufacturer?.ManufacturerName?.Contains(BoxFind.Text.Trim()) ?? false)
                ).Where(q => q.Supplier?.SupplierName == _filtParam
                || _filtParam == "все поставщики").ToList();

            if (_sortParam == "по возрастанию")
            {
                _products = _products.OrderBy(q => q.Amount).ToList();
            }
            else if (_sortParam == "по убыванию")
            {
                _products = _products.OrderByDescending(q => q.Amount).ToList();
            }

            DrawProductItem(_products);
        }

        private void Buutton_delite_product(object sender, RoutedEventArgs e)
        {
            Product prod = (Product)(BoxProduct.SelectedItem as ItemEquipment).DataContext;
            if (prod != null)
            {
                var order = _context.OrderDetails.FirstOrDefault(q => q.ProductId == prod.Id);

                if (order != null)
                {
                    MessageBox.Show("Продукт не можен быть удален, он участвует в заказе", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                _context.Product.Remove(prod);
                _context.SaveChanges();
                _products = _context.Product.ToList();
                Sort();
                if (prod.Photo != null)
                {
                    try
                    {
                        File.Delete(Path.Combine(_projPath, "Images", prod.Photo));
                    }
                    catch (Exception ex) {}
                }
            }
            else
            {
                MessageBox.Show("Выберете продукт для удаления", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}