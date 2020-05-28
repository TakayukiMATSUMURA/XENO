using System.Collections.Generic;

namespace XENO.Brains
{
    public interface IBrain
    {
        int MakeDecision(List<int> cardNumbers);
        int MakeDecisionOnSoldier();
        int MakeDecisionOnSage(List<int> cardNumbers);
        int MakeDecisionOnPublicExecution(List<int> cardNumbers);
    }
}
