using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static Mid.App;
using System.Data.SqlClient;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Mid
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            InventoryList.ItemsSource = GetCustomers((App.Current as App).ConnectionString);
        }
        private ObservableCollection<Customers> GetCustomers(string connectionString)
        {
            const string GetCustomersQuery = "select CustomerID, CustomerName, Address, Phone" +
               " from Customers";

            var Customers = new ObservableCollection<Customers>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetCustomersQuery;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var customers  = new Customers();
                                    customers.CustomerID = reader.GetInt32(0);
                                    customers.CustomerName = reader.GetString(1);
                                    customers.Address = reader.GetString(2);
                                    customers.Phone = reader.GetInt32(3);
                                    Customers.Add(customers);
                                }
                            }
                        }
                    }
                }
                return Customers;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }
    }
}
