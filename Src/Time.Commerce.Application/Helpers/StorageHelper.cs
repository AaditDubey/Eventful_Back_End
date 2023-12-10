namespace Time.Commerce.Application.Helpers
{
    public static class StorageHelper
    {
        public static string GetCurrentDirectory()
        {
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(currentDirectory))
                Directory.CreateDirectory(currentDirectory);
            return currentDirectory;
        }

        public static string GetFilePath(string filePath)
            => Path.Combine(GetCurrentDirectory(), filePath);
    }
}
