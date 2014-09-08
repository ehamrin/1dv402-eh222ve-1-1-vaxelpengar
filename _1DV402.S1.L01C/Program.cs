using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1DV402.S1.L01C.Properties;

namespace _1DV402.S1.L01C
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = Resources.applicationTitle;
            ConsoleKeyInfo keyPress;

            do
            {
                /****************************************************************
                 * Clear any previous content every time loop starts. */
                Console.Clear();

                /****************************************************************
                 * Retrieve user input on Amount to be paid and received payment.*/
                double subtotal = ReadPositiveDouble(Resources.Cost_Prompt);
                uint paid = ReadUint(Resources.Cash_Prompt, subtotal);

                /****************************************************************
                 * Calculate decimal points to be rounded off. */
                uint total = (uint)Math.Round(subtotal, 0);
                double roundingOffAmount = Math.Round((total - subtotal), 2);

                /****************************************************************
                 * Calculate change to be returned.*/
                uint change = paid - total;

                /****************************************************************
                 * Split change into denominations to be returned. */
                uint[] denominations = new uint[] { 500, 100, 50, 20, 10, 5, 1 };
                uint[] notes = SplitIntoDenominations(change, denominations);

                /****************************************************************
                 * Print receipt. */
                ViewReceipt(subtotal, roundingOffAmount, total, paid, change, denominations, notes);

                /****************************************************************
                 * Check for user input of the Escape-key for termination of loop.*/
                ViewMessage(Resources.Continue_Prompt);
                keyPress = Console.ReadKey();

            } while (keyPress.Key != ConsoleKey.Escape);

        }

        static double ReadPositiveDouble(string message)
        {
            bool inputValid = false;
            double inputNumber = 0;

            do
            {
                Console.Write(message);
                string input = Console.ReadLine();

                try
                {
                    //Throws an Exception if NaN input.
                    inputNumber = Convert.ToDouble(input);

                    //Checks if valid amount, else - throw an Exception.
                    inputNumber = Math.Round(inputNumber, 2);
                    double inputToCheck = Math.Round(inputNumber, 0);

                    if (inputToCheck > 0)
                    {
                        inputValid = true;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                catch
                {
                    ViewMessage(Resources.invalidPositiveDouble, true);
                }

            } while (inputValid == false);
            return inputNumber;
        }

        static uint ReadUint(string message, double lowestAmount)
        {
            bool inputValid = false;
            uint inputNumber = 0;

            do
            {
                Console.Write(message);
                string input = Console.ReadLine();
                try
                {
                    //Catches any NaN inputs
                    inputNumber = Convert.ToUInt32(input);

                    //Checks if valid amount
                    if (inputNumber >= lowestAmount)
                    {
                        inputValid = true;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                catch
                {
                    ViewMessage(Resources.invalidPayment, true);
                }
            } while (inputValid == false);
            return inputNumber;
        }

        static uint[] SplitIntoDenominations(uint change, uint[] denominations)
        {
            uint[] denominationReturn = new uint[denominations.Length];

            for (int i = 0; i < denominations.Length; i++)
            {
                uint changeReturn = change / denominations[i];

                change = change % denominations[i];

                denominationReturn[i] = changeReturn;
            }
            return denominationReturn;
        }

        static void ViewMessage(string message, bool isError = false)
        {
            if (isError)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message);
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message);
            }

            Console.ResetColor();
        }

        static void ViewReceipt(double subtotal, double roundingOffAmount, uint total, uint paid, uint change, uint[] denominations, uint[] notes)
        {

            Console.WriteLine();
            Console.WriteLine("Receipt");
            Console.WriteLine("* * * * * * * * * * * * * * * *");
            Console.WriteLine("Subtotal:\t\t{0,5}kr", subtotal);
            Console.WriteLine("Rounding off:\t\t{0,5}kr", roundingOffAmount);
            Console.WriteLine("Total:\t\t\t{0,5}kr", total);
            Console.WriteLine("Received payment:\t{0,5}kr", paid);
            Console.WriteLine("Change:\t\t\t{0,5}kr", change);
            Console.WriteLine("* * * * * * * * * * * * * * * *");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Value\t|\t#");
            Console.WriteLine("-------------------------------");
            for (int i = 0; i < denominations.Length; i++)
            {
                if (notes[i] > 0)
                {
                    Console.WriteLine("{0}\t:\t{1}", denominations[i], notes[i]);
                }
            }
        }
    }
}
