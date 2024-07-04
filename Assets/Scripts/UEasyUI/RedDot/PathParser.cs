using System.Collections.Generic;

namespace UEasyUI
{
    public class PathParser
    {
        /// <summary>
        /// Key Child
        /// Value Parent
        /// </summary>
        Dictionary<string, string> m_PathMap = new Dictionary<string, string>();

        public string GetParent(string path)
        {
            string value;
            if (m_PathMap.TryGetValue(path, out value))
            {
                return value;
            }
            int index = path.LastIndexOf('/');
            if (index > 0)
            {
                value = path.Substring(0, index);
                m_PathMap.Add(path, value);
            }
            return value;
        }
    }
}
