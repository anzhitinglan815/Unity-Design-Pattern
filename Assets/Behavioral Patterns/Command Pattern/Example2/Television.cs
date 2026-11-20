//-------------------------------------------------------------------------------------
//	Television.cs
//	具体接收者：电视
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// The 'Receiver' class
/// 电视实现接收者接口，提供开关机和音量调节的实际操作
/// </summary>
public class Television : IElectronicDevice
{
    protected int volume = 0;   // 当前音量

    public void On()
    {
        Debug.Log("TV is On");
    }

    public void Off()
    {
        Debug.Log("TV is Off");
    }

    public void VolumeUp()
    {
        ++volume;
        Debug.Log("TV Turned Volume Up to " + volume);
    }

    public void VolumeDown()
    {
        // 音量不会低于 0
        if (volume > 0)
            --volume;
        Debug.Log("TV Turned Volume Down to " + volume);
    }
}
