using DataBase;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Classes;

public class AddOrder : Work<OrderDataEntry, Order, OrderDbContext>
{
    public AddOrder(OrderDataEntry inputModel, Order processModel) : base(inputModel, processModel)
    {
    }

    public override async Task job(OrderDbContext dbContext)
    {
        _dbContext = dbContext;

        using (var TransAction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                string Errors =await Validation();
                if(Errors != null)
                {
                    Console.WriteLine(Errors);
                    return;
                }
                int OrderId = await AddOrdertoDatabase(_inputModel);
                await AddOrderItemtoDatabase(OrderId, _inputModel.Products);
                await ConfreamOrder(OrderId);
                await TransAction.CommitAsync();
                return;
            }
            catch (Exception ex)
            {
                await TransAction.RollbackAsync();
                Console.Clear();
                Console.WriteLine(ex.Message);
            }
        }

    }


    private async Task<string> Validation()
    {
        //await Task.Delay(2000);
        List<string> Errors = new List<string>();

        if (_inputModel.Products.Count() == 0) Errors.Add("Pleas select a Product ");

        var Product = _dbContext.products.IgnoreAutoIncludes().Include(x => x.OrderItems).ToList().Where(x => _inputModel.Products.Any(y => y.Key == x.ProductId)).ToList();

        if (Product.Count() != _inputModel.Products.Count()) Errors.Add("Product Not Found ");

        Product.ForEach(x =>
         {
             var Used = x.OrderItems.Sum(x => x.Count) + _inputModel.Products.Single(y => y.Key == x.ProductId).Value;
             if (x.Buyed < Used) Errors.Add("Insufficient inventory ");
         });

        if (Errors.Count() != 0)
        {
            return JsonSerializer.Serialize(Errors);
        }
        return null;
    }
    private async Task<int> AddOrdertoDatabase(OrderDataEntry order)
    {

        var _order = new Order()
        {
            CreateDate = DateTime.Now,
            IsDeleted = false,
            UpdateDate = DateTime.Now,
            PersonId = order.personId,
            State = OrderStateEnum.Workind,
        };

        var Item = await _dbContext.AddAsync(_order);
        await _dbContext.SaveChangesAsync();
        return Item.Entity.OrderId;
    }
    private async Task AddOrderItemtoDatabase(int OrderId, List<KeyValuePair<int, decimal>> Prodacts)
    {

        var Products = _dbContext.products.ToList();

        var OrderItems = Prodacts.Select(x =>
        new OrderItem()
        {
            OrderId = OrderId,
            ProductId = x.Key,
            ProductPrice = Products.Single(z => z.ProductId == x.Key).Price,
            Count = x.Value,
            CreateDate = DateTime.Now,
            IsDeleted = false,
            UpdateDate = DateTime.Now,

        }); ;
        await _dbContext.orderItems.AddRangeAsync(OrderItems);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<int> ConfreamOrder(int OrderId)
    {
        Order order = _dbContext.orders.IgnoreAutoIncludes().Include(x=>x.OrderItems).Single(x => x.OrderId == OrderId);
        order.Price = order.OrderItems.Sum(x => x.TotalPrise());
        order.State = OrderStateEnum.Ended;
        var Item = _dbContext.Update(order);
        await _dbContext.SaveChangesAsync();
        return Item.Entity.OrderId;
    }


}
