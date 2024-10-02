using Azure;
using Stripe.Checkout;
namespace FoodOrdering.Services
{
    public class CartService:ICartService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CartService(ApplicationDbContext context,UserManager<IdentityUser>userManager
            ,IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddItem(int productId,int quantity)
        {
            using var transaction=context.Database.BeginTransaction();
            try
            {
                var userId = GetUser();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user not logged in");
                }
                var cart =GetCart(userId);
                if (cart == null)
                {
                    cart = new ShoppingCart()
                    {
                        UserId = userId,
                    };
                    context.ShoppingCarts.Add(cart);
                }
                context.SaveChanges();
                //check if product exist in cart
                var cartItem = context.CartDetails.FirstOrDefault(c => c.ProductId == productId && c.ShoppingCartId == cart.Id);
                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;

                }
                else
                {
                    //add product to cart details
                    cartItem = new CartDetails()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        ShoppingCartId = cart.Id
                    };
                    context.CartDetails.Add(cartItem);

                }
                context.SaveChanges();
                transaction.Commit();
               
            }
            catch (Exception ex)
            {
                return -1;
            }
            var itemCount = await GetCartItemsCount();
            return itemCount;
            
        }

        public async Task<int> GetCartItemsCount(string userId="")
        {
            if(string.IsNullOrEmpty(userId)) 
            {
                userId=GetUser();
            }
            var data=await(from cart in context.ShoppingCarts
                      join cartDetails in context.CartDetails
                      on cart.Id equals cartDetails.ShoppingCartId
                      where cart.UserId == userId
                      select new {cartDetails.Id}
                      ).ToListAsync();
            return data.Count;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId=GetUser();
            if (userId is null)
            {
                throw new Exception("Invalid User");
            }
                var shoppingCart = await context.ShoppingCarts
                                        .Include(s => s.CartDetails)
                                        .ThenInclude(c => c.Product)
                                        .ThenInclude(p => p.Category)
                                        .Where(s => s.UserId == userId)  
                                         .SingleOrDefaultAsync();
            
            return shoppingCart;
                                     
        }
        public async Task<int> RemoveItem(int productId)
        {
            try
            {
                var userId = GetUser();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("user not logged in");
                }
                var cart = GetCart(userId);
                if (cart == null)
                {
                    throw new Exception("Invalid cart");
                }
                
                //check if product exist in cart
                var cartItem = context.CartDetails.FirstOrDefault(c => c.ProductId == productId && c.ShoppingCartId == cart.Id);
                if (cartItem != null)
                {
                    if(cartItem.Quantity>1)
                        cartItem.Quantity = cartItem.Quantity-1;
                    else 
                        context.CartDetails.Remove(cartItem);
                }
                context.SaveChanges();
                
            }
            catch (Exception ex)
            {
               
            }
            var itemCount = await GetCartItemsCount();
            return itemCount;
        }
        public async Task<Session?> CheckOut(PaymentDto payment)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var userId= GetUser();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User Not logged in");
                var cart = GetCart(userId);
                if (cart == null)
                    throw new Exception("Invalid cart");
                var cartDetails = context.CartDetails.Include(c=>c.Product).Where(c => c.ShoppingCartId == cart.Id);

                if (cartDetails.Count()==0)
                    throw new Exception("Cart is Empty");

                
                //create order
                var order = new Order
                {
                    UserId = userId,
                    OrderStatusId=1,
                    CreatedDate = DateTime.Now,
                    //TotalOrderPrice=
                    
                };
                context.Orders.Add(order);
                context.SaveChanges();
                //move cartDetails to order Details
                foreach (var item in cartDetails)
                {
                    var orderDetails = new OrderDetails
                    {
                        OrderId = order.Id,
                        Price=item.Product.Price,
                        Quantity=item.Quantity,
                        ProductId=item.ProductId,
                    };
                    context.Add(orderDetails);


                }
                context.SaveChanges();
                var totalOrderPrice = context.OrderDetails.Where(o=>o.OrderId==order.Id).Select(o=>o.Price*o.Quantity).Sum();
                order.TotalOrderPrice = totalOrderPrice;
               
                Session session=new Session();
                if (payment != null)
                {
                    Paymant paymantData = new Paymant()
                    {
                        Name = payment.Name,
                        Email = payment.Email,
                        Address = payment.Address,
                        PhoneNumber = payment.PhoneNumber,
                        PaymentMode = (int)PaymentMethods.COD,
                        OrderId = order.Id,

                    };
                    var domain = "https://localhost:44319/";
                    var options = new SessionCreateOptions
                    {
                        SuccessUrl = domain + $"Payment/PaymentConfirmed",
                        LineItems = new List<SessionLineItemOptions>(),

                        Mode = "payment",
                    };
                    foreach (var item in cartDetails)
                    {
                        var sessionItem = new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)item.Product.Price * item.Quantity,
                                Currency = "USD",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Name,
                                    Description = item.Product.Description,
                                    //Images = new List<string>
                                    //{
                                    //    "https://localhost:44319/img/Products/burger1.jpg"
                                    //  // domain+ "img/Products/"+item.Product.ImgeUrl
                                    //},

                                },



                            },
                            Quantity = item.Quantity,
                        };
                        options.LineItems.Add(sessionItem);

                    }


                    var service = new SessionService();
                    session = service.Create(options);







                    context.Paymants.Add(paymantData);
                    context.SaveChanges();
                    order.PaymentId = paymantData.Id;
                    //remove cartDetails
                    context.CartDetails.RemoveRange(cartDetails);
                    context.SaveChanges();
                
                }
                transaction.Commit();
                return session;

                
            }
            catch (Exception ex) 
            {
               return null;
            }
        }
        public  bool CheckOutCOD(PaymentDto payment)
        {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    var userId = GetUser();
                    if (string.IsNullOrEmpty(userId))
                        throw new Exception("User Not logged in");
                    var cart = GetCart(userId);
                    if (cart == null)
                        throw new Exception("Invalid cart");
                    var cartDetails = context.CartDetails.Include(c => c.Product).Where(c => c.ShoppingCartId == cart.Id);

                    if (cartDetails.Count() == 0)
                        throw new Exception("Cart is Empty");


                    //create order
                    var order = new Order
                    {
                        UserId = userId,
                        OrderStatusId = 1,
                        CreatedDate = DateTime.Now,
                        

                    };
                    context.Orders.Add(order);
                    context.SaveChanges();
                    //move cartDetails to order Details
                    foreach (var item in cartDetails)
                    {
                        var orderDetails = new OrderDetails
                        {
                            OrderId = order.Id,
                            Price = item.Product.Price,
                            Quantity = item.Quantity,
                            ProductId = item.ProductId,
                        };
                        context.Add(orderDetails);


                    }
                    context.SaveChanges();
                    var totalOrderPrice = context.OrderDetails.Where(o => o.OrderId == order.Id).Select(o => o.Price * o.Quantity).Sum();
                    order.TotalOrderPrice = totalOrderPrice;
                    if (payment != null)
                    {
                    Paymant paymantData = new Paymant() {
                        Name = payment.Name,
                        Email = payment.Email,
                        Address = payment.Address,
                        PhoneNumber = payment.PhoneNumber,
                        PaymentMode = (int)PaymentMethods.COD,
                        OrderId = order.Id,
                    };
                    context.Paymants.Add(paymantData);
                    context.SaveChanges();
                    order.PaymentId = paymantData.Id;
                    //remove cartDetails
                    context.CartDetails.RemoveRange(cartDetails);

                    context.SaveChanges();

                }
                    transaction.Commit();
                    return true;


                }
                catch (Exception ex)
                {
                    return false;
                }
            

        }

        private ShoppingCart GetCart(string userId)
        {
            var cart= context.ShoppingCarts.FirstOrDefault(c => c.UserId == userId);
            return cart;

        }

        private string GetUser() {
            var principle = httpContextAccessor.HttpContext.User;
            var userId = userManager.GetUserId(principle);
            return userId;
        }
    }
}
