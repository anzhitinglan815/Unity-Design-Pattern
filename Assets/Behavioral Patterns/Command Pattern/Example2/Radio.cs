//-------------------------------------------------------------------------------------
//	Radio.cs
//	具体接收者：收音机
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// The 'Receiver' class
/// 收音机与电视实现同一接收者接口，因此也能被同一套命令控制
/// </summary>
public class Radio : IElectronicDevice
{
    protected int volume = 0;   // 当前音量

    public void On()
    {
        Debug.Log("Radio is On");
    }

    public void Off()
    {
        Debug.Log("Radio is Off");
    }

    public void VolumeUp()
    {
        ++volume;
        Debug.Log("Radio Turned Volume Up to " + volume);
    }

    public void VolumeDown()
    {
        // 音量不会低于 0
        if (volume > 0)
            --volume;
        Debug.Log("Radio Turned Volume Down to " + volume);
    }
}