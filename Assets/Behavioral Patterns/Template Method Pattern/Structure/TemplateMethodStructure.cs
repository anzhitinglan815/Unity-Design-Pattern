//-------------------------------------------------------------------------------------
//	TemplateMethodStructure.cs
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// 客户端：模板方法模式的最小结构演示
/// 用同一套模板方法流程分别执行两个具体子类，验证“算法骨架不变、步骤可变”
/// </summary>
public class TemplateMethodStructure : MonoBehaviour
{
	void Start ( )
	{
        // 两个对象都以抽象基类类型引用，客户端只依赖算法的骨架
        AbstractClass aA = new ConcreteClassA();
        aA.TemplateMethod();   // 使用 A 的实现执行同一套流程

        AbstractClass aB = new ConcreteClassB();
        aB.TemplateMethod();   // 使用 B 的实现执行同一套流程

    }
}

/// <summary>
/// The 'AbstractClass' abstract class
/// 抽象类：定义算法骨架（模板方法）以及需要子类实现的基本操作
/// </summary>
abstract class AbstractClass
{
    // 基本操作：算法的具体步骤，延迟到子类中实现
    public abstract void PrimitiveOperation1();
    public abstract void PrimitiveOperation2();

    // The "Template method"
    // 模板方法：定义算法骨架，按固定顺序调用各个步骤
    // 注意它是非虚方法（不标记 virtual/abstract），子类无法改变流程顺序
    public void TemplateMethod()
    {
        PrimitiveOperation1();  // 步骤一：由子类决定具体行为
        PrimitiveOperation2();  // 步骤二：由子类决定具体行为
        Debug.Log("");          // 步骤三：固定行为，所有子类共用
    }
}

/// <summary>
/// A 'ConcreteClass' class
/// 具体类 A：实现算法的各个基本操作
/// </summary>
class ConcreteClassA : AbstractClass
{
    public override void PrimitiveOperation1()
    {
        Debug.Log("ConcreteClassA.PrimitiveOperation1()");
    }
    public override void PrimitiveOperation2()
    {
        Debug.Log("ConcreteClassA.PrimitiveOperation2()");
    }
}

/// <summary>
/// A 'ConcreteClass' class
/// 具体类 B：实现算法的各个基本操作，流程与 A 相同、步骤内容不同
/// </summary>
class ConcreteClassB : AbstractClass
{
    public override void PrimitiveOperation1()
    {
        Debug.Log("ConcreteClassB.PrimitiveOperation1()");
    }
    public override void PrimitiveOperation2()
    {
        Debug.Log("ConcreteClassB.PrimitiveOperation2()");
    }
}
