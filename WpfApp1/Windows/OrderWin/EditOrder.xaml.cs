using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {
        private ExampleDbContext _context;
        private Order _order;
        public Order NewOrder { get; set; }
        public EditOrder(Order order, ExampleDbContext context)
        {
            _context = context;
            _order = order;
            InitializeComponent();
            BoxStatus.ItemsSource = context.OrderStatus.ToList();
            BoxArc.Text = order.Code;
            BoxDateOrder.Text = order.OrderDate.ToString();
            BoxDateDelivery.Text = order.DeliveryDate.ToString();
            BoxStatus.SelectedItem = order.OrderStatus;
            BoxDelivary.ItemsSource = context.PickupPoint.ToList();
            BoxDelivary.SelectedItem = order.PickupPoint;
        }
        private void Button_save(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxDateDelivery.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxDateOrder.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxArc.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxDelivary.Text.Trim()))
            {
                try
                {
                    _order.Code = BoxArc.Text.Trim();
                    _order.OrderStatus = (OrderStatus)BoxStatus.SelectedItem;
                    _order.PickupPoint = (PickupPoint)BoxDelivary.SelectedItem;
                    _order.OrderDate = DateOnly.FromDateTime(DateTime.Parse(BoxDateOrder.Text.Trim()));
                    _order.DeliveryDate = DateOnly.FromDateTime(DateTime.Parse(BoxDateDelivery.Text.Trim()));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка редактирования заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _context.SaveChanges();
                DialogResult = true;
                return;
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            return;
        }
    }
}
