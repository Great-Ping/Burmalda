namespace Burmalda.Entities.Users;

[Flags]
public enum UserPermissions
{
    None            = 0b000,
    Frozen          = 0b001,
    CanAdministrate = 0b010,
    CanHoldAuctions = 0b100
}