



namespace BattleInArea.Core
{
    public abstract class State
    {
        public State(string newKey, params string[] newTransition)
        {
            Key = newKey;
            Transition = newTransition;
        }

        private string mKey;
        public string Key
        {
            get { return mKey; }
            private set { mKey = value; }
        }

        private string[] mTransition;
        public string[] Transition
        {
            get { return mTransition; }
            private set { mTransition = value; }
        }
        
        public virtual void OnEnter() { }
        public virtual bool OnExecute() { return false; }
        public virtual void OnExit() { }
        public virtual bool IsTransition() { return false; }
    }
}
