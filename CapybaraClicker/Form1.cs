using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapybaraClicker.Properties;

namespace CapybaraClicker
{
    public partial class Form1 : Form
    {
        private const int SkySpeed = 4;
        private readonly Random _rand = new Random();
        private int _sumMoney;
        private bool _is2X;
        private int _coinsPerSecond;
        private int _coinsPerClick = 1;
        private const int FillingSpeedProgressBar = 8;
        readonly CustomProgressBar _customProgressBar = new CustomProgressBar();

        private readonly List<Capybara> _capybarasList = new List<Capybara>
        {
            new Capybara(Resources.capybara_fish, 100),
            new Capybara(Resources.capybara_student, 1000),
            new Capybara(Resources.capybara_cook, 8000),
            new Capybara(Resources.capybara_skate, 75000),
            new Capybara(Resources.capybara_board, 500000)
        };

        private readonly List<Modification> _modificationsList = new List<Modification>()
        {
            new Modification(Resources.modifIcon1, "Монетка", "+1 Монета в сек.", 20, 1,
                TypesOfModifications.AddCoinsPerSecond),
            new Modification(Resources.modifIcon2, "Лёгкий клик", "+1 Монета за клик", 100, 1,
                TypesOfModifications.AddCoinsPerClick),
            new Modification(Resources.modifIcon3, "Парочка", "+25 Монет в сек.", 1500, 25,
                TypesOfModifications.AddCoinsPerSecond),
            new Modification(Resources.modifIcon4, "Мощный клик", "+50 Монет за клик", 10000, 50,
                TypesOfModifications.AddCoinsPerClick)
        };

        public Form1()
        {
            InitializeComponent();
            CreateProgressBar();
        }

        private void CreateProgressBar()
        {
            _customProgressBar.Size = new Size(200, 30);
            _customProgressBar.Location = new Point(capybara.Left, capybara.Top + capybara.Height + 15);
            _customProgressBar.Minimum = 0;
            _customProgressBar.Maximum = 100;
            _customProgressBar.Value = 0;
            Controls.Add(_customProgressBar);

            var bandProgressBar = new PictureBox();
            bandProgressBar.Image = Resources.bandProgressBar;
            bandProgressBar.SizeMode = PictureBoxSizeMode.StretchImage;
            bandProgressBar.BackColor = Color.Brown;
            bandProgressBar.Location =
                new Point(Convert.ToInt32(_customProgressBar.Left + _customProgressBar.Width * 0.7),
                    _customProgressBar.Top);
            bandProgressBar.Size = new Size(5, 15);
            Controls.Add(bandProgressBar);

            var icon2X = new PictureBox();
            icon2X.Image = Resources._2xIcon;
            icon2X.SizeMode = PictureBoxSizeMode.StretchImage;
            icon2X.BackColor = Color.Brown;
            icon2X.Location =
                new Point(Convert.ToInt32(_customProgressBar.Left + _customProgressBar.Width * 0.88),
                    _customProgressBar.Top + 4);
            icon2X.Size = new Size(20, 20);
            Controls.Add(icon2X);

            _customProgressBar.BringToFront();
            bandProgressBar.BringToFront();
            icon2X.BringToFront();
        }

        private void timerAddCoinsPerSecond_Tick(object sender, EventArgs e)
        {
            _sumMoney += _coinsPerSecond;
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            if (_customProgressBar.Value > 0)
                _customProgressBar.Value -= 1;

            sumCoinsLabel.Text = FormatCoinCount(_sumMoney);

            Update2XStatus();
            UpdateModificationsPanel();
            UpdateCapybarasBuyButton();
            MoveSky();
        }

        private static string FormatCoinCount(long count)
        {
            if (count < 1000)
                return count.ToString();
            if (count < 1000000)
                return (count / 1000.0).ToString("0.##K");
            if (count < 1000000000)
                return (count / 1000000.0).ToString("0.##M");
            if (count < 1000000000000)
                return (count / 1000000000.0).ToString("0.##B");
            return (count / 1000000000000.0).ToString("0.##T");
        }

        private void Update2XStatus()
        {
            _is2X = _customProgressBar.Value >= 70;
            coinsPerClickLabel.Text = $"{int.Parse(FormatCoinCount(_coinsPerClick)) * (_is2X ? 2 : 1)} за клик";
        }

        private void UpdateModificationsPanel()
        {
            foreach (var modification in _modificationsList)
                UpdateModificationPanel(modification);
        }

        private void UpdateModificationPanel(Modification modification)
        {
            if (_sumMoney >= modification.Cost)
                EnableModificationPanel(modification);
            else
                DisableModificationPanel(modification);
        }

        private void EnableModificationPanel(Modification modification)
        {
            var panel = FindModificationPanel(modification.Cost);
            if (panel != null)
                UpdatePanelControls(panel, modification);
        }

        private Panel FindModificationPanel(int cost)
        {
            foreach (Panel panel in modificationsPanel.Controls)
                if (int.Parse((string)panel.Tag) == cost)
                    return panel;

            return null;
        }

        private void UpdatePanelControls(Control panel, Modification modification)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is PictureBox img)
                    img.Image = modification.Image;

                if (control is Label label1 && (string)label1.Tag == "bigLabel")
                    label1.Text = modification.BigModifLabel;

                if (control is Label label2 && (string)label2.Tag == "smallLabel")
                    label2.Text = modification.SmallModifLabel;

                if (control is Button buyButton)
                    buyButton.Enabled = true;
            }
        }

        private void DisableModificationPanel(Modification modification)
        {
            var panel = FindModificationPanel(modification.Cost);
            foreach (Control control in panel.Controls)
                if (control is Button buyButton)
                    buyButton.Enabled = false;
        }

        private void UpdateCapybarasBuyButton()
        {
            foreach (Control control in capybarasPanel.Controls)
                if (control is Button && int.Parse((string)control.Tag) <= _sumMoney)
                    control.Enabled = true;
        }

        private void MoveSky()
        {
            foreach (Control control in Controls)
                if ((string)control.Tag == "sky")
                {
                    control.Left -= SkySpeed;
                    if (control.Left <= -250)
                    {
                        var skyPos = _rand.Next(1200, 1800);
                        control.Left = skyPos;
                    }
                }
        }
        
        private void capybara_MouseDown(object sender, MouseEventArgs e)
        {
            if (_customProgressBar.Value + FillingSpeedProgressBar <= 100)
                _customProgressBar.Value += FillingSpeedProgressBar;

            if (_is2X)
                _sumMoney += _coinsPerClick * 2;
            else
                _sumMoney += _coinsPerClick;

            capybara.Width -= 30;
            capybara.Height -= 30;

            var timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (s, args) =>
            {
                capybara.Width += 30;
                capybara.Height += 30;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        private void capybarasShopButton_Click(object sender, EventArgs e)
        {
            capybarasPanel.Visible = true;
            modificationsPanel.Visible = false;
        }

        private void modificationsShopButton_Click(object sender, EventArgs e)
        {
            capybarasPanel.Visible = false;
            modificationsPanel.Visible = true;
        }

        private void capybaraCell_MouseDown(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).Width -= 10;
            ((PictureBox)sender).Height -= 10;
            capybara.Image = ((PictureBox)sender).Image;
        }

        private void capybaraCell_MouseUp(object sender, MouseEventArgs e)
        {
            ((PictureBox)sender).Width += 10;
            ((PictureBox)sender).Height += 10;
        }

        private void capybaraBuyButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in capybarasPanel.Controls)
                if (control is PictureBox capybaraCell && capybaraCell.Tag == ((Button)sender).Tag)
                {
                    var purchasedCapybara =
                        _capybarasList.Find(capybara => capybara.Cost == int.Parse((string)capybaraCell.Tag));
                    _sumMoney -= purchasedCapybara.Cost;
                    ((Button)sender).Visible = false;
                    capybaraCell.Image = purchasedCapybara.Image;
                    capybaraCell.Enabled = true;
                }
        }

        private void modifButton_Click(object sender, EventArgs e)
        {
            var amountOfModif = (string)((Button)sender).Tag;
            var cost = int.Parse(amountOfModif);
            _sumMoney -= cost;
            var selectedModification = _modificationsList.FirstOrDefault(modification => modification.Cost == cost);
            if (selectedModification != null)
            {
                ApplyModification(selectedModification);
            }
        }

        private void ApplyModification(Modification modification)
        {
            if (modification.Type == TypesOfModifications.AddCoinsPerSecond)
            {
                _coinsPerSecond += modification.Bonus;
                UpdateCoinsPerSecondLabel();
            }
            else
            {
                _coinsPerClick += modification.Bonus;
                UpdateCoinsPerClickLabel();
            }
        }

        private void UpdateCoinsPerSecondLabel() =>
            coinsPerSecondLabel.Text = $"{FormatCoinCount(_coinsPerSecond)} в сек.";

        private void UpdateCoinsPerClickLabel() =>
            coinsPerClickLabel.Text = $"{FormatCoinCount(_coinsPerClick)} за клик";
    }
}