using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boking
{
    public class ControllerManager : MonoSingleton<ControllerManager>
    {

        public Dictionary<string, DialogController> m_DialogControllers = new Dictionary<string, DialogController>();

        public Dictionary<string, ViewController> m_ViewControllers = new Dictionary<string, ViewController>();

        /// <summary>
        /// 注册弹窗Controller
        /// </summary>
        public void Register(string controllerName, DialogController controller)
        {
            m_DialogControllers.Add(controllerName, controller);
        }

        /// <summary>
        /// 注册视图Controller
        /// </summary>
        public void Register(string controllerName, ViewController controller)
        {
            m_ViewControllers.Add(controllerName, controller);
        }
        
        public bool GetDialogController(string controllerName, out DialogController controller)
        {
            return m_DialogControllers.TryGetValue(controllerName, out controller);
        }

        public bool GetViewController(string controllerName, out ViewController controller)
        {
            return m_ViewControllers.TryGetValue(controllerName, out controller);
        }


        public void Show(string controllerName)
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
