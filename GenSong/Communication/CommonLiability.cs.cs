using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenSong
{
    public enum NotifyCommand
    {
        Connect,
        Break,
        Listen,
        Accept,
        SendData,
        RecvData,
    }
    public enum ForkWrokStatus
    {
        PickOver,
        PickError,
        PutOver,
        PutError,
        AcceptTask
    };
    public enum eNotifyType
    {
        HeartBeatColor,
        ConnectStatusColor,
        Connect,
        Break,
        Listen,
        Accept,
        SendData,
        RecvData,
        SCStatus,
        CarryTask,
        PutOver,
        SRMError,
        Finished_Initial
    };
    public enum eStatusNotify
    {
        EquipmentInfo,
        HeartBeatBreak,
        SRMError
    };
}
