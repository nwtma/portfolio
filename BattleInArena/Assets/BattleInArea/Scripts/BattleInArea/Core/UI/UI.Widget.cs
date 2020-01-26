


namespace BattleInArea.Core
{
    public partial class UI
    {
        public abstract class Widget : UnityEngine.MonoBehaviour, Interfaces.IHasIdentity
        {
            public abstract int ID
            {
                get;
            }

            protected virtual void Awake()
            {
            }

            public virtual void Show() // 키고
            {
                gameObject.SetActive(true);
            }

            public virtual void Hide() // 끈다
            {
                gameObject.SetActive(false);
            }
        }
    }
}