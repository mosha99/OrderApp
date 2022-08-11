using Classes;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Services;
using System.Text.Json;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    public OrderDbContext OrderDb { get; }

    public OrderController(OrderDbContext orderDb)
    {
        OrderDb = orderDb;
    }

    // GET: api/<OrderController>
    [HttpGet]
    public async Task<List<KeyValuePair<int, decimal>>> Get()
    {
        List<KeyValuePair<int, decimal>> selected;
        selected = OrderDb.products.Include(x => x.OrderItems).Select(x => new KeyValuePair<int, decimal>(x.ProductId, x.amunt())).ToList();
        return selected;

    }

    // GET api/<OrderController>/5
    [HttpGet("{id}")]
    public Order Get(int id)
    {

            var Item = OrderDb.orders
                .Include(x => x.Person)
                .Include(x => x.OrderItems).ThenInclude(x => x.Product)
                .Single(x => x.OrderId == id);

            return Item;
        
    }

    // POST api/<OrderController>
    [HttpPost]
    public async Task<int> Post([FromBody] OrderDataEntry value)
    {
        Work<OrderDataEntry, Order, OrderDbContext> work = new AddOrder(value, null);
        await QueueService<Work<OrderDataEntry, Order, OrderDbContext>, OrderDataEntry, Order>.AddToQueue(work);
        return value.personId;
    }

    // PUT api/<OrderController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<OrderController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
