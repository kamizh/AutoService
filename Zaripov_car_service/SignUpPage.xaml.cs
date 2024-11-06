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

namespace Zaripov_car_service
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;
            DataContext = _currentService;

            var _currentClient = ZaripovAutoserviceEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }


        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if (TBStart.Text.Length != 5)
                errors.AppendLine("Укажмте коректное время!");
            int time = Convert.ToInt32(TBStart.Text[0] + TBStart.Text[1]);
            if(time > 23 || time < 0)
            {
                errors.AppendLine("Время должно быть с 0 до 23");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);
            if (_currentClientService.ID == 0)
                ZaripovAutoserviceEntities.GetContext().ClientService.Add(_currentClientService);
            try
            {
               ZaripovAutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string start = TBStart.Text;
            string end = TBEnd.Text;
            if (start.Length == 0)
                TBEnd.Text = "";
            if(start.Length == 5) 
            {
                int time = _currentService.DurationInSeconds;
                int hour = time / 60;
                int minute = time % 60;
                string str = start[0].ToString();
                str += start[1];
                hour = Convert.ToInt32(str) + hour;
                str = start[3].ToString();
                str += start[4];
                minute = Convert.ToInt32(str) + minute;
                if(minute >= 60)
                {
                    hour++;
                    minute = minute - 60;
                }
                TBEnd.Text = hour.ToString() + ":" + minute.ToString();
                if (TBEnd.Text.Length <= 4)
                    TBEnd.Text += '0';
            }
        }
    }
}