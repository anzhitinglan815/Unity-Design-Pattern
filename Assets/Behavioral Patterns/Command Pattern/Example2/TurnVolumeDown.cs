//-------------------------------------------------------------------------------------
//	TurnVolumeDown.cs
//	具体命令：调低音量
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// The 'ConcreteCommand' class
/// 把“调低音量”请求封装成命令对象
/// </summary>
public class TurnVolumeDown : ICommand
{
    IElectronicDevice device;

    public TurnVolumeDown(IElectronicDevice device)
    {
        this.device = device;
    }

    // 执行：调低音量
    public void Execute()
    {
        this.device.VolumeDown();
    }

    // 撤销：调高音量
    public void Undo()
    {
        this.device.VolumeUp();
    }
}
