using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            int totalAmount = PerformanceAmount();
            int volumeCredits = PeformanceAmount();
            string customerInvoiceStatement = CustomerInvoiceStatement(invoice);
            NewPerformance(invoice, plays, ref totalAmount, ref volumeCredits, ref customerInvoiceStatement);
            customerInvoiceStatement = PerformanceResults(totalAmount, volumeCredits, customerInvoiceStatement);
            return customerInvoiceStatement;
        }

        private static string PerformanceResults(int totalAmount, int volumeCredits, string result)
        {
            result += String.Format(SetCultureInfo(), "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static void NewPerformance(Invoice invoice, Dictionary<string, Play> plays, ref int totalAmount, ref int volumeCredits, ref string result)
        {
            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                var thisAmount = 0;
                switch (play.Type)
                {
                    case "tragedy":
                        thisAmount = 40000;
                        if (perf.Audience > 30)
                        {
                            thisAmount += 1000 * (perf.Audience - 30);
                        }
                        break;
                    case "comedy":
                        thisAmount = 30000;
                        if (perf.Audience > 20)
                        {
                            thisAmount += 10000 + 500 * (perf.Audience - 20);
                        }
                        thisAmount += 300 * perf.Audience;
                        break;
                    default:
                        throw new Exception("unknown type: " + play.Type);
                }
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                result += String.Format(SetCultureInfo(), "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }
        }

        private static string CustomerInvoiceStatement(Invoice invoice)
        {
            return string.Format("Statement for {0}\n", invoice.Customer);
        }

        private static int PeformanceAmount()
        {
            return 0;
        }

        private static int PerformanceAmount()
        {
            return 0;
        }

        private static CultureInfo SetCultureInfo()
        {
            return new CultureInfo("en-US");
        }
    }
}
