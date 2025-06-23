using System;
using System.Windows;
using variant3.Models;

namespace variant3
{
    public partial class AddEditInventoryWindow : Window
    {
        public Inventory NewInventory { get; private set; }
        public string WindowTitle { get; private set; }

        // Конструктор для добавления нового инвентаря
        public AddEditInventoryWindow()
        {
            InitializeComponent();
            WindowTitle = "Добавление инвентаря";
            NewInventory = new Inventory
            {
                PublicationDate = DateOnly.FromDateTime(DateTime.Now),
                State = "в наличии"
            };
            DataContext = this;
        }

        // Конструктор для редактирования существующего инвентаря
        public AddEditInventoryWindow(Inventory inventoryToEdit)
        {
            InitializeComponent();
            WindowTitle = "Редактирование инвентаря";
            NewInventory = new Inventory
            {
                Id = inventoryToEdit.Id,
                InventoryNumber = inventoryToEdit.InventoryNumber,
                Name = inventoryToEdit.Name,
                Type = inventoryToEdit.Type,
                Description = inventoryToEdit.Description,
                PublicationDate = inventoryToEdit.PublicationDate,
                State = inventoryToEdit.State,
                UserId = inventoryToEdit.UserId
            };
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewInventory.InventoryNumber) ||
                string.IsNullOrWhiteSpace(NewInventory.Name) ||
                string.IsNullOrWhiteSpace(NewInventory.Type))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}