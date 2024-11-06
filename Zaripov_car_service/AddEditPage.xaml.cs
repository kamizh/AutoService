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

namespace Zaripov_car_service
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {

        private Service _currentServise = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                _currentServise = SelectedService;
            DataContext = _currentServise;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentServise.Title))
                errors.AppendLine("Укажите название услуги");
            if (_currentServise.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");
            if (_currentServise.DiscountInt == null)
                errors.AppendLine("Укажите скидку");
            if (_currentServise.DiscountInt > 100)
                errors.AppendLine("Скидка не может быть больше 100");
            if (_currentServise.DiscountInt < 0)
                errors.AppendLine("Скидка не может быть отрицательной! ");
            if (string.IsNullOrWhiteSpace(_currentServise.DurationInSeconds.ToString()))
                errors.AppendLine("Укажите длительность услуги");
            int seconds = Convert.ToInt32(_currentServise.DurationInSeconds.ToString());
            if (seconds <= 0 || seconds > 240)
            {
                errors.AppendLine("Длительность не может быть больше 240 минут или меньше 0");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var allServices = ZaripovAutoserviceEntities.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentServise.Title).ToList();
            if(allServices.Count == 0)
            {
                if(_currentServise.ID == 0)
                    ZaripovAutoserviceEntities.GetContext().Service.Add(_currentServise);
                try
                {
                    ZaripovAutoserviceEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString()); 
                }
            }
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }

            if (_currentServise.ID == 0)
                ZaripovAutoserviceEntities.GetContext().Service.Add(_currentServise);
            try
            {
                ZaripovAutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
    }
}
