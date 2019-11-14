using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public enum DialogActionType
    {
        NONE,       // 无
        SCALE,      // 缩放
        POSITION    // 从指定位置缩放弹出
    }

    public enum DialogShowType
    {
        NORMAL, // 正常打开弹窗
        EVENT   // 需要派发事件，由监听方再次判定是否需要打开弹窗
    }

    public delegate void DialogCompletedCallback();
    
    public class DialogParams
    {
        public int Id { get; set; }

        /// <summary>
        /// 弹窗打开类型为指定位置缩放打开时，指定的起始位置
        /// </summary>
        public Vector2 InitialPosition { get; set; }

        /// <summary>
        /// 正常位置，默认屏幕中心
        /// </summary>
        public Vector2 NormalPosition { get; set; }

        /// <summary>
        /// 弹窗创建时，传入的参数
        /// </summary>
        public object InitialParams { get; set; }

        /// <summary>
        /// 执行进入动画的类型
        /// </summary>
        public DialogActionType EnterActionType { get; set; }

        /// <summary>
        /// 执行退出动画的类型
        /// </summary>
        public DialogActionType ExitActionType { get; set; }

        /// <summary>
        /// 显示弹窗时，需要怎么做？
        /// </summary>
        public DialogShowType ShowType { get; set; }

        /// <summary>
        /// 此弹窗是否保持在最上层
        /// 如果有两个弹窗同时声明，则后来者居上
        /// </summary>
        public bool IsKeepTop { get; set; }

        /// <summary>
        /// 此弹窗是否一直显示
        /// </summary>
        public bool IsKeepShow { get; set; }

        /// <summary>
        /// 此弹窗是否被锁定了，如果是，则不能进行其他操作，比如上拉弹窗层
        /// </summary>
        public bool IsKeepLock { get; set; }

        /// <summary>
        /// 此弹窗是否唯一
        /// </summary>
        public bool IsKeepUnique { get; set; }

        /// <summary>
        /// 此弹窗是否需要从服务器请求数据
        /// </summary>
        public bool IsNeedRequestData { get; set; }

        /// <summary>
        /// 弹窗进入动画完成回调
        /// </summary>
        public DialogCompletedCallback ShowCompletedCallback;

        private static int m_DialogId = 0;
        private static int DialogId => m_DialogId++;

        public DialogParams()
        {
            Id = DialogId;

            InitialPosition = new Vector2();

            NormalPosition = new Vector2();

            InitialParams = null;

            EnterActionType = DialogActionType.NONE;

            ExitActionType = DialogActionType.NONE;

            ShowType = DialogShowType.NORMAL;

            IsKeepTop = false;

            IsKeepShow = false;

            IsKeepLock = false;

            IsKeepUnique = true;

            IsNeedRequestData = true;

            ShowCompletedCallback = null;
        }
    }

    public class Dialog : MonoBehaviour
    {
        /// <summary>
        /// 标记是否正在执行进入或退出动作
        /// </summary>
        private bool m_IsActing;

        /// <summary>
        /// 弹窗进入的过渡动画完成
        /// </summary>
        private bool m_IsEnterCompleted;

        /// <summary>
        /// 是否存在背景
        /// </summary>
        private bool m_IsHasBackgroud;

        /// <summary>
        /// 此弹窗服务器数据是否请求完成（是否有回包）
        /// </summary>
        public bool m_IsDataLoadingCompleted;

        /// <summary>
        /// 此弹窗相关的Controller
        /// </summary>
        private DialogController Controller { get => ControllerManager.Instance.GetController(ControllerName); }

        /// <summary>
        /// 此弹窗相关的Controller的Name
        /// </summary>
        private string ControllerName { get; set; }

        private DialogCompletedCallback CloseCompletedCallback;


        private DialogParams m_DialogParams;


        private void Awake()
        {
            m_IsActing = false;
            m_IsEnterCompleted = false;
            m_IsHasBackgroud = false;

            m_IsDataLoadingCompleted = false;
        }

        private void Start()
        {
            InitData();

            InitUI();

            InitInternationalLanguageUI();
        }

        /// <summary>
        /// 设置弹窗打开时的外部传进来的参数
        /// </summary>
        /// <param name="showParams"></param>
        public void SetShowParams(ref DialogParams dialogParams)
        {
            m_DialogParams = dialogParams;
        }

        /// <summary>
        /// 初始化数据
        /// 继承者根据自己的业务逻辑，完成此接口
        /// </summary>
        private void InitData()
        {

        }

        /// <summary>
        /// 初始化UI
        /// 继承者需要在此接口中完成所有UI的引用赋值
        /// </summary>
        private void InitUI()
        {

        }
        
        /// <summary>
        /// 初始化国际化语言
        /// </summary>
        public void InitInternationalLanguageUI()
        {

        }

        /// <summary>
        /// 初始化点击事件、滑动事件、触摸事件
        /// </summary>
        private void InitClick()
        {

        }

        /// <summary>
        /// 进入动作开始
        /// </summary>
        private void OnEnterActionStart()
        {
            m_IsActing = true;
            m_IsEnterCompleted = false;
        }

        /// <summary>
        /// 进入动作完成
        /// </summary>
        private void OnEnterActionCompleted()
        {
            m_IsActing = false;
            m_IsEnterCompleted = true;

            SetNormalStatus();

            InitClick();

            if (m_IsDataLoadingCompleted)
            {
                RefreshUI();
            }

            m_DialogParams.ShowCompletedCallback?.Invoke();
        }

        private void StartEnterAction()
        {
            OnEnterActionStart();

            if (m_DialogParams.EnterActionType == DialogActionType.SCALE)
            {
                StartScaleEnterAction();
            }
            else if (m_DialogParams.EnterActionType == DialogActionType.POSITION)
            {
                StartPositionEnterAction();
            }
            else
            {
                OnEnterActionCompleted();
            }
        }

        private void StartScaleEnterAction()
        {
            OnEnterActionCompleted();
        }

        private void StartPositionEnterAction()
        {
            OnEnterActionCompleted();
        }

        private void OnExitActionStart()
        {
            m_IsActing = true;
        }

        /// <summary>
        /// 退出动作完成
        /// </summary>
        private void OnExitActionCompleted()
        {
            CloseCompletedCallback?.Invoke();

            CloseCompletedCallback = null;
        }

        private void StartExitAction()
        {
            OnExitActionStart();

            if (m_DialogParams.ExitActionType == DialogActionType.SCALE)
            {
                StartScaleExitAction();
            }
            else
            {
                OnExitActionCompleted();
            }
        }

        private void StartScaleExitAction()
        {
            OnExitActionCompleted();
        }

        /// <summary>
        /// 设置弹窗进入正常状态
        /// </summary>
        private void SetNormalStatus()
        {
            
        }

        /// <summary>
        /// 数据请求完成并回包
        /// </summary>
        public void OnDataLoadingCompleted()
        {
            m_IsDataLoadingCompleted = true;

            if (m_IsEnterCompleted)
            {
                RefreshUI();
            }
        }

        /// <summary>
        /// 请求此界面的服务器数据
        /// </summary>
        public void RequestServerData()
        {

        }

        /// <summary>
        /// 刷新界面UI
        /// </summary>
        public void RefreshUI()
        {

        }

        /// <summary>
        /// 打开此界面
        /// </summary>
        public void Show()
        {
            StartEnterAction();
        }

        /// <summary>
        /// 直接显示此界面
        /// 用于隐藏界面后重新显示界面
        /// </summary>
        public void Display()
        {
            SetNormalStatus();

            gameObject.SetActive(true);
        }

        /// <summary>
        /// 关闭此界面
        /// </summary>
        public void Close(DialogCompletedCallback callback)
        {
            CloseCompletedCallback = callback;

            StartExitAction();
        }

        /// <summary>
        /// 关闭此界面
        /// </summary>
        public void Close()
        {
            StartExitAction();
        }

        /// <summary>
        /// 隐藏此界面
        /// 如果被隱藏時有被打開的動作，則先停止，並設置爲正常模式，執行完成回調
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
    }

}
