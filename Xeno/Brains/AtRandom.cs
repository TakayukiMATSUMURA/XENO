using System;
using System.Collections.Generic;

namespace XENO.Brains
{
    public class AtRandom : IBrain
    {
        public int MakeDecision(List<int> cardNumbers)
        {
            return cardNumbers[new Random().Next(0, cardNumbers.Count)];
        }

        public int MakeDecisionOnSoldier()
        {
            return new Random().Next(1, 10 + 1);
        }

        public int MakeDecisionOnSage(List<int> cardNumbers)
        {
            return cardNumbers[new Random().Next(0, cardNumbers.Count)];
        }

        public int MakeDecisionOnPublicExecution(List<int> cardNumbers)
        {
            return cardNumbers[new Random().Next(0, cardNumbers.Count)];
        }
    }

}
