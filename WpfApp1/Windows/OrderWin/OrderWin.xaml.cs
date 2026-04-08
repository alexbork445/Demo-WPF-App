using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1.Windows.OrderWin
{
    /// <summary>
    /// Логика взаимодействия для OrderWin.xaml
    /// </summary>
    public partial class OrderWin : Window
    {
        private PaulDbBorkAsContext _context;

        public OrderWin(PaulDbBorkAsContext context)
        {
            InitializeComponent();
            _context = context;

            List<Order> orders = _context.Orders
                .Include(q => q.PickupPoint)
                .Include(q => q.Status)
                .ToList();
            BoxOrder.ItemsSource = _context.Orders.ToList();

            if (Cookies.LoggedUser.Role.RoleName == "Администратор")
            {
                BoxOrder.MouseDoubleClick += BoxProduct_MouseDoubleClick; ;
                PanelBottomButton.Visibility = Visibility.Visible;
            }
            else
            {
                PanelBottomButton.Visibility = Visibility.Collapsed;
            }
        }
        private void BoxProduct_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Order order = BoxOrder.SelectedItem as Order;
            if (order != null)
            {
                EditOrder edit = new EditOrder(order, _context);

                if (edit.ShowDialog() == true)
                {
                    BoxOrder.ItemsSource = _context.Orders.ToList();
                }
            }
        }
        private void Button_add_reques(object sender, RoutedEventArgs e)
        {
            AddOrder add = new AddOrder(_context);
            if (add.ShowDialog() == true)
            {
                BoxOrder.ItemsSource = _context.Orders.ToList();
            }
        }
        private void Buutton_delite_reques(object sender, RoutedEventArgs e)
        {
            Order prod = BoxOrder.SelectedItem as Order;
            if (prod != null)
            {
                _context.Orders.Remove(prod);
                _context.SaveChanges();
                //var ordersArticles = _context.Orders.Where(q => q.E.Contains(prod)).FirstOrDefault();
                //if (ordersArticles != null)
                //{
                //    _context.Orders.RemoveRange(ordersArticles);
                //}
                //Context.Orders.Remove(prod);
                //Context.SaveChanges();
                //BoxOrder.ItemsSource = Context.Orders.ToList();
            }
            else
            {
                MessageBox.Show("Выберете заказ для удаления");
            }
        }
        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            return;
        }
    }
}
