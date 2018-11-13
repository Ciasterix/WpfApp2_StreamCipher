using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2_StreamCipher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Encryptor encryptor = null;

        public MainWindow()
        {
            InitializeComponent();
            encryptor = new Encryptor();
        }

        private void Button_LoadFile_Click(object sender, RoutedEventArgs e)
        {
            string plainText = encryptor.ReadTextFromFile();
            /*
            if (encryptor.checkIfStringContainsOnlyAsciiChars(plainText) == false)
            {
                encryptor.ShowErrorPopUp("Wczytany plik zawiera niedozwolony znak spoza znaków ASCII.");
                return;
            }
            */
            TextBox_PlainText.Text = plainText;
            string binaryText = encryptor.ConvertStringToBinaryString(plainText);
            TextBox_BinaryText.Text = binaryText;
        }

        private void Button_LoadBinaryText_Click(object sender, RoutedEventArgs e)
        {
            string binaryText = encryptor.ReadTextFromFile();
            if (encryptor.checkIfStringIsBinary(binaryText) == false)
            {
                encryptor.ShowErrorPopUp("Wczytany plik zawiera niedozwolony znak (inny niż 0 lub 1).");
                return;
            }
            else
            {
                TextBox_BinaryText.Text = binaryText;
                string plainText = encryptor.convertBitStringToString(binaryText);
                TextBox_PlainText.Text = plainText;
            }
        }

        private void Button_Info_Click(object sender, RoutedEventArgs e)
        {
            encryptor.ShowInfoPopUp();
        }

        private void Button_LoadKey_Click(object sender, RoutedEventArgs e)
        {
            string keyText = encryptor.ReadTextFromFile();
            if (encryptor.checkIfStringIsBinary(keyText) == false)
            {
                encryptor.ShowErrorPopUp("Wczytany plik zawiera niedozwolony znak (inny niż 0 lub 1).");
                return;
            }
            TextBox_CipherKey.Text = keyText;
        }

        private void Button_CipherOrDecipher1_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_BinaryText.Text.Length == 0)
                encryptor.ShowErrorPopUp("Pole \"Tekst binarnie\" jest puste.");
            else if(encryptor.checkIfStringIsBinary(TextBox_BinaryText.Text) == false)
                encryptor.ShowErrorPopUp("Pole \"Tekst binarnie\" zawiera niedozwolony znak (inny niż 0 lub 1).");
            else if (TextBox_CipherKey.Text.Length == 0)
                encryptor.ShowErrorPopUp("Pole \"Klucz szyfrowania\" jest puste.");
            else if (encryptor.checkIfStringIsBinary(TextBox_CipherKey.Text) == false)
                encryptor.ShowErrorPopUp("Pole \"Klucz szyfrowania\" zawiera niedozwolony znak (inny niż 0 lub 1).");
            else
            {
                string binaryText = TextBox_BinaryText.Text;
                string binaryKey = TextBox_CipherKey.Text;
                string cipheredText = "";

                if (binaryText.Length > binaryKey.Length)
                {
                    encryptor.ShowPopUp("Klucz jest zbyt krótki, dlatego zostanie powielony do odpowiedniej długości.", "Za krótki klucz!");
                    encryptor.RepeatKeyToMatchLength(binaryText, ref binaryKey);
                    TextBox_CipherKey.Text = binaryKey;
                }

                cipheredText = encryptor.CipherBinaryStringUsingBinaryString(binaryText, binaryKey);
                TextBox_CipheredText.Text = cipheredText;
                TextBox_CipheredTextAsChars.Text = encryptor.convertBitStringToString(cipheredText);
            }
        }

        private void TextBox_PlainText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Label_PlainTextLength.Content = "Długość tekstu do zaszyfrowania: " + TextBox_PlainText.Text.Length;
            TextBox_BinaryText.Text = encryptor.ConvertStringToBinaryString(TextBox_PlainText.Text);
        }

        private void TextBox_BinaryText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Label_BinaryTextLength.Content = "Liczba bitów do zaszyfrowania: " + TextBox_BinaryText.Text.Length;
        }

        private void TextBox_CipherKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            Label_CipherKeyLength.Content = "Liczba bitów klucza szyfrowania: " + TextBox_CipherKey.Text.Length;
        }

        private void TextBox_CipheredText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Label_CipheredTextLength.Content = "Liczba bitów szyfrogramu: " + TextBox_CipheredText.Text.Length;
        }

        private void TextBox_CipheredTextAsChars_TextChanged(object sender, TextChangedEventArgs e)
        {
            Label_CipheredTextAsCharsLength.Content = "Długość szyfrogramu: " + TextBox_CipheredTextAsChars.Text.Length;
        }

        private void Button_ClearAllFields_Click(object sender, RoutedEventArgs e)
        {
            TextBox_PlainText.Text = "";
            TextBox_BinaryText.Text = "";
            TextBox_CipherKey.Text = "";
            TextBox_CipheredText.Text = "";
            TextBox_CipheredTextAsChars.Text = "";
        }

        private void Button_SaveCipherAsChars_Click(object sender, RoutedEventArgs e)
        {
            encryptor.SaveToFileAsTextString(TextBox_CipheredTextAsChars.Text);
        }

        private void Button_SaveCipherBinary_Click(object sender, RoutedEventArgs e)
        {
            encryptor.SaveToFileAsTextString(TextBox_CipheredText.Text);
        }
    }
}
