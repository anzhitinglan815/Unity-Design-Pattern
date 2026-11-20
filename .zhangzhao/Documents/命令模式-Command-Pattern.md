# 命令模式（Command Pattern）

> 类型：行为型模式
> 本项目案例目录：`Assets/Behavioral Patterns/Command Pattern/`

## 一、定义

**将一个请求封装为一个对象，从而使你可用不同的请求对客户进行参数化；对请求排队或记录请求日志，以及支持可撤销的操作。**

英文原文：*Encapsulate a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.*

核心思路：把“谁在什么时候做什么”这件事本身变成对象。请求的发起者（Invoker）只持有命令对象并触发 `Execute()`，真正干活的是接收者（Receiver）；二者互不认识，从而可以随时替换、排队、记录和撤销命令。

## 二、解决什么问题

- **请求的发送者与执行者耦合过紧**：按钮、快捷键、AI 决策等发起方不得不了解具体业务类的接口。
- **需要撤销/重做**：希望把每一步操作记录成对象，按顺序反向执行。
- **需要排队、批处理或延迟执行**：如回合制游戏的指令队列、宏操作、任务系统。
- **需要记录日志或支持回放**：命令对象本身即可序列化为一条“操作记录”。
- **需要参数化调用方**：同一个按键/菜单项，在不同配置下执行不同操作。
- 需要把一组操作组合成一个整体（宏命令），对外表现为一个命令。

## 三、角色结构

| 角色 | 职责 | 本项目的对应类 |
| --- | --- | --- |
| Command（抽象命令） | 声明执行接口；需要撤销时再声明撤销接口 | `Command`、`ICommand` |
| ConcreteCommand（具体命令） | 绑定接收者与一个动作/参数，调用接收者完成请求 | `ConcreteCommand`、`CalculatorCommand`、`TurnTVOn/Off`、`TurnVolumeUp/Down`、`TurnItAllOff`、`MoveCommand`、`ConcreteCommand1/2` |
| Receiver（接收者） | 真正知道如何完成请求的业务对象 | `Receiver`、`Calculator`、`Television`、`Radio`、`MoveCommandReceiver`、`Receiver1/2` |
| Invoker（调用者） | 持有命令并触发执行，不关心命令内部细节；可维护命令历史 | `Invoker`、`User`、`DeviceButton`、`InputHandler` |
| Client（客户端） | 创建接收者和具体命令，并把命令装配到调用者上 | `CommandStructure`、`CommandExample1`、`TestCommandPattern`、`CommandPatternExample4` |

其中 `IElectronicDevice` 是接收者接口，`TurnItAllOff` 是一种**宏命令（Macro Command）**——把多个接收者聚合为一个命令。

结构图可参考：[UML_Picture/command.gif](../../UML_Picture/command.gif)

## 四、C# 实现要点

1. **命令持有接收者**：具体命令在构造时接收 Receiver，`Execute()` 中调用接收者对应方法，这是“绑定请求”的关键。
2. **调用者只依赖抽象**：Invoker 的字段类型应为 `Command` / `ICommand`，绝不出现具体命令类名；这样同一按键可随时换绑不同命令。
3. **撤销的两种实现**：
   - **反向操作**：保存足够参数，撤销时执行相反操作（本项目的计算器用相反运算符，移动命令用相反方向）。简单高效，但要求操作可逆。
   - **状态快照**：执行前保存接收者状态，撤销时恢复（可配合备忘录模式）。适用于不可逆操作（如删除、随机、消耗资源）。
4. **注意命令的可重入性**：`Execute()` 与 `UnExecute()` 之间不要修改命令自身保存的参数，否则撤销会使用错误信息。
5. **撤销/重做游标**：Invoker 用一个索引指向“当前已执行到哪条命令”；撤销时索引回退并调用 `UnExecute()`，重做时前移并调用 `Execute()`。
6. **宏命令**：具体命令内部持有一个接收者列表，`Execute()` 遍历执行；组合模式（Composite）常与命令模式配合。
7. **接口拆分**：若并非所有命令都支持撤销，可拆成 `ICommand` 与 `IUndoableCommand`，避免具体命令被迫实现空方法。

## 五、结合本项目案例

### 1. `Structure/CommandStructure.cs`——最小结构

场景：`TestCommandStructure.unity`，运行后 Console 输出：

```
Called Receiver.Action()
```

要点：

- `Command` 是抽象命令，持有 `Receiver` 引用并声明 `Execute()`。
- `ConcreteCommand.Execute()` 调用 `receiver.Action()`，完成“请求 → 接收者”的绑定。
- `Invoker` 通过 `SetCommand()` 接收命令、`ExecuteCommand()` 触发执行，自身不知道命令做什么。
- `CommandStructure.Start()` 是客户端：先创建接收者，再创建命令，最后装配给调用者。

### 2. `Example1/CommandExample1.cs`——支持无限撤销/重做的计算器

场景：`TestCommandExample1.unity`，运行后 Console 输出：

```
Current value = 100 ( following +100 )
Current value = 50 ( following -50 )
Current value = 500 ( following *10 )
Current value = 250 ( following /2 )

---- Undo 4 levels
Current value = 500 ( following *2 )
Current value = 50 ( following /10 )
Current value = 100 ( following +50 )
Current value = 0 ( following -100 )
Current value = 100 ( following +100 )
Current value = 50 ( following -50 )
Current value = 500 ( following *10 )
```

要点：

- `CalculatorCommand` 保存操作符、操作数和接收者 `Calculator`，一次计算即一个命令对象。
- `UnExecute()` 用相反运算符再算一次：撤销 `/2` 就执行 `*2`，从而回到原值。
- `User` 是调用者，同时承担命令历史管理：`_commands` 保存全部命令，`_current` 是撤销/重做游标。
- `Compute()` 创建命令 → 立即执行 → 加入历史；`Undo()` 游标回退并撤销；`Redo()` 游标前移并重做。
- 注意：`Undo(4)` 撤销的是最近 4 条命令，输出顺序与输入顺序相反；`Redo(3)` 再按原顺序重做前 3 条。
- 阅读代码时留意 `Redo()` 的边界条件写成 `_current < _commands.Count - 1`：如果只撤销最后一条命令，游标刚好等于 `Count - 1`，该条命令将无法重做。本示例先 `Undo(4)` 把游标归零，再 `Redo(3)`，恰好绕开了这个边界；若要用作通用实现，应改为 `_current < _commands.Count`。

### 3. `Example2/TestCommandPattern.cs`——遥控器与电子设备

场景：`command pattern example 1.unity`，运行后 Console 输出：

```
------------------Command Pattern--------------
TV is On
TV is Off
TV Turned Volume Up to 1
TV Turned Volume Up to 2
TV Turned Volume Up to 3
TV Turned Volume Down to 2
TV is Off
Radio is Off
TV is On
Radio is On
```

要点：

- `ICommand` 定义 `Execute()` / `Undo()`；`TurnTVOn`、`TurnTVOff`、`TurnVolumeUp`、`TurnVolumeDown` 是具体命令，分别绑定一个 `IElectronicDevice` 接收者。
- `DeviceButton` 是调用者：构造时注入命令，`Press()` 只调用 `cmd.Execute()`，对设备一无所知。
- `Television`、`Radio` 实现统一的接收者接口，因此同一套命令可以作用于不同设备。
- `TVRemove` 是简单工厂，集中创建接收者，客户端只拿到 `IElectronicDevice`。
- `TurnItAllOff` 是宏命令：持有一个设备列表，一次 `Press()` 关闭所有设备，`PressUndo()` 再统一打开。
- 每条命令的 `Undo()` 都是其 `Execute()` 的逆操作（开↔关、升↔降），所以撤销逻辑无需调用者参与。

### 4. `Example3/InputHandler.cs`——可撤销/重做的物体移动

场景：`command pattern example2.unity`（目录名与实际场景名不一致，注意按文件名打开）。

操作方式：

- 方向键：向上/下/左/右移动 `objectToMove`；
- `U`：撤销上一步移动；
- `R`：重做一步；
- 屏幕左上角 `OnGUI` 显示命令历史，`>` 标记当前游标位置。

要点：

- `MoveCommand` 保存目标物体、方向、距离和接收者 `MoveCommandReceiver`。
- `UnExecute()` 复用接收者，但把方向取反、移动相同距离，从而回到原位。
- `InputHandler` 是调用者，维护 `commands` 历史与 `currentCommandNum` 游标，同时监听键盘并对外暴露 `MoveUp/Down/Left/Right` 供 UI 按钮调用。
- `Start()` 会检查 `objectToMove` 是否赋值；未赋值时输出 `Debug.LogError` 并禁用组件，不进行演示。
- 撤销后若继续产生新移动，新命令会追加到列表末尾而不是截断“未来”命令；阅读逻辑时需注意历史列表并不做分支清理。

### 5. `Example4/CommandPatternExample4.cs`——命令队列

场景：`CommandPatternExample4.unity`，运行后 Console 输出：

```
Receiver1.Action:Command[hi]
Receiver2.Action:Param[666]
```

要点：

- `ConcreteCommand1` 与 `ConcreteCommand2` 分别携带字符串和整数参数，并绑定各自的接收者。
- `Invoker` 用 `List<Command>` 保存命令，`AddCommand()` 只入队，`ExecuteCommand()` 才统一执行并清空列表。
- 这是“对请求排队”的直观演示：命令的创建与执行在时间上分离，调用者可以攒一批操作后统一处理。
- 与 `Example2` 的 `TurnItAllOff` 相比，这里是多条异构命令的顺序队列；宏命令则是把多条操作合并成单个命令对象。

## 六、优点与代价

**优点**

- 解耦请求发起者与执行者：调用者只依赖命令接口，接收者可自由替换。
- 支持撤销/重做、排队、延迟执行、日志与回放等高级能力。
- 符合开闭原则：新增操作只需新增具体命令类，调用者代码不变。
- 可以把多个命令组合成宏命令或事务，整体执行/撤销。
- 命令对象天然适合序列化，便于存档、网络同步和录像回放。

**代价**

- 每个操作都需要一个命令类，类数量增加；简单场景可能显得繁琐。
- 撤销逻辑需要额外设计（逆操作或状态快照），并要保证每个命令都正确实现。
- 参数较多时，命令对象与接收者之间需要传参，增加样板代码。
- 如果命令保存了执行时刻的上下文却未完整记录，撤销可能产生错误结果。

## 七、与相关模式的区别

| 对比项 | 命令模式 | 策略模式 | 备忘录模式 |
| --- | --- | --- | --- |
| 封装对象 | 一个“请求”（动作 + 接收者 + 参数） | 一个“算法” | 一个“状态快照” |
| 调用方 | Invoker 持有命令，可在未来执行 | Context 持有策略，立即委托 | Caretaker 保存快照，按需恢复 |
| 是否关注接收者 | 是，具体命令绑定接收者 | 通常否，策略自身完成算法 | 否，关注状态本身 |
| 典型用途 | 撤销/重做、队列、宏、日志 | 切换算法/行为 | 撤销、存档、回滚 |

补充说明：

- 命令模式与策略模式结构相似，区别在**意图**：策略是“换一种做法”，命令是“把一次请求变成对象”。
- 需要精确恢复不可逆操作时，命令模式常与备忘录模式配合：命令在 `Execute()` 前保存接收者快照，`UnExecute()` 时恢复。
- 宏命令与组合模式（Composite）结合，可形成命令树，实现批量执行与撤销。

## 八、在 Unity 中的实践提示

- **输入重绑定**：把“跳跃/攻击/交互”封装为命令，按键映射表保存 `KeyCode → ICommand`，换键无需改业务代码。
- **撤销系统**：编辑器工具、建造/摆放系统、卡牌与回合制游戏用命令栈实现 Undo/Redo。
- **动作队列**：把玩家的连招、AI 指令放入队列，逐个执行，避免一帧内同时执行多个动作。
- **录像/回放与网络同步**：只记录命令及参数（而非每帧状态），回放时重新执行命令即可还原过程。
- **宏操作**：一键整理背包、批量出售等组合多个接收者的宏命令。
- 命令对象若需要引用场景对象，注意对象销毁后的空引用；可在命令中保存 ID 而非直接引用，或在使用前判空。
- 若只是为了解耦 UI 与逻辑，事件/委托可能更轻量；当需要**记录、排队、撤销**时，命令模式才真正体现价值。

## 九、如何运行案例

1. 用 Unity 2022.3.20f1 打开项目。
2. 分别打开以下场景并进入 Play 模式，观察 Console / Game 视图：
   - `Assets/Behavioral Patterns/Command Pattern/Structure/TestCommandStructure.unity`
   - `Assets/Behavioral Patterns/Command Pattern/Example1/TestCommandExample1.unity`
   - `Assets/Behavioral Patterns/Command Pattern/Example2/command pattern example 1.unity`
   - `Assets/Behavioral Patterns/Command Pattern/Example3/command pattern example2.unity`（先确认 `InputHandler.objectToMove` 已指定目标物体，再用方向键 / U / R 操作）
   - `Assets/Behavioral Patterns/Command Pattern/Example4/CommandPatternExample4.unity`
3. 对照本文第三节，在代码中找出抽象命令、具体命令、接收者和调用者；可尝试新增一个具体命令（如“静音”），观察调用者无需修改即可使用它。
