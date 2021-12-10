using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : MonoBehaviour
{
    public Material mat;
    public Vector2 pos;
    public float scale, angle;

    private Vector2 smoothPos;
    private float smoothScale, smoothAngle;


    private void UpdateShader()
    {
        smoothPos = Vector2.Lerp(smoothPos, pos, .03f); // cada vez acerca al vector smoothpos al vector pos en un factor .03 , y cuando se mueva el vector posicion para que no hayan paradas abruptas
        smoothScale = Mathf.Lerp(smoothScale, scale, .03f); // para que no hayan paradas abruptas el vector scale se re reemplaza con smooth scale 
        smoothAngle = Mathf.Lerp(smoothAngle, angle, .03f);

        float aspect = (float)Screen.width / (float)Screen.height; // arregla el aspect ratio cuando se da play a Game (mi pc es 16:9 otros 4:3) asi el fractal no se deforma

        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if (aspect > 1f)
            scaleY /= aspect;
        else
            scaleX *= aspect;

        mat.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        mat.SetFloat("_Angle", smoothAngle);
    }

    private void HandleInputs()
    {
        if (Input.GetKey(KeyCode.M)) // M para que haga zoom
        scale *= .99f; // se reduce la escala a nivel de porcentaje, porque si es un valor fijo se vería como si cada vez el zoom fuera más rapido.
        if (Input.GetKey(KeyCode.Z)) // z para que haga zoom out
            scale *= 1.01f; // se maneja la escala a nivel de porcentaje, porque si es un valor fijo se vería como si cada vez el zoom fuera más rapido.

        Vector2 dir = new Vector2(.01f * scale, 0);
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);
        dir = new Vector2(dir.x*c, dir.x*s); // para que cuando se gire la derecha siga siendo la derecha
        if (Input.GetKey(KeyCode.A))
            pos -= dir;
        if (Input.GetKey(KeyCode.D))
            pos += dir;

        dir = new Vector2(-dir.y, dir.x);// ajusta para y 
        if (Input.GetKey(KeyCode.S))
            pos -= dir;
        if (Input.GetKey(KeyCode.W)) // para moverse
            pos += dir;

        if (Input.GetKey(KeyCode.E)) // para que rote
            angle -= .01f; 
        if (Input.GetKey(KeyCode.Q)) 
            angle += .01f;


    }

    // cambio a Fixed para que se ejectute ciertas veces por segundo, no de acuerdo a la velocidad del pc 
    void FixedUpdate() 
    {
        HandleInputs();
        UpdateShader();
    }
}
