using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Neuroshima
{
    public partial class MainWindow : Window
    {
        IFileManager fileManager;

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            fileManager.SavePlayers(gracze);
            base.OnClosing(e);
        }

        private void saveAll_Click(object sender, RoutedEventArgs e)
        {
            fileManager.SavePlayers(gracze);
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ModelView.GetModelView();
            fileManager = new XmlFileManager(gracze);
            ModelView.CollectionOfPlayers = fileManager.LoadPlayers();
        }

        private void skillSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCurrentSkillToSelected();
            wyszukiwarka.Text = "";
            IfEditingFocusCurrentOnBox(skill);
        }

        public void SetCurrentSkillToSelected()
        {
            if (gracze.SelectedIndex >= 0)
            {
                skill.Text = (gracze.SelectedItem as Player).PlayerData.Skill[skillSet.SelectedIndex].ToString();
                ModelView.CurrentSkill = skill.Text;
            }
        }

        private void attributeSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCurrentAttributeToSelected();
            IfEditingFocusCurrentOnBox(atrybut);
        }

        public void SetCurrentAttributeToSelected()
        {
            if (gracze.SelectedIndex >= 0)
                atrybut.Text = (gracze.SelectedItem as Player).PlayerData.Attribute[attributeSet.SelectedIndex].ToString();
        }

        public void IfEditingFocusCurrentOnBox(TextBox textBox)
        {
            if (tryb_szybki.IsChecked == true)
            {
                textBox.Text = "";
                textBox.Focus();
            }
        }

        private void wyszukiwarka_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowMatching();
        }

        public void ShowMatching()
        {
            for (int i = 0; i < PlayerData.SkillCount; i++)
            {
                if ((skillSet.Items[i] as ListBoxItem).Content.ToString().StartsWith(wyszukiwarka.Text, StringComparison.OrdinalIgnoreCase))
                    (skillSet.Items[i] as ListBoxItem).Visibility = Visibility.Visible;
                else
                    (skillSet.Items[i] as ListBoxItem).Visibility = Visibility.Collapsed;
            }
        }

        private void atrybut_TextChanged(object sender, TextChangedEventArgs e)
        {
            ControlAtrybutInput();
            IfEditingSave(sender, e);
        }

        public void ControlAtrybutInput()
        {
            const int maxAttributeValue = 40;
            if (!int.TryParse(atrybut.Text, out int temp) || (atrybut.Text[0] == '0')) atrybut.Text = "";
            if (!String.IsNullOrEmpty(atrybut.Text))
                if (Convert.ToInt32(atrybut.Text) > maxAttributeValue)
                    atrybut.Text = atrybut.Text.Remove(1, 1);
        }

        private void skill_TextChanged(object sender, TextChangedEventArgs e)
        {
            ControlSkillInput();
            IfEditingSave(sender, e);
        }

        public void ControlSkillInput()
        {
            const int maxSkillTextLength = 2;
            if (!int.TryParse(skill.Text, out int temp)) skill.Text = "";
            if (skill.Text.Length > maxSkillTextLength)
                skill.Text = skill.Text.Remove(1, 1);
        }

        private void utrudnienie_TextChanged(object sender, TextChangedEventArgs e)
        {
            ControlPenaltyInput();
            IfEditingSave(sender, e);
        }

        public void ControlPenaltyInput()
        {
            const int maxPenaltyTextLength = 3;
            if (!int.TryParse(utrudnienie.Text, out int temp)) utrudnienie.Text = "";
            if (utrudnienie.Text.Length > maxPenaltyTextLength)
                utrudnienie.Text = utrudnienie.Text.Remove(1, 1);
        }

        public void IfEditingSave(object sender, TextChangedEventArgs e)
        {
            if (tryb_szybki.IsChecked == true)
                zmiana_Click(sender, e);
        }

        private void gracze_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tryb_szybki.IsChecked = false;
            ChangeAllSelections(sender, e);
        }

        public void ChangeAllSelections(object sender, SelectionChangedEventArgs e)
        {
            if (attributeSet.SelectedIndex >= 0)
                attributeSet_SelectionChanged(sender, e);

            if (skillSet.SelectedIndex >= 0)
                skillSet_SelectionChanged(sender, e);

            if (gracze.SelectedIndex >= 0)
                utrudnienie.Text = (gracze.SelectedItem as Player).PlayerData.Penalty.ToString();
        }

        private void zmiana_Click(object sender, RoutedEventArgs e)
        {
            if (gracze.SelectedIndex >= 0)
            {
                SaveAttribute();
                SaveSkill();
                SavePenalty();
            }
        }

        public void SaveAttribute()
        {
            if (!String.IsNullOrEmpty(atrybut.Text) && (attributeSet.SelectedIndex >= 0))
                (gracze.SelectedItem as Player).PlayerData.Attribute[attributeSet.SelectedIndex] = Convert.ToInt32(atrybut.Text);
        }

        public void SaveSkill()
        {
            if (!String.IsNullOrEmpty(skill.Text) && (skillSet.SelectedIndex >= 0))
                (gracze.SelectedItem as Player).PlayerData.Skill[skillSet.SelectedIndex] = Convert.ToInt32(skill.Text);
        }

        public void SavePenalty()
        {
            if (!String.IsNullOrEmpty(utrudnienie.Text))
                (gracze.SelectedItem as Player).PlayerData.Penalty = Convert.ToInt32(utrudnienie.Text);
        }

        private void A_TextChanged(object sender, TextChangedEventArgs e)
        {
            DiceTextChanged(A);
        }

        private void B_TextChanged(object sender, TextChangedEventArgs e)
        {
            DiceTextChanged(B);
        }

        private void C_TextChanged(object sender, TextChangedEventArgs e)
        {
            DiceTextChanged(C);
        }

        private void DiceTextChanged(TextBox destination)
        {
            ControlDiceInput(destination);
            if (DicesOk())
                MakeTest();
        }

        public static void ControlDiceInput(TextBox destination)
        {
            const int maxDiceTextLength = 20;
            if (!int.TryParse(destination.Text, out int temp) || (destination.Text[0] == '0')) destination.Text = "";
            if (!String.IsNullOrEmpty(destination.Text))
                if (Convert.ToInt32(destination.Text) > maxDiceTextLength)
                    destination.Text = destination.Text.Remove(1, 1);
        }

        public bool DicesOk()
        {
            return (!String.IsNullOrEmpty(A.Text) && !String.IsNullOrEmpty(B.Text) && !String.IsNullOrEmpty(C.Text));
        }

        private void MakeTest()
        {
            if (TestReady())
                ShowTestResult(TestResult());
        }

        public bool TestReady()
        {
            return ((gracze.SelectedIndex >= 0) && (!String.IsNullOrEmpty(atrybut.Text) && !String.IsNullOrEmpty(skill.Text) && !String.IsNullOrEmpty(utrudnienie.Text)));
        }

        public void ShowTestResult(int passedHardness)
        {
            const int maxHardness = 14;
            zdano.Text = passedHardness.ToString();
            if (passedHardness + 2 > maxHardness)
                trudnosc_zdana.SelectedItem = trudnosc_zdana.Items[maxHardness];
            else if (passedHardness + 2 < 0)
                trudnosc_zdana.SelectedItem = trudnosc_zdana.Items[0];
            else
                trudnosc_zdana.SelectedItem = trudnosc_zdana.Items[passedHardness + 2];
            trudnosc_zdana.ScrollIntoView(trudnosc_zdana.SelectedItem);
        }

        public int TestResult()
        {
            int passedHardness;
            ITester tester = new Tester();
            tester.LoadDices(A, B, C);
            tester.LoadTestValues(atrybut, skill, utrudnienie);
            passedHardness = tester.Result();
            return passedHardness;
        }

        private void rzut_Click(object sender, RoutedEventArgs e)
        {
            RollDices();
        }

        public void RollDices()
        {
            Random random = new Random();
            A.Text = (random.Next(0, 20) + 1).ToString();
            B.Text = (random.Next(0, 20) + 1).ToString();
            C.Text = (random.Next(0, 20) + 1).ToString();
        }

        private void tryb_szybki_Checked_1(object sender, RoutedEventArgs e)
        {
            zmiana.IsEnabled = false;
        }

        private void tryb_szybki_Checked_2(object sender, RoutedEventArgs e)
        {
            zmiana.IsEnabled = true;
        }

        private void dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (NameOk())
                new Player(nowy.Text, ModelView.CollectionOfPlayers);
            nowy.Text = "";
        }

        public bool NameOk()
        {
            bool givenNameIsFree = true;
            int i = 0;
            while (i < gracze.Items.Count && givenNameIsFree)
            {
                if ((gracze.Items[i] as Player).PlayerData.Name == nowy.Text)
                    givenNameIsFree = false;
                i++;
            }
            return ((givenNameIsFree) && !String.IsNullOrEmpty(nowy.Text));
        }

        private void nowy_TextChanged(object sender, TextChangedEventArgs e)
        {
            ControlNewInput();
        }

        public void ControlNewInput()
        {
            const int maxNewTextLength = 10;
            if (nowy.Text.Length > maxNewTextLength)
                nowy.Text = "";
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (gracze.SelectedIndex >= 0)
                ConfirmDeletion();
        }

        public void ConfirmDeletion()
        {
            MessageBoxResult chosenOption = MessageBox.Show("Jesteś pewien?", "Potwierdzenie usunięcia gracza", MessageBoxButton.YesNo);
            if (chosenOption == MessageBoxResult.Yes)
                ModelView.CollectionOfPlayers.Remove(gracze.SelectedItem as Player);
        }
    }
}
