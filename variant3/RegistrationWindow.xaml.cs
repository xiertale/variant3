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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private readonly Variant3Context _context;

        public RegistrationWindow()
        {
            InitializeComponent();
            _context = new Variant3Context();
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            var login = txtLogin.Text;
            var password = txtPassword.Text;
            var surname = txtSurname.Text;
            var name = txtName.Text;
            var phone = txtPhone.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            // Проверяем, существует ли уже пользователь с таким логином
            if (await _context.Users.AnyAsync(u => u.Login == login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return;
            }

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

            var newUser = new User
            {
                Login = login,
                Password = password, // В реальном приложении нужно хешировать!
                Surname = surname,
                Name = name,
                Phone = phone,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                Role = userRole
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            MessageBox.Show("Регистрация успешна. Теперь вы можете войти в систему.");
            this.Close();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
