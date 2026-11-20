//-------------------------------------------------------------------------------------
//	IElectronicDevice.cs
//	接收者接口：命令最终操作的对象
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

// 电子设备（接收者）接口：电视、收音机等都实现它
// 命令类只依赖该接口，因此同一套命令可以作用于不同设备
public interface IElectronicDevice
{
    void On();          // 打开设备
    void Off();         // 关闭设备
    void VolumeUp();    // 调高音量
    void VolumeDown();  // 调低音量
}
