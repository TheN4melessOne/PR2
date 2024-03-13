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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PR2
{
    public partial class MainWindow : Window
    {
        DispatcherTimer ifInactive;
        public MainWindow()
        {
            InitializeComponent();
            MainEmployee = new AuthorizationEmployee("Nameless", "Nameless");

            ifInactive = new DispatcherTimer();
            ifInactive.Interval = TimeSpan.FromSeconds(10);
            ifInactive.Tick += IfInactive_Tick;
            ifInactive.Start();
        }

        AuthorizationEmployee MainEmployee;
        int attemptCount = 0;
        System.Windows.Threading.DispatcherTimer timer;
        private void Send_click(object sender, RoutedEventArgs e)
        {
            if ((Password.Text == MainEmployee.Password) &&
                (Login.Text == MainEmployee.Login))
            {
                MessageBox.Show("Авторизация успешна!");
                CreateEmployeeWindow createEmployeeWindow = new CreateEmployeeWindow();
                this.Close();
                createEmployeeWindow.Show();
            }
            else
            {
                if (attemptCount == 2)
                {
                    attemptCount = 0;
                    MessageBox.Show("Превышено количество попыток авторизации!\n" +
                        "Повторите попытку по истечении одной минуты.");

                    Login.IsEnabled = false;
                    Password.IsEnabled = false;
                    Send.IsEnabled = false;

                    timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(10);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                }
                else
                {
                    attemptCount++;
                    MessageBox.Show($"Неверный логин или пароль!\n" +
                        $"Осталось попыток: {3 - attemptCount}");
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            Login.IsEnabled = true;
            Password.IsEnabled = true;
            Send.IsEnabled = true;
        }

        private void IfInactive_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Приложение не используется более 10 секунд.\n");
            ifInactive.Stop();
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ifInactive.Interval = TimeSpan.FromSeconds(10);
            ifInactive.Start();
        }
    }

    internal class AuthorizationEmployee
    {
        string login;
        string password;

        public AuthorizationEmployee() { }
        public AuthorizationEmployee(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
