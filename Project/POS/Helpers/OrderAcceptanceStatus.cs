using System.Security.Cryptography;

namespace Helpers;

public enum OrderAcceptanceStatus
{
    Unknown = 0,
    BusinessIsPreparing = 1,
    Ready = 2,
    GivenToClient = 4,
    Closed = 5
}