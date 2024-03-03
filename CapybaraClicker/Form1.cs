using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using CapybaraClicker.Properties;

namespace CapybaraClicker
{
    public partial class Form1 : Form
    {
        private readonly CustomProgressBar _customProgressBar = new CustomProgressBar();
        private readonly SoundPlayer _soundPlayer = new SoundPlayer("music.wav");
        private readonly GameModel _model;

        public Form1()
        {
            DataBase.SelectMoneyData();
            _model = new GameModel();
            InitializeComponent();
            CreateProgressBar();
            StartMusic();
            InitializeCapybarasSkins();
            InitializeModifications();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataBase.UpdateMoneyDataInDB(_model.SumMoney, _model.CoinsPerSecond, _model.CoinsPerClick);
        }

        private void InitializeCapybarasSkins()
        {
            foreach (Control control in capybarasPanel.Controls)
            {
                var capybara =
                    _model._capybarasList.Find(capyb => capyb.Cost == int.Parse((string)control.Tag));

                if (capybara.IsBuy)
                {
                    switch (control)
                    {
                        case PictureBox capybaraCell:
                            capybaraCell.Image = capybara.ImgPath;
                            capybaraCell.Enabled = true;
                            break;
                        case Button buyButton:
                            buyButton.Visible = false;
                            break;
                    }
                }
            }
        }

        private void InitializeModifications()
        {
            foreach (Control modifPanel in modificationsPanel.Controls)
            {
                var costPanel = int.Parse((string)modifPanel.Tag);
                var modif = _model._modificationsList.Find(modification => modification.Cost == costPanel);
                if (modif.NumberOfPurchase > 0)
                    UpdatePanelControls(modifPanel, modif);
            }
        }

        private void StartMusic()
        {
            _soundPlayer.LoadAsync();
            _soundPlayer.PlayLooping();
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

        private void onSoundButton_Click(object sender, EventArgs e)
        {
            onSoundButton.Visible = false;
            offSoundButton.Visible = true;
            if (_soundPlayer.IsLoadCompleted)
                _soundPlayer.Stop();
        }

        private void offSoundButton_Click(object sender, EventArgs e)
        {
            onSoundButton.Visible = true;
            offSoundButton.Visible = false;
            if (_soundPlayer.IsLoadCompleted)
                _soundPlayer.PlayLooping();
        }

        private void timerAddCoinsPerSecond_Tick(object sender, EventArgs e)
        {
            _model.AddingCoinsPerSecond();
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            if (_customProgressBar.Value > 0)
                _customProgressBar.Value -= 1;

            sumCoinsLabel.Text = FormatCoinsCount(_model.SumMoney);

            Update2XStatus();
            UpdateModificationsPanel();
            ChangeStateCapybaraBuyButton();
            MoveSky();
        }

        private static string FormatCoinsCount(long count)
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
            _model.Change2XStatus(_customProgressBar.Value >= 70);
            coinsPerClickLabel.Text = $"{FormatCoinsCount(_model.CoinsPerClick * (_model.Is2X ? 2 : 1))} за клик";
        }

        private void UpdateModificationsPanel()
        {
            foreach (var modification in _model._modificationsList)
                UpdateModificationPanel(modification);
        }

        private void UpdateModificationPanel(Modification modification)
        {
            if (_model.SumMoney >= modification.Cost)
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

        private void ChangeStateCapybaraBuyButton()
        {
            foreach (Control control in capybarasPanel.Controls)
                if (control is Button && control.Visible)
                {
                    var capybaraCost = int.Parse((string)control.Tag);
                    control.Enabled = capybaraCost <= _model.SumMoney;
                }
        }

        private void MoveSky()
        {
            const int skySpeed = 4;
            var rand = new Random();

            foreach (Control control in Controls)
                if ((string)control.Tag == "sky")
                {
                    control.Left -= skySpeed;
                    if (control.Left <= -250)
                    {
                        var skyPos = rand.Next(1200, 1800);
                        control.Left = skyPos;
                    }
                }
        }

        private void capybara_MouseDown(object sender, MouseEventArgs e)
        {
            FillProgressBar();

            _model.AddingCoinsPerClick();

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

        private void FillProgressBar()
        {
            const int fillingSpeedProgressBar = 8;
            if (_customProgressBar.Value + fillingSpeedProgressBar <= 100)
                _customProgressBar.Value += fillingSpeedProgressBar;
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
                    var capybaraCost = int.Parse((string)capybaraCell.Tag);
                    var purchasedCapybara = _model.BuyNewCapybara(capybaraCost);
                    ((Button)sender).Visible = false;
                    capybaraCell.Image = purchasedCapybara.ImgPath;
                    capybaraCell.Enabled = true;
                    DataBase.UpdatePurchaseStateCapybara(purchasedCapybara);
                }
        }

        private void modificationBuyButton_Click(object sender, EventArgs e)
        {
            var amountOfModif = (string)((Button)sender).Tag;
            var cost = int.Parse(amountOfModif);
            var purchasedModification = _model.BuyNewModification(cost);
            if (purchasedModification != null)
            {
                purchasedModification.NumberOfPurchase++;
                DataBase.UpdatePurchaseStateModification(purchasedModification);
                ApplyModification(purchasedModification);
            }
        }

        private void ApplyModification(Modification modification)
        {
            if (modification.Type == TypesOfModifications.AddCoinsPerSecond)
            {
                _model.ChangeCoinsPerSecond(modification.Bonus);
                UpdateCoinsPerSecondLabel();
            }
            else
            {
                _model.ChangeCoinsPerClick(modification.Bonus);
                UpdateCoinsPerClickLabel();
            }
        }

        private void UpdateCoinsPerSecondLabel() =>
            coinsPerSecondLabel.Text = $"{FormatCoinsCount(_model.CoinsPerSecond)} в сек.";

        private void UpdateCoinsPerClickLabel() =>
            coinsPerClickLabel.Text = $"{FormatCoinsCount(_model.CoinsPerClick)} за клик";
    }
}