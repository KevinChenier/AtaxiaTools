using System;
using System.Collections.Generic;
using UnityEngine;

public class LayoutHelper
{
    public static List<Vector3> Menu(int nbRow, int nbButton, Rect buttonRect, int padding)
    {
        var layout = new List<Vector3>();
        var nbElementPerRow = (int)Math.Ceiling(nbButton / (double)nbRow);
        var ystart = Start(nbRow, buttonRect.height, padding, true);

        for(var i = 0; i < nbRow; i++)
        {
            var y = ystart - (buttonRect.height + padding) * i;
            var elementInRow = Math.Min(nbButton - i * nbElementPerRow, nbElementPerRow);
            var xstart = Start(elementInRow, buttonRect.width, padding);
            for(var j = 0; j < elementInRow; j++) 
            {
                var x = xstart + (buttonRect.width + padding) * j;
                layout.Add(new Vector3 { x = x, y = y, z = 0 });
            }
        }

        return layout;
    }

    private static float Start(int nbButton, float width, int padding, bool isY = false)
    {
        if (isY)
        {
            return ((nbButton * width + (nbButton - 1) * padding) * .5f) - (width / 2);
        }

        return ((nbButton * width + (nbButton - 1) * padding) * -.5f) + (width / 2);
    }
}