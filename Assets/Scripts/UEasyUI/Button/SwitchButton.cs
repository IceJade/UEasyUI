using UnityEngine;

/********************************************************************************
 * 说    明: 切换按钮,点击后自动切换
 * 版    本: 1.0
 * 创建时间: 2021.06.09
 * 作    者: zhoumingfeng
 * 修改记录:
 * 
 ********************************************************************************/
namespace UEasyUI
{
    public class SwitchButton : CheckBoxButton
    {
        // 不选中时显示的节点
        public GameObject UncheckNode;

        public override void Checked(bool check)
        {
            base.Checked(check);

            if(null != UncheckNode)
                UncheckNode.SetActive(!check);
        }
    }
}
