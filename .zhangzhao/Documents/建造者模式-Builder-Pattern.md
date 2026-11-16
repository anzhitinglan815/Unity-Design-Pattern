# 建造者模式（Builder Pattern）

> 类型：创建型模式
> 本项目案例目录：`Assets/Creational Patterns/Builder Pattern/`

## 一、定义

**将一个复杂对象的构建与它的表示分离，使得同样的构建过程可以创建不同的表示。**

英文原文：*Separate the construction of a complex object from its representation so that the same construction process can create different representations.*

核心思路：Director（指挥者）只负责“按什么顺序构建”，Builder（建造者）负责“每个部件具体怎么造”，Product（产品）保存最终结果。换一个具体建造者，就能用同一套流程得到不同形态的产品。

## 二、解决什么问题

- 对象由多个部件组成，构建步骤较多且顺序重要。
- 同一套构建流程需要产出不同的最终形态（例如不同类型、不同配置的对象）。
- 不希望客户端了解复杂对象的内部组装细节，也不想让构造函数塞满大量参数。
- 需要把“组装过程”和“最终表示”解耦，便于复用和扩展。

## 三、角色结构

| 角色 | 职责 | 本项目的对应类 |
| --- | --- | --- |
| Builder（抽象建造者） | 声明创建产品各部件的接口，以及获取产品的接口 | `Builder`、`VehicleBuilder`、`IRobotBuilder` |
| ConcreteBuilder（具体建造者） | 实现各部件构建逻辑，维护并返回产品 | `ConcreteBuilder1/2`、`MotorCycleBuilder`、`CarBuilder`、`ScooterBuilder`、`OldRobotBuilder` |
| Director（指挥者） | 按固定顺序调用建造者方法，不关心部件细节 | `Director`、`Shop`、`RobotEngineer` |
| Product（产品） | 被构建的复杂对象，保存各部件 | `Product`、`Vehicle`、`Robot` |

结构图可参考：[UML_Picture/builder.gif](../../UML_Picture/builder.gif)

## 四、C# 实现要点

1. 抽象建造者通常包含两类方法：`BuildXxx()` 部件构建方法，以及 `GetResult()` / `GetProduct()` 取产品方法。
2. 具体建造者在内部持有一个产品实例，构造时创建它，构建方法向其中填充部件。
3. 指挥者只依赖抽象建造者，步骤顺序集中在一处；客户端负责把具体建造者交给指挥者。
4. 可以扩展为“流式/链式建造者”（每个 `BuildXxx()` 返回 `this`），适合配置类对象，但本项目案例采用的是经典的指挥者写法。
5. 产品可以是普通类，也可以带索引器或部件集合；关键是由建造者统一负责写入，客户端最后读取成品。

## 五、结合本项目案例

### 1. `Structure/BuilderStructure.cs`——最小结构

场景：`TestBuilderStructure.unity`，运行后 Console 输出：

```
Product Parts -------
PartA
PartB

Product Parts -------
PartX
PartY
```

要点：

- `Director.Construct()` 固定调用 `BuildPartA()`、`BuildPartB()`，构建顺序与产品细节完全解耦。
- `ConcreteBuilder1` 往产品里加入 `PartA`、`PartB`；`ConcreteBuilder2` 加入 `PartX`、`PartY`。
- 客户端用同一个 `Director`，传入不同建造者，就得到两种不同表示的产品。
- `Product` 只提供 `Add()` 与 `Show()`，不关心部件由谁来加。

### 2. `Example1/BuilderPatternExample1.cs`——车辆组装

场景：`BuilderPatternExample1.unity`。

这是更贴近实际的例子：`Shop` 是指挥者，`VehicleBuilder` 是抽象建造者，`MotorCycleBuilder`、`CarBuilder`、`ScooterBuilder` 是具体建造者，`Vehicle` 是产品。

- 指挥者按固定四步构建：`BuildFrame()` → `BuildEngine()` → `BuildWheels()` → `BuildDoors()`。
- 每种建造者给出不同规格：踏板车 50 cc、2 轮、0 门；汽车 2500 cc、4 轮、4 门；摩托车 500 cc、2 轮、0 门。
- 产品 `Vehicle` 用 `Dictionary<string, string>` 保存部件，并通过索引器供建造者写入。

运行后 Console 依次输出三种车辆，例如：

```
Vehicle Type: Scooter
 Frame : Scooter Frame
 Engine : 50 cc
 #Wheels: 2
 #Doors : 0
```

要点：构建流程只有一份（在 `Shop` 中），但最终表示有三种，正是“同样的构建过程可以创建不同的表示”。

### 3. `Example2/BuilderPatternExample2.cs`——机器人组装

场景：`BuilderPatternExample2.unity`，输出：

```
First Robot built
Head: Old Head, torso: Old Torso, Arms: Old Arms, legs: Roller Skates
```

要点：

- `RobotEngineer` 是指挥者，`MakeRobot()` 固定执行“头 → 躯干 → 手臂 → 腿”四步。
- `IRobotBuilder` 是抽象建造者，`OldRobotBuilder` 是具体建造者，内部创建并持有 `Robot`。
- `IRobotPlan` 是产品自身的部件设置接口，`Robot` 实现它；建造者通过 `SetRobotXxx()` 写入部件。
- 客户端通过 `new OldRobotBuilder()` 选择具体建造者，指挥者无需修改；想换一种机器人，只需新增一个 `IRobotBuilder` 实现。

> 说明：该案例中产品也实现了接口（`IRobotPlan`），这是一种常见变体；经典写法中产品不必有接口，只要建造者能填充并返回它即可。

## 六、优点与代价

**优点**

- 构建过程与产品表示分离，流程可复用。
- 客户端只面向指挥者与抽象建造者，不依赖具体产品细节。
- 新增产品形态通常只需新增具体建造者，不必修改指挥者和客户端。
- 可以精细控制构建步骤，避免“参数爆炸”的构造函数。

**代价**

- 类的数量增加：抽象建造者、多个具体建造者、指挥者、产品。
- 产品结构差异很大时，抽象建造者接口可能难以统一。
- 简单对象使用建造者模式容易过度设计。

## 七、与其他创建型模式的区别

- **工厂方法**：一步创建并返回产品，关注“创建哪一个类”。
- **抽象工厂**：一次创建一整个产品族的多个产品，关注产品之间的配套。
- **建造者**：分步骤组装一个复杂产品，关注“怎么一步步构建”，同一流程可产出不同表示。
- **原型**：通过克隆已有对象创建新对象，不涉及分步组装。

## 八、在 Unity 中的实践提示

- 适合构建复杂配置：角色属性、关卡数据、技能组合、UI 布局等；先由建造者逐步填充数据，再生成运行时对象。
- 组装 `GameObject` 时，可以先用建造者累积部件配置，最后统一 `Instantiate` 并挂载组件，避免在 `Start()` 中堆叠大量初始化代码。
- 如果只是少量固定参数，直接用构造函数或工厂即可；当步骤多、变体多时再引入建造者。

## 九、如何运行案例

1. 用 Unity 2022.3.20f1 打开项目。
2. 分别打开以下场景并进入 Play 模式，观察 Console：
   - `Assets/Creational Patterns/Builder Pattern/Structure/TestBuilderStructure.unity`
   - `Assets/Creational Patterns/Builder Pattern/Example1/BuilderPatternExample1.unity`
   - `Assets/Creational Patterns/Builder Pattern/Example2/BuilderPatternExample2.unity`
3. 对照本文第三节，在代码中找出每个案例里的 Builder、ConcreteBuilder、Director 和 Product。
