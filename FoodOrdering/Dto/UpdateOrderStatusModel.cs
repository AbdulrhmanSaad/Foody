using FoodOrdering.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOrdering.Dto
{
    public class UpdateOrderStatusModel
    {
        public int OrderId { get; set; }
        public int orderStatusId { get; set; }
        public IEnumerable<SelectListItem> OrderStatusNames { get; set; }

    }
}
