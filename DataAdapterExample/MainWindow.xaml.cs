using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace DataAdapterExample
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Задание: добавить вторую вкладку в приложение, в которой будет присутствовать
        //поле для ввода запроса и ещё один DataGrid для вывода результата с кнопкой Fill.

        // вспомогательная процедура, создающая подключение к БД
        private SqlConnection GetDbConnection()
        {
            // обработка исключений будет выполняться выше по стеку
            string connectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            // connection.Open(); - в отсоединенном режиме подключение открывает и закрывает DbDataAdapter
            return connection;
        }

        // вспомогательный метод обновления данных
        private void fillData()
        {
            // вытягивание данных из БД согласно запросу
            dataSet = new DataSet();    // в этот DataSet будет выполняться запись результата вытягивания
            dataAdapter.Fill(dataSet, "game_t");  // выгружаем данные из БД и сохраняем в DataSet
            dataGrid.ItemsSource = dataSet.Tables["game_t"].DefaultView;
        }

        public MainWindow()
        {
            InitializeComponent();
            // подготовка объектов
            dataAdapter = new SqlDataAdapter(fillCmd, GetDbConnection());
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);  // строка заполняет INSERT/UPDATE/DELETE-команды
            // реализацией по умолчанию
        }

        // вспомогательные поля
        private SqlDataAdapter dataAdapter;
        private DataSet dataSet;
        private string fillCmd = "SELECT * FROM game_t;";

        private void fillBtn_Click(object sender, RoutedEventArgs e)
        {
            fillData();
        }

        private void customFillBtn_Click(object sender, RoutedEventArgs e)
        {
            // вытягивание данных из БД согласно запросу
            dataSet = new DataSet();    // в этот DataSet будет выполняться запись результата вытягивания
            fillCmd = TextBoxQuery.Text;
            dataAdapter = new SqlDataAdapter($"{fillCmd}", GetDbConnection());
            dataAdapter.Fill(dataSet, "game_t");  // выгружаем данные из БД и сохраняем в DataSet
            dataGrid.ItemsSource = dataSet.Tables["game_t"].DefaultView;
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            dataAdapter.Update(dataSet, "game_t");
            fillData();
        }
    }
}
