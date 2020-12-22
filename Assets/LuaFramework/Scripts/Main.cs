using UnityEngine;
using System.Collections;

namespace LuaFramework {

    /// <summary>
    /// </summary>
    public class Main : MonoBehaviour {

        void Start() {
            AppFacade.Instance.StartUp();   //启动游戏

            //启动框架
            MoonScrpts.Common.Moon = new GameObject("Moon");
            DontDestroyOnLoad(MoonScrpts.Common.Moon);

            MoonScrpts.Common.Moon.AddComponent<MoonScrpts.Lancher>();
        }
    }
}