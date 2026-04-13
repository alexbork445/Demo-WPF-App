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
using WpfApp1.Windows.Product;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User currentUser;
        private List<Equipment> products;
        private readonly string projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private string SortParam = "по возрастанию";
        private string FiltParam = "все поставщики";

        private PaulDbBorkAsContext _context = new();

        public MainWindow()
        {
            Authorization authorization = new Authorization(_context);
            if (authorization.ShowDialog() != true)
            {
                Application.Current.Shutdown();
            }

            InitializeComponent();

            BoxUserName.Text = "гость";
            PanelFind.Visibility = Visibility.Collapsed;
            PanelBottomButton.Visibility = Visibility.Collapsed;
            DrawProductItem(products);

            if (Cookies.LoggedUser != null)
            {
                BoxUserName.Text = Cookies.LoggedUser.FullName;
                currentUser = Cookies.LoggedUser;
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
            MainWindow authorization = new MainWindow();            
            this.Close();
            authorization.ShowDialog();
        }

        public void DrawSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>()
            {
                new Supplier()
                {
                    SupplierId = -1,
                    SupplierName = "все поставщики",
                }
            };
            suppliers.AddRange(_context.Suppliers.ToList());
            ComboBoxItem.ItemsSource = suppliers;
        }
        private void DrawProductItem(List<Equipment> product)
        {
            if (BoxProduct != null)
            {
                BoxProduct.Items.Clear();
                foreach (Equipment item in products)
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
            Equipment product = controller.DataContext as Equipment;
            EditProductWindow edit = new(_context, product);

            if (edit.ShowDialog() == true)
            {
                DrawProductItem(products);
            }
        }

        private void Button_add_product(object sender, RoutedEventArgs e)
        {
            AddProductWindow add = new AddProductWindow(_context);
            if (add.ShowDialog() == true)
            {
                DrawProductItem(products);
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
                SortParam = "по возрастанию";
            }
            else if (radio.Content.ToString() == "по убыванию")
            {
                SortParam = "по убыванию";
            }
            Sort();
        }

        private void ComboSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.SelectedItem != null)
            {
                FiltParam = box.SelectedItem.ToString();
            }
            Sort();
        }

        public void Sort()
        {
            products = _context.Equipment.Include(q => q.Supplier)
                .Include(q => q.Manufacturer)
                .Include(q => q.Type)
                .ToList();

            products = products.Where(q =>
                (q.Description?.Contains(BoxFind.Text.Trim()) ?? false)
                || (q.Article?.Contains(BoxFind.Text.Trim()) ?? false)
                || (q.Name?.Contains(BoxFind.Text.Trim()) ?? false)
                ).Where(q => q.Supplier.SupplierName == FiltParam
                || FiltParam == "все поставщики").ToList();

            if (SortParam == "по возрастанию")
            {
                products = products.OrderBy(q => q.AvailableQuantity).ToList();
            }
            else if (SortParam == "по убыванию")
            {
                products = products.OrderByDescending(q => q.AvailableQuantity).ToList();
            }

            DrawProductItem(products);
        }

        private void Buutton_delite_product(object sender, RoutedEventArgs e)
        {
            Equipment prod = (Equipment)(BoxProduct.SelectedItem as ItemEquipment).DataContext;
            if (prod != null)
            {
                Order? order = _context.Orders.FirstOrDefault(q => q.Equipment.EquipmentId == prod.EquipmentId);

                if (order != null)
                {
                    MessageBox.Show("Продукт не можен быть удален, он участвует в заказе");
                    return;
                }
                _context.Equipment.Remove(prod);
                _context.SaveChanges();
                products = _context.Equipment.ToList();
                DrawProductItem(products);
                if (prod.Photo != null)
                {
                    File.Delete(Path.Combine(projPath, "Images", prod.Photo));
                }
            }
        }
    }
}