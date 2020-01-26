
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Core
{
    public partial class Page
    {
        public abstract class Manager : Interfaces.IManager
        {
            private Dictionary<int, IPage> pages = new Dictionary<int, IPage>();
            private IPage page;

            public abstract void Prepare();

            public void Add(IPage page)
            {
                pages.Add(page.ID, page);
            }

            public void OnUpdate()
            {
                if (page == null) return;

                page.OnExecute();
            }

            public void Chage(int id)
            {
                Core.Coroutine.instance.StartCoroutine(Process(id));
            }

            private IEnumerator Process(int id)
            {
                if (page != null)
                {
                    page.OnExit();
                }

                if (pages.ContainsKey(id))
                {
                    page = pages[id];

                    yield return Core.Coroutine.instance.StartCoroutine(page.OnPreEnter());

                    page.OnEnter();
                }
            }
        }
    }
}