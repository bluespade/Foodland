using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasController : MonoBehaviour {

    GameMaster GM;
    Image fadePanel;
    Color fadePanelColor;
    private bool fullyFaded = false;

    public float fadeAmountPerFrame;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //Get reference to the GameMaster instance
        GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        //Destroy this instance if one already exists
        if (GM.GetCanvasInstance() != null)
        {
            Destroy(gameObject);
        }

        //Get important child components
        fadePanel = transform.Find("CameraFadePanel").GetComponent<Image>();
        fadePanelColor = new Color(0f, 0f, 0f);
        fadePanel.color = fadePanelColor;

        if (fadeAmountPerFrame == 0f)
        {
            fadeAmountPerFrame = 1f;
        }
    }

    // Use this for initialization
    void Start () {
        

        

        //BeginFadeFromBlack();
	}

    public void BeginFadeFromBlack()
    {
        StopCoroutine(FadeToBlack());
        StartCoroutine(FadeFromBlack());
    }

    public void BeginFadeToBlack()
    {
        StopCoroutine(FadeFromBlack());
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeFromBlack()
    {
        fullyFaded = false;
        float targetAlpha = 0f;

        while (fadePanel.color.a > targetAlpha)
        {
            fadePanelColor.a -= fadeAmountPerFrame;
            fadePanel.color = fadePanelColor;
            print("We are fading in...   " + fadePanelColor.a + "   " + fadeAmountPerFrame);
            yield return null;
        }

        if (fadePanel.color.a < 0f)
        {
            fadePanelColor.a = 0f;
            fadePanel.color = fadePanelColor;
        }

        print("Done fading in.");
    }

    IEnumerator FadeToBlack()
    {
        float targetAlpha = 1f;
        while (fadePanel.color.a < targetAlpha)
        {
            fadePanelColor.a += fadeAmountPerFrame;
            fadePanel.color = fadePanelColor;
            print("We are fading out...   " + fadePanelColor.a + "   " + fadeAmountPerFrame);
            yield return null;
        }

        if (fadePanel.color.a > 1f)
        {
            fadePanelColor.a = 1f;
            fadePanel.color = fadePanelColor;
        }

        print("Done fading out.");

        fullyFaded = true;
    }

    public bool GetFadeStatus()
    {
        return fullyFaded;
    }
}
