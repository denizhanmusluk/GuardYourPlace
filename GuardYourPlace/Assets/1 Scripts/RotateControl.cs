using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateControl : MonoBehaviour
{
    public bool pressed = false;

    private float m_previousY;
    private float dY;
    private float m_previousX;
    private float dX;
    [Range(1, 50)] [SerializeField] public float rotSensivity;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotControl();
    }
    void rotControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_previousX = Input.mousePosition.x;
            dX = 0f;
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressed = false;
            dX = 0f;
            dY = 0f;

        }

        if (pressed == true)
        {
            dX = (Input.mousePosition.x - m_previousX);
            dY = (Input.mousePosition.y - m_previousY);

            transform.Rotate(0, dX * Time.deltaTime, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + Mathf.Clamp(dX, -rotSensivity, rotSensivity), transform.eulerAngles.z), rotSensivity * Mathf.Abs(dX) * Time.deltaTime);
            m_previousX = Input.mousePosition.x;
            m_previousY = Input.mousePosition.y;
        }
    }
}
