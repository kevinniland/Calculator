using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Calculator_v3
{
    public sealed partial class MainPage : Page
    {
        String calculation; // Used to denote what type of calculation is to be performed.
        double number1; // Stores the first number entered
        double number2; // Stores the second number entered
        double total; // Stores the result of the calculation

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Saves the total
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            double currTotal = (double)localSettings.Values["Current Total"];

            if (total > currTotal)
            {
                localSettings.Values["Current Total"] = total;
            }
        }

        // Prints out a number to a screen. 
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            /* The text of the input box is updated to show the value of whatever button is pressed.
             * Gets the Button object and the content and prints the value. */
            inputBox.Text += ((Button)sender).Content.ToString();
        }

        // Adds a decimal point before or after a number
        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            // Checks if there is a decimal point already in the inputBox
            if (inputBox.Text.Contains("."))
            {
                // if there is, leave the inputBox as it is
                inputBox.Text = inputBox.Text;
            }
            // else, prints out a decimal point
            else
            {
                inputBox.Text += ".";
            }
        }

        // Prints the division sign to the screen and will subtract the two numbers when the equals button is clicked
        private void Divide_Click(object sender, RoutedEventArgs e)
        {
            // Checks if the lenght of the inputBox is greater than 0
            if (inputBox.Text.Length > 0)
            {
                // Parses the first number into the inputBox
                number1 = double.Parse(inputBox.Text);

                calculation = "/"; // Dictates what calculation to perform through the switch statement below
                inputBox.Text = "";
                inputBox.Text = number1 + "÷"; // Updates the input box to show the the first number plus the operand e.g 1 ÷
            }
        }

        // Prints the multiplication sign to the screen and will subtract the two numbers when the equals button is clicked
        private void Multiply_Click(object sender, RoutedEventArgs e)
        {

            if (inputBox.Text.Length > 0)
            {
                number1 = double.Parse(inputBox.Text);

                calculation = "*";
                inputBox.Text = "";
                inputBox.Text = number1 + "x";
            }
        }

        // Prints the subtraction sign to the screen and will subtract the two numbers when the equals button is clicked
        private void Subtract_Click(object sender, RoutedEventArgs e)
        {
            if (inputBox.Text.Length > 0)
            {
                number1 = double.Parse(inputBox.Text);

                calculation = "-";
                inputBox.Text = "";
                inputBox.Text = number1 + "-";
            }
        }

        // Prints the addition sign to the screen and will subtract the two numbers when the equals button is clicked
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (inputBox.Text.Length > 0)
            {
                number1 = double.Parse(inputBox.Text);

                calculation = "+";
                inputBox.Text = "";
                inputBox.Text = number1 + "+";
            }
        }

        // Prints the percentage sign to the screen
        private void Percentage_Click(object sender, RoutedEventArgs e)
        {
            if (inputBox.Text.Length > 0)
            {
                number1 = double.Parse(inputBox.Text);

                calculation = "%";
                inputBox.Text = "";
                inputBox.Text = number1 + "%";
            }
        }

        // Clears the whole 'screen'
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            inputBox.Text = ""; // Will 'reset' the inputBox
            outputBox.Text = ""; // Will 'reset' the outputBox
        }

        // Changes the value of a number to either its positive or negative value, depending on its current state
        private void PositiveNegative_Click(object sender, RoutedEventArgs e)
        {
            // Checks if a number starts with a minus sign
            if (inputBox.Text.StartsWith("-"))
            {
                inputBox.Text = inputBox.Text.Substring(1);
            }
            // else if, the inputBox isn't null or empty and the number in the inputBox isn't 0
            else if (!string.IsNullOrEmpty(inputBox.Text) && decimal.Parse(inputBox.Text) != 0)
            {
                // Prints out a minus sign before the number
                inputBox.Text = "-" + inputBox.Text;
            }
        }

        /* Performs the relevant calculation when clicked. First, it splits the string to make sure no operand is being parsed.
         * Uses a switch statement to determine what calculation is to be performed, then calls the appropriate function. Then prints
         * the total to the screen. */
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            // Checks if the length of the input box is greater than 0
            if (inputBox.Text.Length > 0)
            {
                char[] operations = { '+', '-', '\\', '*', '%' }; // Array of the all the operations that will be performed
                string[] inputs = inputBox.Text.Split(operations); // Array of the all the inputs that will beentered and subsequently splits the the operands from the inputs

                // if the length of the inputs entered is 2
                if (inputs.Length == 2)
                {
                    bool pass = double.TryParse(inputs[0], out number1);
                    pass = double.TryParse(inputs[1], out number2);
                }

                // switch statement to perform the various calculations
                switch (calculation)
                {
                    // Calls the division calculation and prints the result 
                    case "/":
                        Divide_Calculation();
                        outputBox.Text = total.ToString();
                        break;
                    // Calls the multiplication calculation and prints the result
                    case "*":
                        Multiply_Calculation();
                        outputBox.Text = total.ToString();
                        break;
                    // Calls the subtraction calculation and prints the result
                    case "-":
                        Subtract_Calculation();
                        outputBox.Text = total.ToString();
                        break;
                    // Calls the addition calculation and prints the result
                    case "+":
                        Add_Calculation();
                        outputBox.Text = total.ToString();
                        break;
                    // Calls the percentage calculation and prints the result
                    case "%":
                        Percentage_Calculation();
                        outputBox.Text = total.ToString();
                        break;
                }
            }
        }

        // Divides two numbers
        private void Divide_Calculation()
        {
            // Checks if the second number entered is 0
            if (number2 == 0)
            {
                // if it is, prints an error as a number cannot be divided by 0
                inputBox.Text = number1 + " cannot be divided by 0. Please try again.";
            }
            // else, divides the two numbers
            else
            {
                total = number1 / number2;
            }
        }

        // Multiplys two numbers
        private void Multiply_Calculation()
        {
            total = number1 * number2;
        }

        // Subtracts two numbers
        private void Subtract_Calculation()
        {
            total = number1 - number2;
        }

        // Adds two numbers
        private void Add_Calculation()
        {
            total = number1 + number2;
        }

        // Gets the appropriate percentage calculation. For example, 10 % 1000 reads as "10 percent of 1000"
        private void Percentage_Calculation()
        {
            number1 *= number2;

            total = number1 / 100;
        }
    }
}
