//-------------------------------------------------------------------------------------
//	PrototypeStructure.cs
//	原型模式最小结构演示：通过克隆已有实例来创建新对象
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class PrototypeStructure : MonoBehaviour
{

    void Start( )
    {
        // 创建两个原型实例，并分别克隆出新对象
        // Create two instances and clone each

        ConcretePrototype1 p1 = new ConcretePrototype1("I");
        ConcretePrototype1 c1 = (ConcretePrototype1)p1.Clone();
        Debug.Log("Cloned: "+c1.Id);

        ConcretePrototype2 p2 = new ConcretePrototype2("II");
        ConcretePrototype2 c2 = (ConcretePrototype2)p2.Clone();
        Debug.Log("Cloned: "+c2.Id);

    }
}

/// <summary>
/// 抽象原型：声明克隆自身的接口
/// The 'Prototype' abstract class
/// </summary>
abstract class Prototype
{
    // 原型保存的标识，仅用于验证克隆结果
    private string _id;

    // 构造函数：外部通过 new 创建原型实例（克隆时不会执行构造函数）
    // Constructor
    public Prototype(string id)
    {
        this._id = id;
    }

    // 只读属性：便于客户端读取克隆后对象的标识
    // Gets id
    public string Id
    {
        get { return _id; }
    }

    // 克隆接口：由具体原型决定如何复制自身
    public abstract Prototype Clone();
}

/// <summary>
/// 具体原型 1：实现克隆自身的逻辑
/// A 'ConcretePrototype' class
/// </summary>
class ConcretePrototype1 : Prototype
{
    // 构造函数：仅创建原型实例时调用
    // Constructor
    public ConcretePrototype1(string id)
      : base(id)
    {
    }

    // 返回浅拷贝：MemberwiseClone 会逐字段复制当前对象
    // Returns a shallow copy
    public override Prototype Clone()
    {
        return (Prototype)this.MemberwiseClone();
    }
}

/// <summary>
/// 具体原型 2：实现克隆自身的逻辑
/// A 'ConcretePrototype' class
/// </summary>
class ConcretePrototype2 : Prototype
{
    // 构造函数：仅创建原型实例时调用
    // Constructor
    public ConcretePrototype2(string id)
      : base(id)
    {
    }

    // 返回浅拷贝：MemberwiseClone 会逐字段复制当前对象
    // Returns a shallow copy
    public override Prototype Clone()
    {
        return (Prototype)this.MemberwiseClone();
    }
}


// 原型模式一般要实现 Clone接口

