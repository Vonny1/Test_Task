using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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
using WPF_TaskManager.Models;
using Newtonsoft.Json;
using WPF_TaskManager.Commands;
using System.Net.Http;

namespace WPF_TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Job> listAll = new List<Job>();
        List<Job> listMains = new List<Job>();
        List<Status> listStatus = new List<Status>();

        Job selectedJob = new Job(); //Переменная для хранения выбранной задачи из TreeView
        Job newJob = new Job();//Новая создаваемая задача
        int selectedJobId;
        public MainWindow()
        {
            InitializeComponent();
            gridNewJob.Visibility = Visibility.Collapsed;

            listAll = GetAllJobs(); // Получаем список задач с сервера
            Status appointed = new Status { Id = 1, Name = "Appointed" };
            Status inprogress = new Status { Id = 2, Name = "InProgress" };
            Status stopped = new Status { Id = 3, Name = "Stopped" };
            Status completed = new Status { Id = 4, Name = "Completed" };
            List<Status> statuses = new List<Status> { appointed, inprogress, stopped, completed };

            //Список задач для TreeView
            listMains = listAll
                .Where(x => x.ParentId == null)
                .ToList();

            treeJobs2.ItemsSource = listMains;



        }

        public List<Job> GetAllJobs() // Функция на получение всех задач с сервера
        {
            //GET запрос к серверу
            string responseText;
            WebRequest request = WebRequest.Create("https://localhost:44301/job/getall"); 
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            response.Close();

            //Десереализация JSON в List<Job>
            List<Job> listJobsAll = JsonConvert.DeserializeObject<List<Job>>(responseText);
            return listJobsAll;

        }

        private void treeJobs2_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) //Событие смены выбранной задачи в TreeView
        {
            gridSelectedItem.Visibility = Visibility.Visible;
            buttonCompleted.Visibility = Visibility.Collapsed;
            buttonInProgress.Visibility = Visibility.Collapsed;
            buttonStopped.Visibility = Visibility.Collapsed;
            gridNewJob.Visibility = Visibility.Collapsed;
            gridDelete.Visibility = Visibility.Collapsed;
            buttonGridDelete.Visibility = Visibility.Visible;

            //Записываем в selectedJob задачу из treeView
            selectedJobId = Convert.ToInt32(globalSelectedItemId.Text);
            selectedJob = listAll
                .Where(x => x.Id == Convert.ToInt32(selectedJobId))
                .FirstOrDefault();
            
            string jsonJob = JsonConvert.SerializeObject(selectedJob);
            jsonSelItem.Text = jsonJob;
            //В grid появляется выбранная задача selectedJob
            gridSelectedItem.DataContext = selectedJob;
            //Вывод даты в привычный формат день/месяц/год
            dateString.Text = selectedJob.RegDate?.ToString();

            //В зависимости от статуса задачи меняется цвет фона и появляются нужные кнопки
            switch (selectedJob.StatusId)
            {
                case 1:
                    gridSelectedItem.Background = new SolidColorBrush(Color.FromRgb(230, 230, 255));
                    buttonInProgress.Visibility = Visibility.Visible;
                    break;
                case 2:
                    gridSelectedItem.Background = new SolidColorBrush(Color.FromRgb(179, 179, 255));
                    buttonStopped.Visibility = Visibility.Visible;
                    buttonCompleted.Visibility = Visibility.Visible;
                    break;
                case 3:
                    gridSelectedItem.Background = new SolidColorBrush(Color.FromRgb(255, 255, 128));
                    buttonInProgress.Visibility = Visibility.Visible;
                    buttonCompleted.Visibility = Visibility.Visible;
                    break;
                case 4:
                    gridSelectedItem.Background = new SolidColorBrush(Color.FromRgb(153, 255, 102));
                    break;
            }

           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //Кнопка добавления подзадачи
        {
            //newJob(подзадача) привязывается к выбранно задаче
            gridNewJob.Visibility = Visibility.Visible;
            newJob.ParentId = selectedJob.Id;
            newJobParentIdText.Text = newJob.ParentId.ToString();


        }

        private void closeGridNewJob_Click(object sender, RoutedEventArgs e) //Кнопка закрытия Grid
        {
            gridNewJob.Visibility = Visibility.Collapsed;
        }

        private async void  addNewJob_Click(object sender, RoutedEventArgs e) //Кнопка отправки созданной задачи на сервер 
        {

            if (newJobNameText.Text == "" || newJobDescriptionText.Text == "" || newJobPlannedTimeMinText.Text == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                try
                {
                    newJob = new Job
                    {
                        ParentId = selectedJob.Id,
                        Name = newJobNameText.Text,
                        Description = newJobDescriptionText.Text,
                        Asignee = newJobAsigneeText.Text,
                        StatusId = 1,
                        PlannedTimeMin = Convert.ToInt32(newJobPlannedTimeMinText.Text),
                        RegDate = DateTime.Now
                    };
                    string jsonNewJob;
                    string jsonNewJob1;
                    string jsonNewJob2;
                    if (newJob.ParentId!=0)
                    {
                        jsonNewJob = String.Format(@" ""ParentId"": {0},  ""Name"": ""{1}"", ""Description"":""{2}"", ""Asignee"":""{3}"", ""StatusId"":{4}, ""PlannedTimeMin"":{5}, ""Regdate"":""{6}""",
    newJob.ParentId, newJob.Name, newJob.Description, newJob.Asignee, newJob.StatusId, newJob.PlannedTimeMin, newJob.RegDate);
                        jsonNewJob1 = "{" + jsonNewJob;
                        jsonNewJob2 = jsonNewJob1 + "}";

                    }
                    else
                    {
                        jsonNewJob = String.Format(@" ""ParentId"": {0},  ""Name"": ""{1}"", ""Description"":""{2}"", ""Asignee"":""{3}"", ""StatusId"":{4}, ""PlannedTimeMin"":{5}, ""Regdate"":""{6}""",
    "null", newJob.Name, newJob.Description, newJob.Asignee, newJob.StatusId, newJob.PlannedTimeMin, newJob.RegDate);
                        jsonNewJob1 = "{" + jsonNewJob;
                        jsonNewJob2 = jsonNewJob1 + "}";

                    }

                    jsonSelItem.Text = jsonNewJob2;
                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(
                            "https://localhost:44301/job/create",
                             new StringContent(jsonNewJob2, Encoding.UTF8, "application/json"));
                    }

                    MessageBox.Show("Задача создана!");
                    gridNewJob.Visibility = Visibility.Collapsed;
                    //Обновление списка задач с сервера
                    listAll = GetAllJobs();
                    listMains = listAll
            .Where(x => x.ParentId == null)
            .ToList();

                    treeJobs2.ItemsSource = listMains;
                }
                catch
                {
                    MessageBox.Show("Некорректно заполнены поля");
                }
            }
        }

        private void addNewMainJob_Click(object sender, RoutedEventArgs e) //Кнопка добавления главной задачи(не подзадачи)
        {
            //Открывает grid
            gridSelectedItem.Visibility = Visibility.Collapsed;
            selectedJob = new Job();
            newJobParentIdText.Text = selectedJob.Id.ToString();
            string test = JsonConvert.SerializeObject(selectedJob);
            jsonSelItem.Text = test;
            gridNewJob.Visibility = Visibility.Visible;

        }



        private void buttonGridDelete_Click(object sender, RoutedEventArgs e) //Кнопка удаления задачи
        {
            //Открывает grid и заносит в DataContext выбранную задачу из дерева
            gridDelete.Visibility = Visibility.Visible;
            gridDelete.DataContext = selectedJob;
        }

        private void buttonConfirmDelete_Click(object sender, RoutedEventArgs e) //Кнопка отправки запроса удаления выбранной задачи на сервер
        {
            //Удаляется через GET запрос по ID
            WebRequest request = WebRequest.Create($"https://localhost:44301/job/delete/{selectedJob.Id}");
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string a = reader.ReadToEnd();
                }
            }
            response.Close();
            gridDelete.Visibility = Visibility.Collapsed;

            //Обновление списка задач с сервера
            listAll = GetAllJobs();
            listMains = listAll
    .Where(x => x.ParentId == null)
    .ToList();

            treeJobs2.ItemsSource = listMains;
        }

        private void Button_Click(object sender, RoutedEventArgs e) //Кнопка НЕТ в окошке удаления задачи
        {
            gridDelete.Visibility = Visibility.Collapsed;
        }

        private async void buttonCompleted_Click(object sender, RoutedEventArgs e) //Кнопка смены статуса задачи на "Выполнена", отправка запроса на сервер
        {
            selectedJob.StatusId = 4;
            selectedJob.CompleteDate = DateTime.Now;
            string jsonJob = JsonConvert.SerializeObject(selectedJob);
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    "https://localhost:44301/job/edit",
                     new StringContent(jsonJob, Encoding.UTF8, "application/json"));
            }
            //Обновление списка задач с сервера
            gridNewJob.Visibility = Visibility.Collapsed;
            listAll = GetAllJobs();
            listMains = listAll
    .Where(x => x.ParentId == null)
    .ToList();
            treeJobs2.ItemsSource = listMains;
        }

        private async void buttonInProgress_Click(object sender, RoutedEventArgs e)//Кнопка смены статуса задачи на "В работе", отправка запроса на сервер
        {
            selectedJob.StatusId = 2;
            string jsonJob = JsonConvert.SerializeObject(selectedJob);
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    "https://localhost:44301/job/edit",
                     new StringContent(jsonJob, Encoding.UTF8, "application/json"));
            }

            //Обновление списка задач с сервера
            gridNewJob.Visibility = Visibility.Collapsed;
            listAll = GetAllJobs();
            listMains = listAll
    .Where(x => x.ParentId == null)
    .ToList();

            treeJobs2.ItemsSource = listMains;

        }

        private async void buttonStopped_Click(object sender, RoutedEventArgs e) //Кнопка смены статуса задачи на "Приостановлено", отправка запроса на сервер
        {
            selectedJob.StatusId = 3;
            string jsonJob = JsonConvert.SerializeObject(selectedJob);
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    "https://localhost:44301/job/edit",
                     new StringContent(jsonJob, Encoding.UTF8, "application/json"));
            }

            //Обновление списка задач с сервера
            gridNewJob.Visibility = Visibility.Collapsed;
            listAll = GetAllJobs();
            listMains = listAll
    .Where(x => x.ParentId == null)
    .ToList();

            treeJobs2.ItemsSource = listMains;


        }
    }
}
