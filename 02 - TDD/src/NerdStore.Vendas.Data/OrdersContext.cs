using MediatR;
using Microsoft.EntityFrameworkCore;
using NerdStore.Core;
using NerdStore.Sales.Domain;

namespace NerdStore.Vendas.Data;

public class OrdersContext : DbContext, IUnitOfWork
{
  private readonly IMediator _mediator;
  public OrdersContext(DbContextOptions<OrdersContext> options, IMediator mediator) : base(options)
  {
    _mediator = mediator;
  }

  public DbSet<Order> Orders { get; set; }
  public DbSet<OrderItem> OrderItems { get; set; }

  public async Task<bool> Commit()
  {
    var success = base.SaveChanges() > 0;

    if (success) await _mediator.PublishEvents(this);

    return success;
  }
}

public static class MediatorExtension
{
  public static async Task PublishEvents(this IMediator mediator, OrdersContext context)
  {
    var domainEntities = context.ChangeTracker
      .Entries<Entity>()
      .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Count != 0)
      .ToList();

    var domainEvents = domainEntities
      .SelectMany(x => x.Entity.Notifications)
      .ToList();

    domainEntities.ForEach(x => x.Entity.ClearEvents());

    var tasks = domainEvents.Select(async (domainEvent) => await mediator.Publish(domainEvent));
  

    await Task.WhenAll(tasks);
  }
}

