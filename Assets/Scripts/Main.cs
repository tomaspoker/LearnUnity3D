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

        }

        // Update is called once per frame
        void Update()
        {
            TimerManager.Instance.Update();
        }
    }

}