namespace Time.Commerce.Core.Constants
{
    public static class CoreSystemConst
    {
        public const string JWT_BEARER_SCHEMA = "Bearer";

        public const string ID_CLAIM_CLIENT_ORG_ID = "id.client.organizationId";
        public const string USER_CLAIMS_SUB = "sub";
        public const string USER_CLAIMS_ROLE = "role";

        /*organizationId*/
        public const string ORGANIZATION_ID = "00000000-0000-0000-0000-000000000001";
        public const string ORGANIZATION_NAME = "Identity";

        /*Role*/
        public const string SYSTEM_ADMIN_ROLE = "SystemAdmin";
        public const string ADMIN_ROLE = "Admin";
        //public const string EMPLOYEE_ROLE = "Employee";
        //public const string USER = "User";
        //public const string GUEST_ROLE = "Guest";
        //public const string VENDOR_ROLE = "Vendor";
        //public const string SELLER_ROLE = "Seller";
        public const string CUSTOMER_ROLE = "Customer";
        public const string EVENT_HOST_ROLE = "Event Host";

        //
        /*Policy*/
        public const string SYSTEM_ADMIN_POLICY = "SystemAdminPolicy";
        public const string ADMIN_POLICY = "AdminPolicy";
        public const string EVENT_HOST_POLICY = "EventHostPolicy";

        /*Common*/
        public const string ADMIN_AREA = "Admin";
        public const string API_ROUTE_FORMAT = "api/v1/[controller]";
        public const string ADMIN_API_ROUTE_FORMAT = "[area]/api/v1/[controller]";

        public const string ACCESS_TOKEN_COOKIE_KEY = "X-T";//X-Access-Token
        public const string REFRESH_TOKEN_COOKIE_KEY = "X-F";//X-Refresh-Token
    }
}
