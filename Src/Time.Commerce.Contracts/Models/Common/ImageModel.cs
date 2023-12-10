namespace Time.Commerce.Contracts.Models.Common
{
    public partial class ImageModel
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string MimeType { get; set; }
        public string Title { get; set; }
        public string AlternateText { get; set; }
        public int Order { get; set; }
    }
}
