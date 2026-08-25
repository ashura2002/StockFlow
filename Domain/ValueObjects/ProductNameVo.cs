using Domain.Exceptions;


namespace Domain.ValueObjects
{
    public class ProductNameVo
    {
        public string Value { get;  }

        private ProductNameVo(string value)
        {
            Value = value;
        }

        public static ProductNameVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainRuleException("Product name cannot be empty.");
            value = value.Trim();
            value = char.ToUpper(value[0]) + value.Substring(1).ToLower();

            return new ProductNameVo(value);
        }
    }
}
