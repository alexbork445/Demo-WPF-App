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
        public EditOrder(Order order, ExampleDbContext context)
        {
            InitializeComponent();
            _context = context;
            PanelOrder.DataContext = order;
            _order = order;
            BoxStatus.ItemsSource = context.OrderStatus.ToList();
            BoxStatus.SelectedItem = order.OrderStatus;
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
                    _order.OrderDate = DateOnly.Parse(BoxDateOrder.Text.Trim());
                    _order.DeliveryDate = DateOnly.Parse(BoxDateDelivery.Text.Trim());
                    _order.Code = BoxArc.Text.Trim();
                    _order.PickupPoint = _context.PickupPoint.FirstOrDefault(q => q.Address == BoxDelivary.Text.Trim());
                    _order.OrderStatus = BoxStatus.SelectedItem as OrderStatus;

                    _context.Entry(_order).State = EntityState.Modified;
                    _context.SaveChanges();

                    DialogResult = true;
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            return;
        }
    }
}
