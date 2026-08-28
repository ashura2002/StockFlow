
using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public record CategoryNameVo
    {
        public string Value { get; }

        private CategoryNameVo(string value)
        {
            Value = value;
        }

        public static CategoryNameVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainBadRequestException("Category name cannot be empty.");

            value = value.Trim();
            if (value.Length <= 3)
                throw new DomainBadRequestException("Category name must above 4 characters.");

            return new CategoryNameVo(value);
        }
    }
}
