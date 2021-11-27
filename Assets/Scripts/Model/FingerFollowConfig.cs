namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class FingerFollowConfig : IToolConfig
    {
        public int repetitions = 5;
        public Types.Mode mode;
    }
}