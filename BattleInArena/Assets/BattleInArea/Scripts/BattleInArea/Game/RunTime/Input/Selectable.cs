using UnityEngine;
using UnityEditor;


namespace BattleInArea.Game
{
    public class Selectable : MonoBehaviour
    {
        public delegate void Delegate_Selected();

        public Delegate_Selected fun;

        public void OnSelected()
        {
            fun();
        }

        public void OnUnselected()
        {
            Debug.Log("OnUnselected");
        }
    }
}