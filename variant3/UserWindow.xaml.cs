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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private readonly Variant3Context _context;
        private readonly User _currentUser;

        public UserWindow()
        {
            InitializeComponent();
            _context = new Variant3Context();
            _currentUser = LoginWindow.CurrentUser;
            LoadInventory();
        }

        private async void LoadInventory()
        {
            var inventory = await _context.Inventories.ToListAsync();
            dgInventory.ItemsSource = inventory;
        }

        private async void btnTake_Click(object sender, RoutedEventArgs e)
        {
            if (dgInventory.SelectedItem is Inventory selectedItem)
            {
                if (selectedItem.State != "в наличии")
                {
                    MessageBox.Show("Этот инвентарь уже выдан или на обслуживании");
                    return;
                }

                selectedItem.State = "выдана";
                selectedItem.UserId = _currentUser.Id;

                _context.Inventories.Update(selectedItem);
                await _context.SaveChangesAsync();

                LoadInventory();
                MessageBox.Show("Инвентарь успешно выдан");
            }
            else
            {
                MessageBox.Show("Выберите инвентарь из списка");
            }
        }

        private async void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (dgInventory.SelectedItem is Inventory selectedItem)
            {
                if (selectedItem.UserId != _currentUser.Id || selectedItem.State != "выдана")
                {
                    MessageBox.Show("Вы можете списать только инвентарь, выданный вам");
                    return;
                }

                selectedItem.State = "в наличии";
                selectedItem.UserId = null;

                _context.Inventories.Update(selectedItem);
                await _context.SaveChangesAsync();

                LoadInventory();
                MessageBox.Show("Инвентарь успешно списан");
            }
            else
            {
                MessageBox.Show("Выберите инвентарь из списка");
            }
        }

        private void btnMyInventory_Click(object sender, RoutedEventArgs e)
        {
            var myInventoryWindow = new MyInventoryWindow(_currentUser);
            myInventoryWindow.ShowDialog();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}
