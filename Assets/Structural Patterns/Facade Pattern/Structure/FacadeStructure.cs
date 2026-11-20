//-------------------------------------------------------------------------------------
//	FacadeStructure.cs
//	外观模式的最小结构：用一个统一入口协调多个子系统
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// 客户端：只与 Facade 交互，不直接调用任何子系统
/// </summary>
public class FacadeStructure : MonoBehaviour
{
	void Start ( )
    {
        // 创建外观对象，由它负责组织内部的子系统
        Facade facade = new Facade();

        // 客户端只需调用两个高层方法，具体调用了哪些子系统由 Facade 决定
        facade.MethodA();
        facade.MethodB();
    }
}

/// <summary>
/// The 'Subsystem ClassA' class
/// 子系统一：提供自己的业务方法，不知道 Facade 的存在
/// </summary>
class SubSystemOne
{
    public void MethodOne()
    {
        Debug.Log(" SubSystemOne Method");
    }
}

/// <summary>
/// The 'Subsystem ClassB' class
/// 子系统二：提供自己的业务方法
/// </summary>
class SubSystemTwo
{
    public void MethodTwo()
    {
        Debug.Log(" SubSystemTwo Method");
    }
}

/// <summary>
/// The 'Subsystem ClassC' class
/// 子系统三：提供自己的业务方法
/// </summary>
class SubSystemThree
{
    public void MethodThree()
    {
        Debug.Log(" SubSystemThree Method");
    }
}

/// <summary>
/// The 'Subsystem ClassD' class
/// 子系统四：提供自己的业务方法
/// </summary>
class SubSystemFour
{
    public void MethodFour()
    {
        Debug.Log(" SubSystemFour Method");
    }
}

/// <summary>
/// The 'Facade' class
/// 外观：知道哪些子系统负责处理请求，并把客户端请求转发给它们
/// </summary>
class Facade
{
    // 外观持有子系统的引用，但子系统不持有外观的引用（单向依赖）
    private SubSystemOne _one;
    private SubSystemTwo _two;
    private SubSystemThree _three;
    private SubSystemFour _four;

    public Facade()
    {
        // 由外观统一创建并管理子系统，客户端无需了解它们的构造方式
        _one = new SubSystemOne();
        _two = new SubSystemTwo();
        _three = new SubSystemThree();
        _four = new SubSystemFour();
    }

    // 高层方法 A：按固定顺序协调三个子系统，完成一组操作
    public void MethodA()
    {
        Debug.Log("\nMethodA() ---- ");
        _one.MethodOne();
        _two.MethodTwo();
        _four.MethodFour();
    }

    // 高层方法 B：协调另外一组子系统调用
    public void MethodB()
    {
        Debug.Log("\nMethodB() ---- ");
        _two.MethodTwo();
        _three.MethodThree();
    }
}
