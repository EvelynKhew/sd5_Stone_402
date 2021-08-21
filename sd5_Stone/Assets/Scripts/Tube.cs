using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{

    //NaCl concentration
    public float nacl = 0f;
    //Std protein concentration
    public float sp = 0f;
    //Final protein concentration
    public float fp = 0f;
    //Dye always 4
    public static float dye = 4f;

    //Color: See virtualbiochemlab/blob/master/Frontend/virtualbiochemlab/src/Entities/Color.ts
    public Color color;
    public float r = 0f;
    public float g = 0f;
    public float b = 0f;
    public float opacity = 0f;

    public float transmission = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setContents(float nacl, float sp, float fp)
    {
        this.nacl = nacl;
        this.sp = sp;
        this.fp = fp;
    }

    public void reset()
    {
        this.nacl = 0f;
        this.sp = 0f;
        this.fp = 0f;
        this.color = new Color(0, 0, 0, 0);
    }

    /*
     * ALEJANDRO
     * Fills the tube to the given height and blends this.color with the given color param
     * fill is called in doImmobileTask(), so its called every time the use clicks on a test tube while holding the pipette
     * May need to change the parameters
     */
    public void fill(Color color, float height)
    {

    }

    public void setColor(Color color)
    {
        //For how to blend two colors, see /Frontend/virtualbiochemlab/src/Pages/Modules/Module1/Module1.tsx
        /*
         * export function blend(x: number)
    {
      let blue = 65;
      let blue2 = 105;
      let blue3 = 225;
      let brown = 139;
      let brown2 = 69;
      let brown3 = 19;
      return new Color({color: `rgba(
        ${blue*(x/.01)+brown*((.01-x)/.01)}, 
        ${blue2*(x/.01)+brown2*((.01-x)/.01)}, 
        ${blue3*(x/.01)+brown3*((.01-x)/.01)}, 
        1)`
      });
    }

         * 
         */


        //this.color = color;
        /*
        float blue = 65;
        float blue2 = 105;
        float blue3 = 225;
        float brown = 139;
        float brown2 = 69;
        float brown3 = 19;

        this.r = (float)(blue * (x / 0.01f) + brown * ((0.01 - x) / 0.01f));
        this.g = (float)(blue2 * (x / 0.01f) + brown2 * ((0.01 - x) / 0.01f));
        this.r = (float)(blue3 * (x / 0.01f) + brown3 * ((0.01 - x) / 0.01f));
        this.opacity = (float)1;
        //Somehow make color from r,g,b?
        color = new Color(r, g, b, opacity);
        */

        //TODO Alejandro: Change color of F_Liquid_02
    }

    public void setTransmission(float transmission)
    {
        this.transmission = transmission;
    }

    public float getTransmission()
    {
        return this.transmission;
    }
}
