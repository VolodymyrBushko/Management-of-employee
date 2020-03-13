using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VolodWPF.Classes;

namespace VolodWPF
{
    public partial class MainWindow : Window
    {
        private String path;
        private Company company;
        private delegate void FindBy();

        public MainWindow()
        {
            InitializeComponent();

            path = @"D:\_FILEXML.xml";
            company = Company.Deserializer(path);
            listWorkers.ItemsSource = company.workers;
            var uri = new Uri("LightStyle.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            this.Closed += MainWindow_Closed;

            MessageBox.Show("Як користуватися ComboBox для пошуку:\n" +
                "Доприкладу, якщо треба знайти імя, ми в поле ComboBox\n" +
                "Вводимо це імя і тоді вибираємо пункт імя, а якщо треба\n" +
                "Показати все, то просто вибираэмо пункт Show All." +
                "Файл створюється за шляхом: D:\\\\_FILEXML.xml");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Company.Serializer(company, path);
        }

        private void WorkerAge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private Boolean IsSetAllTextBoxes()
        {
            Boolean flag = true;
            foreach (var item in this.mainGrid.Children)
            {
                if (item is TextBox && string.IsNullOrWhiteSpace((item as TextBox).Text))
                    flag = false;
            }
            return flag;
        }

        private void ClearAllTextBox()
        {
            foreach (var item in this.mainGrid.Children)
                if (item is TextBox)
                    (item as TextBox).Text = string.Empty;
        }

        private void WorkerAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsSetAllTextBoxes())
            {
                company.workers.Add(new Worker(
                    workerName.Text,
                    workerSurname.Text,
                    workerAge.Text));

                ClearAllTextBox();
            }
        }

        private void WorkerDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listWorkers.SelectedItem != null)
                company.workers.Remove(listWorkers.SelectedItem as Worker);
            else
                MessageBox.Show("SelectedItem == null");
        }

        private void EditWorker_Click(object sender, RoutedEventArgs e)
        {
            if (listWorkers.SelectedItem != null && IsSetAllTextBoxes())
            {
                company.workers.Remove(listWorkers.SelectedItem as Worker);
                company.workers.Add(new Worker(
                    workerName.Text,
                    workerSurname.Text,
                    workerAge.Text));

                ClearAllTextBox();
            }
            else
                MessageBox.Show("SelectedItem == null or IsSetAllTextBoxes() == false");
        }

        private void MakeFind(object sender, MouseButtonEventArgs e)
        {
            FindBy findBy = null;
            switch ((sender as ComboBoxItem).Name)
            {
                case "findName":
                    findBy = FindName;
                    break;
                case "findSurname":
                    findBy = FindSurname;
                    break;
                case "findAge":
                    findBy = FindAge;
                    break;
            }
            listWorkers.ItemsSource = null;
            listWorkers.Items.Clear();
            findBy();
            if (listWorkers.Items.IsEmpty)
                listWorkers.Items.Add("Not found");
        }

        private void ShowAll_Selected(object sender, MouseButtonEventArgs e)
        {
            try
            {
                listWorkers.Items.Clear();
                listWorkers.ItemsSource = company.workers;
            }
            catch { }
        }

        private void FindName()
        {
            foreach (var item in company.workers)
                if (item.Name.Equals(findWorkers.Text))
                    listWorkers.Items.Add(item);
        }

        private void FindSurname()
        {
            foreach (var item in company.workers)
                if (item.Surname.Equals(findWorkers.Text))
                    listWorkers.Items.Add(item);
        }

        private void FindAge()
        {
            foreach (var item in company.workers)
                if (item.Age.Equals(findWorkers.Text))
                    listWorkers.Items.Add(item);
        }

        private void StylesButton_Click(object sender, RoutedEventArgs e)
        {
            string style, lastStyle;
            if (stylesButton.Content.Equals("Dark"))
            {
                stylesButton.Content = "Light";
                style = "DarkStyle.xaml";
                lastStyle = "LightStyle.xaml";
            }
            else
            {
                stylesButton.Content = "Dark";
                style = "LightStyle.xaml";
                lastStyle = "DarkStyle.xaml";
            }
            var uri = new Uri(style, UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Remove(new Uri(lastStyle, UriKind.Relative));
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}