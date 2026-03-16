using ZombieRescue.Core;

namespace ZombieRescue.Modes
{
    public interface IGameModeController
    {
        void Initialize(GameSessionManager session);
        void OnMatchStart();
        void OnMatchEnd(bool victory);
        void OnPlayerDied(int playerIndex);
        void CheckVictoryConditions();
    }
}
