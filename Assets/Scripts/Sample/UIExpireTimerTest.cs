using UEasyUI;
using UnityEngine;

public class UIExpireTimerTest : MonoBehaviour
{
    public UIExpireTimer uiExpireTimer;

    // Start is called before the first frame update
    void Start()
    {
        uiExpireTimer.StartCountdown(8000000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
