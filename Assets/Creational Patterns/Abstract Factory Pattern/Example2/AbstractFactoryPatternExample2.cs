//-------------------------------------------------------------------------------------
//	AbstractFactoryPatternExample2.cs
//	抽象工厂模式示例 2：战舰工厂负责为飞船提供配套的武器与引擎
//-------------------------------------------------------------------------------------

// allows a family of related objects without specifying the concrete class
// when there are many objects that can be added or changed dynamically
// you can model everything you can image and have those objects interact through common interfaces

// negative: can get very complicated


using UnityEngine;
using System.Collections;

namespace AbstractFactoryPatternExample2
{

    public class AbstractFactoryPatternExample2 : MonoBehaviour
    {
	    void Start ( )
	    {
            // 选择 UFO 造船厂，下订单建造 UFO
            EnemyShipBuilding ufoBuilder = new UFOEnemyShipBuilding();
            ufoBuilder.orderShip(ShipType.UFO);
        }
    }

    /// <summary>
    /// 可建造的飞船类型
    /// </summary>
    public enum ShipType
    {
        UFO
    }


    /// <summary>
    /// 抽象建造者/创建者：定义下单流程，具体创建哪种飞船由子类决定
    /// （这里是抽象工厂与模板方法/工厂方法的组合）
    /// </summary>
    public abstract class EnemyShipBuilding
    {
        // 抽象“订单”：具体工厂决定根据类型创建哪艘飞船
        // abstract order form:
        protected abstract EnemyShip MakeEnemyShip(ShipType type);

        // 固定下单流程：先造船，再展示、跟随、开火
        public EnemyShip orderShip(ShipType type)
        {
            EnemyShip ship = MakeEnemyShip(type);

            ship.MakeShip();
            ship.DisplayShip();
            ship.FollowHeroShip();
            ship.Shoot();

            return ship;
        }
    }

    /// <summary>
    /// 具体创建者：UFO 造船厂，负责创建 UFO 并注入对应的零件工厂
    /// </summary>
    public class UFOEnemyShipBuilding : EnemyShipBuilding
    {
        // 创建不同飞船时，这里的选择逻辑会不同
        // Make Ship varies per ship type...
        protected override EnemyShip MakeEnemyShip(ShipType type)
        {
            EnemyShip ship = null;

            if (type == ShipType.UFO)
            {
                // 为 UFO 选定配套的武器/引擎工厂（产品族）
                IEnemyShipFactory factory = new UFOEnemyShipFactory();
                ship = new UFOEnemyShip(factory);
                ship.name = "UFO";
            }

            return ship;
        }
    }




    /// <summary>
    /// 抽象工厂：声明创建一整套配套零件（武器 + 引擎）的接口
    /// </summary>
    public interface IEnemyShipFactory
    {
        IESWeapon AddESGun();
        IESEngine AddESEngine();
    }

    /// <summary>
    /// 具体工厂：UFO 零件工厂，生产 UFO 专用武器和引擎
    /// </summary>
    public class UFOEnemyShipFactory : IEnemyShipFactory
    {
        // 每个具体工厂可以提供不同的武器和引擎组合
        // each factory can add different weapons and stuff
        public IESWeapon AddESGun()
        {
            return new ESUFOGun();
        }

        public IESEngine AddESEngine()
        {
            return new ESUFOEngine();
        }
    }




    /// <summary>
    /// 抽象产品：飞船，持有由抽象工厂创建的武器和引擎
    /// </summary>
    public abstract class EnemyShip
    {
        public string name;
        // 零件类型均为抽象产品，飞船不依赖具体实现
        protected IESEngine engine;
        protected IESWeapon weapon;

        // 由具体飞船决定如何从工厂取得零件
        public abstract void MakeShip();

        public void DisplayShip()
        {
            Debug.Log(name + " is on the screen.");
        }

        public void FollowHeroShip()
        {
            Debug.Log(name + " follows hero ship with " + engine.ToStringEX());
        }

        public void Shoot()
        {
            Debug.Log(name + " shoots and does " + weapon.ToStringEX());
        }

        public string ToStringEX()
        {
            return "The " + name + " has a speed of " + engine.ToStringEX() + " a firepower of " + weapon.ToStringEX();
        }
    }

    /// <summary>
    /// 具体产品：UFO，向注入的零件工厂索取武器与引擎
    /// </summary>
    public class UFOEnemyShip : EnemyShip
    {
        IEnemyShipFactory factory;

        // 通过构造函数注入具体零件工厂
        public UFOEnemyShip(IEnemyShipFactory factory)
        {
            this.factory = factory;
        }

        // 从工厂一次性取得配套的武器和引擎
        public override void MakeShip()
        {
            Debug.Log("Making enemy ship " + name);
            weapon = factory.AddESGun();
            engine = factory.AddESEngine();
        }
    }



    // 可替换的武器与引擎产品：可能被换入换出的零件
    // possible Weapons to swap in and out
    public interface IESWeapon
    {
        string ToStringEX();
    }

    public interface IESEngine
    {
        string ToStringEX();
    }

    /// <summary>
    /// 具体产品：UFO 专用武器
    /// </summary>
    public class ESUFOGun : IESWeapon
    {
        public string ToStringEX()
        {
            return "20 damage";
        }
    }

    /// <summary>
    /// 具体产品：UFO 专用引擎
    /// </summary>
    public class ESUFOEngine : IESEngine
    {
        public string ToStringEX()
        {
            return "1000 mph";
        }
    }


}
