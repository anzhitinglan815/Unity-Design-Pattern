//-------------------------------------------------------------------------------------
//	BuilderPatternExample1.cs
//	建造者模式示例 1：商店（指挥者）按相同步骤组装不同类型的车辆
//-------------------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This real-world code demonstates the Builder pattern in which different vehicles are assembled in a step-by-step fashion.
//The Shop uses VehicleBuilders to construct a variety of Vehicles in a series of sequential steps.

namespace BuilderPatternExample1
{
    public class BuilderPatternExample1 : MonoBehaviour
    {
        void Start()
        {
            // 建造者由客户端创建，指挥者只负责按流程调用
            VehicleBuilder builder;

            // 创建商店（指挥者），后续用它来组装各种车辆
            // Create shop with vehicle builders
            Shop shop = new Shop();

            // 依次用不同建造者构建并展示车辆
            // Construct and display vehicles
            builder = new ScooterBuilder();
            shop.Construct(builder);
            builder.Vehicle.Show();

            builder = new CarBuilder();
            shop.Construct(builder);
            builder.Vehicle.Show();

            builder = new MotorCycleBuilder();
            shop.Construct(builder);
            builder.Vehicle.Show();

        }
    }

    /// <summary>
    /// 指挥者：商店，按固定顺序完成车辆各部件的安装
    /// The 'Director' class
    /// </summary>
    class Shop
    {
        // 指挥者只依赖 VehicleBuilder 抽象，步骤顺序固定
        // Builder uses a complex series of steps
        public void Construct(VehicleBuilder vehicleBuilder)
        {
            vehicleBuilder.BuildFrame();
            vehicleBuilder.BuildEngine();
            vehicleBuilder.BuildWheels();
            vehicleBuilder.BuildDoors();
        }
    }

    /// <summary>
    /// 抽象建造者：声明车辆各部件的构建方法，并提供获取产品的入口
    /// The 'Builder' abstract class
    /// </summary>
    abstract class VehicleBuilder
    {
        // 正在构建的车辆，由具体建造者创建
        protected Vehicle vehicle;

        // 只读属性：构建完成后客户端通过它取得产品
        // Gets vehicle instance
        public Vehicle Vehicle
        {
            get { return vehicle; }
        }

        // 各部件构建步骤，由具体建造者实现
        // Abstract build methods
        public abstract void BuildFrame();
        public abstract void BuildEngine();
        public abstract void BuildWheels();
        public abstract void BuildDoors();
    }

    /// <summary>
    /// 具体建造者：摩托车，决定各部件的具体规格
    /// The 'ConcreteBuilder1' class
    /// </summary>
    class MotorCycleBuilder : VehicleBuilder
    {
        // 构造时先创建产品，并指定车辆类型
        public MotorCycleBuilder()
        {
            vehicle = new Vehicle("MotorCycle");
        }

        public override void BuildFrame()
        {
            vehicle["frame"] = "MotorCycle Frame";
        }

        public override void BuildEngine()
        {
            vehicle["engine"] = "500 cc";
        }

        public override void BuildWheels()
        {
            vehicle["wheels"] = "2";
        }

        public override void BuildDoors()
        {
            vehicle["doors"] = "0";
        }
    }


    /// <summary>
    /// 具体建造者：汽车，决定各部件的具体规格
    /// The 'ConcreteBuilder2' class
    /// </summary>
    class CarBuilder : VehicleBuilder
    {
        // 构造时先创建产品，并指定车辆类型
        public CarBuilder()
        {
            vehicle = new Vehicle("Car");
        }

        public override void BuildFrame()
        {
            vehicle["frame"] = "Car Frame";
        }

        public override void BuildEngine()
        {
            vehicle["engine"] = "2500 cc";
        }

        public override void BuildWheels()
        {
            vehicle["wheels"] = "4";
        }

        public override void BuildDoors()
        {
            vehicle["doors"] = "4";
        }
    }

    /// <summary>
    /// 具体建造者：踏板车，决定各部件的具体规格
    /// The 'ConcreteBuilder3' class
    /// </summary>
    class ScooterBuilder : VehicleBuilder
    {
        // 构造时先创建产品，并指定车辆类型
        public ScooterBuilder()
        {
            vehicle = new Vehicle("Scooter");
        }

        public override void BuildFrame()
        {
            vehicle["frame"] = "Scooter Frame";
        }

        public override void BuildEngine()
        {
            vehicle["engine"] = "50 cc";
        }

        public override void BuildWheels()
        {
            vehicle["wheels"] = "2";
        }

        public override void BuildDoors()
        {
            vehicle["doors"] = "0";
        }
    }

    /// <summary>
    /// 产品：车辆，用字典保存各部件信息
    /// The 'Product' class
    /// </summary>
    class Vehicle
    {
        // 车辆类型
        private string _vehicleType;
        // 部件表：键为部件名（frame/engine/...），值为具体规格
        private Dictionary<string, string> _parts =
          new Dictionary<string, string>();

        // 构造函数：指定车辆类型
        // Constructor
        public Vehicle(string vehicleType)
        {
            this._vehicleType = vehicleType;
        }

        // 索引器：建造者通过部件名写入或读取部件信息
        // Indexer
        public string this[string key]
        {
            get { return _parts[key]; }
            set { _parts[key] = value; }
        }

        // 输出车辆及全部部件，便于观察建造结果
        public void Show()
        {
            Debug.Log("\n---------------------------");
            Debug.Log("Vehicle Type: " + _vehicleType);
            Debug.Log(" Frame : " + _parts["frame"]);
            Debug.Log(" Engine : " + _parts["engine"]);
            Debug.Log(" #Wheels: " + _parts["wheels"]);
            Debug.Log(" #Doors : " + _parts["doors"]);
        }
    }
}



//一个车辆由4个部件构成，分别是车架，发动机，轮子，车门
//踏板车建造者，建造踏板车的4个部件
//汽车建造者，建造汽车的4个部件
//摩托车建造者，建造摩托车的4个部件
//抽象建造者，抽象出4个部件的建造方法，并提供获取产品的入口，具体建造这负责实现

// 指挥者，根据传入的建造者，按固定顺序调用建造者的4个部件建造方法，组装出车辆


