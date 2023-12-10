﻿namespace Time.Commerce.Domains.Entities.Base
{
    /// <summary>
    /// Represents a localized property
    /// </summary>
    public partial class LocalizedProperty
    {
        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public string LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the locale key
        /// </summary>
        public string LocaleKey { get; set; }

        /// <summary>
        /// Gets or sets the locale value
        /// </summary>
        public string LocaleValue { get; set; }

        /// <summary>
        /// This used if content so long, we need use more document to save its
        /// </summary>
        public string DocumentId { get; set; }
    }
}
