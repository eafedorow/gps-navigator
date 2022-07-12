using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown : MonoBehaviour
{



    public Transform destinationPoints;

    public Transform usermark;

    Pathfinding pathfinding;

    public GameObject stopButton;
    public GameObject header;
    public Text headerText;

    public GameObject searchPanel;
    public GameObject dropPanel;

    public Transform buttonPrefab;
    public InputField inputField;

    public RectTransform listOfPoints;

    private string currentTarget;

    private bool isPathBuilding;


    public LineRenderer line;



    private void Start()
    {
        pathfinding = GameObject.Find("A*").GetComponent<Pathfinding>();
        isPathBuilding = false;
    }


    public void SearchPoints() {

        string searchText = inputField.text;
        if (!string.IsNullOrEmpty(searchText))
        {
            foreach (Transform child in listOfPoints)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < destinationPoints.childCount; i++)
            {
                if (destinationPoints.GetChild(i).name.ToLower().Contains(inputField.text.ToLower()))
                {
                    var instance = GameObject.Instantiate(buttonPrefab.gameObject) as GameObject;
                    instance.transform.SetParent(listOfPoints, false);


                    instance.name = destinationPoints.GetChild(i).name;
                    instance.GetComponentInChildren<Text>().text = instance.name;

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
        else {
            foreach (Transform child in listOfPoints)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < destinationPoints.childCount; i++)
            {
                
                var instance = GameObject.Instantiate(buttonPrefab.gameObject) as GameObject;
                instance.transform.SetParent(listOfPoints, false);


                instance.name = destinationPoints.GetChild(i).name;
                instance.GetComponentInChildren<Text>().text = instance.name;

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

    public void DropdownCall()
    {
        StartCoroutine(ShowPanel());
    }
    private float stepForAplha = 0.04f;
    private float timeUpdateAlpha = 0.01f;
    IEnumerator ShowPanel()
    {
        
        Debug.Log(dropPanel.GetComponent<CanvasGroup>().alpha);
        dropPanel.SetActive(true);

        float alpha = 0f;

        while (dropPanel.GetComponent<CanvasGroup>().alpha < 1.0f)
        {

            alpha += stepForAplha;
            dropPanel.GetComponent<CanvasGroup>().alpha = alpha;
            yield return new WaitForSeconds(timeUpdateAlpha);
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

                yield return new WaitForSeconds(0.6f);
            }
        }
    }


    IEnumerator HidePanel()
    {

        if (dropPanel.activeInHierarchy)
        {
            float alpha = 1.0f;

            while (dropPanel.GetComponent<CanvasGroup>().alpha > 0)
            {

                alpha -= stepForAplha;
                dropPanel.GetComponent<CanvasGroup>().alpha = alpha;
                yield return new WaitForSeconds(timeUpdateAlpha);
            }
            dropPanel.SetActive(false);
        }
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
