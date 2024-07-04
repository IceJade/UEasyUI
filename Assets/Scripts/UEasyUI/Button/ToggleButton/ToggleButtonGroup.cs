using System.Collections.Generic;
using UnityEngine;

namespace UEasyUI
{
    public class ToggleButtonGroup : MonoBehaviour
    {
        private List<ToggleButton> btnToggleList = new List<ToggleButton>(5);

        private void OnDestroy()
        {
            btnToggleList.Clear();
        }

        public void Register(ToggleButton button)
        {
            btnToggleList.Add(button);
        }

        public void NotifyToggleOn(ToggleButton toggle)
        {
#if UNITY_EDITOR
            ValidateToggleIsInGroup(toggle);
#endif

            // disable all toggles in the group
            for (var i = 0; i < btnToggleList.Count; i++)
            {
                if (btnToggleList[i] == toggle)
                    continue;

                btnToggleList[i].Set(false);
            }
        }

        private void ValidateToggleIsInGroup(ToggleButton toggle)
        {
            if (toggle == null || !btnToggleList.Contains(toggle))
                Log.Error("Toggle {0} is not part of ToggleGroup {1}", toggle.name, this.name);
        }
    }
}

