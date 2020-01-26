
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Core
{
    public partial class UI
    {
        public abstract class Manager : MonoBehaviour, Interfaces.IManager
        {
            protected Dictionary<int, Widget> controllers = new Dictionary<int, Widget>();

            public virtual void Prepare()
            {
                Widget[] tmp = GameObject.Find("Client").GetComponentsInChildren<Widget>();
                for (int i = 0, ii = tmp.Length; ii > i; ++i)
                {
                    controllers.Add(tmp[i].ID, tmp[i]);
                    tmp[i].Hide();
                }
            }

            public T Get<T>(int key)  where T : Widget
            {
                if (controllers.ContainsKey (key))
                {
                    return (T)controllers[key];
                }

                return default(T);
            }

            public void Show(int key)
            {
                Widget ctn = Get<Widget>(key);
                ctn.Show();
            }

            public void Hide(int key)
            {
                Widget ctn = Get<Widget>(key);
                ctn.Hide();
            }
        }
    }
}