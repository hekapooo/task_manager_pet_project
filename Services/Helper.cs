using System.Collections.Generic;
using Avalonia.Media;
using TaskManager.Models;

namespace TaskManager.Services;

public static class Helper
{
    private static readonly Dictionary<StatusType, List<StatusType>> _allowedStatuses = new()
    {
        { StatusType.None, new() { StatusType.None, StatusType.Appointed }},
        { StatusType.Appointed, new() { StatusType.Appointed, StatusType.Inprocess } },
        { StatusType.Inprocess, new () { StatusType.Inprocess,
                                         StatusType.Suspended, StatusType.Completed } },
        { StatusType.Suspended, new() { StatusType.Suspended, StatusType.Inprocess } },
        { StatusType.Completed, new() { StatusType.Completed } },
    };

    private static readonly Dictionary<StatusType, IBrush> _statusToBrush = new()
    {
            { StatusType.Appointed, Brushes.Black },
            { StatusType.Inprocess, Brushes.Blue },
            { StatusType.Completed, Brushes.Green },
            { StatusType.Suspended, Brushes.Red }
    };

    public static List<StatusType> GetAllowedStatuses(StatusType inputType)
    {
        if (_allowedStatuses.ContainsKey(inputType))
        {
            return _allowedStatuses[inputType];
        }
        else
        {
            return new List<StatusType>() { StatusType.None };
        }
    }

    public static IBrush GetBrush(StatusType status)
    {
        if (_statusToBrush.ContainsKey(status))
        {
            return _statusToBrush[status];
        }
        else
        {
            return Brushes.Black;
        }
    }
}
