namespace Burmalda.Routing.Auctions.Entities;

public class SendDonateToNewLotRequestBody
{
    public decimal Amount { get; set; }
    public string Message { get; set; }
    public string LotTitle { get; set; }
}