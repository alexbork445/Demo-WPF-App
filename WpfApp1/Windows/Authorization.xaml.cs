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

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private PaulDbBorkAsContext _context;
        public Authorization(PaulDbBorkAsContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void Button_authorization(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxLogin.Text) && !string.IsNullOrWhiteSpace(BoxPassword.Text))
            {
                User user = _context.Users.Include(u => u.Role).FirstOrDefault(q => q.Login == BoxLogin.Text.Trim() && q.Password == BoxPassword.Text.Trim());
                if (user != null)
                {
                    Cookies.LoggedUser = user;
                    DialogResult = true;
                    return;
                }
                else
                {
                    MessageBox.Show("Пользователь не найден");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void Button_authorization_gouest(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            return;
        }
    }
}
