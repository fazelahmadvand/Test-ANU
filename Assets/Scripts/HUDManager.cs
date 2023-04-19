using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager>
{

    [SerializeField] private TMP_Text counterTxt;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleTxt;
    [SerializeField] private TMP_Text restartTxt;
    [SerializeField] private Button btn;
    [SerializeField] private Button nextSceneBtn;


    [SerializeField] private Image img;
    [SerializeField] private float fadeDuration = .5f;
    private void Start()
    {
        panel.SetActive(false);
        img.gameObject.SetActive(true);
        img.DOFade(0, fadeDuration).OnComplete(() => img.gameObject.SetActive(false));
    }

    public void SetCounterValue(int val)
    {
        counterTxt.text = "X" + val;
    }


    public void ShowGameResult(GameState state)
    {
        panel.SetActive(true);
        if (state == GameState.Win)
        {
            restartTxt.text = "Restart";
            nextSceneBtn.gameObject.SetActive(true);

        }
        else if (state == GameState.Lose)
        {
            restartTxt.text = "Try Again";
            nextSceneBtn.gameObject.SetActive(false);

        }
        titleTxt.text = state.ToString();
        btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });

        nextSceneBtn.onClick.AddListener(() =>
        {
            var count = SceneManager.sceneCount;

            var currenScene = SceneManager.GetActiveScene();

            bool isEnd = currenScene.buildIndex > count - 1;
            if (isEnd)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(currenScene.buildIndex + 1);

            }

        });

    }








}
