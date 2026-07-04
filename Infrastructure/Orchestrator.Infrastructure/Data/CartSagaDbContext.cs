using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestrator.Application.Saga.SagaDatas;

namespace Orchestrator.Infrastructure.Data;

public class CartSagaDbContext : SagaDbContext
{
    public CartSagaDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations {
        get
        {
            yield return new CartStateMap();
        }
    }
}

public class CartStateMap : SagaClassMap<CartStateSagaData>
{
    protected override void Configure(EntityTypeBuilder<CartStateSagaData> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);
    }
}