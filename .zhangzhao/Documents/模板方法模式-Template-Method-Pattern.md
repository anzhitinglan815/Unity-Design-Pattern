# 模板方法模式（Template Method Pattern）

> 类型：行为型模式
> 本项目案例目录：`Assets/Behavioral Patterns/Template Method Pattern/`

## 一、定义

**定义一个操作中的算法的框架，而将一些步骤延迟到子类中。模板方法使得子类可以不改变一个算法的结构即可重定义该算法的某些特定步骤。**

英文原文：*Define the skeleton of an algorithm in an operation, deferring some steps to subclasses. Template Method lets subclasses redefine certain steps of an algorithm without changing the algorithm's structure.*

核心思路：在抽象基类中用一个方法（模板方法）把算法的执行顺序写死，把其中会变化的步骤声明为抽象方法或虚方法，交给子类实现；子类只能替换步骤内容，无法改变流程本身。

它体现的是著名的**好莱坞原则（Hollywood Principle）**——“Don't call us, we'll call you”：子类不主动调用流程，而是由基类的模板方法在合适的时机回调子类的实现。

## 二、解决什么问题

- 多个类执行**相同或相似的流程**，只在个别步骤上存在差异，导致大量重复代码。
- 算法的整体结构应当保持稳定，不希望子类改写执行顺序，只允许定制部分步骤。
- 希望把公共流程集中到一处维护，避免修改流程时逐个改动所有子类。
- 某些步骤是可选的，需要子类决定“做不做”，而不是“怎么做”。
- 需要在不改变算法结构的前提下，控制子类扩展的边界（哪些能改、哪些不能改）。

## 三、角色结构

| 角色 | 职责 | 本项目的对应类 |
| --- | --- | --- |
| AbstractClass（抽象类） | 定义抽象的基本操作，并实现模板方法，规定算法骨架 | `AbstractClass`、`Hoagie` |
| Template Method（模板方法） | 按固定顺序调用基本操作与钩子，通常声明为非虚方法 | `AbstractClass.TemplateMethod()`、`Hoagie.MakeSandwich()` |
| Primitive Operation（基本操作） | 算法中的可变步骤，由子类实现（抽象方法） | `PrimitiveOperation1/2()`、`AddMeat/AddCheese/AddVegetables/AddCondiments()` |
| Hook（钩子） | 带默认实现的虚方法，子类可选择性地重写，常用于控制某步骤是否执行 | `CustomerWantsMeat/Cheese/Vegetables/Condiments()` |
| ConcreteClass（具体类） | 实现基本操作、按需重写钩子 | `ConcreteClassA/B`、`ItalienHoagie`、`VeggieHoagie` |
| Client（客户端） | 通过抽象类类型调用模板方法 | `TemplateMethodStructure`、`TemplateMethodPatternExample1` |

结构图可参考：[UML_Picture/template.gif](../../UML_Picture/template.gif)

## 四、C# 实现要点

1. **模板方法保持非虚**：模板方法一般不加 `virtual`，防止子类重写并破坏算法骨架；这是该模式的关键约束。
2. **基本操作用 `abstract`**：子类必须实现的步骤声明为抽象方法，编译期即可保证不会遗漏。
3. **可选步骤用钩子**：需要子类决定“做不做”或提供默认行为时，使用 `protected virtual` 方法返回值或空实现，子类按需 `override`。
4. **访问修饰符**：基本操作与钩子通常为 `protected`，避免客户端越过模板方法直接调用某个步骤，破坏执行顺序。
5. **少用继承层级**：模板方法依赖继承，流程变化时可能引发“子类爆炸”；如果变化的是整个算法而非固定流程中的步骤，应考虑策略模式。
6. **可在模板方法中安排固定步骤**：如示例中的 `CutBun()`、`WrapTheHoagie()`，它们不是虚方法，所有子类共用。

## 五、结合本项目案例

### 1. `Structure/TemplateMethodStructure.cs`——最小结构

场景：`TestTemplateMethodStructure.unity`，运行后 Console 输出：

```
ConcreteClassA.PrimitiveOperation1()
ConcreteClassA.PrimitiveOperation2()

ConcreteClassB.PrimitiveOperation1()
ConcreteClassB.PrimitiveOperation2()

```

要点：

- `AbstractClass.TemplateMethod()` 按 `PrimitiveOperation1()` → `PrimitiveOperation2()` → `Debug.Log("")` 的固定顺序执行，两个子类共用这套骨架。
- `PrimitiveOperation1/2()` 是抽象方法，`ConcreteClassA/B` 分别给出自己的实现。
- `Start()` 用 `AbstractClass` 类型引用具体对象，客户端只认识模板方法，不关心步骤细节。
- 每执行完一个对象都会输出一个空行，便于在 Console 中区分两次流程。

### 2. `Exmaple1/TemplateMethodPatternExample1.cs`——制作潜艇堡

场景：`TemplateMethodPatternExample1.unity`（目录名保留原有拼写 `Exmaple1`），运行后 Console 输出：

```
Making new Sandwich
Bun is Cut
Adding the Meat: Salami
Adding the Cheese: Provolone
Adding the Vegetables: Tomatoes
Adding the Condiments: Vinegar
Hoagie is wrapped.
Making new Sandwich
Bun is Cut
Adding the Vegetables: Tomatoes
Adding the Condiments: Vinegar
Hoagie is wrapped.
```

要点：

- `Hoagie.MakeSandwich()` 是模板方法，固定了“切面包 → 加配料 → 包装”的流程。
- `AddMeat()`、`AddCheese()`、`AddVegetables()`、`AddCondiments()` 是抽象的基本操作，由 `ItalienHoagie` 和 `VeggieHoagie` 实现。
- `CustomerWantsMeat()` 等是**钩子**：基类默认返回 `true`，`VeggieHoagie` 重写为 `false`，于是模板方法跳过加肉和加奶酪两步。
- `VeggieHoagie.AddMeat()`、`AddCheese()` 虽然留空，但由于对应钩子返回 `false`，它们根本不会被调用——这正是钩子的价值：用“是否执行”代替“执行空逻辑”。
- `CutBun()`、`WrapTheHoagie()` 是固定步骤，所有子类共用，子类无法改变。
- `BadExample.ItalienHoagie` 是对照用的反面示例：不用模板方法时，每个子类都要复制整套 `MakeSandwich()` 流程，流程一改就要改所有类。注意该类位于嵌套命名空间 `TemplateMethodPatternExample1.BadExample` 中，不参与场景运行。

## 六、优点与代价

**优点**

- 公共流程只写一次，消除子类中的重复代码，符合代码复用。
- 流程由基类集中控制，子类只能扩展指定步骤，结构稳定、边界清晰。
- 通过钩子提供灵活的扩展点，无需修改基类即可定制可选行为。
- 符合开闭原则：新增变体通常只需新增子类，不改动模板方法。

**代价**

- 依赖继承：C# 只有单继承，子类一旦继承抽象类，就难以再复用它处的实现。
- 子类数量可能膨胀，每个变体都需要一个子类。
- 流程中的每一步都可能被 `override`，调试时需要沿着继承链跳转，理解成本随层级增加。
- 模板方法把控制权交给基类，子类与基类形成较强的耦合（“基类调用子类”的反向控制）。

## 七、与相关模式的区别

| 对比项 | 模板方法模式 | 策略模式 | 工厂方法模式 |
| --- | --- | --- | --- |
| 复用方式 | 继承：子类重写步骤 | 组合：上下文持有策略对象 | 继承：子类决定创建哪种产品 |
| 变化的内容 | 固定流程中的个别步骤 | 整个算法可整体替换 | 对象创建的具体类型 |
| 控制方向 | 基类调用子类（反向控制） | 上下文委托策略 | 基类调用子类的创建方法 |
| 运行时切换 | 不支持，编译期确定 | 支持，运行时替换策略 | 不支持 |
| 典型场景 | 流程固定、步骤可变的业务/构建过程 | 同任务多种算法、AI 行为、支付方式 | 产品族创建 |

补充说明：

- 工厂方法模式可以看作模板方法模式的特例——模板方法中的“创建对象”步骤由子类实现。
- 如果算法整体都可能变化，而不只是其中几步，策略模式更合适；如果只是少数步骤不同且流程稳定，模板方法开销更小。

## 八、在 Unity 中的实践提示

- **角色/敌人的行为流程**：`UpdateAI()` 中按“感知 → 决策 → 行动 → 清理”固定顺序执行，各敌人子类只重写具体步骤。
- **UI 面板生命周期**：`Open()` 里统一执行“初始化数据 → 刷新界面 → 播放动画”，子类实现各自的刷新逻辑。
- **资源加载/关卡流程**：加载 → 初始化 → 校验 → 进入，把各阶段的实现交给具体关卡或资源类型。
- **编辑器工具与资源导入**：统一的处理流程中，按资源类型重写解析步骤。
- 注意 MonoBehaviour 的生命周期由 Unity 调用，无法作为模板方法；模板方法更适合你自建的普通 C# 类，或把 MonoBehaviour 作为驱动、把流程放在普通类中。
- 若变体很多、且需要运行时切换，考虑用策略模式或组合代替继承，避免类爆炸。

## 九、如何运行案例

1. 用 Unity 2022.3.20f1 打开项目。
2. 分别打开以下场景并进入 Play 模式，观察 Console：
   - `Assets/Behavioral Patterns/Template Method Pattern/Structure/TestTemplateMethodStructure.unity`
   - `Assets/Behavioral Patterns/Template Method Pattern/Exmaple1/TemplateMethodPatternExample1.unity`
3. 对照本文第三节，在代码中找出模板方法、基本操作与钩子；可尝试新增一个具体类（例如只加蔬菜和调料的变体），验证无需修改模板方法即可得到新的制作流程。
