using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetPointLocation : MonoBehaviour
{
	private string url = "https://icosy.ru/api/Points";

	private string urlAuth = "https://icosy.ru/api/Auth/login";

	public Transform destPoints;

	public float leftBorderLat = 59.127582114979205f;

	public float leftBorderLon = 37.843737602233894f;

	private string token ="";

	public GameObject buttonPrefab;

	public RectTransform listOfPoints;

	public GameObject dropPanel;
	public GameObject searchPanel;

	Pathfinding pathfinding;
	public Transform destinationPoints;

	public Transform usermark;
	private string currentTarget;

	public LineRenderer line;

	public GameObject stopButton;
	public GameObject header;
	public Text headerText;
	private bool isPathBuilding;


	[SerializeField]
	private GameObject point;

    public void Start()
    {
		pathfinding = GameObject.Find("A*").GetComponent<Pathfinding>();
		StartCoroutine(AuthRequest());
		StartCoroutine(MakePointsRequest());
    }


	public IEnumerator AuthRequest()
	{
		var bodyData = JsonUtility.ToJson("{\"login\": \"gps.client.user\", \"password\": \"gps#user1\"}");
		var postData = System.Text.Encoding.UTF8.GetBytes(bodyData);

		Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
		dataDictionary.Add("login", "gps.client.user");
		dataDictionary.Add("password", "gps#user1");

		string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(dataDictionary);
		Debug.Log(jsonData);


		UnityWebRequest request = UnityWebRequest.Post(urlAuth, jsonData);
		UploadHandlerRaw uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
		request.uploadHandler = uploadHandler;

		request.SetRequestHeader("Content-Type", "application/json");
		request.SetRequestHeader("accept", "application/json");

		yield return request.SendWebRequest();

        if (request.error != null)
		{
			Debug.Log(request.error);
		}
		else
		{
			var user = JsonConvert.DeserializeObject<User>(request.downloadHandler.text);

			token = user.token;

			Debug.Log(token);
		}

	}

	public IEnumerator MakePointsRequest() {

		while(token == "")
        {
			yield return new WaitForSeconds(1);
		}

		UnityWebRequest request = UnityWebRequest.Get(url);

		string authorization = authenticate("gps.client.user", "gps#user1");
		request.SetRequestHeader("Authorization", "Bearer " + token);

		yield return request.SendWebRequest();

		if (request.isNetworkError || request.isHttpError)
		{
			Debug.Log(request.error);
		}
		else {
			var points = JsonConvert.DeserializeObject<List<PointsResponse>>(request.downloadHandler.text);

			foreach (Transform child in listOfPoints)
			{
				Destroy(child.gameObject);
			}

			for (int i = 0; i < points.Count; i++) {
				float deltaLat = (float)GetDistance(points[i].latitude, points[i].longitude, leftBorderLat, points[i].longitude);
				float deltaLon = (float)GetDistance(points[i].latitude, points[i].longitude, points[i].latitude, leftBorderLon);
				var newPoint = GameObject.Instantiate(point, new Vector3(-885.8f + deltaLon, 0.0f, -381.7f + deltaLat),new Quaternion(0.0f,0f,0f, 0f), destPoints);
				newPoint.name = points[i].name;
				newPoint.transform.SetParent(this.transform);

			
				var instance = GameObject.Instantiate(buttonPrefab.gameObject) as GameObject;
				instance.transform.SetParent(listOfPoints, false);

					
				instance.name = newPoint.name;
				instance.GetComponentInChildren<Text>().text = newPoint.name;

				instance.GetComponent<Button>().onClick.AddListener(
					() =>
					{
						StartCoroutine(HidePanel());
						headerText.text = "Откуда: Мое местоположение\nКуда: " + instance.name;
						Transform destPoint = destinationPoints.transform.Find(instance.name);
						currentTarget = destPoint.name;
						stopButton.SetActive(true);
						header.SetActive(true);
						isPathBuilding = true;
						GyroControl.isPathBuilding = true;
						StartCoroutine(FindPath(destPoint.position, destPoint.name));
					}
					);
				
			}

		}

	}


	private float stepForAplha = 0.02f;
	private float timeUpdateAlpha = 0.02f;

	IEnumerator HidePanel() {

		if (dropPanel.activeInHierarchy) {
			float alpha = 1.0f;

			while (dropPanel.GetComponent<CanvasGroup>().alpha > 0) {

				alpha -= stepForAplha;
				dropPanel.GetComponent<CanvasGroup>().alpha = alpha;
				yield return timeUpdateAlpha;
			}
			dropPanel.SetActive(false);
		}
	}

	IEnumerator FindPath(Vector3 target, string name)
	{

		if (isPathBuilding)
		{
			Vector3 prevPosition = Vector3.zero;
			while (currentTarget == name)
			{

				if (prevPosition != usermark.position)
				{
					pathfinding.FindPath(usermark.position, target);
				}
				prevPosition = usermark.position;

				yield return new WaitForSeconds(1.0f);


			}
		}
	}

	string authenticate(string username, string password)
	{
		string auth = username + ":" + password;
		auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
		auth = "Basic " + auth;
		return auth;
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
	public void ChangePathBuilding()
	{
		isPathBuilding = false;
		line.positionCount = 0;
		header.SetActive(false);
		GyroControl.isPathBuilding = false;
		stopButton.SetActive(false);
	}
}
