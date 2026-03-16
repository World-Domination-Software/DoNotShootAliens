using UnityEngine;

namespace ZombieRescue.Scoring
{
    /// <summary>
    /// Defines the points and multipliers used to compute the final match score.
    /// </summary>
    [CreateAssetMenu(fileName = "ScoreProfile", menuName = "ZombieRescue/Score Profile")]
    public class ScoreProfile : ScriptableObject
    {
        [Header("Awards")]
        [Tooltip("Points earned for each zombie eliminated.")]
        [SerializeField] public int pointsPerZombieKill = 100;
        [Tooltip("Points earned for each survivor successfully rescued.")]
        [SerializeField] public int pointsPerSurvivorRescued = 500;

        [Header("Penalties")]
        [Tooltip("Points deducted for each survivor lost.")]
        [SerializeField] public int penaltyPerSurvivorLost = 200;
        [Tooltip("Points deducted per point of damage taken (across all players).")]
        [SerializeField] public int penaltyPerDamageTaken = 1;

        [Header("Bonuses")]
        [Tooltip("Bonus score per second remaining on the clock when the match is won.")]
        [SerializeField] public float timeBonusPerSecondRemaining = 10f;
        [Tooltip("Multiplier applied to the score when the player achieves perfect accuracy.")]
        [SerializeField] public float accuracyBonusMultiplier = 1.5f;
    }
}
