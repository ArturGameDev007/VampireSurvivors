namespace _Project.Scripts.Infrastructure.Player
{
    public class PlayerProvider : IPlayerProvider
    {
        public Character PlayerTransform { get; private set; }

        public void SetPlayer(Character playerTransform)
        {
            PlayerTransform = playerTransform;
        }
    }
}