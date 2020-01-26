using UnityEngine;
using UnityEditor;

namespace BattleInArea.Game
{
    public class InputManager : MonoBehaviour
    {
        public void Click()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                int layerMask = 1 << LayerMask.NameToLayer("Default");  // Player 레이어만 충돌 체크함
                if (Physics.Raycast(ray, out hit))
                {
                    Selectable comp = hit.transform.GetComponent<Selectable>();
                    if (comp == null)
                        return;
                    Debug.Log(comp);
                    comp.OnSelected();

                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UI.Manager.uiManager.Escape();
            }
        }
    }
}

