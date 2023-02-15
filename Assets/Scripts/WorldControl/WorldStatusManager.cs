using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldStatusManager : MonoBehaviour
{
    public static WorldStatusManager WSMInstance;
    [SerializeField] GameObject sun;
    [SerializeField] GameObject moon;
    public float timeOfDay;
    float hourTimer;
    float secondTimer;
    private const float secondsPerHour = 10;
    private const int hoursPerDay = 24;
    [ field : SerializeField] public float currentTemperature { get; private set; }
    [SerializeField] float maxTemperature;
    [SerializeField] float minTemperature;
    
    float nextDayTemp;
    float lastDayTemp;
    float nextNightTemp;
    bool needtoChangeTheTemp = true;
    [SerializeField] float temperatureDeviation;
    public bool isDark;
    [SerializeField] Text tempText;

    public float timeSpeed=1;
    // Start is called before the first frame update
    private void Awake() {
        WSMInstance = this;
    }
    void Start()
    {
        UpdateDayNightCycle();
        SetNextMinMaxTemperature();
        currentTemperature = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DayNightCycle();
        Time.timeScale = timeSpeed;
        tempText.text = currentTemperature.ToString();
    }

    private void UpdateDayNightCycle()
    {
        if (timeOfDay < 6 || timeOfDay > 18)
        {
            isDark = true;
        }
        else
        {
            isDark = false;
        }
        if (timeOfDay > 0 && timeOfDay < 12)
        {
            currentTemperature = currentTemperature + (nextDayTemp / 12);
        }
        else
        {
            currentTemperature = currentTemperature - ((lastDayTemp - nextNightTemp) / 12);
        }
    }

    private void DayNightCycle()
    {
        hourTimer += Time.deltaTime;
        secondTimer += Time.deltaTime;
        if (secondTimer >= 1){
            sun.transform.Rotate(1.5f, 0, 0);
            moon.transform.Rotate(1.5f,0,0);
            secondTimer=0;
        }
        if (hourTimer >= secondsPerHour)
        {
            hourTimer = 0;
            timeOfDay++;
            if(timeOfDay == 0 || timeOfDay == 12){
                SetNextMinMaxTemperature();
                lastDayTemp = nextDayTemp;
            }
            if (timeOfDay >= hoursPerDay)
            {
                timeOfDay = 0;
            }
            UpdateDayNightCycle();
        }
    }
    void SetNextMinMaxTemperature()
    {
        nextDayTemp = Random.Range(maxTemperature - temperatureDeviation, maxTemperature);
        nextNightTemp = Random.Range(minTemperature, minTemperature + temperatureDeviation);
    }
}
