//-------------------------------------------------------------------------------------
//	BuilderStructure.cs
//	建造者模式最小结构演示：指挥者按固定步骤指挥建造者组装产品
//-------------------------------------------------------------------------------------


// 一个产品有多个部件
//一个建造者，可以建造多个部件，组成产品
//另一个建造者，也可以建造多个部件，组成产品
//指挥者，可以注入建造者，建造产品


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuilderStructure : MonoBehaviour
{
	void Start ( )
    {
        // 创建指挥者和两个具体建造者
        // Create director and builders
        Director director = new Director();

        Builder b1 = new ConcreteBuilder1();
        Builder b2 = new ConcreteBuilder2();

        // 用同样的构建流程分别建造两个不同表示的产品
        // Construct two products
        director.Construct(b1);
        Product p1 = b1.GetResult();
        p1.Show();

        director.Construct(b2);
        Product p2 = b2.GetResult();
        p2.Show();
    }
}

/// <summary>
/// 指挥者：负责安排构建步骤的顺序，不关心每个部件具体怎么建
/// The 'Director' class
/// </summary>
class Director
{
    // 指挥者按固定顺序调用建造者的构建方法
    // Builder uses a complex series of steps
    public void Construct(Builder builder)
    {
        builder.BuildPartA();
        builder.BuildPartB();
    }
}

/// <summary>
/// 抽象建造者：定义创建产品各部件的方法，以及获取最终产品的接口
/// The 'Builder' abstract class
/// </summary>
abstract class Builder
{
    // 构建部件的步骤由具体建造者实现
    public abstract void BuildPartA();
    public abstract void BuildPartB();

    // 返回构建完成的产品
    public abstract Product GetResult();
}

/// <summary>
/// 具体建造者 1：把部件 A、B 组装成产品
/// The 'ConcreteBuilder1' class
/// </summary>
class ConcreteBuilder1 : Builder
{
    // 每个具体建造者内部维护自己的产品实例
    private Product _product = new Product();

    public override void BuildPartA()
    {
        _product.Add("PartA");
    }

    public override void BuildPartB()
    {
        _product.Add("PartB");
    }

    public override Product GetResult()
    {
        return _product;
    }
}

/// <summary>
/// 具体建造者 2：用不同的部件名构建出另一种表示
/// The 'ConcreteBuilder2' class
/// </summary>
class ConcreteBuilder2 : Builder
{
    // 每个具体建造者内部维护自己的产品实例
    private Product _product = new Product();

    public override void BuildPartA()
    {
        _product.Add("PartX");
    }

    public override void BuildPartB()
    {
        _product.Add("PartY");
    }

    public override Product GetResult()
    {
        return _product;
    }
}

/// <summary>
/// 产品：正在被构建的复杂对象，内部保存各个部件
/// The 'Product' class
/// </summary>
class Product
{
    // 组成产品的部件列表
    private List<string> _parts = new List<string>();

    // 由建造者调用，向产品中加入一个部件
    public void Add(string part)
    {
        _parts.Add(part);
    }

    // 输出最终产品的所有部件，便于观察建造结果
    public void Show()
    {
      Debug.Log("\nProduct Parts -------");
        foreach (string part in _parts)
        {
            Debug.Log(part);
        }

    }
}




// 一个产品有多个部件
//一个建造者，可以建造多个部件，组成产品
//另一个建造者，也可以建造多个部件，组成产品
//指挥者，可以注入建造者，建造产品
