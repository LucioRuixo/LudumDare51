using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerHandle : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [Space]
    [SerializeField] private int duration = 10;
    [SerializeField] private int warningTime = 5;
    [SerializeField, Range(0f, 1f)] private float warningFadeDuration = 0.5f;

    private float timer;

    public event Action OnTimerEnd;

    private void Start()
    {
        StartTimer();
    }

    private void EndTimer()
    {
        OnTimerEnd?.Invoke();
    }

    public void StartTimer()
    {
        StartCoroutine(Timer(duration));
    }

    #region Coroutines
    private IEnumerator Timer(float duration)
    {
        timer = duration;

        float timeSinceLastSecond = 0f;
        bool warn = false;

        while (timer > 0f)
        {
            yield return null;
            timeSinceLastSecond += Time.deltaTime;

            if (timeSinceLastSecond >= 1f)
            {
                AudioManager.Get().PlayGameplaySFX(AudioManager.GameplaySFXs.Clock);

                warn = timer < warningTime + 0.1f;
                if (warn) StartCoroutine(FlashRed(0.5f));

                timeSinceLastSecond = 0f;
            }

            timer -= Time.deltaTime;
            if (timer < 0f) timer = 0f;

            timerText.text = $"{timer.ToString("0.00")}";
        }

        EndTimer();
    }

    private IEnumerator FlashRed(float fadeDuration)
    {
        Color initialColor = timerText.color;
        float fadeDurationHalf = fadeDuration / 2f;

        float t = 0f;
        while (timerText.color != Color.red)
        {
            t += Time.deltaTime / fadeDurationHalf;
            timerText.color = Color.Lerp(initialColor, Color.red, t);

            yield return null;
        }

        t = 0f;
        while (timerText.color != initialColor)
        {
            t += Time.deltaTime / fadeDurationHalf;
            timerText.color = Color.Lerp(Color.red, initialColor, t);

            yield return null;
        }
    }
    #endregion
}