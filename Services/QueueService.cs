using Classes;
using DataBase;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public static class QueueService<Work, inputModel, processModel>
    where Work : Work<inputModel, processModel, OrderDbContext>
    where inputModel : class
    where processModel : class
{
    private static ConcurrentQueue<Work> _concurrentQueue = new ConcurrentQueue<Work>();
    public static bool IsWorking { get; private set; }

    public static async Task AddToQueue(Work Item)
    {
        await Task.Run(() => _concurrentQueue.Enqueue(Item));
        if (!IsWorking) await Runer();
    }
    public static int GetQueueCount()
    {
        var count = _concurrentQueue.Count();
        return count;
    }

    private static async Task Runer()
    {
        IsWorking = true;

        string[] intarry = { null };
        using (var DataBase = new OrderDbContext())
        {
            while (true)
            {
                Console.WriteLine( "--"+_concurrentQueue.Count());
                if (!_concurrentQueue.TryDequeue(out Work Item)) break;
                await Item.job(DataBase);
            }
        }

        IsWorking = false;
    }
}
