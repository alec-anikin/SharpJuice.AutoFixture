# SharpJuice.AutoFixture #

[![NuGet](https://img.shields.io/nuget/v/SharpJuice.AutoFixture.svg)](https://www.nuget.org/packages/SharpJuice.AutoFixture/)


[AutoFixture](https://github.com/AutoFixture/AutoFixture) extension for partially specifying constructor parameters. 

### Samples ###

Class under test
```csharp
public class Order
{
  //Modest constructor
  public Order(int id, string[] lineItems, decimal discount, string createdBy) {...}
  
  //Greedy constructor
  public Order(int id, string[] lineItems, decimal discount, Guid customerId, DateTimeOffset createdAt) {...}
}
```

Creating single instance with best fit constructor (modest constructor with all matched arguments)
```csharp
fixture.Create<Order>(new { id = 100500, createdAt = DateTimeOffset.Now  });
``` 
Anonymous type property must match (case-insensitive) with one of constructor parameters.


Best fit constructor configuration
```csharp
fixture.CustomizeConstructor<Order>(new { id = 100500, createdBy = "someuser" });
```

Greedy constructor configuration
```csharp
fixture.CustomizeGreedyConstructor<Order>(new { discount = 10.0m, customerId = Guid.NewGuid() });
```


Freezing constructor parameter for a type. All instances of the type will get the same parameter instance. 
```csharp
fixture.FreezeParameter<Order, decimal>();
``` 