
namespace BattleInArea.Game
{
    public partial class Page
    {
        /// <summary>
        /// Battle In Area UI Manager
        /// </summary>
        public class Manager : Core.Page.Manager
        {
            public override void Prepare()
            {
                Add(new Page.Title());
                Add(new Page.Lobby());
                Add(new Page.InGame());
                Add(new Page.Loding());
                Add(new Page.Credit());
                Add(new Page.MainMenu());
                Add(new Page.Result());
                Add(new Page.Book());
                Add(new Page.RoomList());
            }
        }
    }
}