using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;

public class Person: BaseEntity
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public string NationalCode { get; set; }

    public ICollection<Order> Orders { get; set; }
}
