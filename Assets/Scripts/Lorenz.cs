using System;
using UnityEngine;
using TMPro;

public class Lorenz : MonoBehaviour
{
    // vv Private Exposed vv //

    [SerializeField]
    private LineRenderer line;

    [SerializeField]
    private TMP_InputField[] colourInputs;

    // vv Private vv //

    private float[] colourValues = new float[6];
    private Vector3 center = new Vector3(0.0f, 0.0f, 25.0f);
    private float nextStep = 0.0f;
    private float stepRate = 0.01f;
    private float a = 10;
    private float b = 28;
    private float c = 8.0f / 3.0f;
    private float x = 0.01f;
    private float y = 0;
    private float z = 0;

    ////////////////////////////////////////

    #region Private Functions

    private void Start()
    {
        Camera.main.transform.LookAt(center);
    }

    private void Update()
    {
        CameraMovement();
        LorenzCalculation();
    }

    private void CameraMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.RotateAround(center, Vector3.up, 40 * Time.deltaTime);
            Camera.main.transform.LookAt(center);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.RotateAround(center, -Vector3.up, 40 * Time.deltaTime);
            Camera.main.transform.LookAt(center);
        }
    }

    private void LorenzCalculation()
    {
        if (Time.time > nextStep)
        {
            nextStep = Time.time + stepRate;

            float dt = 0.01f;
            float dx = (a * (y - x)) * dt;
            float dy = (x * (b - z) - y) * dt;
            float dz = (x * y - c * z) * dt;
            x += dx;
            y += dy;
            z += dz;

            line.positionCount++;
            line.SetPosition(line.positionCount - 1, new Vector3(x, y, z));
        }
    }
    #endregion

    #region Public Functions

    public void ColoursChanged()
    {
        for (int i = 0; i < colourInputs.Length; i++)
        {
            if (colourInputs[i].text.Length <= 0.0f)
            {
                colourValues[i] = 0.0f;
                continue;
            }

            colourValues[i] = Int32.Parse(colourInputs[i].text) / 255.0f;

            if (colourValues[i] > 1.0f)
            {
                colourValues[i] = 1.0f;
                colourInputs[i].text = "255";
            }
                
            else if (colourValues[i] < 0.0f)
            {
                colourValues[i] = 0.0f;
                colourInputs[i].text = "0";
            }
        }

        line.startColor = new Color(colourValues[0], colourValues[1], colourValues[2]);
        line.endColor = new Color(colourValues[3], colourValues[4], colourValues[5]);
    }

    public void ChangeSimulationSpeed(bool direction)
    {
        if (direction && stepRate > 0.001f) stepRate -= 0.001f;
        if (!direction) stepRate += 0.001f;
    }

    public void Reset()
    {
        nextStep = 3f;
        x = 0.01f;
        y = 0.0f;
        z = 0.0f;
        line.positionCount = 0;
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    #endregion
}
