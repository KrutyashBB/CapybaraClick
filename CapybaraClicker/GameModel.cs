using System;
using System.Collections.Generic;
using System.Linq;
using CapybaraClicker.Properties;

namespace CapybaraClicker
{
    public class GameModel
    {
        public long SumMoney { get; private set; } = DataBase.SelectSumMoney();
        public long CoinsPerSecond { get; private set; } = DataBase.SelectCoinsPerSecond();
        public long CoinsPerClick { get; private set; } = DataBase.SelectCoinsPerClick();
        public bool Is2X { get; private set; }

        public List<Capybara> _capybarasList { get; private set; } = DataBase.SelectCapybaras();

        public List<Modification> _modificationsList { get; private set; } = DataBase.SelectModifications();


        public void AddingCoinsPerSecond()
        {
            SumMoney += CoinsPerSecond;
        }

        public void Change2XStatus(bool is2X)
        {
            Is2X = is2X;
        }

        public void AddingCoinsPerClick()
        {
            if (Is2X)
                SumMoney += CoinsPerClick * 2;
            else
                SumMoney += CoinsPerClick;
        }

        public Capybara BuyNewCapybara(int capybaraCost)
        {
            SumMoney -= capybaraCost;
            return _capybarasList.Find(capybara => capybara.Cost == capybaraCost);
        }

        public Modification BuyNewModification(int costModification)
        {
            SumMoney -= costModification;
            return _modificationsList.FirstOrDefault(modification => modification.Cost == costModification);
        }


        public void ChangeCoinsPerSecond(int bonus)
        {
            CoinsPerSecond += bonus;
        }

        public void ChangeCoinsPerClick(int bonus)
        {
            CoinsPerClick += bonus;
        }
    }
}