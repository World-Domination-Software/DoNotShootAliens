using UnityEngine;
using UnityEngine.Events;
using ZombieRescue.Modes;
using ZombieRescue.Scoring;
using ZombieRescue.Spawning;

namespace ZombieRescue.Core
{
    public class GameSessionManager : MonoBehaviour
    {
        // ── Singleton ──────────────────────────────────────────────────────────
        public static GameSessionManager Instance { get; private set; }

        // ── Inspector fields ───────────────────────────────────────────────────
        [Header("Configuration")]
        [SerializeField] private MapLevelConfig currentMap;

        [Header("Scene References")]
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private MapSpawnController spawnController;

        // ── Runtime state ──────────────────────────────────────────────────────
        public GameModeType CurrentMode { get; private set; }
        public MatchState CurrentState { get; private set; }
        public MapLevelConfig CurrentMap => currentMap;

        [Header("Events")]
        public UnityEvent<MatchState> OnMatchStateChanged = new UnityEvent<MatchState>();

        private IGameModeController _gameModeController;

        // ── Lifecycle ──────────────────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // ── Public API ─────────────────────────────────────────────────────────
        public void StartMatch(GameModeType mode, MapLevelConfig map)
        {
            CurrentMode = mode;
            currentMap  = map;

            _gameModeController = mode == GameModeType.Coop
                ? (IGameModeController)gameObject.AddComponent<CoopModeController>()
                : (IGameModeController)gameObject.AddComponent<SinglePlayerModeController>();

            _gameModeController.Initialize(this);

            spawnController.SpawnZombies();
            spawnController.SpawnSurvivors();
            spawnController.SpawnPlayers(mode == GameModeType.Coop ? 4 : 1);

            SetMatchState(MatchState.Playing);
            _gameModeController.OnMatchStart();
        }

        public void EndMatch(bool victory)
        {
            _gameModeController.OnMatchEnd(victory);
            // Set the specific outcome first so listeners can react, then move to Results.
            SetMatchState(victory ? MatchState.Victory : MatchState.Defeat);
        }

        public void SetMatchState(MatchState state)
        {
            CurrentState = state;
            OnMatchStateChanged.Invoke(state);
        }
    }
}
