using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoSingleton<UImanager>
{
    [SerializeField] private Image sprayGauge;
    [SerializeField] private Text sprayGaugeText;

    [SerializeField] private Text StageProgress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSprayGauge(float _gauge)
    {
        sprayGaugeText.text = $"Spray : {_gauge:F1}%";
        sprayGauge.fillAmount = _gauge/100f;
    }

    public void SetStageProgress(float progress)
    {
        StageProgress.text = $"맵 진행 : {progress*100:F1}%";
    }
}
