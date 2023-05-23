using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldStatusManager : MonoBehaviour
{
    public static WorldStatusManager WSMInstance;
    [SerializeField] GameObject sun;
    [SerializeField] GameObject moon;
    [SerializeField] float rotationSpeed= 1.5f;
    public float timeOfDay;
    float hourTimer;
    float secondTimer;
    private const float secondsPerHour = 10;
    private const int hoursPerDay = 24;
    [ field : SerializeField] public float currentTemperature { get; private set; }
    [SerializeField] float maxTemperature;
    [SerializeField] float minTemperature;
    [SerializeField] Slider worldSpeedSlider;
    
    float nextDayTemp;
    float lastDayTemp;
    float nextNightTemp;
    [SerializeField] float temperatureDeviation;
    public bool isDark;
    
    [SerializeField] Text tempText;
    [SerializeField] Text timeText;
    [SerializeField] Text worldSpeedText;
    GameObject[] lights;
    public bool isTitle = false;

    public float timeSpeed=1;
    public bool isTutorial=false;
    // Start is called before the first frame update
    private void Awake() {
        WSMInstance = this;
        lights = GameObject.FindGameObjectsWithTag("TimeLight");
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
        if (!isTitle)
        {
            if (!isTutorial)
            {
                timeSpeed = worldSpeedSlider.value;
            }
            Time.timeScale = timeSpeed;
            worldSpeedText.text = $"Time speed x {timeSpeed}";
            string time = timeOfDay.ToString();
            if (hourTimer < 9)
            {
                time += $":0{hourTimer.ToString("F0")}";
            }
            else
            {
                time += $":0{9}";
            }
            timeText.text = $"Time: {time}";
            tempText.text = $"Temp:{currentTemperature.ToString("F1")}°C";
        }

        sun.transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        moon.transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
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
        if (timeOfDay > 3 && timeOfDay < 15)
        {
            currentTemperature = currentTemperature + (nextDayTemp / 12);
            if(currentTemperature > maxTemperature){
                currentTemperature = maxTemperature;
            }
        }
        else
        {
            currentTemperature = currentTemperature - ((lastDayTemp - nextNightTemp) / 12);
            if (currentTemperature < minTemperature)
            {
                currentTemperature = minTemperature;
            }
        }
        if (timeOfDay < 7 || timeOfDay > 17)
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(false);
            }
        }
    }

    private void DayNightCycle()
    {
        
        hourTimer += Time.deltaTime;
        secondTimer += Time.deltaTime;
        if (secondTimer >= 1){
            //sun.transform.Rotate(1.5f, 0, 0);
           // moon.transform.Rotate(1.5f,0,0);
           // secondTimer=0;
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
    public void ChangeWorldSpeed(int speed){
        worldSpeedSlider.value=speed;
        timeSpeed = speed;

    }
}
