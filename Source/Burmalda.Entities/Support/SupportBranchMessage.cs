using Burmalda.Entities.Auctions;
using Burmalda.Entities.Users;

namespace Burmalda.Entities.Support;

public class SupportBranchMessage : BaseEntity
{
    public required ulong SenderId { get; set; }
    public virtual required User? Sender { get; set; }
    public required string Message { get; set; }
    public required DateTimeOffset SendDate { get; set; }
}