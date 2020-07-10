using System;

namespace Ecommerce.Core
{
    public class Order
    {
        public Order(
            String userId,
            String orderId,
            int amount)
        {
            UserId = userId;
            OrderId = orderId;
            Amount = amount;
        }

        public string UserId { get; }
        public string OrderId { get; }
        public int Amount { get; }

        public override string ToString() => string.Join("|", UserId, OrderId, Amount);
    }
}