using System;
using System.Collections.Generic;
using System.IO;
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
using TestIgnatJunior.DataFolder;
using CsvHelper;

namespace TestIgnatJunior
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddOrganization(string name, string inn, string legalAddress, string actualAddress)
        {
            try
            {
                var organization = new Organizations
                {
                    Name = name,
                    INN = inn,
                    LegalAddress = legalAddress,
                    ActualAddress = actualAddress
                };

                DBEntities context = DBEntities.GetContext();
                context.Organizations.Add(organization);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddEmployee(string lastName, string firstName, string middleName, DateTime birthDate, string passportSeries, string passportNumber)
        {
            try
            {
                var employee = new Employees
                {
                    LastName = lastName,
                    FirstName = firstName,
                    MiddleName = middleName,
                    BirthDate = birthDate,
                    PassportSeries = passportSeries,
                    PassportNumber = passportNumber
                };

                DBEntities context = DBEntities.GetContext();
                context.Employees.Add(employee);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddOrgan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddOrganization(NameOrg.Text, INN.Text, YrAdress.Text, FactAdress.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddEmployee(name.Text, Surname.Text, Therdname.Text, (DateTime)date.SelectedDate, series.Text, number.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ExportToCsv(string filePath, List<Organizations> organizations)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(organizations);
            }
        }
        public void ExportToCsv(string filePath, List<Employees> organizations)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(organizations);
            }
        }

        public List<Organizations> ImportFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Organizations>().ToList();
                return records;
            }
        }

        public void ExportEmployeesToCsv(string filePath, List<Employees> employees)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(employees);
            }
        }

        public List<Employees> ImportEmployeesFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Employees>().ToList();
                return records;
            }
        }

        private void ExportEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Employees";
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV documents (.csv)|*.csv";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;
                    DBEntities context = DBEntities.GetContext();
                    var employees = context.Employees.ToList();
                    ExportEmployeesToCsv(filename, employees);
                    MessageBox.Show("Успешно.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ImportEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV documents (.csv)|*.csv";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;
                    var employees = ImportEmployeesFromCsv(filename);
                    DBEntities context = DBEntities.GetContext();
                    context.Employees.AddRange(employees);
                    context.SaveChanges();
                    MessageBox.Show("успешно.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExportOrg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Organizations";
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV documents (.csv)|*.csv";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;
                    DBEntities context = DBEntities.GetContext();
                    var organizations = context.Organizations.ToList();
                    ExportToCsv(filename, organizations);
                    MessageBox.Show("Успешный экспорт.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ImportOrg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV documents (.csv)|*.csv";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;
                    var organizations = ImportFromCsv(filename);
                    DBEntities context = DBEntities.GetContext();
                    context.Organizations.AddRange(organizations);
                    context.SaveChanges();
                    MessageBox.Show("Успешный импорт.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
