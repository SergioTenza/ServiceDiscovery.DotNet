namespace ServiceDiscovery.Dotnet.Shared;

public class DomainEntity
{
    public Guid Id {get;init;} = Guid.NewGuid();
    public DateTime CreatedAt {get;init;} = DateTime.UtcNow;    
    public DateTime UpdatedAt {get;init;} = DateTime.UtcNow;    
}
