//-------------------------------------------------------------------------------------
//	TurnItAllOff.cs
//	宏命令：一次性控制一组设备
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The 'ConcreteCommand' class (Macro Command)
/// 组合命令：把多个设备的行为聚合成一个命令，调用者只需按一次
/// </summary>
public class TurnItAllOff : ICommand
{
    List<IElectronicDevice> devices;   // 一组接收者

    public TurnItAllOff(List<IElectronicDevice> devices)
    {
        this.devices = devices;
    }

    // 执行：关闭列表中的所有设备
    public void Execute()
    {
        foreach (IElectronicDevice device in devices)
        {
            device.Off();
        }
    }

    // 撤销：重新打开列表中的所有设备
    public void Undo()
    {
        foreach (IElectronicDevice device in devices)
        {
            device.On();
        }
    }
}