namespace Time.Commerce.Contracts.Views.Cms
{
    public class FolderView
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public List<FolderView> Children { get; set; } = new List<FolderView>();
    }
}
