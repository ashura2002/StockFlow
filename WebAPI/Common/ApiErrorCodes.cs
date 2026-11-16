namespace WebAPI.Common
{
    public static class ApiErrorCodes
    {
        public const string ValidationError = "VALIDATION_ERROR";
        public const string DomainRuleValidation = "DOMAIN_RULE_VALIDATION";
        public const string ConcurrencyException = "CONCURRENCY_CONFLICT";
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
    }
}
