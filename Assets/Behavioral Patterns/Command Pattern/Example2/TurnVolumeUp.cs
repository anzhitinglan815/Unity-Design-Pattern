//-------------------------------------------------------------------------------------
//	TurnVolumeUp.cs
//	具体命令：调高音量
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// The 'ConcreteCommand' class
/// 把“调高音量”请求封装成命令对象
/// </summary>
public class TurnVolumeUp : ICommand
{
    IElectronicDevice device;

    public TurnVolumeUp(IElectronicDevice device)
    {
        this.device = device;
    }

    // 执行：调高音量
    public void Execute()
    {
        this.device.VolumeUp();
    }

    // 撤销：调低音量
    public void Undo()
    {
        this.device.VolumeDown();
    }
}
