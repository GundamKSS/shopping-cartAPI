namespace MyAPI_1.Models;

public class PickupRequest
{
    public string ActionType { get; set; } = string.Empty; // "update", "clear_all", "clear_category"
    public List<CheckoutItem> Items { get; set; } = new();
}