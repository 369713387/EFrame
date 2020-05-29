using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFrame.Example.FSM
{
    public enum PlayerState
    {
        Idle,
        Run,
        Attack,
        Dead
    }

    public enum PlayerEvent
    {
        Idle,
        Run,
        Attack,
        Dead
    }

    /// <summary>
    /// FSM示例代码
    /// 状态定义：1.站立，2.跑步，3.攻击，4.死亡
    /// 状态转换：1.站立->跑步，2.跑步->站立，3.站立->攻击，
    ///           4.攻击->站立，5.攻击->死亡，6.站立->死亡
    ///           7.跑步->死亡
    /// </summary>
    public class EFrameExample_FSM : MonoBehaviour
    {
        //创建状态机
        FSM<PlayerState, PlayerEvent> fsm = new FSM<PlayerState, PlayerEvent>(
            (playerState, playerEvent) => { Debug.Log("状态改变"); }
            );

        //创建状态
        FSM<PlayerState, PlayerEvent>.FSMState<PlayerState> idleState = 
            new FSM<PlayerState, PlayerEvent>.FSMState<PlayerState>(
                PlayerState.Idle
                , () => { Debug.Log("进入站立状态"); }
                , () => { Debug.Log("退出站立状态"); }
                );
        FSM<PlayerState, PlayerEvent>.FSMState<PlayerState> runState =
            new FSM<PlayerState, PlayerEvent>.FSMState<PlayerState>(
                PlayerState.Run
                , () => { Debug.Log("进入跑步状态"); }
                , () => { Debug.Log("退出跑步状态"); }
                );
        FSM<PlayerState, PlayerEvent>.FSMState<PlayerState> attackState =
            new FSM<PlayerState, PlayerEvent>.FSMState<PlayerState>(
                PlayerState.Attack
                , () => { Debug.Log("进入攻击状态"); }
                , () => { Debug.Log("退出攻击状态"); }
                );
        FSM<PlayerState, PlayerEvent>.FSMState<PlayerState> deadState =
            new FSM<PlayerState, PlayerEvent>.FSMState<PlayerState>(
                PlayerState.Dead
                , () => { Debug.Log("进入死亡状态"); }
                , () => { Debug.Log("退出死亡状态"); }
                );

        //创建状态转换
        FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState,PlayerEvent> idle2run =
            new FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent>(
                PlayerState.Idle
                ,PlayerEvent.Run
                ,PlayerState.Run
                ,RunEvent
                );

        FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent> run2idle =
            new FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent>(
                PlayerState.Run
                , PlayerEvent.Idle
                , PlayerState.Idle
                , IdleEvent
                );

        FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent> idle2attack =
            new FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent>(
                PlayerState.Idle
                , PlayerEvent.Attack
                , PlayerState.Attack
                , AttackEvent
                );

        FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent> attack2idle =
            new FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent>(
                PlayerState.Attack
                , PlayerEvent.Idle
                , PlayerState.Idle
                , IdleEvent
                );

        FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent> attack2dead =
            new FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent>(
                PlayerState.Attack
                , PlayerEvent.Dead
                , PlayerState.Dead
                , DeadEvent
                );

        FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent> run2dead =
            new FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent>(
                PlayerState.Run
                , PlayerEvent.Dead
                , PlayerState.Dead
                , DeadEvent
                );

        FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent> idle2dead =
            new FSM<PlayerState, PlayerEvent>.FSMTranslation<PlayerState, PlayerEvent>(
                PlayerState.Idle
                , PlayerEvent.Dead
                , PlayerState.Dead
                , DeadEvent
                );

        //添加状态
        void AddState()
        {
            fsm.AddState(idleState);
            fsm.AddState(runState);
            fsm.AddState(attackState);
            fsm.AddState(deadState);
        }

        //添加转换关系
        void AddTranslation()
        {
            fsm.AddTransition(idle2run);
            fsm.AddTransition(run2idle);
            fsm.AddTransition(idle2attack);
            fsm.AddTransition(attack2idle);
            fsm.AddTransition(run2dead);
            fsm.AddTransition(idle2dead);
            fsm.AddTransition(attack2dead);
        }

        //开启状态机
        private void Start()
        {
            AddState();
            AddTranslation();
            fsm.Start(PlayerState.Idle);            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                fsm.HandleEvent(PlayerEvent.Idle);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                fsm.HandleEvent(PlayerEvent.Run);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                fsm.HandleEvent(PlayerEvent.Attack);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                fsm.HandleEvent(PlayerEvent.Dead);
            }
        }

        #region 状态事件

        static void IdleEvent(object[] parm)
        {
            Debug.Log("执行站立事件");
        }

        static void RunEvent(object[] parm)
        {
            Debug.Log("执行跑步事件");
        }

        static void AttackEvent(object[] parm)
        {
            Debug.Log("执行攻击事件");
        }

        static void DeadEvent(object[] parm)
        {
            Debug.Log("执行死亡事件");
        }
        #endregion

    }

}


