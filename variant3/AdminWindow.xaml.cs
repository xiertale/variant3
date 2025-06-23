using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using variant3.Models;

namespace variant3
{
    public partial class AdminWindow : Window
    {
        private readonly Variant3Context _context;

        public AdminWindow()
        {
            InitializeComponent();
            _context = new Variant3Context();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                // Загружаем инвентарь с информацией о пользователе
                var inventory = await _context.Inventories
                    .Include(i => i.User)
                    .ToListAsync();
                dgAdminInventory.ItemsSource = inventory;

                // Загружаем пользователей с их ролями
                var users = await _context.Users
                    .Include(u => u.Role)
                    .ToListAsync();
                dgUsers.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private async void btnAddInventory_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditInventoryWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    _context.Inventories.Add(addWindow.NewInventory);
                    await _context.SaveChangesAsync();
                    LoadData();
                    MessageBox.Show("Инвентарь успешно добавлен");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}");
                }
            }
        }

        private async void btnEditInventory_Click(object sender, RoutedEventArgs e)
        {
            if (dgAdminInventory.SelectedItem is Inventory selectedItem)
            {
                var editWindow = new AddEditInventoryWindow(selectedItem);
                if (editWindow.ShowDialog() == true)
                {
                    try
                    {
                        _context.Inventories.Update(editWindow.NewInventory);
                        await _context.SaveChangesAsync();
                        LoadData();
                        MessageBox.Show("Изменения сохранены");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при редактировании: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите инвентарь для редактирования");
            }
        }

        private async void btnDeleteInventory_Click(object sender, RoutedEventArgs e)
        {
            if (dgAdminInventory.SelectedItem is Inventory selectedItem)
            {
                if (MessageBox.Show("Удалить выбранный инвентарь?", "Подтверждение", 
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _context.Inventories.Remove(selectedItem);
                        await _context.SaveChangesAsync();
                        LoadData();
                        MessageBox.Show("Инвентарь удален");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите инвентарь для удаления");
            }
        }

        private async void btnAssignInventory_Click(object sender, RoutedEventArgs e)
        {
            if (dgAdminInventory.SelectedItem is Inventory selectedInventory && 
                dgUsers.SelectedItem is User selectedUser)
            {
                if (selectedInventory.State != "в наличии")
                {
                    MessageBox.Show("Этот инвентарь уже выдан или на обслуживании");
                    return;
                }

                selectedInventory.State = "выдана";
                selectedInventory.UserId = selectedUser.Id;

                try
                {
                    _context.Inventories.Update(selectedInventory);
                    await _context.SaveChangesAsync();
                    LoadData();
                    MessageBox.Show("Инвентарь успешно выдан пользователю");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите инвентарь и пользователя");
            }
        }

        private async void btnUnassignInventory_Click(object sender, RoutedEventArgs e)
        {
            if (dgAdminInventory.SelectedItem is Inventory selectedInventory)
            {
                if (selectedInventory.State != "выдана")
                {
                    MessageBox.Show("Этот инвентарь не выдан пользователю");
                    return;
                }

                selectedInventory.State = "в наличии";
                selectedInventory.UserId = null;

                try
                {
                    _context.Inventories.Update(selectedInventory);
                    await _context.SaveChangesAsync();
                    LoadData();
                    MessageBox.Show("Инвентарь успешно списан");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите инвентарь для списания");
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}