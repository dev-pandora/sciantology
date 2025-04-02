public interface IBattleMinigame
{
    void Init(Group playerGroup, Group enemyGroup);
    void Tick();
    void UpdateMinigame();
    void EndMinigame();
    bool IsMinigameComplete { get; }
}