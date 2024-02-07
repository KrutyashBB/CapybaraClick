using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;


namespace CapybaraClicker
{
    public class DataBase
    {
        static int sumMoney;
        static int coinsPerSecond;
        static int coinsPerClick;

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

        public static void SelectMoneyData()
        {
            var sqlExpression = "SELECT * FROM MoneyData";
            using (var connection = new SQLiteConnection("Data Source=CapybaraClickerDB.db"))
            {
                connection.Open();

                var command = new SQLiteCommand(sqlExpression, connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            sumMoney = reader.GetInt32(1);
                            coinsPerSecond = reader.GetInt32(2);
                            coinsPerClick = reader.GetInt32(3);
                        }
                    }
                }
            }
        }

        public static int SelectSumMoney() => sumMoney;

        public static int SelectCoinsPerSecond() => coinsPerSecond;

        public static int SelectCoinsPerClick() => coinsPerClick;

        public static void UpdateMoneyDataInDB(long sumMoney, long coinsPerSecond, long coinsPerClick)
        {
            var sqlExpression =
                $"UPDATE MoneyData SET SumMoney={sumMoney}, CoinsPerSecond={coinsPerSecond}, CoinsPerClick={coinsPerClick} WHERE Id=1";
            using (var connection = new SQLiteConnection("Data Source=CapybaraClickerDB.db"))
            {
                connection.Open();

                var command = new SQLiteCommand(sqlExpression, connection);

                command.ExecuteNonQuery();
            }
        }
        
        public static List<Capybara> SelectCapybaras()
    {
        var capybarasList = new List<Capybara>();
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

                        capybarasList.Add(new Capybara(Image.FromFile(imgPath), cost, isBuy));
                    }
                }

                return capybarasList;
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

        public static List<Modification> SelectModifications()
        {
            var modificationsList = new List<Modification>();
            var sqlExpression = "SELECT * FROM Modifications";
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
                            var iconPath = reader.GetString(1);
                            var bigModifLabel = reader.GetString(2);
                            var smallModifLabel = reader.GetString(3);
                            var cost = reader.GetInt32(4);
                            var bonus = reader.GetInt32(5);
                            var typeOfMadif =
                                (TypesOfModifications)Enum.Parse(typeof(TypesOfModifications), reader.GetString(6));
                            var countOfPurchase = reader.GetInt32(7);

                            modificationsList.Add(new Modification(Image.FromFile(iconPath), bigModifLabel,
                                smallModifLabel, cost, bonus, countOfPurchase, typeOfMadif));
                        }
                    }

                    return modificationsList;
                }
            }
        }

        public static void UpdatePurchaseStateModification(Modification modification)
        {
            var sqlExpression =
                $"UPDATE Modifications SET NumberOfPurchases={modification.NumberOfPurchase} WHERE Cost={modification.Cost}";
            using (var connection = new SQLiteConnection("Data Source=CapybaraClickerDB.db"))
            {
                connection.Open();

                var command = new SQLiteCommand(sqlExpression, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
