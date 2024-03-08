using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bankomat
{
    public class User
    {
        public string CardNumber { get; set; }
        public string Pin { get; set; }
        public double Money { get; set; }
        public string Debet { get; set; }
        public double DebetAmount { get; set; }
    }

    public partial class MainWindow : Window
    {
        public User currentUser;
        public List<User> users;
        public Boolean CardOrNot = false; // false - Without, true - With
        public int language = 0; // 0 - Polish, 1 - English
        public int Currency = 0; // 0 - PLN, 1 - EUR, 2 - USD
        public string currencyText = "";
        public int numberOf100;
        public int numberOf50;
        public int numberOf20;
        public Boolean BlikOrNot = true;
        double withdrawAmount = 0;
        public Boolean WdorD = true;
        public MainWindow()
        {
            InitializeComponent();
            InitializeUsers();

            BlikCode.TextChanged += BlikCode_TextChanged;
            BlikCode.PreviewTextInput += BlikCode_PreviewTextInput;
        }

        private const int MaxBlikCodeLength = 6;

        private void BlikCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length > MaxBlikCodeLength)
            {
                textBox.Text = textBox.Text.Substring(0, MaxBlikCodeLength);
            }
        }

        private void BlikCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Ograniczenie tylko do cyfr
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }

            // Ograniczenie do maksymalnie 6 cyfr
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text + e.Text).Length > MaxBlikCodeLength)
            {
                e.Handled = true;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        { 
            Kliknij.Visibility = Visibility.Hidden;
            Pln.Visibility = Visibility.Hidden;
            Eur.Visibility = Visibility.Hidden;
            Usd.Visibility = Visibility.Hidden;
            Pin.Visibility = Visibility.Hidden;
            PinBtn.Visibility = Visibility.Hidden;
            Card.Visibility = Visibility.Hidden;
            Withdraw.Visibility = Visibility.Hidden;
            WithdrawBlik.Visibility = Visibility.Hidden;
            Deposit.Visibility = Visibility.Hidden;
            DepositBlik.Visibility = Visibility.Hidden;
            Status.Visibility = Visibility.Hidden;
            Welcome.Visibility = Visibility.Hidden;
            Wait.Visibility = Visibility.Hidden;
            Fifty.Visibility = Visibility.Hidden;
            Onehundred.Visibility = Visibility.Hidden;
            Twohundred.Visibility = Visibility.Hidden;
            ConfirmMoney.Visibility = Visibility.Hidden;
            Amount.Visibility = Visibility.Hidden;
            Back.Visibility = Visibility.Hidden;
            RightText.Visibility = Visibility.Hidden;
            Yes.Visibility = Visibility.Hidden;
            No.Visibility = Visibility.Hidden;
            Yes2.Visibility = Visibility.Hidden;
            No2.Visibility = Visibility.Hidden;
            BlikCode.Visibility = Visibility.Hidden;
            BlikBtn.Visibility = Visibility.Hidden;
            Twenty.Visibility = Visibility.Hidden;
            Fifty2.Visibility = Visibility.Hidden;
            Onehundred2.Visibility = Visibility.Hidden;
            Confirm2.Visibility = Visibility.Hidden;
            Infor.Visibility = Visibility.Hidden;
            Twenty2.Visibility = Visibility.Hidden;
            Fifty3.Visibility = Visibility.Hidden;
            Onehundred3.Visibility = Visibility.Hidden;
        }

        public void Language_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                string selectedLanguage = clickedButton.Content.ToString();

                ResourceDictionary languageResource = null;
                if (selectedLanguage == "Polski")
                {
                    language = 0;
                    Polski.Visibility = Visibility.Hidden;
                    English.Visibility = Visibility.Hidden;
   
                    languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
                }
                else if (selectedLanguage == "English")
                {
                    language = 1;
                    Polski.Visibility = Visibility.Hidden;
                    English.Visibility = Visibility.Hidden;

                    languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
                }

                TransactionWithCard.Content = languageResource["TransactionWithCard"];
                TransactionWithoutCard.Content = languageResource["TransactionWithoutCard"];

                TransactionWithCard.Visibility = Visibility.Visible;
                TransactionWithoutCard.Visibility = Visibility.Visible;

                TransakcjaTextBlock.Text = languageResource["TransactionText"] as string;
            }
        }

        public void TransactionWithCard_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            TransactionWithCard.Visibility = Visibility.Hidden;
            TransactionWithoutCard.Visibility = Visibility.Hidden;
            Pln.Visibility = Visibility.Visible;
            Eur.Visibility = Visibility.Visible;
            Usd.Visibility = Visibility.Visible;
            CardOrNot = true;
            TransakcjaTextBlock.Text = languageResource["Currency"] as string;
        }

        public void TransactionWithoutCard_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            TransactionWithCard.Visibility = Visibility.Hidden;
            TransactionWithoutCard.Visibility = Visibility.Hidden;
            Pln.Visibility = Visibility.Visible;
            Eur.Visibility = Visibility.Visible;
            Usd.Visibility = Visibility.Visible;
            CardOrNot = false;
            TransakcjaTextBlock.Text = languageResource["Currency"] as string;
        }

        public void Currency_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                string selectedCurrency = clickedButton.Content.ToString();

                ResourceDictionary languageResource = null;
                if (language == 0)
                {
                    languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
                }
                else
                {
                    languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
                }
                if (selectedCurrency == "PLN")
                {
                    Currency = 0;
                    currencyText = "PLN";
                    
                }
                else if (selectedCurrency == "EUR")
                {
                    Currency = 1;
                    currencyText = "EUR";
                }
                else if (selectedCurrency == "USD")
                {
                    Currency = 2;
                    currencyText = "USD";
                }
                Pln.Visibility = Visibility.Hidden;
                Eur.Visibility = Visibility.Hidden;
                Usd.Visibility = Visibility.Hidden;
                Withdraw.Content = languageResource["Withdraw"];
                WithdrawBlik.Content = languageResource["WithdrawBlik"];
                Deposit.Content = languageResource["Deposit"];
                DepositBlik.Content = languageResource["DepositBlik"];
                Status.Content = languageResource["AccStatus"];
                Welcome.Text = languageResource["Welcome"] as string;
                if (CardOrNot)
                {
                    TransakcjaTextBlock.Text = languageResource["Insert"] as string;
                    ShowPinInput();
                }
                else
                {
                    WithdrawBlik.Visibility = Visibility.Visible;
                    DepositBlik.Visibility = Visibility.Visible;
                    TransakcjaTextBlock.Visibility = Visibility.Hidden;
                    Welcome.Visibility = Visibility.Visible;
                }
            }
        }
        public void ShowPinInput()
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            TransakcjaTextBlock.Text = languageResource["Pin"] as string;
            PinBtn.Content = languageResource["Confirm"];
            Pin.Visibility = Visibility.Visible;
            PinBtn.Visibility = Visibility.Visible;
            Card.Visibility = Visibility.Visible;
        }

        public async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            string enteredCardNumber = Card.Text;
            string enteredPin = Pin.Text;
            currentUser = users.FirstOrDefault(u => u.CardNumber == enteredCardNumber);

            Withdraw.Content = languageResource["Withdraw"];
            WithdrawBlik.Content = languageResource["WithdrawBlik"];
            Deposit.Content = languageResource["Deposit"];
            DepositBlik.Content = languageResource["DepositBlik"];
            Status.Content = languageResource["AccStatus"];
            Welcome.Text = languageResource["Welcome"] as string;

            if (currentUser != null && currentUser.Pin == enteredPin)
            {
                await Task.Delay(4000);
                MessageBox.Show($"{languageResource["Correct"]}");
                await Task.Delay(4000);
                Withdraw.Visibility = Visibility.Visible;
                WithdrawBlik.Visibility = Visibility.Visible;
                Deposit.Visibility = Visibility.Visible;
                Status.Visibility = Visibility.Visible;
                Welcome.Visibility = Visibility.Visible;
                Card.Visibility = Visibility.Hidden;
                Pin.Visibility = Visibility.Hidden;
                PinBtn.Visibility = Visibility.Hidden;
                TransakcjaTextBlock.Visibility = Visibility.Hidden;
                if(Currency == 1)
                {
                    currentUser.Money = currentUser.Money / 4.5;
                }
                else if(Currency == 2)
                {
                    currentUser.Money = currentUser.Money / 4;
                }
            }
            else
            {
                await Task.Delay(4000);
                MessageBox.Show($"{languageResource["incorrect"]}");
                Pin.Text = string.Empty;
                Card.Text = string.Empty;
                PinBtn.Visibility = Visibility.Hidden;
                Pln.Visibility = Visibility.Visible;
                Eur.Visibility = Visibility.Visible;
                Usd.Visibility = Visibility.Visible;
                Card.Visibility = Visibility.Hidden;
                Pin.Visibility = Visibility.Hidden;
                TransakcjaTextBlock.Text = languageResource["Currency"] as string;
            }
        }

        public void InitializeUsers()
        {
            users = new List<User>
            {
                new User { CardNumber = "1234567890123456", Pin = "1234", Money = 1500, Debet = "Yes", DebetAmount = 200}, // 1500PLN
                new User { CardNumber = "9876543210987654", Pin = "5678" , Money = 500, Debet = "No",  DebetAmount = 0},
                new User { CardNumber = "5962316578672356", Pin = "2521" , Money = 201, Debet = "Yes", DebetAmount = 1000},
            };
        }

        public void Withdraw_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            RightText.Text = languageResource["Withdraw"] as string;
            RightText.Visibility = Visibility.Visible;
            Back.Content = languageResource["Back"];
            Back.Visibility = Visibility.Visible;
            Fifty.Visibility = Visibility.Visible;
            Onehundred.Visibility = Visibility.Visible;
            Twohundred.Visibility = Visibility.Visible;
            Amount.Visibility = Visibility.Visible;
            ConfirmMoney.Visibility = Visibility.Visible;
            Withdraw.Visibility = Visibility.Hidden;
            WithdrawBlik.Visibility = Visibility.Hidden;
            Deposit.Visibility = Visibility.Hidden;
            DepositBlik.Visibility = Visibility.Hidden;
            Status.Visibility = Visibility.Hidden;
            Welcome.Visibility = Visibility.Hidden;
            BlikCode.Visibility = Visibility.Hidden;
            BlikBtn.Visibility = Visibility.Hidden;
            BlikOrNot = false;
            WdorD = true;
        }


        public void Deposit_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            WdorD = false;
            Potwierdzenie();
        }

        public void WithdrawBlik_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            RightText.Text = languageResource["WithdrawBlik"] as string;
            RightText.Visibility = Visibility.Visible;
            Back.Content = languageResource["Back"];
            Back.Visibility = Visibility.Visible;
            Fifty.Visibility = Visibility.Visible;
            Onehundred.Visibility = Visibility.Visible;
            Twohundred.Visibility = Visibility.Visible;
            Amount.Visibility = Visibility.Visible;
            ConfirmMoney.Visibility = Visibility.Visible;
            Withdraw.Visibility = Visibility.Hidden;
            WithdrawBlik.Visibility = Visibility.Hidden;
            Deposit.Visibility = Visibility.Hidden;
            DepositBlik.Visibility = Visibility.Hidden;
            Status.Visibility = Visibility.Hidden;
            Welcome.Visibility = Visibility.Hidden;
            BlikCode.Visibility = Visibility.Hidden;
            BlikBtn.Visibility = Visibility.Hidden;
            BlikOrNot = true;
            WdorD = true;
        }

        public void DepositBlik_Click(object sender, RoutedEventArgs e)
        { 
            WdorD = false;
            Potwierdzenie();

        }

        public async void Status_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            Withdraw.Visibility = Visibility.Hidden;
            WithdrawBlik.Visibility = Visibility.Hidden;
            Deposit.Visibility = Visibility.Hidden;
            DepositBlik.Visibility = Visibility.Hidden;
            Status.Visibility = Visibility.Hidden;
            Fifty.Visibility = Visibility.Hidden;
            Onehundred.Visibility = Visibility.Hidden;
            Twohundred.Visibility = Visibility.Hidden;
            ConfirmMoney.Visibility = Visibility.Hidden;
            Amount.Visibility = Visibility.Hidden;
            Welcome.Visibility = Visibility.Hidden;
            Back.Content = languageResource["Back"];
            Back.Visibility = Visibility.Visible;
            RightText.Visibility = Visibility.Visible;
            BlikCode.Visibility = Visibility.Hidden;
            BlikBtn.Visibility = Visibility.Hidden;
            TransakcjaTextBlock.Visibility = Visibility.Hidden;
            Yes.Visibility = Visibility.Hidden;
            No.Visibility = Visibility.Hidden;
            Yes2.Visibility = Visibility.Hidden;
            No2.Visibility = Visibility.Hidden;
            RightText.Text = languageResource["AccStatus"] as string;
            Wait.Text = languageResource["Wait"] as string;
            Wait.Visibility = Visibility.Visible;
            await Task.Delay(5000);
            currentUser.Money = Math.Round(currentUser.Money, 2);
            MessageBox.Show($"{languageResource["Your"]}: {currentUser.Money} {currencyText}");
        }

        public void WithdrawMoney_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                string selectedMoney = clickedButton.Content.ToString();

                ResourceDictionary languageResource = null;
                if (language == 0)
                {
                    languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
                }
                else
                {
                    languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
                }

                string InputAmount = Amount.Text;

                if (selectedMoney == "50")
                {
                    withdrawAmount = 50;
                }
                else if (selectedMoney == "100")
                {
                    withdrawAmount = 100;
                }
                else if (selectedMoney == "200")
                {
                    withdrawAmount = 200;
                }
                else if (selectedMoney == "Wypłać")
                    if (!double.TryParse(InputAmount, out withdrawAmount) || withdrawAmount <= 0)
                    {
                        withdrawAmount = Double.Parse(InputAmount);
                    }


                this.numberOf100 = (int)(withdrawAmount / 100);
                this.numberOf50 = (int)((withdrawAmount % 100) / 50);
                this.numberOf20 = (int)(((withdrawAmount % 100) % 50) / 20);

                if (BlikOrNot == true)
                {
                    Blik();
                }
                else
                {
                    if (currentUser.Money < withdrawAmount && currentUser.Debet == "No")
                    {
                        MessageBox.Show($"{languageResource["Not"]}");
                        return;
                    }
                    else if(currentUser.Debet == "Yes" && currentUser.DebetAmount + currentUser.Money < withdrawAmount)
                    {
                        MessageBox.Show($"{languageResource["Not"]}");
                        return;
                    }

                    currentUser.Money -= withdrawAmount;
                    Potwierdzenie();
                }        
            }
        }

        public void Back_Click(object sender, RoutedEventArgs e)
        {
            if (CardOrNot)
            {
                Withdraw.Visibility = Visibility.Visible;
                WithdrawBlik.Visibility = Visibility.Visible;
                Deposit.Visibility = Visibility.Visible;
                Status.Visibility = Visibility.Visible;
                Welcome.Visibility = Visibility.Visible;
                Wait.Visibility = Visibility.Hidden;
                BlikCode.Visibility = Visibility.Hidden;
                BlikBtn.Visibility = Visibility.Hidden;
                RightText.Visibility = Visibility.Hidden;
                Back.Visibility = Visibility.Hidden;
            }
            else
            {
                WithdrawBlik.Visibility = Visibility.Visible;
                DepositBlik.Visibility = Visibility.Visible;
                Welcome.Visibility = Visibility.Visible;
                RightText.Visibility = Visibility.Hidden;
                Back.Visibility = Visibility.Hidden;
                Fifty.Visibility = Visibility.Hidden;
                Onehundred.Visibility = Visibility.Hidden;
                TransakcjaTextBlock.Visibility = Visibility.Hidden;
            }
        }

        public void Potwierdzenie()
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            if (CardOrNot)
            {
                Fifty.Visibility = Visibility.Hidden;
                Onehundred.Visibility = Visibility.Hidden;
                Twohundred.Visibility = Visibility.Hidden;
                ConfirmMoney.Visibility = Visibility.Hidden;
                Amount.Visibility = Visibility.Hidden;
                Withdraw.Visibility = Visibility.Hidden;
                WithdrawBlik.Visibility = Visibility.Hidden;
                Deposit.Visibility = Visibility.Hidden;
                DepositBlik.Visibility = Visibility.Hidden;
                BlikCode.Visibility = Visibility.Hidden;
                BlikBtn.Visibility = Visibility.Hidden;
                Status.Visibility = Visibility.Hidden;
                Welcome.Visibility = Visibility.Hidden;
                Wait.Visibility = Visibility.Hidden;
                RightText.Visibility = Visibility.Hidden;
                Back.Visibility = Visibility.Hidden;
                TransakcjaTextBlock.Text = languageResource["Confirmation"] as string;
                TransakcjaTextBlock.Visibility = Visibility.Visible;
                Yes.Content = languageResource["Yes"];
                No.Content = languageResource["No"];
                Yes2.Content = languageResource["Yes"];
                No2.Content = languageResource["No"];
                BlikOrNot = true;
                if (WdorD)
                {
                    Yes.Visibility = Visibility.Visible;
                    No.Visibility = Visibility.Visible;
                }
                else
                {
                    Yes2.Visibility = Visibility.Visible;
                    No2.Visibility = Visibility.Visible;
                }               
            }
            else
            {
                WithdrawBlik.Visibility = Visibility.Hidden;
                DepositBlik.Visibility = Visibility.Hidden;
                Welcome.Visibility = Visibility.Hidden;
                RightText.Visibility = Visibility.Hidden;
                Back.Visibility = Visibility.Hidden;
                BlikCode.Visibility = Visibility.Hidden;
                BlikBtn.Visibility = Visibility.Hidden;
                TransakcjaTextBlock.Text = languageResource["Confirmation"] as string;
                TransakcjaTextBlock.Visibility = Visibility.Visible;
                Yes.Content = languageResource["Yes"];
                No.Content = languageResource["No"];
                Yes2.Content = languageResource["Yes"];
                No2.Content = languageResource["No"];
                BlikOrNot = false;
                if (WdorD)
                {
                    Yes.Visibility = Visibility.Visible;
                    No.Visibility = Visibility.Visible;
                }
                else
                {
                    Yes2.Visibility = Visibility.Visible;
                    No2.Visibility = Visibility.Visible;
                }
            }
        }

        public void Blik()
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            Fifty.Visibility = Visibility.Hidden;
            Onehundred.Visibility = Visibility.Hidden;
            Twohundred.Visibility = Visibility.Hidden;
            ConfirmMoney.Visibility = Visibility.Hidden;
            Amount.Visibility = Visibility.Hidden;
            Withdraw.Visibility = Visibility.Hidden;
            WithdrawBlik.Visibility = Visibility.Hidden;
            Deposit.Visibility = Visibility.Hidden;
            DepositBlik.Visibility = Visibility.Hidden;
            Status.Visibility = Visibility.Hidden;
            Welcome.Visibility = Visibility.Hidden;
            Wait.Visibility = Visibility.Hidden;
            RightText.Visibility = Visibility.Hidden;
            Back.Visibility = Visibility.Hidden;
            TransakcjaTextBlock.Text = languageResource["Blik"] as string;
            TransakcjaTextBlock.Visibility = Visibility.Visible;
            BlikCode.Visibility = Visibility.Visible;
            BlikBtn.Content = languageResource["Confirm"];
            BlikBtn.Visibility = Visibility.Visible;
        }

        public void Yes_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            if (WdorD)
            {
                Yes.Visibility = Visibility.Hidden;
                No.Visibility = Visibility.Hidden;
                if (BlikOrNot)
                {
                    currentUser.Money = Math.Round(currentUser.Money, 2);
                    MessageBox.Show($"{languageResource["Withdraw"]}: {numberOf100}x100 + {numberOf50}x50 + {numberOf20}x20 = {withdrawAmount} {currencyText}");
                }
                else
                {
                    MessageBox.Show($"{languageResource["Withdraw"]}: {numberOf100}x100 + {numberOf50}x50 + {numberOf20}x20 = {withdrawAmount} {currencyText}");
                }
                if (CardOrNot)
                {
                    Withdraw.Visibility = Visibility.Visible;
                    WithdrawBlik.Visibility = Visibility.Visible;
                    Deposit.Visibility = Visibility.Visible;
                    Status.Visibility = Visibility.Visible;
                    Welcome.Visibility = Visibility.Visible;
                    Wait.Visibility = Visibility.Hidden;
                    RightText.Visibility = Visibility.Hidden;
                    Back.Visibility = Visibility.Hidden;
                    TransakcjaTextBlock.Visibility = Visibility.Hidden;
                }
                else
                {
                    Welcome.Visibility = Visibility.Visible;
                    WithdrawBlik.Visibility = Visibility.Visible;
                    DepositBlik.Visibility = Visibility.Visible;
                    TransakcjaTextBlock.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                if (BlikOrNot)
                {
                    Yes2.Visibility = Visibility.Hidden;
                    No2.Visibility = Visibility.Hidden;
                    TransakcjaTextBlock.Visibility = Visibility.Hidden;
                    Information2();
                }
                else
                {
                    Yes2.Visibility = Visibility.Hidden;
                    No2.Visibility = Visibility.Hidden;
                    TransakcjaTextBlock.Visibility = Visibility.Hidden;
                    Blik(); 
                }
            }
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            if (WdorD)
            {
                Yes.Visibility = Visibility.Hidden;
                No.Visibility = Visibility.Hidden;
                if (CardOrNot)
                {
                    Withdraw.Visibility = Visibility.Visible;
                    WithdrawBlik.Visibility = Visibility.Visible;
                    Deposit.Visibility = Visibility.Visible;
                    Status.Visibility = Visibility.Visible;
                    Welcome.Visibility = Visibility.Visible;
                    Wait.Visibility = Visibility.Hidden;
                    RightText.Visibility = Visibility.Hidden;
                    Back.Visibility = Visibility.Hidden;
                    TransakcjaTextBlock.Visibility = Visibility.Hidden;
                }
                else
                {
                    TransakcjaTextBlock.Visibility = Visibility.Hidden;
                    WithdrawBlik.Visibility = Visibility.Visible;
                    DepositBlik.Visibility = Visibility.Visible;
                    Welcome.Visibility = Visibility.Visible;
                    RightText.Visibility = Visibility.Hidden;
                    Back.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                if (BlikOrNot)
                {
                    Yes2.Visibility = Visibility.Hidden;
                    No2.Visibility = Visibility.Hidden;
                    TransakcjaTextBlock.Visibility = Visibility.Hidden;
                    Information2();
                }
                else
                {
                    Yes2.Visibility = Visibility.Hidden;
                    No2.Visibility = Visibility.Hidden;
                    TransakcjaTextBlock.Visibility = Visibility.Hidden;
                    Blik();
                }
            }  
        }

        public async void Blik_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(3000);
            if (WdorD)
            {
                Potwierdzenie();
            }
            else
            {
                Information2();
            }
        }

        public void Choose()
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            Infor.Visibility = Visibility.Hidden; 
            TransakcjaTextBlock.Text = languageResource["Banknotes"] as string;
            TransakcjaTextBlock.Visibility = Visibility.Visible;
            Twenty.Visibility = Visibility.Visible;
            Fifty2.Visibility = Visibility.Visible;
            Onehundred2.Visibility = Visibility.Visible;
            Confirm2.Content = languageResource["Confirm"];
            Confirm2.Visibility = Visibility.Visible;
            Twenty2.Visibility = Visibility.Visible;
            Fifty3.Visibility = Visibility.Visible;
            Onehundred3.Visibility = Visibility.Visible;
        }

        public async void Information2()
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            Infor.Text = languageResource["Banknotes2"] as string;
            Infor.Visibility = Visibility.Visible;
            BlikCode.Visibility = Visibility.Hidden;
            BlikBtn.Visibility = Visibility.Hidden;
            TransakcjaTextBlock.Visibility = Visibility.Hidden;
            await Task.Delay(6000);
            Choose();
        }

        public void AddToAccount(object sender, RoutedEventArgs e)
        {
            ResourceDictionary languageResource = null;
            if (language == 0)
            {
                languageResource = (ResourceDictionary)this.Resources["PolishTexts"];
            }
            else
            {
                languageResource = (ResourceDictionary)this.Resources["EnglishTexts"];
            }
            int numberOfTwenty = int.Parse(Twenty.Text);
            int numberOfFifty = int.Parse(Fifty2.Text);
            int numberOfOneHundred = int.Parse(Onehundred2.Text);

            double depositAmount = (numberOfTwenty * 20) + (numberOfFifty * 50) + (numberOfOneHundred * 100);

            MessageBox.Show($"{languageResource["Deposit"]}: {numberOfOneHundred}x100 + {numberOfFifty}x50 + {numberOfTwenty}x20 = {depositAmount} {currencyText}");

            Twenty.Visibility = Visibility.Hidden;
            Fifty2.Visibility = Visibility.Hidden;
            Onehundred2.Visibility = Visibility.Hidden;
            Confirm2.Visibility = Visibility.Hidden;
            Twenty2.Visibility = Visibility.Hidden;
            Fifty3.Visibility = Visibility.Hidden;
            Onehundred3.Visibility = Visibility.Hidden;
            TransakcjaTextBlock.Visibility = Visibility.Hidden;

            if (CardOrNot)
            {
                currentUser.Money += depositAmount;
                Withdraw.Visibility = Visibility.Visible;
                WithdrawBlik.Visibility = Visibility.Visible;
                Deposit.Visibility = Visibility.Visible;
                Status.Visibility = Visibility.Visible;
                Welcome.Visibility = Visibility.Visible;
            }
            else
            {
                WithdrawBlik.Visibility = Visibility.Visible;
                DepositBlik.Visibility = Visibility.Visible;
                Welcome.Visibility = Visibility.Visible;
            }
        }
    }
}

