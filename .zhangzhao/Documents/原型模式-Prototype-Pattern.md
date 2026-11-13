# 原型模式（Prototype Pattern）

> 类型：创建型模式
> 本项目案例目录：`Assets/Creational Patterns/Prototype Pattern/`

## 一、定义

**用原型实例指定创建对象的种类，并且通过拷贝这些原型创建新的对象。**

也就是说，客户端不再通过 `new` 加构造函数去创建对象，而是先准备一个“样板对象”（原型），之后需要新对象时直接**克隆**这个样板。

英文原文：*Specify the kind of objects to create using a prototypical instance, and create new objects by copying this prototype.*

## 二、解决什么问题

当创建对象的成本较高，或者客户端不应该依赖具体类时，原型模式很有价值：

- 对象的构造过程复杂（需要读取配置、查询数据库、组合多个部件），但复制一个现成对象很快。
- 客户端只想拿到“和某个已有对象一样”的实例，不关心它具体是哪个类、怎么构造。
- 需要在运行时动态增删“可创建的类型”，例如把用户自定义的样式存进原型管理器。
- 希望避免为每个产品类都写一个对应的工厂类。

## 三、角色结构

| 角色 | 职责 | 本项目的对应类 |
| --- | --- | --- |
| Prototype（抽象原型） | 声明克隆自身的接口 | `Prototype`、`ColorPrototype`、`IAnimal` |
| ConcretePrototype（具体原型） | 实现克隆自身 | `ConcretePrototype1/2`、`Color`、`Sheep` |
| Client（客户端） | 通过原型克隆出新对象 | `PrototypeStructure`、`PrototypePatternExample1/2` |
| Prototype Manager（原型管理器，可选） | 用字典保存一批原型，按 key 取用 | `ColorManager` |

结构图可参考：[UML_Picture/prototype.gif](../../UML_Picture/prototype.gif)

## 四、C# 实现要点

1. **克隆接口**：抽象原型提供 `Clone()`；.NET 还内置了 `ICloneable` 接口，示例 2 的 `IAnimal` 就继承了它。
2. **`MemberwiseClone()` 是浅拷贝**：它创建新对象并逐字段复制。值类型字段被完整复制，引用类型字段只复制引用，新旧对象仍指向同一批子对象。
3. **深拷贝**：如果原型含有引用类型成员且后续会被修改，需要在 `Clone()` 里手动克隆这些成员，或借助序列化/反序列化实现。
4. **构造函数不会执行**：通过 `MemberwiseClone` 创建的对象不会调用构造函数，因此初始化逻辑不会被重复执行——这既是性能优势，也意味着构造函数中的副作用不会发生。

## 五、结合本项目案例

### 1. `Structure/PrototypeStructure.cs`——最小结构演示

场景：`TestPrototypeStructure.unity`，运行后 Console 输出：

```
Cloned: I
Cloned: II
```

要点：

- 抽象类 `Prototype` 持有 `Id` 并提供抽象方法 `Clone()`。
- `ConcretePrototype1` 和 `ConcretePrototype2` 都通过 `MemberwiseClone()` 返回浅拷贝。
- 客户端 `PrototypeStructure` 只依赖 `Prototype` 抽象类型，克隆时再向下转型拿到具体类型，因此无需知道对象是怎么构造出来的。

### 2. `Example1/PrototypePatternExample1.cs`——颜色原型管理器

场景：`PrototypePatternExample1.unity`。

这个例子演示了**原型 + 原型管理器**的组合：

- `ColorPrototype` 是抽象原型，`Color` 是具体原型，内部保存 RGB 三个 `int` 字段。
- `ColorManager` 本质上是一个 `Dictionary<string, ColorPrototype>`，并暴露索引器，客户端可以用 `"red"`、`"peace"` 这样的名字存取原型。
- 客户端先注册系统预设颜色，再注册用户自定义颜色，最后按名字克隆：

```csharp
Color color1 = colormanager["red"].Clone() as Color;
Color color2 = colormanager["peace"].Clone() as Color;
Color color3 = colormanager["flame"].Clone() as Color;
```

- `Color.Clone()` 内部会 `Debug.Log` 输出被克隆的 RGB 值，运行场景即可在 Console 中看到克隆过程。
- 由于 `Color` 只有值类型字段，`MemberwiseClone()` 在这里已经等价于完整拷贝，不存在共享子对象的问题。

> 小提示：该文件通过 `using UnityEngine;` 引入了 Unity 的 `Color`，但命名空间内又定义了同名的 `Color` 类；在 `PrototypePatternExample1` 命名空间内，未限定的 `Color` 会解析为本地原型类。

### 3. `Example2/PrototypePatternExample2.cs`——克隆工厂

场景：`PrototypePatternExample2.unity`，运行后 Console 大致输出：

```
Made Sheep
Sally: Hello I'm a Sheep
Clone of Sally: Hello I'm a Sheep
Sally Hash: ... - Cloned Sheep Hash: ...
```

要点：

- `IAnimal : ICloneable` 声明克隆能力，`Sheep` 实现 `Clone()`。
- `CloneFactory.GetClone()` 只接收 `IAnimal`，不关心具体类型，统一调用 `Clone()`，这就是“客户端与具体类解耦”的体现。
- `new Sheep()` 会打印一次 `Made Sheep`，而克隆出来的对象**不会再打印**——直接印证了 `MemberwiseClone()` 不调用构造函数。
- 两个对象的 `GetHashCode()` 不同，说明克隆得到的是独立的新实例，而不是同一个引用。

## 六、优点与代价

**优点**

- 隐藏对象创建细节，客户端只面向抽象原型编程。
- 复制已有对象通常比重新构造更快、更简单。
- 可以在运行时动态注册/替换原型，扩展新“类型”无需修改客户端代码。

**代价**

- 深拷贝实现较麻烦，尤其是对象图复杂、存在循环引用时。
- 每个具体原型都必须实现 `Clone()`，类数量可能增加。
- 克隆出的对象与原型之间的引用关系需要仔细设计，避免意外共享状态。

## 七、与其他创建型模式的区别

- **工厂方法 / 抽象工厂**：关注“由谁创建、创建哪一族对象”；原型关注“通过复制已有对象来创建”。
- **建造者**：关注复杂对象的分步构建过程；原型直接复制一个已经构建好的成品。
- 实际项目中三者可以组合，例如用原型管理器保存建造者产出的样板对象。

## 八、在 Unity 中的补充说明

- 本目录的案例操作的是**普通 C# 对象**，用于理解模式本身。
- Unity 场景中的 `GameObject` / `Component` 克隆通常使用 `Instantiate()`，它复制的是 Unity 序列化数据，可以视为引擎内置的原型实现。
- 若原型包含 `UnityEngine.Object` 引用，浅拷贝会让多个克隆共享同一个资源引用；这通常是期望的行为（节省内存），但修改引用本身时要注意区分“改引用”和“改资源内容”。

## 九、如何运行案例

1. 用 Unity 2022.3.20f1 打开项目。
2. 分别打开以下场景并进入 Play 模式，观察 Console：
   - `Assets/Creational Patterns/Prototype Pattern/Structure/TestPrototypeStructure.unity`
   - `Assets/Creational Patterns/Prototype Pattern/Example1/PrototypePatternExample1.unity`
   - `Assets/Creational Patterns/Prototype Pattern/Example2/PrototypePatternExample2.unity`
3. 对照本文第三、五节理解每个角色在代码中的位置。
