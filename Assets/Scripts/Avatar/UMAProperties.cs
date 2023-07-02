using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UMAProperties
{
    public string skinColor;
    public string hairColor;
    public string lipstickColor;
    public string eyeColor;
    public double height;

    public static UMAProperties CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<UMAProperties>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    //Fill and return default values for the given UMAProperties obj
    public static UMAProperties GetDefaultUMAPropertySet(UMAProperties umaProps)
    {
        umaProps.skinColor = "#BEA79DFF";
        umaProps.hairColor = "#000000FF";
        umaProps.eyeColor = "#0000FFFF";
        umaProps.lipstickColor = "#FF0000FF";
        umaProps.height = 0.5f;


        return umaProps;
    }
}
