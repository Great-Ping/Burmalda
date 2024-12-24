using Burmalda.Entities.Auctions;
using Burmalda.Entities.Users;

namespace Burmalda.Entities.Support;

public class SupportBranch : BaseEntity
{
    public required string Title { get; init;}
    public required bool IsPrivate { get; init;}
    public required SupportBranchState State { get; init;}
    public virtual required List<SupportBranchMessage>? Messages { get; init;}
    
    public required ulong OwnerId { get; init;}
    public virtual required User? Owner { get; init;}

    public required ulong? InspectorId { get; init;}
    public virtual required User? Inspector { get; init;}

}

