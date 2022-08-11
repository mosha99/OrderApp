using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;

public class Order : BaseEntity
{
    public int OrderId { get; set; }
    [Precision(18, 2)]
    public decimal Price { get; set; }

    public OrderStateEnum State { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }

    public decimal TotalPrice()
    {
        if (State == OrderStateEnum.Ended) return Price;
        if (OrderItems == null) throw new Exception("Plese Include OrderItem before Use This Method");
        decimal totalPrice = 0; 
        foreach (OrderItem item in OrderItems)
        {
            totalPrice += item.TotalPrise();
        }
        return totalPrice;
    }
}
