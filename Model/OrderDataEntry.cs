using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;

public class OrderDataEntry 
{
    public int personId { get; set; }
    public List<KeyValuePair<int,decimal>> Products { get; set; }
}
