namespace DataVolumeManager
{
    public class FolderRule
    {
        public string Path { get; set; }
        public int KeepDays { get; set; }
        public bool Enabled { get; set; }
        public string Extensions { get; set; }
        public bool Recursive { get; set; }
        public int MaxDepth { get; set; }
        public bool DryRun { get; set; }
    }
}
