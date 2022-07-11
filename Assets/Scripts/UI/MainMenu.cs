using PONG.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI lives1;
    [SerializeField] TextMeshProUGUI lives2;
    [SerializeField] TextMeshProUGUI lives3;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Round1Lives(Slider slider)
    {
        gameManager.SetR1Lives((int)slider.value);
        lives1.SetText(slider.value.ToString("00"));
    }

    public void Round2Lives(Slider slider)
    {
        gameManager.SetR2Lives((int)slider.value);
        lives2.SetText(slider.value.ToString("00"));
    }

    public void Round3Lives(Slider slider)
    {
        gameManager.SetR3Lives((int) slider.value);
        lives3.SetText(slider.value.ToString("00"));
    }
}
