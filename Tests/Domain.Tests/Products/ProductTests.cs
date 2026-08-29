using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.Products;

public sealed class ProductTests
{
    // specific behavior with fixed inputs
    [Fact]
    public void Create_ShouldCreateProduct()
    {
        // Arrange
        var productName = ProductNameVo.Create("Laptop");
        var price = 1000m;
        var stock = 10;
        var categoryId = Guid.NewGuid();
        var supplierId = Guid.NewGuid();

        // Act
        var product = Product.Create(
            productName,
            price,
            stock,
            categoryId,
            supplierId);

        // Assert
        product.ProductName.Should().Be(productName);
        product.Price.Should().Be(price);
        product.Stock.Should().Be(stock);
        product.CategoryId.Should().Be(categoryId);
        product.SupplierId.Should().Be(supplierId);
    }

    // [Theory] allows us to test the same behavior using multiple input values.
    // [InlineData] provides the test data for each test case.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_WhenPriceIsInvalid_ShouldThrow(decimal price)
    {
        // Arrange
        var productName = ProductNameVo.Create("Laptop");

        // Act
        var act = () => Product.Create(
            productName,
            price,
            10,
            Guid.NewGuid(),
            Guid.NewGuid());

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Price must be greater than 0.");
    }

    // [Theory] allows us to test the same behavior using multiple input values.
    // [InlineData] provides the test data for each test case.
    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Create_WhenStockIsNegative_ShouldThrow(int stock)
    {
        // Arrange
        var productName = ProductNameVo.Create("Laptop");

        // Act
        var act = () => Product.Create(
            productName,
            1000m,
            stock,
            Guid.NewGuid(),
            Guid.NewGuid());

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Stock cannot be negative.");
    }

    [Fact]
    public void UpdatePrice_WhenProductIsNotDeleted_ShouldUpdatePrice()
    {
        // Arrange
        var product = CreateProduct();

        // Act
        product.UpdatePrice(2000m);

        // Assert
        product.Price.Should().Be(2000m);
    }

    [Fact]
    public void UpdatePrice_WhenPriceIsInvalid_ShouldThrow()
    {
        // Arrange
        var product = CreateProduct();

        // Act
        var act = () => product.UpdatePrice(0);

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Price must be greater than 0.");
    }

    [Fact]
    public void UpdateProductStock_WhenStockIsValid_ShouldUpdateStock()
    {
        // Arrange
        var product = CreateProduct();

        // Act
        product.UpdateProductStock(20);

        // Assert
        product.Stock.Should().Be(20);
    }

    [Fact]
    public void UpdateProductStock_WhenStockIsNegative_ShouldThrow()
    {
        // Arrange
        var product = CreateProduct();

        // Act
        var act = () => product.UpdateProductStock(-1);

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Stock cannot be negative.");
    }

    [Fact]
    public void UpdateProductName_WhenProductIsNotDeleted_ShouldUpdateName()
    {
        // Arrange
        var product = CreateProduct();
        var newName = ProductNameVo.Create("Keyboard");

        // Act
        product.UpdateProductName(newName);

        // Assert
        product.ProductName.Should().Be(newName);
    }

    [Fact]
    public void UpdateProductDescriptions_ShouldUpdateDescription()
    {
        // Arrange
        var product = CreateProduct();

        // Act
        product.UpdateProductDescriptions("New description");

        // Assert
        product.ProductDescriptions.Should().Be("New description");
    }

    [Fact]
    public void UpdateProductImage_ShouldUpdateImageInformation()
    {
        // Arrange
        var product = CreateProduct();

        // Act
        product.UpdateProductImage(
            "https://example.com/image.jpg",
            "public-id");

        // Assert
        product.ProductImageUrl.Should()
            .Be("https://example.com/image.jpg");

        product.ProductImagePublicId.Should()
            .Be("public-id");
    }

    [Fact]
    public void SoftDelete_ShouldSetDeletedAt()
    {
        // Arrange
        var product = CreateProduct();

        // Act
        product.SoftDelete();

        // Assert
        product.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public void SoftDelete_WhenAlreadyDeleted_ShouldDoNothing()
    {
        // Arrange
        var product = CreateProduct();
        product.SoftDelete();

        var deletedAt = product.DeletedAt;

        // Act
        product.SoftDelete();

        // Assert
        product.DeletedAt.Should().Be(deletedAt);
    }

    [Fact]
    public void DecreaseStock_WhenQuantityIsAvailable_ShouldDecreaseStock()
    {
        // Arrange
        var product = CreateProduct(stock: 10);

        // Act
        product.DecreaseStock(3);

        // Assert
        product.Stock.Should().Be(7);
    }

    [Fact]
    public void DecreaseStock_WhenQuantityExceedsStock_ShouldThrow()
    {
        // Arrange
        var product = CreateProduct(stock: 10);

        // Act
        var act = () => product.DecreaseStock(11);

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Out of stock.");
    }

    [Fact]
    public void IncreaseStock_WhenQuantityIsValid_ShouldIncreaseStock()
    {
        // Arrange
        var product = CreateProduct(stock: 10);

        // Act
        product.IncreaseStock(5);

        // Assert
        product.Stock.Should().Be(15);
    }

    // [Theory] allows us to test the same behavior using multiple input values.
    // [InlineData] provides the test data for each test case.
    [Theory]
    [InlineData(0)]    // boundary value
    [InlineData(-1)]  // negative
    [InlineData(-10)] // another negative value
    public void IncreaseStock_WhenQuantityIsInvalid_ShouldThrow(int quantity)
    {
        // Arrange
        var product = CreateProduct();

        // Act
        var act = () => product.IncreaseStock(quantity);

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Quantity must be greater than 0.");
    }

    [Fact]
    public void UpdatePrice_WhenProductIsDeleted_ShouldThrow()
    {
        // Arrange
        var product = CreateProduct();
        product.SoftDelete();

        // Act
        var act = () => product.UpdatePrice(2000m);

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Can't update product price if product is deleted.");
    }

    [Fact]
    public void UpdateProductStock_WhenProductIsDeleted_ShouldThrow()
    {
        // Arrange
        var product = CreateProduct();
        product.SoftDelete();

        // Act
        var act = () => product.UpdateProductStock(20);

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Can't update product stock if product is deleted.");
    }

    private static Product CreateProduct(
        int stock = 10)
    {
        return Product.Create(
            ProductNameVo.Create("Laptop"),
            1000m,
            stock,
            Guid.NewGuid(),
            Guid.NewGuid());
    }
}