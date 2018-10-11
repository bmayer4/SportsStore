using System;
using System.Collections.Generic;
using System.Text;
using SportsStore.Controllers;
using SportsStore.Models;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            //Arrange - create a mock repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Arrange - create an empty cart 
            Cart cart = new Cart();

            // Arrange - create the order
            Order order = new Order();

            // Arrange - create an instance of the controller
            OrderController target = new OrderController(mock.Object, cart);

            // Act 
            ViewResult result = target.Checkout(order) as ViewResult;

            // Assert - check that the order hasn't been stored 
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // Assert - check that the method is returning the default view 
            Assert.True(string.IsNullOrEmpty(result.ViewName));  //s.o. - If the name of the view was not specified, you cannot determine it from the ViewResult returned by the action

            // Assert - check that I am passing an invalid model to the view
            Assert.False(result.ViewData.ModelState.IsValid);

        }


        public void Cannot_Checkout_Invalid_Shipping_Details()
        {
            //Arrange - create a mock repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Arrange - create an cart with one item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - create an instance of the controller
            OrderController target = new OrderController(mock.Object, cart);

            //Arrange - add an error to the model
            target.ModelState.AddModelError("error", "error");

            // Act  - try to checkout
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            // Assert - check that the order hasn't been passed to store
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // Assert - check that the method is returning the default view 
            Assert.True(string.IsNullOrEmpty(result.ViewName)); 

            // Assert - check that I am passing an invalid model to the view
            Assert.False(result.ViewData.ModelState.IsValid);

        }

        public void Can_Checkout_And_Submit_Order()
        {
            //Arrange - create a mock repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Arrange - create a cart with one item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - create an instance of the controller
            OrderController target = new OrderController(mock.Object, cart);

            // Act  - try to checkout
            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            // Assert - check that the order has been stored 
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);

            // Assert - check that the method is redirecting to the Completed action
            Assert.Equal("Completed", result.ActionName);  

        }
    }
}
