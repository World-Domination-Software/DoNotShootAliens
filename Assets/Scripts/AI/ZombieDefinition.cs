using UnityEngine;

namespace ZombieRescue.AI
{
    /// <summary>
    /// Immutable zombie blueprint defined in the editor via a ScriptableObject asset.
    /// </summary>
    [CreateAssetMenu(fileName = "ZombieDefinition", menuName = "ZombieRescue/Zombie Definition")]
    public class ZombieDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string zombieName = "Zombie";

        [Header("Stats")]
        [SerializeField] private float maxHealth      = 100f;
        [SerializeField] private float moveSpeed      = 2.5f;
        [SerializeField] private float attackDamage   = 10f;
        [SerializeField] private float attackRange    = 1.5f;
        [SerializeField] private float detectionRadius = 15f;
        [SerializeField] private float attackCooldown = 1f;

        [Header("Scoring")]
        [SerializeField] private int scoreValue = 100;

        // ── Public read-only properties ────────────────────────────────────────
        public string ZombieName       => zombieName;
        public float  MaxHealth        => maxHealth;
        public float  MoveSpeed        => moveSpeed;
        public float  AttackDamage     => attackDamage;
        public float  AttackRange      => attackRange;
        public float  DetectionRadius  => detectionRadius;
        public float  AttackCooldown   => attackCooldown;
        public int    ScoreValue       => scoreValue;
    }
}
