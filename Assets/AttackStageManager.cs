using System.Collections.Generic;
using UnityEngine;

public class AttackStageManager : MonoBehaviour
{
    private List<string> attackStages = new List<string> { "SummonMinions", "PawSlam", "Claw", "HairBallRoll" };
    private List<string> usedStages = new List<string>();

    public string GetNextAttackStage()
    {
        if (attackStages.Count == 0)
        {
            Debug.LogWarning("All attack stages have been used. Resetting...");
            ResetStages();
        }

        int randomIndex = Random.Range(0, attackStages.Count);
        string nextStage = attackStages[randomIndex];
        attackStages.RemoveAt(randomIndex);
        usedStages.Add(nextStage);
        return nextStage;
    }

    public bool AllStagesUsed()
    {
        return attackStages.Count == 0;
    }

    public void ResetStages()
    {
        attackStages.AddRange(usedStages);
        usedStages.Clear();
    }
}
