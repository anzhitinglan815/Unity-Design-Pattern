//-------------------------------------------------------------------------------------
//	TurnTVOn.cs
//	具体命令：打开设备
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// The 'ConcreteCommand' class
/// 把“打开”这个请求绑定到具体的接收者设备上
/// </summary>
public class TurnTVOn : ICommand
{
    IElectronicDevice device;   // 接收者：命令实际操作的设备

    public TurnTVOn(IElectronicDevice device)
    {
        this.device = device;
    }

    // 执行：打开设备
    public void Execute()
    {
        this.device.On();
    }

    // 撤销：反向操作即关闭设备
    public void Undo()
    {
        this.device.Off();
    }
}
