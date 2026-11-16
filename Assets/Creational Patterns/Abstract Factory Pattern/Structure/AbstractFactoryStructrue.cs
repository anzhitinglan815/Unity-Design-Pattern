//-------------------------------------------------------------------------------------
//	AbstractFactoryStructrue.cs
//	抽象工厂模式最小结构演示：一个工厂负责创建一整族相互关联的产品
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class AbstractFactoryStructrue : MonoBehaviour
{
    void Start()
    {
        // 使用抽象工厂 1，创建并运行产品族 1
        // Abstract factory #1
        AbstractFactory factory1 = new ConcreteFactory1();
        Client client1 = new Client(factory1);
        client1.Run();

        // 使用抽象工厂 2，创建并运行产品族 2
        // Abstract factory #2
        AbstractFactory factory2 = new ConcreteFactory2();
        Client client2 = new Client(factory2);
        client2.Run();
    }
}

/// <summary>
/// 抽象工厂：声明创建一族产品（ProductA、ProductB）的接口
/// The 'AbstractFactory' abstract class
/// </summary>
abstract class AbstractFactory
{
    public abstract AbstractProductA CreateProductA();
    public abstract AbstractProductB CreateProductB();
}


/// <summary>
/// 具体工厂 1：创建产品族 1（ProductA1 + ProductB1）
/// The 'ConcreteFactory1' class
/// </summary>
class ConcreteFactory1 : AbstractFactory
{
    public override AbstractProductA CreateProductA()
    {
        return new ProductA1();
    }
    public override AbstractProductB CreateProductB()
    {
        return new ProductB1();
    }
}

/// <summary>
/// 具体工厂 2：创建产品族 2（ProductA2 + ProductB2）
/// The 'ConcreteFactory2' class
/// </summary>
class ConcreteFactory2 : AbstractFactory
{
    public override AbstractProductA CreateProductA()
    {
        return new ProductA2();
    }
    public override AbstractProductB CreateProductB()
    {
        return new ProductB2();
    }
}

/// <summary>
/// 抽象产品 A：产品族中由工厂创建的一类产品
/// The 'AbstractProductA' abstract class
/// </summary>
abstract class AbstractProductA
{
}

/// <summary>
/// 抽象产品 B：需要与产品 A 交互的另一类产品
/// The 'AbstractProductB' abstract class
/// </summary>
abstract class AbstractProductB
{
    // 产品 B 与产品 A 交互，保证同族产品可以配套使用
    public abstract void Interact(AbstractProductA a);
}


/// <summary>
/// 具体产品 A1（属于产品族 1）
/// The 'ProductA1' class
/// </summary>
class ProductA1 : AbstractProductA
{
}

/// <summary>
/// 具体产品 B1（属于产品族 1）
/// The 'ProductB1' class
/// </summary>
class ProductB1 : AbstractProductB
{
    public override void Interact(AbstractProductA a)
    {
        Debug.Log(this.GetType().Name + " interacts with " + a.GetType().Name);
    }
}

/// <summary>
/// 具体产品 A2（属于产品族 2）
/// The 'ProductA2' class
/// </summary>
class ProductA2 : AbstractProductA
{
}

/// <summary>
/// 具体产品 B2（属于产品族 2）
/// The 'ProductB2' class
/// </summary>
class ProductB2 : AbstractProductB
{
    public override void Interact(AbstractProductA a)
    {
        Debug.Log(this.GetType().Name + " interacts with " + a.GetType().Name);
    }
}

/// <summary>
/// 客户端：只依赖抽象工厂和抽象产品，不关心具体实现
/// The 'Client' class. Interaction environment for the products.
/// </summary>
class Client
{
    private AbstractProductA _abstractProductA;
    private AbstractProductB _abstractProductB;

    // 构造函数注入抽象工厂，由工厂一次性创建同族产品
    // Constructor
    public Client(AbstractFactory factory)
    {
        _abstractProductB = factory.CreateProductB();
        _abstractProductA = factory.CreateProductA();
    }

    // 客户端只调用抽象产品的接口
    public void Run()
    {
        _abstractProductB.Interact(_abstractProductA);
    }
}
