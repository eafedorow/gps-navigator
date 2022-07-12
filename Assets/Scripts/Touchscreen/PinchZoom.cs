
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinchZoom : MonoBehaviour,IDragHandler, IBeginDragHandler
{
   public float perspectiveZoomSpeed = .03f;
   public float orthoZoomSpeed = .05f;
   public float spedswipe = .15f;
   public Camera camera;
    public Transform usermark;

    private bool dragging;
    private float timer;
    private Transform oldPosition;

    public GameObject dropPanel;

    private bool isMovingBack;


    void Start() {
        timer = 0.0f;
        isMovingBack = true;
        oldPosition = camera.transform;

    }

    void FixedUpdate() {
        if (isMovingBack)
        {
            camera.transform.position = Vector3.Lerp(new Vector3(oldPosition.position.x, oldPosition.position.y, oldPosition.position.z), new Vector3(usermark.position.x, 250.0f, usermark.position.z), Time.deltaTime);

        }

    }
    void Update()
    {
       
        if (Input.touchCount > 0)
        {

          


            timer = 0.0f;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (Input.touchCount == 2)
            {


                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudediff = prevTouchDeltaMag - touchDeltaMag;



                if (camera.orthographic)
                {
                    camera.orthographicSize += deltaMagnitudediff * orthoZoomSpeed;
                    camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 30f, 390);
                }
                else
                {
                    camera.fieldOfView += deltaMagnitudediff * perspectiveZoomSpeed;
                    camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 30f, 150.0f);
                }

            }
            else if (Input.touchCount == 1)
            {

                timer = 0.0f;
                dragging = true;

                camera.transform.position += new Vector3((touchZero.position.x - touchZero.deltaPosition.x), 0, touchZero.position.y - touchZero.deltaPosition.y);

            }

        }
        else
        {
            timer += Time.deltaTime;

            if (timer > 1.5f)
            {
                dragging = false;
                timer = 0.0f;
                oldPosition = camera.transform;
            }
        }
    }

    public void ChangeIsMovingBack()
    {
        isMovingBack = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    
    }

    private float stepForAplha = 0.02f;
    private float timeUpdateAlpha = 0.02f;

    IEnumerator HidePanel()
    {

        if (dropPanel.activeInHierarchy)
        {
            float alpha = 1.0f;

            while (dropPanel.GetComponent<CanvasGroup>().alpha > 0)
            {

                alpha -= stepForAplha;
                dropPanel.GetComponent<CanvasGroup>().alpha = alpha;
                yield return timeUpdateAlpha;
            }
            dropPanel.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        isMovingBack = false;
        timer = 0.0f;
        float x = -eventData.delta.x * 1.5f * spedswipe;
        float z = -eventData.delta.y * spedswipe;
        float rotation = Mathf.Abs(camera.transform.eulerAngles.y % 360);
        StartCoroutine(HidePanel());



        if (rotation > 135f && rotation < 225)
        {
            if ((camera.transform.position.x <= -700.0f && x < 0.0f) || (camera.transform.position.x >= 900.0f && x > 0.0f))
            {
                x = 0.0f;
            }
            if ((camera.transform.position.z <= -500.0f && z < 0.0f) || (camera.transform.position.z >= 500.0f && z > 0.0f))
            {
                z = 0.0f;
            }
            camera.transform.position += new Vector3(-x, 0, -z);


        }
        else if (rotation > 45f && rotation < 135f){

            if ((camera.transform.position.x <= -700.0f && x < 0.0f) || (camera.transform.position.x >= 900.0f && x > 0.0f))
            {
                x = 0.0f;
            }
            if ((camera.transform.position.z <= -500.0f && z < 0.0f) || (camera.transform.position.z >= 500.0f && z > 0.0f))
            {
                z = 0.0f;
            }
            camera.transform.position += new Vector3(z, 0, -x);
        }
        else if (rotation > 225 && rotation < 315)
        {

            if ((camera.transform.position.x <= -700.0f && x < 0.0f) || (camera.transform.position.x >= 900.0f && x > 0.0f))
            {
                x = 0.0f;
            }
            if ((camera.transform.position.z <= -500.0f && z < 0.0f) || (camera.transform.position.z >= 500.0f && z > 0.0f))
            {
                z = 0.0f;
            }
            camera.transform.position += new Vector3(-z, 0, x);
        }
        else if (rotation > 315 || rotation < 45)
        {

            if ((camera.transform.position.x <= -700.0f && x < 0.0f) || (camera.transform.position.x >= 900.0f && x > 0.0f))
            {
                x = 0.0f;
            }
            if ((camera.transform.position.z <= -500.0f && z < 0.0f) || (camera.transform.position.z >= 500.0f && z > 0.0f))
            {
                z = 0.0f;
            }
            camera.transform.position += new Vector3(x, 0, z);

        }
    }

    
}
