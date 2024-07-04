using System.Collections;
using UEasyUI;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    [SerializeField]
    private TimerComponent _timer;

    [SerializeField]
    private RedDotComponent _redDot;

    [SerializeField]
    private EventComponent _event;

    public static TimerComponent Timer { get; private set; }

    public static RedDotComponent RedDot { get; private set; }

    public static EventComponent Event { get; private set; }

    public static Localization Localization { get; private set; }

    private void Awake()
    {
        Timer = _timer;
        RedDot = _redDot;
        Event = _event;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
