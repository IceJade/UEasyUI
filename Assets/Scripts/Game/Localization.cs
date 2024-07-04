using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace UEasyUI
{
    // 这个地方可以优化一下，所有的DialogId支持字符串也支持整形，
    // 在初始化的时候加到两个字典中
    public class Localization : Singleton<Localization>
    {
        public Language Language { get; set; } = Language.ChineseSimplified;

        public string GetString(int language_id)
        {
            return "";
        }
    }
}
