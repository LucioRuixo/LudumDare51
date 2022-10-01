using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerHandle : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [Space]
    [SerializeField] private float duration = 10f;

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

        while (timer > 0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            timer -= Time.deltaTime;
            if (timer < 0f) timer = 0f;

            timerText.text = $"{timer.ToString("0.00")} s";
        }

        EndTimer();
    }
    #endregion
}