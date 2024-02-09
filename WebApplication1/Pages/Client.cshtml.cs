using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Numerics;

namespace WebApplication1.Pages
{
    public class ClientModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();
        public int total { get; set; }

        public void OnGet()
        {
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
                string connectionString = configuration.GetConnectionString("DbConnection");

                //string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=demo;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string req = "SELECT c.customer_id, c.first_name, c.last_name, c.email, COUNT(o.order_id) AS OrderCount, o.order_status " +
             "FROM customers c " +
             "LEFT JOIN orders o ON c.customer_id = o.customer_id " +
             "GROUP BY c.customer_id, c.first_name, c.last_name, c.email , o.order_status " +
             "ORDER BY OrderCount DESC";


                    string reqCount = "select COUNT(*) AS TotalCustomers from customers";
                    using (SqlCommand cmd = new SqlCommand(req, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo client = new ClientInfo();
                                client.id = reader.GetInt32(0);
                                client.first_name = reader.GetString(1);
                                client.Last_name = reader.GetString(2);
                                client.email = reader.GetString(3);
                                client.OrderCount = reader.GetInt32(reader.GetOrdinal("OrderCount"));
                                //client.OrderStatus = reader.GetInt32(4);
                                int orderStatus = reader.GetInt32(4);

                                UpdateOrderStatusCounts(client,orderStatus);

                               // client.OrderStatus = orderStatus;
                                listClients.Add(client);

                            }
                        }
                    }
                    using (SqlCommand cmd = new SqlCommand(reqCount, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                total = reader.GetInt32(0);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateOrderStatusCounts(ClientInfo client, int orderStatus)
        {
            switch (orderStatus)
            {
                case 1:
                    client.RejectedCount++;
                    break;
                case 2:
                    client.PendingCount++;
                    break;
                case 3:
                    client.ProcessingCount++;
                    break;
                default:
                    client.CompletedCount++;
                    break;
            }
        }
    }

 
    public class ClientInfo()
    {
        public int id;
        public string first_name;
        public string Last_name;
        public string email;
        public int OrderCount;
        public int OrderStatus;
        public int RejectedCount = 0;
        public int PendingCount = 0;
        public int ProcessingCount = 0;
        public int CompletedCount = 0;
    }

}
