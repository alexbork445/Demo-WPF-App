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
using System.Windows.Navigation;
using System.IO;
using WpfApp1.Models;

namespace WpfApp1.UserControllers
{
    /// <summary>
    /// Логика взаимодействия для ItemEquipment.xaml
    /// </summary>
    public partial class ItemEquipment : UserControl
    {
        private string _projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        
        public ItemEquipment(Product equipment)
        {
            InitializeComponent();
            DataContext = equipment;
            string path = equipment.Photo == null ? Path.Combine(_projPath, "Images", "Defaults", "picture.png") : Path.Combine(_projPath, "Images", equipment.Photo);

            Uri uri = new Uri(path);
            try
            {
                BitmapImage bitmap = new(uri);
                BoxImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                BitmapImage bitmap = new(new Uri(Path.Combine(_projPath, "Images", "Defaults", "picture.png")));
                BoxImage.Source = bitmap;
            }

            if (equipment.Discount >= 15)
            {
                BoxDiscount.Background = new BrushConverter().ConvertFrom("#2E8B57") as SolidColorBrush;
            }
            if (equipment.Discount > 0)
            {
                BoxPrice.Foreground = Brushes.Red;
                BoxPrice.TextDecorations.Add(TextDecorations.Strikethrough);
                BoxNewPrice.Text = (equipment.Price * (1 - equipment.Discount.Value / (decimal)100.0)).ToString();
            }

            if (equipment.Amount == 0)
            {
                BoxCount.Foreground = Brushes.Blue;
            }
        }
    }
}
