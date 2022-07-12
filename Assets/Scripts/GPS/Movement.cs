using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	private float leftBorderLat = 59.127582114979205f;

	private float leftBorderLon = 37.843737602233894f;

    void FixedUpdate()
    {
        float lat = Input.location.lastData.latitude;
        float lon = Input.location.lastData.longitude;
        float deltaLat = (float)GetDistance(lat, lon, leftBorderLat, lon);
        float deltaLon = (float)GetDistance(lat, lon, lat, leftBorderLon);

        this.transform.position = new Vector3(-885.8f + deltaLon, 1.0f, -381.7f + deltaLat);
    }

	public double GetDistance(double lat1, double long1, double lat2, double long2)
	{
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
}
