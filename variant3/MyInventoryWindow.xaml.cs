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
using variant3.Models;

namespace variant3
{
    /// <summary>
    /// Логика взаимодействия для MyInventoryWindow.xaml
    /// </summary>
    public partial class MyInventoryWindow : Window
    {
        public MyInventoryWindow(User user)
        {
            InitializeComponent();
            LoadMyInventory(user);
        }

        private async void LoadMyInventory(User user)
        {
            using (var context = new Variant3Context())
            {
                var myInventory = await context.Inventories
                    .Where(i => i.UserId == user.Id && i.State == "выдана")
                    .ToListAsync();

                dgMyInventory.ItemsSource = myInventory;
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
