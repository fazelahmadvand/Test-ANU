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
        }
        else if (state == GameState.Lose)
        {
            restartTxt.text = "Try Again";

        }
        titleTxt.text = state.ToString();
        btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });


    }








}
