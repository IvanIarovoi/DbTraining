using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DbTraining
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=localhost;Initial Catalog=UsersDB;Integrated Security=True";

            InsertNewUser(connectionString);
            ViewUsers(connectionString);

            Console.Read();
        }

        static void InsertNewUser(string connectionString)
        {
            Console.Write("Введите имя добавляемого пользователя: ");
            string name = Console.ReadLine();

            int age;
            bool success = false;
            Console.Write("Введите возраст добавляемого пользователя: ");
            do
            {
                success = int.TryParse(Console.ReadLine(), out age);
                if(!success) Console.Write("Возраст введён некорректно, повторите ввод: ");
            } while (!success);
            
            string sqlExpression = string.Format("INSERT INTO Users (Name, Age) VALUES ('{0}', {1})", name, age);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Добавлено объектов: {0}", number);
            }
        }

        static void ViewUsers(string connectionString)
        {
            string sqlExpression = "SELECT * FROM Users";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object id = reader.GetValue(0);
                        object name = reader.GetValue(1);
                        object age = reader.GetValue(2);

                        Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
                    }
                }

                reader.Close();
            }

            Console.Read();
        }
    }
}
