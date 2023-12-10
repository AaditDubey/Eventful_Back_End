﻿using Time.Commerce.Contracts.Models.Common;

namespace Time.Commerce.Contracts.Models.Cms
{
    public class MessageQueryModel : BaseQueryModel
    {
        public string StoreId { get; set; }
        public string Type { get; set; }
        public bool? Published { get; set; }
    }
}
