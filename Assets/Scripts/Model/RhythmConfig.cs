namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class RhythmConfig : IToolConfig
    {
        public Types.RhythmMode mode;

        public int nbNotes;
        public int bpm;
        
        public int repetitions;
        public int nbNotesPerRepetitions;
    }
}
