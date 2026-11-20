//-------------------------------------------------------------------------------------
//	TurnTVOff.cs
//	具体命令：关闭设备
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// The 'ConcreteCommand' class
/// 把“关闭”这个请求绑定到具体的接收者设备上
/// </summary>
public class TurnTVOff : ICommand
{
    IElectronicDevice device;   // 接收者：命令实际操作的设备

    public TurnTVOff(IElectronicDevice device)
    {
        this.device = device;
    }

    // 执行：关闭设备
    public void Execute()
    {
        this.device.Off();
    }

    // 撤销：反向操作即打开设备
    public void Undo()
    {
        this.device.On();
    }
}

