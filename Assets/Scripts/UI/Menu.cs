using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public CanvasGroup textCanvasGroup;
    public Text text;

    public Image step1;
    public Image step2;

    public GameObject menu;

    /// <summary>
	/// Широта
	/// </summary>
	public float latitude;

    /// <summary>
    /// Долгота
    /// </summary>
    public float longitude;

    private int buttonPressed = 0;

    public void StartChecking()
    {
        buttonPressed++;
        StartCoroutine(CheckPosition());

    }

    private float stepForAplha = 0.01f;
    private float timeUpdateAlpha = 0.02f;


    public IEnumerator CheckPosition()
    {
        if(buttonPressed == 1) {
            step1.color = Color.gray;
            step2.color = Color.red;

            while (textCanvasGroup.alpha != 0)
            {
                textCanvasGroup.alpha -= 0.08f;
                yield return new WaitForSeconds(0.03f);

            }

            text.text = "Авторизация...";


            while (textCanvasGroup.alpha != 1)
            {
                textCanvasGroup.alpha += 0.08f;
                yield return new WaitForSeconds(0.03f);

            }
            yield return new WaitForSeconds(0.7f);
/*
            Если геолокация у пользователя выключена
                или же у приложения нет прав отслеживания местоположения*/
           /* if (!Input.location.isEnabledByUser)
            {

                while (textCanvasGroup.alpha != 0)
                {
                    textCanvasGroup.alpha -= 0.08f;
                    yield return new WaitForSeconds(0.03f);

                }
                text.text = "GPS не доступен...";

                while (textCanvasGroup.alpha != 1)
                {
                    textCanvasGroup.alpha += 0.08f;
                    yield return new WaitForSeconds(0.03f);

                }
                yield break;
            }


            //Инициализация точности расчёта GPS координат
            Input.location.Start();


            if (Input.location.status == LocationServiceStatus.Failed)
            {
                while (textCanvasGroup.alpha != 0)
                {
                    textCanvasGroup.alpha -= 0.08f;
                    yield return new WaitForSeconds(0.03f);

                }
                text.text = "Не удалось определить текущее положение...";
                while (textCanvasGroup.alpha != 1)
                {
                    textCanvasGroup.alpha += 0.08f;
                    yield return new WaitForSeconds(0.03f);

                }
                yield break;
            }

            //Получение текущих данных
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;



*/

            /*
                        double leftBorderLat = 59.127582114979205f;

                        double leftBorderLon = 37.843737602233894f;

                        double rightBorderLat = 59.138359f;

                        double rightBorderLon = 37.879180f;


                        if (latitude > leftBorderLat && longitude > leftBorderLon &&
                            latitude < rightBorderLat && longitude < rightBorderLon)
                        {

            */
            while (textCanvasGroup.alpha != 0)
                {
                    textCanvasGroup.alpha -= 0.08f;
                    yield return new WaitForSeconds(0.03f);

                }
                text.text = "Загрузка карты...";
                while (textCanvasGroup.alpha != 1)
                {
                    textCanvasGroup.alpha += 0.08f;
                    yield return new WaitForSeconds(0.03f);

                }
                yield return new WaitForSeconds(0.7f);
                while (textCanvasGroup.alpha != 0)
                {
                    textCanvasGroup.alpha -= 0.08f;
                    yield return new WaitForSeconds(0.03f);

                }
                text.text = "Получение точек назначения...";
                while (textCanvasGroup.alpha != 1)
                {
                    textCanvasGroup.alpha += 0.08f;
                    yield return new WaitForSeconds(0.03f);

                }
                yield return new WaitForSeconds(1.5f);

                if (menu.activeInHierarchy)
                {
                    float alpha = 1.0f;

                    while (menu.GetComponent<CanvasGroup>().alpha > 0)
                    {

                        alpha -= stepForAplha;
                        menu.GetComponent<CanvasGroup>().alpha = alpha;
                        yield return timeUpdateAlpha;
                    }
                    menu.SetActive(false);
                }
       /* }
        else
        {

            while (textCanvasGroup.alpha != 0)
            {
                textCanvasGroup.alpha -= 0.08f;
                yield return new WaitForSeconds(0.03f);

            }
            text.text = "Вы находитель не на территории ОАО \"Северсталь-метиз\"...";
            while (textCanvasGroup.alpha != 1)
            {
                textCanvasGroup.alpha += 0.08f;
                yield return new WaitForSeconds(0.03f);

            }
            yield return new WaitForSeconds(0.7f);
        }*/



    }
        buttonPressed--;
    }

    public void exitApplictaion() {
        Application.Quit();
    }
}
