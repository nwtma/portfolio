using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public class ResultUIController : Core.UI.Controller
    {
        Event.Selected evt;
        public GameObject[] VictoryUI;
        public GameObject[] DefeacUI;

        private float time = 0f;

        private int Index = 0;
        private List<int> playerIDList = new List<int>();

        public virtual void ResultUiShow()
        {

        }

        public void VictoryUIShow()
        {

        }

        public void DefeactUIShow()
        {

        }

        public override int ID
        {
            get { return (int)UI.ID.BookUIController; }
        }

        private void Start()
        {
            evt = Core.Event.Getter.Get<Event.Selected>();
        }

        

    }
}