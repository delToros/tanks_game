using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class WebRequestForDirectionalLight : MonoBehaviour
{
    public Light DirectionalLight;

    private string _url = "https://api.open-meteo.com/v1/forecast?latitude=40.71&longitude=-74.01&current_weather=true";

    private void Start()
    {
        StartCoroutine(FetchData());
    }

    private IEnumerator FetchData()
    {
        UnityWebRequest request = UnityWebRequest.Get(_url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string JSONResponse = request.downloadHandler.text;

            WeatherData weather = JsonUtility.FromJson<WeatherData>(JSONResponse);

            UpdateLighting(weather);
        }
        else
        {
            Debug.Log(request.error);
        }
    }

    private void UpdateLighting(WeatherData weather)
    {
        if (weather.current_weather.is_day == 1)
        {
            DirectionalLight.intensity = weather.current_weather.temperature / 4;
        }
        else
        {
            DirectionalLight.intensity = 0.0f;
        }
    }

}



[Serializable]
public class WeatherData
{
    public CurrentWeather current_weather;
}

[Serializable]
public class CurrentWeather
{
    public float temperature;
    public float is_day;
}