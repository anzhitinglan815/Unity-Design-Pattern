using UnityEngine;
using System.Collections;

// 反面示例：把飞行行为写进继承体系。
// 不会飞的动物被迫重写 Fly() 并留下空实现，一旦要修改飞行方式，
// 就必须改动所有子类，违反开闭原则——这正是策略模式要解决的问题。

public class Animal_BadExample
{
	// bad code: 在基类中实现所有动物共用的飞行行为
	public virtual void Fly()
	{
		Debug.Log ("Can Fly");
	}
}

public class Dog_BadExample : Animal_BadExample
{
	// bad code: 狗不会飞，只能重写成一个空方法
	public override void Fly()
	{
		// override
	}
}

public class Cat_BadExample : Animal_BadExample
{
	// bad code: 猫不会飞，同样被迫重写空方法
	public override void Fly()
	{
		// override
	}
}

public class Bird_BadExample : Animal_BadExample
{
	// bad code: 鸟会飞，但行为被继承体系固定，无法在运行时替换
	public override void Fly()
	{
	}
}

// Rembember:
// ALWAYS
// eliminate duplicate code
// eliminate technices that allow one class to affect others
// 中文补充：始终消除重复代码，避免让一个类的改动影响到其他类