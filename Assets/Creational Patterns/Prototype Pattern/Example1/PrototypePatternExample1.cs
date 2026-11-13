//-------------------------------------------------------------------------------------
//	PrototypePatternExample1.cs
//	原型模式示例 1：用原型管理器保存一批颜色样板，需要时按名称克隆
//-------------------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This real-world code demonstrates the Prototype pattern in which new Color objects are created by copying pre-existing, user-defined Colors of the same type.

namespace PrototypePatternExample1
{
    public class PrototypePatternExample1 : MonoBehaviour
    {
        void Start()
        {
            // 创建原型管理器，用于按名称保存所有颜色原型
            ColorManager colormanager = new ColorManager();

            // 注册系统预设的标准颜色原型
            // Initialize with standard colors
            colormanager["red"] = new Color(255, 0, 0);
            colormanager["green"] = new Color(0, 255, 0);
            colormanager["blue"] = new Color(0, 0, 255);

            // 注册用户自定义的颜色原型
            // User adds personalized colors
            colormanager["angry"] = new Color(255, 54, 0);
            colormanager["peace"] = new Color(128, 211, 128);
            colormanager["flame"] = new Color(211, 34, 20);

            // 客户端按名称取出原型，并通过 Clone 得到新对象
            // User clones selected colors
            Color color1 = colormanager["red"].Clone() as Color;
            Color color2 = colormanager["peace"].Clone() as Color;
            Color color3 = colormanager["flame"].Clone() as Color;

        }
    }

    /// <summary>
    /// 抽象原型：声明克隆自身的接口
    /// The 'Prototype' abstract class
    /// </summary>
    abstract class ColorPrototype
    {
        // 克隆接口：返回一个与当前对象内容相同的新实例
        public abstract ColorPrototype Clone();
    }

    /// <summary>
    /// 具体原型：颜色对象，负责克隆自身
    /// 注意：本命名空间内的 Color 会遮蔽 UnityEngine.Color，此处使用的是自定义原型类
    /// The 'ConcretePrototype' class
    /// </summary>
    class Color : ColorPrototype
    {
        // RGB 三个分量均为值类型，浅拷贝即可完整复制
        private int _red;
        private int _green;
        private int _blue;

        // 构造函数：仅在创建原型时执行，克隆时不会调用
        // Constructor
        public Color(int red, int green, int blue)
        {
            this._red = red;
            this._green = green;
            this._blue = blue;
        }

        // 返回浅拷贝；由于字段都是值类型，此处等价于完整复制
        // Create a shallow copy
        public override ColorPrototype Clone()
        {
            // 打印被克隆颜色的 RGB 值，方便在 Console 中观察克隆过程
            Debug.Log("Cloning color RGB: (" + _red + " ," + _green + "," + _blue + ")");

            return this.MemberwiseClone() as ColorPrototype;
        }
    }

    /// <summary>
    /// 原型管理器：用字典保存一批原型，客户端可按名称取用
    /// Prototype manager
    /// </summary>
    class ColorManager
    {
        // 以名称为键保存原型实例
        private Dictionary<string, ColorPrototype> _colors = new Dictionary<string, ColorPrototype>();

        // 索引器：set 时注册原型，get 时取出原型用于克隆
        // Indexer
        public ColorPrototype this[string key]
        {
            get { return _colors[key]; }
            set { _colors.Add(key, value); }
        }
    }
}
