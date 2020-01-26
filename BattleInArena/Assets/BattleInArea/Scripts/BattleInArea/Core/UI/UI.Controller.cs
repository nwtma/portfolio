




namespace BattleInArea.Core
{
    public partial class UI
    {
        public abstract class Controller : Widget
        {
            protected Widget[] widget;

            protected override void Awake()
            {
                base.Awake();

                widget = gameObject.GetComponentsInChildren<Widget>();
            }
        }
    }
}