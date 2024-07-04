using UnityEngine;
using UnityEngine.UI;

public class UIHomeForm : MonoBehaviour
{
    public Button btnTimer;
    public Button btnRedDot;
    public Button btnGray;

    public Button btnHomeTimer;
    public Button btnHomeRedDot;

    public GameObject objTimeDemo;
    public GameObject objRedDotDemo;

    private void Awake()
    {
        btnTimer.onClick.RemoveAllListeners();
        btnTimer.onClick.AddListener(() =>
        {
            this.HideAll();

            this.objTimeDemo.SetActive(true);
            this.btnHomeTimer.gameObject.SetActive(true);
        });

        btnRedDot.onClick.RemoveAllListeners();
        btnRedDot.onClick.AddListener(() =>
        {
            this.HideAll();

            this.objRedDotDemo.SetActive(true);
            this.btnHomeRedDot.gameObject.SetActive(true);
        });

        btnHomeTimer.onClick.RemoveAllListeners();
        btnHomeTimer.onClick.AddListener(() =>
        {
            this.ShowHome();
        });

        btnHomeRedDot.onClick.RemoveAllListeners();
        btnHomeRedDot.onClick.AddListener(() =>
        {
            this.ShowHome();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        this.HideAll();

        this.btnTimer.gameObject.SetActive(true);
        this.btnRedDot.gameObject.SetActive(true);
        this.btnGray.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowHome()
    {
        this.HideAll();

        this.gameObject.SetActive(true);
        this.btnTimer.gameObject.SetActive(true);
        this.btnRedDot.gameObject.SetActive(true);
        this.btnGray.gameObject.SetActive(true);
    }

    private void HideAll()
    {
        this.btnTimer.gameObject.SetActive(false);
        this.btnRedDot.gameObject.SetActive(false);
        this.btnGray.gameObject.SetActive(false);

        this.btnHomeTimer.gameObject.SetActive(false);
        this.btnHomeRedDot.gameObject.SetActive(false);

        this.objTimeDemo.SetActive(false);
        this.objRedDotDemo.SetActive(false);
    }
}
