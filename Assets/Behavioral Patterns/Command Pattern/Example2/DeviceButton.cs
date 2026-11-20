//-------------------------------------------------------------------------------------
//	DeviceButton.cs
//	调用者（Invoker）：遥控器上的按键
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// The 'Invoker' class
/// 按键只持有命令接口，按下时触发命令，完全不知道命令会操作哪台设备
/// </summary>
public class DeviceButton
{
    ICommand cmd;

    // 构造时注入命令，同一个按键即可绑定不同命令
    public DeviceButton(ICommand cmd)
    {
        this.cmd = cmd;
    }

    // 按下按键：执行命令（调用者不关心具体行为）
    public void Press()
    {
        this.cmd.Execute(); // actually the invoker (device button) has no idea what it does
    }

    // 撤销按键：执行命令的反向操作
    public void PressUndo()
    {
        this.cmd.Undo();
    }
}

