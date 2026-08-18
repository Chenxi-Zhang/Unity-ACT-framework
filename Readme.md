

# Unity ACT Framework

A Unity-based ACT framework that uses Timeline to author action events and drive gameplay through an event-centric architecture.

基于 Unity 的轻量级 ACT 框架，通过 Timeline 配置动作事件，以事件驱动战斗逻辑。

目前框架还在施工，欢迎观看：[施工视频](https://www.bilibili.com/video/BV1oUVAzWEMa)

**请注意**：项目的AI部分使用了Node Canvas，它是一个付费插件，如果要使用，请自行导入

## Overview

这是一个专为动作游戏设计的 Unity 框架，核心思想是将角色的所有动作（攻击、移动、防御等）都通过 Unity Timeline 来编排，而不是传统的动画状态机。Timeline 不仅控制动画播放，还通过自定义的 Clip 来驱动游戏逻辑，实现真正的时间轴驱动的战斗系统。

框架采用组件化设计，Actor 作为核心控制器管理各个子系统，各系统之间通过事件通信，保持松耦合。所有配置数据使用 ScriptableObject 管理，方便策划调整和快速迭代。

## Features

**战斗系统**
- Timeline 驱动的动作编排，所有动作都在 Timeline 中配置
- 8 方向攻击系统，支持动画平滑过渡
- 基于物理的碰撞检测，可配置的攻击判定框
- 高级反击系统：基于命中/未命中的反击机制，支持类型匹配
- 防御机制：招架和格挡，带不同动画响应
- Hit Counter：命名化的反击类型系统，实现复杂的战斗场景

**移动系统**
- 锁定状态下的移位移动（Strafing）
- 支持 Root Motion 和物理驱动的移动方式
- 受控移动：用于交互和剧情的脚本化移动
- 独立的根节点和模型旋转控制
- IK 注视系统：角色可以看向目标敌人

**相机系统**
- 锁定机制：点击锁定目标，支持平滑切换
- 智能目标选择：基于距离和角度的优先级排序
- 相机状态机：跟随和锁定状态的平滑过渡
- 集成 Cinemachine：专业级相机控制

**输入系统**
- 基于优先级的输入处理：高优先级动作会打断低优先级
- 输入缓冲：实现连招输入队列
- 使用 Unity 新输入系统

## How It Works

框架的核心是 Actor 组件，它就像一个指挥官，管理着角色的所有子系统。当玩家按下攻击键时：

1. Input 系统接收到输入，放入缓冲队列
2. Actor 检查当前是否可以攻击
3. 如果可以，通过 ActionPlayableDirector 播放对应的 Timeline
4. Timeline 中的 AttackClip 在特定帧激活攻击碰撞体
5. 碰撞体检测到命中，触发 Hit Counter 系统
6. 根据 HitCounterConfig，执行对应的反击动画
7. Timeline 播放完成，检查缓冲队列是否有下一个动作

整个流程都是事件驱动的，各系统之间不直接调用，而是通过事件通信，保持松耦合。

## Configuration

所有配置都使用 ScriptableObject 管理，策划可以在 Inspector 中直接调整：

- **HitCounterConfig**: 注册所有反击类型
- **AttackData**: 配置攻击伤害、Hit Counter 类型等
- **DefenceData**: 配置防御相关的反击动画
- **StrafeMoveAnimation**: 配置移位移动的动画映射
- **ActionTimelineAsset**: 封装 Timeline 资源，配置循环和过渡

这种数据驱动的设计让调整游戏数值和流程变得非常简单，不需要修改代码。

## Dependencies

- Unity Timeline: 核心动画和状态管理
- Unity Input System: 现代化输入处理
- Cinemachine: 相机系统（必需）
- NodeCanvas: AI 行为系统（可选）

## Architecture Highlights

框架的架构设计遵循以下原则：

**组件化解耦**: 每个主要系统都是独立的组件，通过 Actor 协调，没有直接依赖关系。可以轻松扩展或替换单个系统。

**数据驱动**: 大量使用 ScriptableObject 存储配置数据，策划可以独立调整游戏参数，无需修改代码。

**事件驱动**: 系统之间通过事件通信，而不是直接引用。输入事件触发动作队列，碰撞事件触发战斗响应，Timeline 事件驱动状态转换。

**手动 Timeline 控制**: 自定义的 PlayableDirector 实现，提供精确的帧级执行控制，支持暂停/慢动作等效果。
