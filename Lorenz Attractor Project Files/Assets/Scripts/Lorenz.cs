using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lorenz : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] TMP_InputField[] colourInputs;

    float[] colourValues = new float[6];
    float nextStep = 0.0f;
    float stepRate = 0.01f;
    float a = 10;
    float b = 28;
    float c = 8.0f / 3.0f;
    float x = 0.01f;
    float y = 0;
    float z = 0;

    private void Update()
    {
        CameraMovement();
        LorenzCalculation();
    }

    private void CameraMovement()
    {
        Camera.main.transform.LookAt(new Vector3(0, 0, 25));

        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.RotateAround(new Vector3(0, 0, 25), Vector3.up, 40 * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.RotateAround(new Vector3(0, 0, 25), -Vector3.up, 40 * Time.deltaTime);
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

    public void ColoursChanged()
    {
        for (int i = 0; i < colourInputs.Length; i++)
        {
            if (colourInputs[i].text.Length <= 0)
            {
                colourValues[i] = 0f;
                continue;
            }

            colourValues[i] = Int32.Parse(colourInputs[i].text) / 255f;

            if (colourValues[i] > 1f)
            {
                colourValues[i] = 1f;
                colourInputs[i].text = "255";
            }
                
            else if (colourValues[i] < 0f)
            {
                colourValues[i] = 0f;
                colourInputs[i].text = "0";
            }
        }

        line.startColor = new Color(colourValues[0], colourValues[1], colourValues[2]);
        line.endColor = new Color(colourValues[3], colourValues[4], colourValues[5]);
    }

    public void ChangeSimulationSpeed(bool direction)
    {
        if (direction && stepRate > 0.001) stepRate -= 0.001f;
        if (!direction) stepRate += 0.001f;

        Debug.Log(stepRate);
    }

    public void Reset()
    {
        nextStep = 3f;
        x = 0.01f;
        y = 0;
        z = 0;
        line.positionCount = 0;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
