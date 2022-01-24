using Npgsql;
using System;
using System.Data.SqlClient;
namespace DrugStore
{
    class Program
    {
        static void Main(string[] args)
        {
            string exit = "y";
            while (exit!="n")
            {
                Console.WriteLine("Console Application DrugStore Company in C#\r");
                Console.WriteLine("-------------------------------------------\n");
                Console.WriteLine("Choose an option from the following list:");
                Console.WriteLine("\tp - Edit products");
                Console.WriteLine("\td - Edit drugtors");
                Console.WriteLine("\ts - Edit storages");
                Console.WriteLine("\tg - Edit goods");
                Console.WriteLine("\tl - Drugs list");
                Console.Write("Your option? ");
                string result = "";

                // Use a switch statement to do the math.
                switch (Console.ReadLine())
                {
                    case "p":
                        Console.WriteLine("Choose an option from the following list:");
                        Console.WriteLine("\tc - Create new product");
                        Console.WriteLine("\td - Delete product");
                        Console.Write("Your option? ");
                        result = dbConnection.EditProduct(Console.ReadLine());
                        break;
                    case "d":
                        Console.WriteLine("Choose an option from the following list:");
                        Console.WriteLine("\tc - Create new drugstore");
                        Console.WriteLine("\td - Delete drugstore");
                        Console.Write("Your option? ");
                        result = dbConnection.EditDrugstore(Console.ReadLine());
                        break;
                    case "s":
                        Console.WriteLine("Choose an option from the following list:");
                        Console.WriteLine("\tc - Create new storage");
                        Console.WriteLine("\td - Delete storage");
                        Console.Write("Your option? ");
                        result = dbConnection.EditStorage(Console.ReadLine());
                        break;
                    case "g":
                        Console.WriteLine("Choose an option from the following list:");
                        Console.WriteLine("\tc - Create new goods");
                        Console.WriteLine("\td - Delete goods");
                        Console.Write("Your option? ");
                        result = dbConnection.EditGoods(Console.ReadLine());
                        break;
                    case "l":
                        Console.WriteLine("Enter drugstore name");
                        result = dbConnection.InventoryInformation(Console.ReadLine());
                        break;
                    default:
                        Console.WriteLine("Incorrect value");
                        break;
                }
                Console.WriteLine(result);
                Console.WriteLine("Do you want to continue? (y/n)");
                exit = Console.ReadLine();
            }


        }

    }
    class dbConnection
    {
        private static void CreateCommand(string queryString,
    string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public static string EditProduct(string value)
        {
            string conn_param = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=1;Database=ds_database;";
            Console.Write("Enter product name: ");
            string product_name = Console.ReadLine();

            if (value == "c")
            {
                string sql = $"insert into products (product_name) values ('{product_name}')";
                NpgsqlConnection conn = new NpgsqlConnection(conn_param);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                conn.Open(); //Открываем соединение.
                try
                {
                    comm.ExecuteNonQuery();
                }
                catch (Exception PostgresExceptionx)
                {
                    return "The name alredy exist";
                }
                conn.Close();
            }
            else if (value == "d")
            {
                string sql = $"delete from goods where product_id in(select product_id from products where product_name = '{product_name}');delete from products where product_name = '{product_name}';";
                NpgsqlConnection conn = new NpgsqlConnection(conn_param);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                conn.Open(); //Открываем соединение.
                comm.ExecuteNonQuery();
                int count = comm.Statements.Count - 1;
                if (comm.Statements[count].Rows == 0)
                    return "Error";
                conn.Close();

            }
            else 
                return "Incorrect value";

            return "Success!";
        }


        public static string EditDrugstore(string value)
        {
            string conn_param = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=1;Database=ds_database;";
            Console.Write("Enter drugstore name: ");
            string drugstore_name = Console.ReadLine();

            if (value == "c")
            {
                Console.Write("Enter drugstore address: ");
                string drugstore_address = Console.ReadLine();
                Console.Write("Enter drugstore phone: ");
                string drugstore_phone = Console.ReadLine();
                string sql = $"insert into drugstore (ds_name, address, phone) values ('{drugstore_name}','{drugstore_address}','{drugstore_phone}')";
                NpgsqlConnection conn = new NpgsqlConnection(conn_param);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                conn.Open(); //Открываем соединение.
                try
                {
                    comm.ExecuteNonQuery();
                }
                catch (Exception PostgresExceptionx)
                {
                    return "The name alredy exist";
                }
                conn.Close();
            }
            else if (value == "d")
            {
                string sql = $"delete from goods where storage_id in" +
                    $"(select storage_id from storage where ds_id in " +
                    $"( select ds_id from drugstore where ds_name = '{drugstore_name}'));" +
                    $"delete from storage where ds_id in " +
                    $"( select ds_id from drugstore where ds_name = '{drugstore_name}');" +
                    $"delete from drugstore where ds_name = '{drugstore_name}'; ";
                NpgsqlConnection conn = new NpgsqlConnection(conn_param);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                conn.Open(); //Открываем соединение.
                comm.ExecuteNonQuery();
                int count = comm.Statements.Count - 1;
                if (comm.Statements[count].Rows == 0)
                    return "Error";
                conn.Close();

            }
            else return "Incorrect value";
            return "Success!";
        }
        public static string EditStorage(string value)
        {
            string conn_param = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=1;Database=ds_database;";
            Console.Write("Enter storage name: ");
            string storage_name = Console.ReadLine();

            if (value == "c")
            {
                Console.Write("Enter drugstore name: ");
                string drugstore_name = Console.ReadLine();
                string sql = $"insert into storage (ds_id, storage_name) values ((select ds_id from drugstore where ds_name = '{drugstore_name}'),'{storage_name}')";
                NpgsqlConnection conn = new NpgsqlConnection(conn_param);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                conn.Open(); //Открываем соединение.
                try
                {
                    comm.ExecuteNonQuery();
                }
                catch (Exception PostgresExceptionx)
                {
                    return "Incorrect value";
                }
                conn.Close();
            }
            else if (value == "d")
            {
                string sql =
                    $"delete from goods where storage_id in " +
                    $"(select storage_id from storage where storage_name = '{storage_name}');" +
                    $"delete from storage where storage_name = '{storage_name}'; ";
                NpgsqlConnection conn = new NpgsqlConnection(conn_param);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                conn.Open(); //Открываем соединение.
                comm.ExecuteNonQuery();
                int count = comm.Statements.Count - 1;
                if (comm.Statements[count].Rows == 0)
                    return "Storage not found";
                conn.Close();

            }
            else return "Incorrect value";
            return "Success!";
        }
        public static string EditGoods(string value)
        {
            string conn_param = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=1;Database=ds_database;";

            if (value == "c")
            {
                Console.Write("Enter product name: ");
                string product_name = Console.ReadLine();
                Console.Write("Enter storage name: ");
                string storage_name = Console.ReadLine();
                Console.Write("Enter order: ");
                string order = Console.ReadLine();
                string sql = 
                    $"insert into goods (product_id, storage_id, goods_count) " +
                    $"values ((select product_id from products where product_name = '{product_name}')," +
                    $"(select storage_id from storage where storage_name = '{storage_name}'), {order});";
                NpgsqlConnection conn = new NpgsqlConnection(conn_param);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                conn.Open(); //Открываем соединение.
                try
                {
                    comm.ExecuteNonQuery();
                }
                catch (Exception PostgresExceptionx)
                {
                    return "Incorrect value!";
                }
                conn.Close();
            }
            else if (value == "d")
            {
                Console.Write("Enter number of goods: ");
                string goods_id = Console.ReadLine();
                string sql = $"delete from goods where goods_id = {goods_id};";
                NpgsqlConnection conn = new NpgsqlConnection(conn_param);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                conn.Open(); //Открываем соединение.
                comm.ExecuteNonQuery();
                int count = comm.Statements.Count - 1;
                if (comm.Statements[count].Rows == 0)
                    return "Number not found";
                conn.Close();

            }
            else
                return "Incorrect value";

            return "Success!";
        }
        public static string InventoryInformation(string value)
        {
            string conn_param = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=1;Database=ds_database;";
            string sql =
                $"select  pd.product_name, sum(gd.goods_count)" +
                $"from goods gd " +
                $"inner join products pd on gd.product_id = pd.product_id " +
                $"inner join storage st on gd.storage_id = st.storage_id " +
                $"inner join drugstore ds on ds.ds_id = st.ds_id " +
                $"where ds.ds_name = '{value}' " +
                $"group by pd.product_name";
            NpgsqlConnection conn = new NpgsqlConnection(conn_param);
            NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
            conn.Open();
            NpgsqlDataReader reader;
            reader = comm.ExecuteReader();
            int count = 0;
           // if (comm.Statements[count].Rows != 0)
                while (reader.Read())
                {
                    count++;
                    Console.Write(reader.GetString(0));
                    Console.Write("...");
                    Console.WriteLine(reader.GetString(1));
                }
                if (count == 0) return "Drugstore not found";
            //else
            //    return "Drugstore not found";

            conn.Close();
            return "";

        }

    }
}
