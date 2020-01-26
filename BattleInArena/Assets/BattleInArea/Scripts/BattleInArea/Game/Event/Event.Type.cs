



namespace BattleInArea.Game
{
    public partial class Event
    {
        /// <summary>
        /// 이벤트 아이디
        /// </summary>
        public enum ID
        {
            Selected,
            PageModify,
            TurnModify,
            Item,
            GameResult, // 게임결과
            Event,
            EffectModigy,
            Room
        }

        /// <summary>
        /// 선택이벤트 타입
        /// </summary>
        public enum SelectedType
        {
            UI,
            Tile,
            Character,
            Item,
        }

        public enum PageState
        {
            OnEnter,
            OnExit,
            OnExecute,
        }


        public enum Turn
        {
            Invalid,
            MasterClient,
            OtherClient,
        }

        public enum Result
        {
            Alive,
            Die,
        }
        public enum EventType
        {
            AllUsedAP,
        }
    }
}