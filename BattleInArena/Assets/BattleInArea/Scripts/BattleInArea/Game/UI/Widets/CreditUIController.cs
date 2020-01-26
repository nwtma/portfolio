using UnityEngine;
using UnityEditor;

namespace BattleInArea.Game
{
    public class CreditUIController : Core.UI.Controller
    {
        public Event.Selected evt;
        public Animator animator;
        public override int ID
        {
            get { return (int)UI.ID.CreditUIController; }
        }
        private void Start()
        {
            evt = Core.Event.Getter.Get<Event.Selected>();
            animator = GetComponentInChildren<Animator>();
            animator.enabled = true;
        }

        public void OnClickButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.CreditButton;
            Core.Event.Dispatcher.Dispatch(evt);
        }

    }
}