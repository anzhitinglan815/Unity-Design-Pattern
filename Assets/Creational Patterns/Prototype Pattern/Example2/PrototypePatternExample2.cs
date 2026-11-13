//-------------------------------------------------------------------------------------
//	PrototypePatternExample2.cs
//	原型模式示例 2：通过克隆工厂复制原型，客户端无需知道具体类型
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System;

namespace PrototypePatternExample2
{
    public class PrototypePatternExample2 : MonoBehaviour
    {
        void Start()
        {
            // 创建克隆工厂
            CloneFactory factory = new CloneFactory();

            // 创建原型对象：只有 new 会调用构造函数，控制台输出 "Made Sheep"
            Sheep sally = new Sheep();

            // 通过工厂克隆原型，拿到的是一头内容相同的新羊
            Sheep clonedSheep = (Sheep)factory.GetClone(sally);

            // 输出内容与哈希值：内容相同，但哈希值不同，说明是两个独立实例
            Debug.Log("Sally: " + sally.ToStringEX());
            Debug.Log("Clone of Sally: " + clonedSheep.ToStringEX());
            Debug.Log("Sally Hash: " + sally.GetHashCode() + " - Cloned Sheep Hash: " + clonedSheep.GetHashCode());
        }

    }

    /// <summary>
    /// 克隆工厂：只面向 IAnimal 抽象进行克隆，不依赖任何具体原型类
    /// </summary>
    public class CloneFactory
    {
        // 接收任意 IAnimal 原型，调用其 Clone 获得新实例
        public IAnimal GetClone(IAnimal animalSample)
        {
            return (IAnimal)animalSample.Clone();
        }
    }

    /// <summary>
    /// 抽象原型接口：继承 ICloneable，声明对象具备克隆自身的能力
    /// </summary>
    public interface IAnimal : ICloneable
    {
        object Clone();
    }

    /// <summary>
    /// 具体原型：羊，负责克隆自身
    /// </summary>
    public class Sheep : IAnimal
    {
        // 构造函数：仅在 new 时执行；克隆出来的对象不会再次触发这里
        public Sheep()
        {
            Debug.Log("Made Sheep");
        }

        // 克隆自身：MemberwiseClone 不会调用构造函数
        public object Clone()
        {
            Sheep sheep = null;

            try
            {
                sheep = (Sheep)base.MemberwiseClone();
            }
            catch (Exception e)
            {
                Debug.LogError("Error cloning Sheep");
            }

            return sheep;
        }

        // 返回羊的自我描述，用于验证克隆结果
        public string ToStringEX()
        {
            return "Hello I'm a Sheep";
        }
    }

}
