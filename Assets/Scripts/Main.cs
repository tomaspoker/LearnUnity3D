using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Boking
{
    public class Main : MonoBehaviour
    {
        public GameObject m_ObjView;
        public GameObject m_ObjDialog;
        public GameObject m_ObjGlobal;

        public Text m_LblCountDown;
        public Text m_LblClock;
        public Text m_LblLoop;

        // Start is called before the first frame update
        void Start()
        {
            Text lblCountDown = m_LblCountDown.GetComponent<Text>();
            lblCountDown.text = "";

            TimerManager.Instance.Delay(5f, () =>
            {
                lblCountDown.text = "Delay-Completed";
            });

            Text lblClock = m_LblClock.GetComponent<Text>();
            lblClock.text = "";
            TimerManager.Instance.Clock(10f, () =>
            {
                lblClock.text = "Clock-Completed";
            }, (remainTime, elapsedTime) =>
            {
                lblClock.text = remainTime.ToString();
            });

            Text lblLoop = m_LblLoop.GetComponent<Text>();
            lblLoop.text = "";
            TimerManager.Instance.Loop(1.0f, (remainTime, elapsedTime) =>
            {
                lblLoop.text = "Loop - " + elapsedTime.ToString();
            });

            CacheManager.Instance.Set<int>(1, 2);
            CacheManager.Instance.Set<int>(2, 10);
            CacheManager.Instance.Set<string>(3, "v1");

            print(CacheManager.Instance.Get<int>(1));
            print(CacheManager.Instance.Get<int>(2));
            print(CacheManager.Instance.Get<string>(3));

            EventManager.Instance.AddEvent(1, UpdateEvent);
            EventManager.Instance.AddEvent(1, UpdateEvent2);

            EventManager.Instance.DispatchEvent(1);

            EventManager.Instance.RemoveEvent(1, UpdateEvent);

            EventManager.Instance.DispatchEvent(1);

        }

        public void UpdateEvent(params object[] args)
        {
            print("==UpdateEvent==");
        }

        public void UpdateEvent2(params object[] args)
        {
            print("==UpdateEvent2==");
        }

        // Update is called once per frame
        void Update()
        {
            TimerManager.Instance.Update();
        }
    }

}