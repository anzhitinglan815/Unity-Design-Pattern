# 工厂方法模式（Factory Method Pattern）

> 类型：创建型模式
> 本项目案例目录：`Assets/Creational Patterns/Factory Method Pattern/`

## 一、定义

**定义一个用于创建对象的接口，让子类决定实例化哪一个类。工厂方法使一个类的实例化延迟到其子类。**

英文原文：*Define an interface for creating an object, but let subclasses decide which class to instantiate. Factory Method lets a class defer instantiation to subclasses.*

核心思路：创建者（Creator）只声明“怎么创建”的抽象方法，真正 `new` 哪个产品由具体创建者子类决定。客户端面向抽象编程，不需要知道具体产品类。

## 二、解决什么问题

- 客户端需要某个产品，但不应该依赖它的具体类，否则新增产品就要改客户端。
- 创建逻辑可能较复杂（选择类型、初始化配置），希望把它封装在创建者内部。
- 框架需要定义一套流程，但把“创建哪些对象”的细节留给使用者扩展。
- 避免在业务代码中散布大量 `new` 和 `if/else` 类型判断。

## 三、角色结构

| 角色 | 职责 | 本项目的对应类 |
| --- | --- | --- |
| Product（抽象产品） | 定义工厂方法所创建对象的接口 | `Product`、`Page`、`EnemyShip` |
| ConcreteProduct（具体产品） | 实现 Product 接口 | `ConcreteProductA/B`、各 `XxxPage`、`UFOEnemyShip` 等 |
| Creator（抽象创建者） | 声明工厂方法，返回 Product | `Creator`、`Document`、`EnemyShipFactory`（变体） |
| ConcreteCreator（具体创建者） | 重写工厂方法，返回具体产品 | `ConcreteCreatorA/B`、`Resume`、`Report` |

结构图可参考：[UML_Picture/factory.gif](../../UML_Picture/factory.gif)

## 四、C# 实现要点

1. Creator 中的工厂方法声明为 `abstract`（或 `virtual` 提供默认实现），由子类 `override`。
2. 返回类型应为抽象产品，例如 `Product FactoryMethod()`，这样客户端不依赖具体类型。
3. 工厂方法可以返回新实例，也可以返回缓存、对象池中的实例。
4. 若在基类构造函数中调用工厂方法（示例 1 的做法），要意识到此时子类字段尚未初始化；这是常见的教学写法，实际项目中需谨慎。
5. 工厂方法不一定只用于“返回对象”，也可以像示例 1 那样由子类把产品注册到集合中。

## 五、结合本项目案例

### 1. `Structure/FactoryMethodStructure.cs`——最小结构

场景：`TestFactoryMethodStructure.unity`，运行后 Console 输出：

```
Created ConcreteProductA
Created ConcreteProductB
```

要点：

- `Product` 是空抽象类，仅作为产品接口占位；`ConcreteProductA/B` 是具体产品。
- 抽象类 `Creator` 只声明 `public abstract Product FactoryMethod();`，不关心产品如何创建。
- `ConcreteCreatorA/B` 分别重写工厂方法并 `new` 对应产品。
- 客户端 `FactoryMethodStructure` 遍历 `Creator[]`，统一调用 `FactoryMethod()`，只依赖抽象。

### 2. `Example1/FactoryMethodPatternExample1.cs`——文档页面组合

场景：`FactoryMethodPatternExample1.unity`。

这个例子用“文档”作为创建者，用“页面”作为产品：

- `Page` 是抽象产品，`SkillsPage`、`EducationPage`、`ExperiencePage`、`IntroductionPage`、`ResultsPage`、`ConclusionPage`、`SummaryPage`、`BibliographyPage` 是具体产品。
- `Document` 是抽象创建者，内部维护 `List<Page> _pages`，并在构造函数中调用抽象方法 `CreatePages()`，把“创建哪些页面”延迟到子类。
- `Resume`（简历）重写 `CreatePages()`，加入技能页、教育经历页、工作经验页。
- `Report`（报告）重写 `CreatePages()`，加入引言、结果、结论、总结、参考文献页。

运行后 Console 大致输出：

```
Resume--
 SkillsPage
 EducationPage
 ExperiencePage

Report--
 IntroductionPage
 ResultsPage
 ConclusionPage
 SummaryPage
 BibliographyPage
```

要点：

- 客户端只持有 `Document[]`，通过基类 `Pages` 属性读取结果，完全不关心页面是如何被创建的。
- 新增一种文档只需新增一个 `Document` 子类并实现 `CreatePages()`，无需修改客户端。
- 该案例的 `CreatePages()` 返回 `void` 并直接向集合填充产品，是工厂方法的一种变体；同时带有模板方法的味道（基类固定流程，子类填充步骤）。

### 3. `Example2/FactoryMethodPatternExample2.cs`——敌机工厂与反例对比

场景：`FactoryMethodPatternExample2.unity`。

这个例子对比了两种写法：

- `EnemyShipFactory.MakeEnemyShip(ShipType)` 根据枚举参数创建 `UFOEnemyShip`、`RocketEnemyShip` 或 `BigEnemyShip`，客户端只需传入类型。
- 运行后按 `U`、`R`、`B` 分别生成 UFO、火箭、Boss 敌机，Console 输出敌机名称、跟随和攻击信息。
- `BadCodeExample` 是反例：客户端直接 `new` 具体敌机并写 `if/else` 分支，创建逻辑与业务逻辑耦合，新增类型必须修改客户端。

> 严格来说，`EnemyShipFactory + enum` 属于“简单工厂 / 参数化工厂”，并不是 GoF 的工厂方法（它没有通过子类重写工厂方法）。它更加集中、易用，但新增产品时仍需修改工厂内部的分支；而标准工厂方法通过新增 ConcreteCreator 子类来扩展，更符合开闭原则。

## 六、优点与代价

**优点**

- 客户端与具体产品解耦，只依赖抽象。
- 新增产品时通常只需新增具体产品和具体创建者，不必修改已有客户端。
- 创建逻辑集中管理，便于统一初始化、复用实例或加入对象池。

**代价**

- 每新增一种产品，往往需要同时新增对应的创建者类，类的数量增加。
- 抽象层次变多，简单场景下可能显得过度设计。
- 简单工厂变体虽然使用方便，但新增类型需要修改工厂分支，扩展性较弱。

## 七、与其他创建型模式的区别

- **简单工厂**：一个工厂类 + 条件分支，不是 GoF 模式；新增产品要改工厂。
- **工厂方法**：用继承和多态把创建延迟到子类，新增产品通常加子类即可。
- **抽象工厂**：一个工厂负责创建**一整个产品族**的多个产品，强调产品之间的配套关系。
- **原型**：通过克隆已有对象创建新对象，不需要创建者继承体系。
- **建造者**：关注复杂对象的分步组装过程，而不是选择实例化哪个类。

## 八、在 Unity 中的实践提示

- 可以用工厂方法封装 `Instantiate(prefab)`：工厂根据类型/配置返回不同的 `GameObject` 或组件，调用方不接触具体预制体。
- 与对象池结合时，工厂方法可返回池中复用的实例，减少运行时 `Instantiate` 开销。
- 若产品数量少且类型固定，简单工厂足够；当产品族或平台差异明显时，再考虑标准工厂方法或抽象工厂。

## 九、如何运行案例

1. 用 Unity 2022.3.20f1 打开项目。
2. 分别打开以下场景并进入 Play 模式，观察 Console：
   - `Assets/Creational Patterns/Factory Method Pattern/Structure/TestFactoryMethodStructure.unity`
   - `Assets/Creational Patterns/Factory Method Pattern/Example1/FactoryMethodPatternExample1.unity`
   - `Assets/Creational Patterns/Factory Method Pattern/Example2/FactoryMethodPatternExample2.unity`
3. 示例 2 运行后，按 `U` / `R` / `B` 键生成不同类型的敌机。
