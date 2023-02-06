using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] float temperatureDeviation;
    public bool isDark;

    public float timeSpeed=1;
    // Start is called before the first frame update
    private void Awake() {
        WSMInstance = this;
    }
    void Start()
    {
        UpdateDayNightCycle();
    }

    // Update is called once per frame
    void Update()
    {
        DayNightCycle();
        Time.timeScale = timeSpeed;
    }

    private void UpdateDayNightCycle()
    {
        if (timeOfDay < 6 || timeOfDay > 18)
        {
            currentTemperature = Random.Range(minTemperature, minTemperature + temperatureDeviation);
            isDark = true;
        }
        else
        {
            currentTemperature = Random.Range(maxTemperature - temperatureDeviation, maxTemperature);
            isDark = false;
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
            if (timeOfDay >= hoursPerDay)
            {
                timeOfDay = 0;
            }
            UpdateDayNightCycle();
        }
    }
}
