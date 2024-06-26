using System;
using TMPro;
using UnityEngine;

public class DateTimeManager : MonoBehaviour
{
    public TextMeshProUGUI dateTimeText;
    private DateTime gameDateTime;
    private float realTimeElapsed;

    private const float gameDayInRealSeconds = 3600f;

    void Start()
    {
        if (dateTimeText == null)
        {
            Debug.LogError("Text component is not assigned.");
            return;
        }

        gameDateTime = DateTime.Now;
        UpdateDateTimeText();
    }

    void Update()
    {
        realTimeElapsed += Time.deltaTime;
        double gameSecondsElapsed = (realTimeElapsed / gameDayInRealSeconds) * 86400;
        gameDateTime = gameDateTime.AddSeconds(gameSecondsElapsed);
        realTimeElapsed = 0;
        UpdateDateTimeText();
    }

    private void UpdateDateTimeText()
    {
        dateTimeText.text = gameDateTime.ToString("HH:mm / yyyy-MM-dd");
    }
}