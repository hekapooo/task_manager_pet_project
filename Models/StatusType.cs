namespace TaskManager.Models;

public enum StatusType : byte
{
    None = 0b_0000_0000,
    Appointed = 0b_0000_0001,
    Inprocess = 0b_0000_0010,
    Suspended = 0b_0000_0100,
    Completed = 0b_0000_1000
}
