//-------------------------------------------------------------------------------------
//	TestCommandPattern.cs
//	客户端：把命令绑定到按键调用者上，演示命令模式的完整流程
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestCommandPattern : MonoBehaviour
{
    void Start()
    {
        Debug.Log("------------------Command Pattern--------------");

        // 通过简单工厂取得接收者（实际是 Television）
        IElectronicDevice device = TVRemove.GetDevice();

        // 开机命令：把命令和接收者绑定，再装到按键调用者上
        TurnTVOn onCommand = new TurnTVOn(device);
        DeviceButton onPressed = new DeviceButton(onCommand);
        onPressed.Press();      // 输出：TV is On

        // -----------------------
        // 关机命令：复用同一个按键变量，换成新的命令对象
        TurnTVOff offCommand = new TurnTVOff(device);
        onPressed = new DeviceButton(offCommand);
        onPressed.Press();      // 输出：TV is Off

        // 音量加命令：连续按三次，音量从 0 升到 3
        TurnVolumeUp volUpCommand = new TurnVolumeUp(device);
        onPressed = new DeviceButton(volUpCommand);
        onPressed.Press();
        onPressed.Press();
        onPressed.Press();

        // 音量减命令：按一次，音量从 3 降到 2
        TurnVolumeDown volDownCommand = new TurnVolumeDown(device);
        onPressed = new DeviceButton(volDownCommand);
        onPressed.Press();

        // -----------------------
        // 宏命令：把多台设备放进一个列表，用一个命令统一关闭
        Television tv = new Television();
        Radio radio = new Radio();

        List<IElectronicDevice> allDevices = new List<IElectronicDevice>();
        allDevices.Add(tv);
        allDevices.Add(radio);

        TurnItAllOff turnOffDevices = new TurnItAllOff(allDevices);
        DeviceButton turnThemOff = new DeviceButton(turnOffDevices);
        turnThemOff.Press();    // 输出：TV is Off、Radio is Off

        // -----------------------
        // 撤销宏命令：依次重新打开所有设备
        turnThemOff.PressUndo();    // 输出：TV is On、Radio is On

    }
}
