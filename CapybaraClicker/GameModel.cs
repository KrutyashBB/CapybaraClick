using System;
using System.Collections.Generic;
using System.Linq;
using CapybaraClicker.Properties;

namespace CapybaraClicker
{
    public class GameModel
    {
        public long SumMoney { get; private set; } = 50;
        public bool Is2X { get; private set; }
        public long CoinsPerSecond { get; private set; }
        public long CoinsPerClick { get; private set; } = 1;

        public List<Capybara> _capybarasList { get; private set; } = DataBase.SelectCapybaras();

        public List<Modification> _modificationsList { get; private set; } = new List<Modification>()
        {
            new Modification(Resources.modifIcon1, "Монетка", "+1 Монета в сек.", 20, 1,
                TypesOfModifications.AddCoinsPerSecond),
            new Modification(Resources.modifIcon2, "Лёгкий клик", "+1 Монета за клик", 100, 1,
                TypesOfModifications.AddCoinsPerClick),
            new Modification(Resources.modifIcon3, "Парочка", "+25 Монет в сек.", 1500, 25,
                TypesOfModifications.AddCoinsPerSecond),
            new Modification(Resources.modifIcon4, "Мощный клик", "+50 Монет за клик", 10000, 50,
                TypesOfModifications.AddCoinsPerClick),
            new Modification(Resources.modifIcon5, "Щепотка", "+2K Монет в сек.", 100000, 2000,
                TypesOfModifications.AddCoinsPerSecond)
        };


        public void AddingCoinsPerSecond()
        {
            SumMoney += CoinsPerSecond;
        }

        public long GetSumCoins()
        {
            return SumMoney;
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


        public void AddCapybara(Capybara capybara)
        {
            _capybarasList.Add(capybara);
        }

        public void AddModification(Modification modification)
        {
            _modificationsList.Add(modification);
        }
    }
}