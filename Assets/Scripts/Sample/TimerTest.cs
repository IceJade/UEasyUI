using UEasyUI;
using UnityEngine;
using UnityEngine.UI;

public class TimerTest : MonoBehaviour
{
    [SerializeField]
    public Text timeCD;

    private int timerId = 0;
    private float leftTime = 8000;

    // Start is called before the first frame update
    void Start()
    {
        this.timerId = GameEntry.Timer.Startup((float _t) => {
                timeCD.text = TimeUtility.TimeConvert((int)(leftTime - _t));

                if (leftTime <= 0)
                    GameEntry.Timer.Stop(this.timerId);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
