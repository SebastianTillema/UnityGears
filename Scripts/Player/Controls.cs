using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public CharacterController controller;
    public Camera eyeCamera;
    public Joystick moveStick;
    public Joystick lookStick;


    float xRotation = 0f; // vertical look 

    void OnGUI()
    {
        move();
        look();
    }

    void look()
    {
        int speed = 50;
        float h = lookStick.Horizontal * speed * Time.deltaTime;
        float v = lookStick.Vertical * speed * Time.deltaTime;

        xRotation -= v;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        eyeCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * h);
    }

    void move()
    {
        int speed = 12;
        float x = moveStick.Horizontal;
        float z = moveStick.Vertical;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }
}
