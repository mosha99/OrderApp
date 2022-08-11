using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;

public class Product : BaseEntity
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    [Precision(18, 2)]
    public decimal Price { get; set; }
    [Precision(18, 2)]
    public decimal Buyed { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
    public decimal amunt()
    {        
        int? count = OrderItems?.Count();
        if(OrderItems == null) throw new Exception("Plese Include OrderItem before Use This Method");
        if (count == null) throw new Exception("Plese Include OrderItem before Use This Method");
        if (count == 0) return Buyed;
        decimal Sum =Buyed - OrderItems.Sum(x => x.Count);
        return Sum;
    }
}
