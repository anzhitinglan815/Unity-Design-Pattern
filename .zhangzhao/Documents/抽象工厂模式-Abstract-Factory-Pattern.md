# 抽象工厂模式（Abstract Factory Pattern）

> 类型：创建型模式
> 本项目案例目录：`Assets/Creational Patterns/Abstract Factory Pattern/`

## 一、定义

**提供一个创建一系列相关或相互依赖对象的接口，而无需指定它们具体的类。**

英文原文：*Provide an interface for creating families of related or dependent objects without specifying their concrete classes.*

核心思路：抽象工厂不生产单个产品，而是生产**一整套相互配套的产品族**（例如同一风格的按钮 + 窗口，同一品牌的武器 + 引擎）。客户端只依赖抽象工厂和抽象产品，更换工厂即可整体切换产品族。

## 二、解决什么问题

- 系统中存在多个产品族，且同一族内的产品必须配套使用。
- 客户端不应依赖具体产品类，也不应知道产品或工厂是如何创建的。
- 需要整体切换实现（例如换主题、换平台、换阵营），而不希望逐个修改创建代码。
- 多个工厂类共享一组创建方法，希望通过统一的抽象接口约束它们。

## 三、角色结构

| 角色 | 职责 | 本项目的对应类 |
| --- | --- | --- |
| AbstractFactory（抽象工厂） | 声明创建一族抽象产品的方法 | `AbstractFactory`、`ContinentFactory`、`IEnemyShipFactory` |
| ConcreteFactory（具体工厂） | 实现创建属于同一族的具体产品 | `ConcreteFactory1/2`、`AfricaFactory`、`AmericaFactory`、`UFOEnemyShipFactory` |
| AbstractProduct（抽象产品） | 为一类产品声明接口 | `AbstractProductA/B`、`Herbivore`、`Carnivore`、`IESWeapon`、`IESEngine` |
| ConcreteProduct（具体产品） | 实现抽象产品，属于某个具体产品族 | `ProductA1/B1`、`Wildebeest`、`Lion`、`Bison`、`Wolf`、`ESUFOGun`、`ESUFOEngine` |
| Client（客户端） | 只通过抽象工厂和抽象产品工作 | `Client`、`AnimalWorld`、`UFOEnemyShip` |

结构图可参考：[UML_Picture/abstract.gif](../../UML_Picture/abstract.gif)

## 四、C# 实现要点

1. 抽象工厂通常声明**多个**创建方法，每个方法对应产品族中的一种产品。
2. 具体工厂一次性负责创建同一族的所有产品，保证产品之间的兼容性。
3. 客户端通过构造函数注入或参数传入抽象工厂，代码中不出现具体类名。
4. 新增一个**产品族**较容易：新增一个具体工厂及其产品即可，客户端无需修改。
5. 新增一种**产品类型**较困难：需要修改抽象工厂接口和所有具体工厂，这是抽象工厂的主要扩展代价。

## 五、结合本项目案例

### 1. `Structure/AbstractFactoryStructrue.cs`——最小结构

场景：`TestAbstractFactoryStructrue.unity`（场景名保留原有拼写），运行后 Console 输出：

```
ProductB1 interacts with ProductA1
ProductB2 interacts with ProductA2
```

要点：

- `AbstractFactory` 声明 `CreateProductA()`、`CreateProductB()` 两个创建方法，对应产品族中的两类产品。
- `ConcreteFactory1` 创建 `ProductA1` + `ProductB1`；`ConcreteFactory2` 创建 `ProductA2` + `ProductB2`。
- `Client` 只持有抽象产品，并在构造函数中通过抽象工厂取得同族产品。
- `ProductB.Interact(ProductA)` 体现同族产品之间的协作关系。

### 2. `Example1/AbstractFactoryPatternExample1.cs`——动物世界

场景：`AbstractFactoryPatternExample1.unity`，运行后 Console 输出：

```
Lion eats Wildebeest
Wolf eats Bison
```

要点：

- `ContinentFactory` 是抽象工厂，声明创建 `Herbivore`（食草动物）和 `Carnivore`（食肉动物）。
- `AfricaFactory` 创建 `Wildebeest`（角马）+ `Lion`（狮子）；`AmericaFactory` 创建 `Bison`（野牛）+ `Wolf`（狼）。
- `AnimalWorld` 是客户端，只依赖抽象工厂与抽象产品，调用 `RunFoodChain()` 时使用同大陆配套的动物。
- 更换大陆只需换一个具体工厂，`AnimalWorld` 与业务逻辑完全不用改。

### 3. `Example2/AbstractFactoryPatternExample2.cs`——飞船与零件工厂

场景：`AbstractFactoryPatternExample2.unity`，运行后 Console 输出：

```
Making enemy ship UFO
UFO is on the screen.
UFO follows hero ship with 1000 mph
UFO shoots and does 20 damage
```

要点：

- `IEnemyShipFactory` 是抽象工厂，声明 `AddESGun()`（武器）和 `AddESEngine()`（引擎）两个创建方法。
- `UFOEnemyShipFactory` 是具体工厂，返回 `ESUFOGun` + `ESUFOEngine` 这一套配套零件。
- `IESWeapon`、`IESEngine` 是抽象产品，`ESUFOGun`、`ESUFOEngine` 是具体产品。
- `UFOEnemyShip` 通过构造函数接收零件工厂，在 `MakeShip()` 中一次性取得配套的武器和引擎。
- `EnemyShipBuilding` / `UFOEnemyShipBuilding` 负责“下单建造”流程，更接近工厂方法/模板方法的组合；真正的抽象工厂角色由 `IEnemyShipFactory` 承担。

> 说明：该案例把“选择造哪种飞船”和“为飞船提供配套零件”两层职责拆开了，是抽象工厂在游戏开发中的常见用法。

## 六、优点与代价

**优点**

- 客户端与具体产品解耦，只依赖抽象。
- 保证同一产品族内的对象可以配套使用。
- 切换产品族只需替换具体工厂，符合开闭原则（对新增族开放）。
- 创建逻辑集中，便于统一管理。

**代价**

- 新增一种产品类型需要修改抽象工厂及所有具体工厂，扩展成本较高。
- 抽象层次较多，简单场景容易过度设计。
- 产品族和产品等级结构需要提前设计，后期调整代价大。

## 七、与工厂方法的区别

| 对比项 | 工厂方法 | 抽象工厂 |
| --- | --- | --- |
| 创建对象 | 一种产品 | 一族相关产品 |
| 扩展方式 | 新增具体创建者子类 | 新增具体工厂及整套产品 |
| 客户端依赖 | 抽象创建者 + 抽象产品 | 抽象工厂 + 多个抽象产品 |
| 典型场景 | 单一类型产品的创建 | 主题、平台、阵营等成套切换 |

简单说：**工厂方法解决“创建哪一个类”，抽象工厂解决“创建哪一整套”**。

## 八、在 Unity 中的实践提示

- **UI 主题切换**：同一套界面控件（按钮、面板、图标）按主题由不同工厂创建，切换主题即切换工厂。
- **平台差异**：不同平台/渠道的登录、支付、统计等实现组成产品族，由平台工厂统一提供。
- **游戏阵营/装备**：如本项目的武器 + 引擎组合，保证同一阵营的零件风格一致。
- 若产品族只有一个产品，用工厂方法即可；只有确实存在“成套切换”需求时再引入抽象工厂。

## 九、如何运行案例

1. 用 Unity 2022.3.20f1 打开项目。
2. 分别打开以下场景并进入 Play 模式，观察 Console：
   - `Assets/Creational Patterns/Abstract Factory Pattern/Structure/TestAbstractFactoryStructrue.unity`
   - `Assets/Creational Patterns/Abstract Factory Pattern/Example1/AbstractFactoryPatternExample1.unity`
   - `Assets/Creational Patterns/Abstract Factory Pattern/Example2/AbstractFactoryPatternExample2.unity`
3. 对照本文第三节，在代码中找出每个案例里的抽象工厂、具体工厂、抽象产品和具体产品。
