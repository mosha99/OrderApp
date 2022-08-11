using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Model;

public class OrderItem : BaseEntity
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    [Precision(18, 2)]
    public decimal Count { get; set; }
    [Precision(18, 2)]
    public decimal ProductPrice { get; set; }

    public decimal TotalPrise() => Count * ProductPrice;
}
