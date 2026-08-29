using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using FluentAssertions;

namespace Domain.Tests.Orders;

public sealed class OrderTests
{
    [Fact]
    public void Create_ShouldCreatePendingOrder()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var order = Order.Create(userId);

        // Assert
        order.UserId.Should().Be(userId);
        order.Status.Should().Be(OrderStatus.Pending);
    }

    [Fact]
    public void Create_ShouldRaiseOrderCreatedDomainEvent()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var order = Order.Create(userId);

        // Assert
        order.DomainEvents
            .Should()
            .Contain(eventItem => eventItem is OrderCreatedDomainEvent);
    }

    [Fact]
    public void AddItem_ShouldAddNewItem()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var quantity = 2;
        var unitPrice = 100m;

        // Act
        order.AddItem(productId, quantity, unitPrice);

        // Assert
        order.OrderItems
            .Should()
            .Contain(item =>
                item.ProductId == productId &&
                item.Quantity == quantity &&
                item.UnitPrice == unitPrice);
    }


    // [Theory] allows us to test the same behavior using multiple input values.
    // [InlineData] provides the test data for each test case.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void AddItem_WhenQuantityIsInvalid_ShouldThrow(int quantity)
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());

        // Act
        var act = () => order.AddItem(
            Guid.NewGuid(),
            quantity,
            100m);

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Quantity must be greater than 0.");
    }

    [Fact]
    public void AddItem_WhenProductAlreadyExists_ShouldIncreaseQuantity()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();

        order.AddItem(productId, 2, 100m);

        // Act
        order.AddItem(productId, 3, 100m);

        // Assert
        order.OrderItems.Should().ContainSingle();

        var item = order.OrderItems.Single();

        item.Quantity.Should().Be(5);
        item.UnitPrice.Should().Be(100m);
    }

    [Fact]
    public void AddItem_ShouldCalculateTotalPrice()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());

        order.AddItem(Guid.NewGuid(), 2, 100m);
        order.AddItem(Guid.NewGuid(), 3, 50m);

        // Act
        var totalPrice = order.TotalPrice;

        // Assert
        totalPrice.Should().Be(350m);
    }

    [Fact]
    public void RemoveItem_WhenItemExists_ShouldRemoveItem()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();

        order.AddItem(productId, 2, 100m);

        // Act
        order.RemoveItem(productId);

        // Assert
        order.OrderItems.Should().BeEmpty();
    }

    [Fact]
    public void RemoveItem_WhenItemDoesNotExist_ShouldDoNothing()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());

        // Act
        order.RemoveItem(Guid.NewGuid());

        // Assert
        order.OrderItems.Should().BeEmpty();
    }

    [Fact]
    public void ConfirmOrder_WhenPending_ShouldConfirmOrder()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());

        // Act
        order.ConfirmOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Confirmed);
    }

    [Fact]
    public void ConfirmOrder_WhenNotPending_ShouldThrow()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());
        order.CancelOrder(OrderCancellationSource.Customer);

        // Act
        var act = () => order.ConfirmOrder();

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Only pending order can be confirmed.");
    }

    [Fact]
    public void ConfirmOrder_WhenAlreadyConfirmed_ShouldDoNothing()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());
        order.ConfirmOrder();

        // Act
        order.ConfirmOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Confirmed);
    }

    [Fact]
    public void CancelOrder_WhenPending_ShouldCancelOrder()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());

        // Act
        var result = order.CancelOrder(
            OrderCancellationSource.Customer);

        // Assert
        result.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void CancelOrder_WhenAlreadyCancelled_ShouldReturnFalse()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());

        order.CancelOrder(
            OrderCancellationSource.Customer);

        // Act
        var result = order.CancelOrder(
            OrderCancellationSource.Customer);

        // Assert
        result.Should().BeFalse();
        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void CancelOrder_WhenNotPending_ShouldThrow()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());
        order.ConfirmOrder();

        // Act
        var act = () => order.CancelOrder(
            OrderCancellationSource.Customer);

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Only pending order can be cancelled.");
    }

    [Fact]
    public void CompleteOrder_WhenConfirmed_ShouldCompleteOrder()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());
        order.ConfirmOrder();

        // Act
        order.CompleteOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public void CompleteOrder_WhenPending_ShouldThrow()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());

        // Act
        var act = () => order.CompleteOrder();

        // Assert
        act.Should()
            .Throw<DomainBadRequestException>()
            .WithMessage("Only confirmed orders can be completed");
    }

    [Fact]
    public void CompleteOrder_WhenAlreadyCompleted_ShouldDoNothing()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid());
        order.ConfirmOrder();
        order.CompleteOrder();

        // Act
        order.CompleteOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Completed);
    }
}