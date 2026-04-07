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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private PaulDbBorkAsContext _context;
        public AddOrder(PaulDbBorkAsContext context)
        {
            InitializeComponent();
            _context = context;
            BoxStatus.ItemsSource = _context.OrderStatuses.ToList();
        }
        private void Button_add(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxRentalQuantity.Text) &&
                !string.IsNullOrWhiteSpace(BoxDateOrder.Text) &&
                !string.IsNullOrWhiteSpace(BoxArc.Text) &&
                !string.IsNullOrWhiteSpace(BoxDelivary.Text))
            {
                try
                {
                    //тут идёт присвоение id как как в таблице я забыл установить автоикремент для поля ID,
                    //поэтому я делаю это руками (так делать не надо)
                    Order order = new Order()
                    {

                        RentalStartDate = DateTime.Parse(BoxDateOrder.Text),
                        RentalQuantity = int.Parse(BoxRentalQuantity.Text),
                        ReceiptCode = decimal.Parse(BoxArc.Text),
                        PickupPoint = _context.PickupPoints.FirstOrDefault(q => q.Address == BoxDelivary.Text),
                        Status = BoxStatus.SelectedItem as OrderStatus
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    DialogResult = true;
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
        }
    }
}
