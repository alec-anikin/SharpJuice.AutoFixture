namespace SharpJuice.AutoFixture.Tests.TestObjects
{
    public sealed class BlackMirror
    {
        public string Season { get; }
        public int Episode { get; }

        public BlackMirror(string season, int episode)
        {
            Season = season;
            Episode = episode;
        }
    }
}