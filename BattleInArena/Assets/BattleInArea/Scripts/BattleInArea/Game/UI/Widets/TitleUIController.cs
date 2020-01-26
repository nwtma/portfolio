using UnityEngine;
using UnityEditor;

namespace BattleInArea.Game
{
    public class TitleUIController : Core.UI.Controller
    {
        public Animator animator;
         public  Event.Selected evt;

        public override int ID
        {
            get { return (int)UI.ID.TitleUIController; }
        }

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            animator.enabled = true;
            evt = Core.Event.Getter.Get<Event.Selected>();
        }

        public void OnClickButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.TitleButton;
            Core.Event.Dispatcher.Dispatch(evt);
        }
    }
}