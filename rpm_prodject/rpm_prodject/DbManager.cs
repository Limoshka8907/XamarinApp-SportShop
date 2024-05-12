using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace rpm_prodject
{
    class DbManager
    {

        string srvrdbname = "rpm";
        string srvrname = "192.168.1.96";
        string srvrusername = "samir";
        string srvrpassword = "123456";
        public static string user_login = "";
        int user_id = 0;
        string order_id = "";
        public static string user_name = "";

        public void TestConnection()
        {
            try
            {

                string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
                SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;
            }
        }
        public void InsertOrdersTableNonItems()
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                DateTime time = DateTime.Now; 
                string format = "yyyy-MM-dd HH:mm:ss"; 
                string sqlFormattedDate = time.ToString(format);
                SelectUserId();
                sqlCommand.CommandText = "INSERT INTO Orders (user_id, order_date, full_price) VALUES (@UserId, @Date, @Price)";
                sqlCommand.Parameters.AddWithValue("@UserId", user_id.ToString());
                sqlCommand.Parameters.AddWithValue("@Date", sqlFormattedDate);
                sqlCommand.Parameters.AddWithValue("@Price", Busket.cost - (Busket.cost / 100 * 10));

                sqlCommand.ExecuteNonQuery();
            }
        }
        public void InsertOrders(Product product, string price, int count, string color, string size)
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                if(size == null || size == "")
                {
                    size = "\t";
                }

                SelectOrder();
                sqlCommand.CommandText = "INSERT INTO OrderItem (order_id, goods_id, quantity, color_hex, size) VALUES (@OrderId, @GoodId, @Count, @Color, @Size)";
                sqlCommand.Parameters.AddWithValue("@OrderId", order_id);
                sqlCommand.Parameters.AddWithValue("@GoodId", product.Id);
                sqlCommand.Parameters.AddWithValue("@Count", count.ToString());
                sqlCommand.Parameters.AddWithValue("@Color", color);
                sqlCommand.Parameters.AddWithValue("@Size", size);

                sqlCommand.ExecuteNonQuery();

            }
        }

        public void SelectOrder() 
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";

            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;

                    sqlCommand.CommandText = "SELECT order_id FROM Orders WHERE user_id = @Uid";
                    sqlCommand.Parameters.AddWithValue("@Uid", user_id.ToString() );
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order_id = reader["order_id"].ToString();
                        }

                    }
                }
            }
        }
        
        public string SelectUserId()
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";

            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = "SELECT user_id FROM UsersNotAdmins WHERE login = @Login";
                    sqlCommand.Parameters.AddWithValue("@Login", user_login);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user_id = Convert.ToInt32(reader["user_id"]);
                            return user_id.ToString();
                        }
                    }
                }
                return user_id.ToString();

            }
        }

        public string SelectUserName()
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";

            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = "SELECT user_name FROM UsersNotAdmins WHERE login = @Login";
                    sqlCommand.Parameters.AddWithValue("@Login", user_login);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user_name = (reader["user_name"]).ToString();
                            return user_name.ToString();
                        }
                    }
                }
                return user_name.ToString();

            }
        }

        public void SelectGoods()
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";

            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;

                    sqlCommand.CommandText = "SELECT * FROM Goods";
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                        }

                    }
                }
            }
        }
        public bool SelectUsersNotAdmins(string login, string password)
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";

            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;  
                    sqlCommand.CommandText = "SELECT * FROM UsersNotAdmins WHERE user_password = @UserPassword AND login = @Login";
                    sqlCommand.Parameters.AddWithValue("@UserPassword", password);
                    sqlCommand.Parameters.AddWithValue("@Login", login);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            user_id = Convert.ToInt32(reader["user_id"]);
                            string user_name = reader["user_name"].ToString();
                            string login_user = reader["login"].ToString();
                            string user_password = reader["user_password"].ToString();

                            if (user_id != 0 && user_password != null && login_user == login && password == user_password)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;


        }

        public void InsertIntoUsersNotAdmins(string user_name, string login, string user_password)
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.CommandText = "INSERT INTO UsersNotAdmins (user_name, login, user_password) VALUES (@UserName, @Login, @UserPassword)";
                sqlCommand.Parameters.AddWithValue("@UserName", user_name);
                sqlCommand.Parameters.AddWithValue("@Login", login);
                sqlCommand.Parameters.AddWithValue("@UserPassword", user_password);

                sqlCommand.ExecuteNonQuery();
            }
        }

        public bool EmailExists(string email)
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.CommandText = "select login FROM UsersNotAdmins WHERE login =  (@Login)";
                sqlCommand.Parameters.AddWithValue("@Login", email);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["login"].ToString().Length != 0) { return false; }
                    }
                    return true;
                }
            }
        }

        public void UpdateUser(string name, string password)
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.CommandText = "UPDATE UsersNotAdmins SET user_name = @userName, user_password = @userPassword WHERE user_id = @userId;";
                sqlCommand.Parameters.AddWithValue("@userName", name);
                sqlCommand.Parameters.AddWithValue("@userPassword", password);
                sqlCommand.Parameters.AddWithValue("@userId", SelectUserId());
                sqlCommand.ExecuteNonQuery();
            }
        }

        public string SelectCountGood(string goodId)
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.CommandText = "select goods_count from Goods WHERE goods_id = @GoodId";
                sqlCommand.Parameters.AddWithValue("@GoodId", goodId);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string count = reader["goods_count"].ToString();
                        return count;
                    }
                    return "0";
                }
            }
        }
        public void UpdateGoodCount(string count, string goodId)
        {
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.CommandText = "UPDATE Goods SET goods_count = @GoodCount WHERE goods_id = @GoodId";
                sqlCommand.Parameters.AddWithValue("@GoodCount", count);
                sqlCommand.Parameters.AddWithValue("@GoodId", goodId);
                sqlCommand.ExecuteNonQuery();

            }
        }
    }
}
