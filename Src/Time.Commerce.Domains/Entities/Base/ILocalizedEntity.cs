namespace Time.Commerce.Domains.Entities.Base
{
    /// <summary>
    /// Represents a localized entity. Use for miultilanguages
    /// </summary>
    public interface ILocalizedEntity
    {
        IList<LocalizedProperty> Locales { get; set; }
    }
}
