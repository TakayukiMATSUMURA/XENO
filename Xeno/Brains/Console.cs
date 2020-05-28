using System;
using System.Collections.Generic;

namespace XENO.Brains
{
    public class ConsoleInput : IBrain
    {
        public int MakeDecision(List<int> cardNumbers)
        {
            return GetNumberFromReadLine(cardNumbers);
        }

        public int MakeDecisionOnSoldier()
        {
            return GetNumberFromReadLine(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        public int MakeDecisionOnPublicExecution(List<int> cardNumbers)
        {
            return GetNumberFromReadLine(cardNumbers);
        }

        public int MakeDecisionOnSage(List<int> cardNumbers)
        {
            return GetNumberFromReadLine(cardNumbers);
        }

        private int GetNumberFromReadLine(List<int> cardNumbers)
        {
            var result = 0;
            var input = string.Empty;
            while (!cardNumbers.Contains(result))
            {
                try
                {
                    Log.Output($"選択するカードの番号を入力してください:({string.Join(" or ", cardNumbers.ToArray())}).");
                    input = Console.ReadLine();
                    result = int.Parse(input);
                }
                catch (Exception)
                {
                    Log.Output($"入力が不正:{input}.");
                }
            }
            return result;
        }
    }

}
