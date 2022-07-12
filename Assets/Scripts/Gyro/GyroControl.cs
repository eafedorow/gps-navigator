using UnityEngine;

public class GyroControl : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;

    private GameObject cameraContainer;
    private Quaternion rot;

    public GameObject cam;

    public Transform destinationPoints;
    public Transform buildingNames;

    private float prevRotation;

    public static bool isPathBuilding;

    private void Start()
    {
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyroEnabled = EnableGyro();

        isPathBuilding = false;

    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            rot = new Quaternion(0, 0, 1, 0);
            return true;

        }
        return false;
    }

    private void Rotate(float rotation)
    {

        Vector3 toDestPoint;

        if (rotation > 45f && rotation < 135)
        {

            toDestPoint = new Vector3(0, 270, 0);

            for (int i = 0; i < destinationPoints.transform.childCount; i++)
            {
                destinationPoints.GetChild(i).transform.eulerAngles = toDestPoint;
                buildingNames.GetChild(i).transform.eulerAngles = toDestPoint;
            }

        }
        else if (rotation > 135f && rotation < 225)
        {
            toDestPoint = new Vector3(0, 0, 0);
            for (int i = 0; i < destinationPoints.transform.childCount; i++)
            {
                destinationPoints.GetChild(i).transform.eulerAngles = toDestPoint;
                buildingNames.GetChild(i).transform.eulerAngles = toDestPoint;
            }

        }
        else if (rotation > 225 && rotation < 315)
        {
            toDestPoint = new Vector3(0, 90, 0);
            for (int i = 0; i < destinationPoints.transform.childCount; i++)
            {
                destinationPoints.GetChild(i).transform.eulerAngles = toDestPoint;
                buildingNames.GetChild(i).transform.eulerAngles = toDestPoint;
            }
        }
        else if (rotation > 315 || rotation < 45)
        {
            toDestPoint = new Vector3(0, 180, 0);
            for (int i = 0; i < destinationPoints.transform.childCount; i++)
            {
                destinationPoints.GetChild(i).transform.eulerAngles = toDestPoint;
                buildingNames.GetChild(i).transform.eulerAngles = toDestPoint;
            }
        }
    }

    private void Update()
    {

        if (gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rot;
            transform.eulerAngles = new Vector3(90, transform.eulerAngles.y + 90, transform.eulerAngles.z);

            if (isPathBuilding)
            {
                float rotation = Mathf.Abs(this.transform.eulerAngles.y % 360);

                Vector3 to;


                if (rotation > 45f && rotation < 135)
                {
                    to = new Vector3(90f, 270, 0);

                    cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.eulerAngles, to, Time.deltaTime);

                    if (Mathf.Abs(prevRotation - rotation) > 90)
                    {
                        Rotate(rotation);
                        prevRotation = Mathf.Abs(this.transform.eulerAngles.y % 360);
                    }
                }
                else if (rotation > 135f && rotation < 225)
                {
                    if (prevRotation < 135f)
                    {
                        to = new Vector3(90f, 360, 0);
                        prevRotation = Mathf.Abs(this.transform.eulerAngles.y % 360);
                    }
                    else
                    {
                        to = new Vector3(90f, 0, 0);
                    }

                    cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.eulerAngles, to, Time.deltaTime);

                    if (Mathf.Abs(prevRotation - rotation) > 90)
                    {
                        Rotate(rotation);
                        prevRotation = Mathf.Abs(this.transform.eulerAngles.y % 360);
                    }

                }
                else if (rotation > 225 && rotation < 315)
                {
                    to = new Vector3(90f, 90, 0);

                    cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.eulerAngles, to, Time.deltaTime);

                    if (Mathf.Abs(prevRotation - rotation) > 90)
                    {
                        Rotate(rotation);
                        prevRotation = Mathf.Abs(this.transform.eulerAngles.y % 360);
                    }
                }
                else if (rotation > 315 || rotation < 45)
                {
                    to = new Vector3(90f, 180, 0);

                    cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.eulerAngles, to, Time.deltaTime);

                    if (Mathf.Abs(prevRotation - rotation) > 90)
                    {
                        Rotate(rotation);
                        prevRotation = Mathf.Abs(this.transform.eulerAngles.y % 360);
                    }
                }
            }
            else {
                Vector3 to = new Vector3(90f, 0, 0);
                cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.eulerAngles, to, Time.deltaTime);
                Rotate(180);
            }
        }


    }
}
