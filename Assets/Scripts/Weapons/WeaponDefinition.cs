using System;
using UnityEngine;

namespace ZombieRescue.Weapons
{
    /// <summary>
    /// Immutable weapon blueprint defined in the editor via a ScriptableObject asset.
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponDefinition", menuName = "ZombieRescue/Weapon Definition")]
    public class WeaponDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string weaponName = "Unnamed Weapon";

        [Header("Firing")]
        [Tooltip("Shots per second.")]
        [SerializeField] private float fireRate = 2f;
        [SerializeField] private float damage = 25f;
        [SerializeField] private float range = 30f;
        [SerializeField] private bool isAutomatic;

        [Header("Ammo")]
        [SerializeField] private int magazineSize = 12;
        [SerializeField] private float reloadDuration = 1.5f;

        [Header("Audio (optional)")]
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private AudioClip reloadSound;

        // ── Public read-only properties ────────────────────────────────────────
        public string    WeaponName     => weaponName;
        public float     FireRate       => fireRate;
        public float     Damage         => damage;
        public float     Range          => range;
        public bool      IsAutomatic    => isAutomatic;
        public int       MagazineSize   => magazineSize;
        public float     ReloadDuration => reloadDuration;
        public AudioClip FireSound      => fireSound;
        public AudioClip ReloadSound    => reloadSound;
    }
}
