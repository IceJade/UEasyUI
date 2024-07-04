using UnityEngine;

namespace UEasyUI
{
    public class RegisterRedDot : MonoBehaviour
    {
        public string Path;

        // Start is called before the first frame update
        void Start()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                GameEntry.RedDot.RegisterObject(Path, gameObject);
            }
        }

        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                GameEntry.RedDot.RemoveObject(Path, gameObject);
            }
        }
    }
}