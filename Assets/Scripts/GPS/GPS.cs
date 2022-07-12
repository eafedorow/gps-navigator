using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
	public static GPS Instance { set; get; }

	/// <summary>
	/// Широта
	/// </summary>
	public float latitude;

	/// <summary>
	/// Долгота
	/// </summary>
	public float longitude;



	/// <summary>
	/// Положение метки
	/// </summary>
	public Transform usermark;

	/// <summary>
	/// Сохранённое значение  долготы и широты. При запуске программы приравнивается к начальным координатам.
	/// </summary>
	public float oldLatValue, oldLonValue;


	public float leftBorderLat;

	public float leftBorderLon;



	//При запуске приложения
	void Start()
	{
		
		Input.location.Start();

		//Запоминание начальных координат
		oldLatValue = Input.location.lastData.latitude;
		oldLonValue = Input.location.lastData.longitude;

		leftBorderLat = 59.127582114979205f;

		leftBorderLon = 37.843737602233894f;
		Instance = this;
		DontDestroyOnLoad(gameObject);
		
	}

	void Update() {
		SyncGPSOnStart();
	}

	/*
	 Координаты для тестирования:
		Музей:
			59.133514f
			37.861081f
		Инструментальный цех:
			59.130665f
			37.860067f
		СПЦ-2:
			59.133018f
			37.849810f
		
			59.128474f
			37.858278f
		Внизу калибровочного:
			59.129480f
			37.867353f
		0,000275
		0,004309
	 */
	public void SyncGPSOnStart()
	{
		float lat = Input.location.lastData.latitude;
		float lon = Input.location.lastData.longitude;
		float deltaLat = (float)GetDistance(lat, lon, leftBorderLat, lon);
		float deltaLon = (float)GetDistance(lat, lon, lat, leftBorderLon);

		usermark.position = new Vector3(-885.8f + deltaLon, 0.0f, -381.7f + deltaLat);
		/*	oldPos = usermark.position;
    */
	}


	public static double GetDistance(double lat1, double long1, double lat2, double long2) {
		double R = 6372795;
		//перевод коордитат в радианы
		lat1 *= Math.PI / 180;
		lat2 *= Math.PI / 180;
		long1 *= Math.PI / 180;
		long2 *= Math.PI / 180;
		//вычисление косинусов и синусов широт и разницы долгот
		var cl1 = Math.Cos(lat1);
		var cl2 = Math.Cos(lat2);
		var sl1 = Math.Sin(lat1);
		var sl2 = Math.Sin(lat2);
		var delta = long2 - long1;
		var cdelta = Math.Cos(delta);
		var sdelta = Math.Sin(delta);
		//вычисления длины большого круга
		var y = Math.Sqrt(Math.Pow(cl2 * sdelta, 2) + Math.Pow(cl1 * sl2 - sl1 * cl2 * cdelta, 2));
		var x = sl1 * sl2 + cl1 * cl2 * cdelta;
		var ad = Math.Atan2(y, x);
		var dist = Convert.ToDouble(ad) * R; //расстояние между двумя координатами в метрах
		return dist;
	}

	
	private IEnumerator StartServiceLocation()
	{
		
		while (true)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);

			/*Если геолокация у пользователя выключена 
			или же у приложения нет прав отслеживания местоположения*/
			if (!Input.location.isEnabledByUser)
			{
				yield break;
			}


			//Инициализация точности расчёта GPS координат
			Input.location.Start();


			if (Input.location.status == LocationServiceStatus.Failed)
				{
					yield break;
				}

				//Получение текущих данных
				latitude = Input.location.lastData.latitude;
				longitude = Input.location.lastData.longitude;

			

			//Корутин ожидает 100 млсек, после чего снова совершаеи проверку GPS
			yield return new WaitForSeconds(0.1f);
			
		}

	}
}
