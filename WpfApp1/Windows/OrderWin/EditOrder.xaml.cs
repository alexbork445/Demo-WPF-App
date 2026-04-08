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
    /// Логика взаимодействия для EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {
        private PaulDbBorkAsContext _context;
        private Order _order;
        public EditOrder(Order order, PaulDbBorkAsContext context)
        {
            InitializeComponent();
            _context = context;
            PanelOrder.DataContext = order;
            _order = order;
            BoxStatus.ItemsSource = context.OrderStatuses.ToList();
            BoxStatus.SelectedItem = order.Status;
        }
        private void Button_save(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxRentalQuantity.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxDateOrder.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxArc.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxDelivary.Text.Trim()))
            {
                try
                {

                    _order.RentalStartDate = DateTime.Parse(BoxDateOrder.Text.Trim());
                    _order.RentalQuantity = int.Parse(BoxRentalQuantity.Text.Trim());
                    _order.ReceiptCode = decimal.Parse(BoxArc.Text.Trim());
                    _order.PickupPoint = _context.PickupPoints.FirstOrDefault(q => q.Address == BoxDelivary.Text.Trim());
                    _order.Status = BoxStatus.SelectedItem as OrderStatus;

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
