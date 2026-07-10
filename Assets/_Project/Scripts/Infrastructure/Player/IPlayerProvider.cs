namespace _Project.Scripts.Infrastructure.Player
{
    public interface IPlayerProvider
    {
        public Character PlayerTransform { get; }

        public void SetPlayer(Character playerTransform);
    }
}