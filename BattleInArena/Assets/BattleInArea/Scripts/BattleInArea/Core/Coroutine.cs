

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Core
{
    public class Coroutine : MonoBehaviour
    {
        public static Coroutine instance;

        private void Awake()
        {
            instance = this;
        }

        static public IEnumerator Begin(IEnumerator routine)
        {
            yield return instance.StartCoroutine(routine);
        }
    }
}
