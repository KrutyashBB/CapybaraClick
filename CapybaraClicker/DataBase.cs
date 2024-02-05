using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;


namespace CapybaraClicker
{
    public class DataBase
    {
        public static void CreateTable()
        {
            var connectionString = "Data Source=CapybaraClickerDB.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var sql =
                    "CREATE TABLE IF NOT EXISTS Capybaras (Id INTEGER PRIMARY KEY, Name TEXT, Cost INTEGER, isBuy INTEGER)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public static List<Capybara> SelectCapybaras()
        {
            var _capybarasList = new List<Capybara>();
            var sqlExpression = "SELECT * FROM Capybaras";
            using (var connection = new SQLiteConnection("Data Source=CapybaraClickerDB.db"))
            {
                connection.Open();

                var command = new SQLiteCommand(sqlExpression, connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            var id = reader.GetInt32(0);
                            var imgPath = reader.GetString(1);
                            var cost = reader.GetInt32(2);
                            var isBuy = reader.GetBoolean(3);

                            _capybarasList.Add(new Capybara(Image.FromFile(imgPath), cost, isBuy));
                        }
                    }

                    return _capybarasList;
                }
            }
        }

        public static void UpdatePurchaseStateCapybara(Capybara capybara)
        {
            var sqlExpression = $"UPDATE Capybaras SET isBuy=1 WHERE Cost={capybara.Cost}";
            using (var connection = new SQLiteConnection("Data Source=CapybaraClickerDB.db"))
            {
                connection.Open();

                var command = new SQLiteCommand(sqlExpression, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}