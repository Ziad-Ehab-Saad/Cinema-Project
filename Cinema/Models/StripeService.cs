using Stripe.Checkout;

namespace Cinema.Models
{
    public class StripeService
    {
        public string CreateCheckoutSession(Order order, HttpContext httpContext)
        {
            string baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new Exception("Order has no items. Cannot create a Stripe checkout session.");
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = order.OrderItems.Select(item =>
                {
                    if (item.Movie == null)
                    {
                        throw new Exception($"Movie not found for OrderItem with ID {item.Id}");
                    }

                    return new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Movie.Name,
                            },
                            UnitAmount = (long)(item.Price * 100) // Convert to cents
                        },
                        Quantity = item.Quantity
                    };
                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{baseUrl}/Customer/Cart/PaymentSuccess?orderId={order.Id}",
                CancelUrl = $"{baseUrl}/Customer/Cart/PaymentFailed?orderId={order.Id}",

            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session.Url;
        }

    }
}
