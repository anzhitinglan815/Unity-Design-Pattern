# 策略模式（Strategy Pattern）

> 类型：行为型模式
> 本项目案例目录：`Assets/Behavioral Patterns/Strategy Pattern/`

## 一、定义

**定义一组算法，将每个算法都封装起来，并且使它们之间可以互换。策略模式让算法可以独立于使用它的客户端而变化。**

英文原文：*Define a family of algorithms, encapsulate each one, and make them interchangeable. Strategy lets the algorithm vary independently from clients that use it.*

核心思路：把“会变化的行为”从使用它的类中抽离出来，封装成实现同一接口的策略类；上下文（Context）只持有抽象策略，运行时替换具体策略即可改变行为，而调用方无需修改。

## 二、解决什么问题

- 一个类存在多种行为/算法，使用大量 `if-else` 或 `switch` 分支来选择。
- 需要在运行时动态切换算法，而不是在编译期写死。
- 同一算法在多个类中重复出现，或算法变化会波及大量子类。
- 希望把算法细节与使用算法的业务逻辑隔离开，避免客户端依赖具体实现。
- 用继承扩展行为会导致子类膨胀（例如每个子类都重写一个空方法）。

## 三、角色结构

| 角色 | 职责 | 本项目的对应类 |
| --- | --- | --- |
| Strategy（抽象策略） | 声明所有具体算法共有的接口 | `Strategy`、`SortStrategy`、`IFly` |
| ConcreteStrategy（具体策略） | 用统一接口实现一种具体算法/行为 | `ConcreteStrategyA/B/C`、`QuickSort`、`ShellSort`、`MergeSort`、`ItFlys`、`CantFly`、`FlyingSuperFast` |
| Context（上下文） | 持有抽象策略引用，把请求委托给当前策略；可提供切换策略的方法 | `Context`、`SortedList`、`Animal` |
| Client（客户端） | 创建具体策略并配置到上下文中 | `StrategyStructure`、`StrategyPatternExample1`、`StrategyPatternExample2` |

结构图可参考：[UML_Picture/strategy.gif](../../UML_Picture/strategy.gif)

## 四、C# 实现要点

1. 抽象策略可以是抽象类（如 `Strategy`），也可以是接口（如 `IFly`）；只有一个方法时接口更轻量。
2. 上下文通过**组合**持有一个策略引用，而不是用继承获得行为。
3. 切换策略有两种常见方式：构造函数注入（结构演示）或运行时 `SetXxxStrategy()` 替换（示例一、示例二）。
4. 客户端只依赖抽象策略，不出现具体算法类名以外的耦合；新增算法只需新增策略类。
5. 策略对象通常无状态、可复用；若策略本身需要数据，应通过方法参数传入，而不是依赖上下文内部状态。
6. 谁来决定使用哪种策略：可以由客户端显式选择，也可以配合简单工厂/配置表按条件创建策略对象。

## 五、结合本项目案例

### 1. `Structure/StrategyStructure.cs`——最小结构

场景：`TestStrategyStructure.unity`，运行后 Console 输出：

```
Called ConcreteStrategyA.AlgorithmInterface()
Called ConcreteStrategyB.AlgorithmInterface()
Called ConcreteStrategyC.AlgorithmInterface()
```

要点：

- `Strategy` 是抽象策略，声明唯一方法 `AlgorithmInterface()`。
- `ConcreteStrategyA/B/C` 分别实现一种算法，彼此互不依赖。
- `Context` 在构造函数中接收策略对象，`ContextInterface()` 只是把调用转发给当前策略。
- `Start()` 创建三个上下文，分别配置不同策略，演示“算法可互换”。

### 2. `Exmaple1/StrategyPatternExample1.cs`——可切换的排序算法

场景：`StrategyPatternExample1.unity`（目录名保留原有拼写 `Exmaple1`），运行后 Console 输出：

```
-------QuickSorted list-------
 Anna
 Jimmy
 Samual
 Sandra
 Vivek
-------ShellSorted list-------
 Anna
 Jimmy
 Samual
 Sandra
 Vivek
-------MergeSorted list-------
 Anna
 Jimmy
 Samual
 Sandra
 Vivek
```

要点：

- `SortStrategy` 是抽象策略，`QuickSort`、`ShellSort`、`MergeSort` 是具体策略。
- `SortedList` 是上下文，内部维护数据列表，`SetSortStrategy()` 可在运行时替换排序算法。
- 同一份学生名单依次用三种策略排序，`SortedList` 的对外接口始终不变。
- 注意：本例中 `QuickSort` 调用 `list.Sort()` 真正完成了排序；`ShellSort` 与 `MergeSort` 仅输出日志、未实现具体算法。由于列表已被快速排序排好，后两次输出仍然是排好序的结果，阅读代码时不要误以为后两种算法生效了。

### 3. `Example2/StrategyPatternExample2.cs`——动物飞行行为

场景：`StrategyPatternExample2.unity`，运行后 Console 输出：

```
Dog: I can't fly
Bird: Flying high
Dog: Flying high
Bird: Flying high
```

要点：

- `IFly` 是抽象策略，`ItFlys`、`CantFly`、`FlyingSuperFast` 是具体策略，分别表示会飞、不会飞、飞得极快。
- `Animal` 是上下文，字段 `flyingType` 组合了一个飞行策略；`TryToFly()` 委托给该策略。
- `Dog` 构造时默认使用 `CantFly`，`Bird` 默认使用 `ItFlys`——子类只负责选择默认策略，不重复实现飞行逻辑。
- `sparky.SetFlyingAbility(new ItFlys())` 在运行时把狗的飞行策略替换掉，行为立即改变，这正是策略模式的价值。
- `Dog` 与 `Bird` 不再因为“会不会飞”而被迫重写方法，继承体系保持稳定。

### 4. `Example2/BadCodeExample.cs`——反面示例

该文件不挂载到场景，仅用于对照说明：

- `Animal_BadExample` 把 `Fly()` 写进基类，`Dog_BadExample`、`Cat_BadExample` 只能重写为空方法，`Bird_BadExample` 的行为也被继承固定。
- 一旦要修改飞行方式，就需要改动所有子类；新增“飞得极快”的动物又要新增子类并重写方法。
- 这种设计让“会变化的行为”污染了继承体系，正是策略模式要消除的问题：把行为抽成策略对象后，行为变化不再影响类结构。

## 六、优点与代价

**优点**

- 算法与使用方解耦，上下文只依赖抽象策略，符合依赖倒置原则。
- 新增算法只需新增具体策略类，无需修改上下文，符合开闭原则。
- 运行时可以自由切换策略，行为灵活。
- 消除大量条件分支，代码可读性和可维护性更好。
- 算法可以被不同上下文复用，避免重复实现。

**代价**

- 客户端必须了解各策略的差异，才能选择合适的策略。
- 策略类数量会随算法增多而增加，简单场景可能显得过度设计。
- 策略与上下文之间如果需要共享数据，会增加通信成本（上下文可把自身数据作为参数传给策略）。
- 仅适合“算法/行为可变”的场景；如果行为固定，直接实现更简单。

## 七、与相关模式的区别

| 对比项 | 策略模式 | 状态模式 |
| --- | --- | --- |
| 关注点 | 同一任务的不同算法可互换 | 对象在不同状态下的行为变化 |
| 切换方式 | 通常由客户端显式选择策略 | 状态之间按规则自行流转 |
| 策略/状态对象 | 彼此独立，通常不知道对方 | 状态对象往往持有上下文并触发转移 |
| 典型场景 | 排序算法、支付方式、AI 行为 | 角色状态机、订单状态、ATM |

简单说：**策略模式是“你选一种做法”，状态模式是“对象自己换状态”**。二者结构相似，区别在意图和切换的主动权。

## 八、在 Unity 中的实践提示

- **角色 AI**：巡逻、追击、逃跑等行为各封装为策略，运行时根据条件替换。
- **移动/控制方式**：键盘、手柄、触屏、自动寻路可做成不同策略，便于切换输入方案。
- **伤害计算/技能效果**：不同伤害公式、暴击规则封装为策略，配合数据配置选择。
- **UI 排序/筛选**：列表按价格、等级、稀有度排序时，每种规则一个策略。
- 如果策略需要 Unity 生命周期或大量序列化字段，可以让策略实现为 `ScriptableObject`，在编辑器中配置资源引用。
- 只有当“确实存在多种可互换实现”时再引入策略；分支很少且稳定的代码，用简单的条件判断反而更清晰。

## 九、如何运行案例

1. 用 Unity 2022.3.20f1 打开项目。
2. 分别打开以下场景并进入 Play 模式，观察 Console：
   - `Assets/Behavioral Patterns/Strategy Pattern/Structure/TestStrategyStructure.unity`
   - `Assets/Behavioral Patterns/Strategy Pattern/Exmaple1/StrategyPatternExample1.unity`
   - `Assets/Behavioral Patterns/Strategy Pattern/Example2/StrategyPatternExample2.unity`
3. 对照本文第三节，在代码中找出每个案例的抽象策略、具体策略和上下文，并尝试新增一个具体策略，验证上下文无需修改即可使用它。
