//-------------------------------------------------------------------------------------
//	TVRemove.cs
//	简单工厂：集中创建接收者
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// 简单工厂：客户端通过它获取设备（接收者），避免直接 new 具体设备类
/// </summary>
public class TVRemove
{
    // 返回接收者接口类型，客户端无需知道具体是电视还是收音机
    public static IElectronicDevice GetDevice()
    {
        return new Television();
    }
}
