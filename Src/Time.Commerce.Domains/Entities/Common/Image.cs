namespace Time.Commerce.Domains.Entities.Common
{
    public partial class Image
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string MimeType { get; set; }
        public string Title { get; set; }
        public string AlternateText { get; set; }
        public int Order { get; set; }
    }
}
