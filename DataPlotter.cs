using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataPlotter : MonoBehaviour
{

    // Name of the input file, no extension
    public string inputfile;

    // Indices for columns to be assigned
    public int columnX = 0;
    public int columnY = 1;
    public int columnZ = 2;

    // Full column names
    public string xName;
    public string yName;
    public string zName;

    // plot scale
    public float plotScale = 100;

    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    // The prefab for the data points to be instantiated
    public GameObject SpherePrefab, CubePrefab, PointPrefab, PointHolder;

    // Use this for initialization
    void Start()
    {

        // Set pointlist to results of function Reader with argument inputfile
        pointList = ReadFile.Read(inputfile);

        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(pointList[1].Keys);

        // Print number of keys (using .count)
        //Debug.Log("There are " + columnList.Count + " columns in CSV");

        //foreach (string key in columnList)
            //Debug.Log("Column name is " + key);

        // Assign column name from columnList to Name variables
        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];

        // Get maxes of each axis
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);

        // Get minimums of each axis
        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);

        //Loop through Pointlist
        for (var i = 0; i < pointList.Count; i++)
        {
            GameObject dataPoint;
            // Get value in poinList at ith "row", in "column" Name, normalize
            float x = (Convert.ToSingle(pointList[i][xName]) - xMin) / (xMax - xMin);
            float y = (Convert.ToSingle(pointList[i][yName]) - yMin) / (yMax - yMin);
            float z = (Convert.ToSingle(pointList[i][zName]) - zMin) / (zMax - zMin);


            //instantiate the prefab with coordinates defined above
            //Debug.Log(pointList[i]["GeneType"].ToString());

            if (pointList[i]["GeneType"].ToString() == "Strong")
            {
                dataPoint = Instantiate(SpherePrefab, new Vector3(x, y, z) * plotScale, Quaternion.identity);
            }
            else
            {
                dataPoint = Instantiate(CubePrefab, new Vector3(x, y, z) * plotScale, Quaternion.identity);
            }

            //GameObject dataPoint = Instantiate(PointPrefab, new Vector3(x, y, z) * plotScale, Quaternion.identity);
            // Make dataPoint child of PointHolder object 
            dataPoint.transform.parent = PointHolder.transform;

            // Gets material color and sets it to a new RGBA color we define
            if (pointList[i]["Species"].ToString() == "setosa") 
            {
                dataPoint.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1.0f);
            }
            else if (pointList[i]["Species"].ToString() == "versicolor")
            {
                dataPoint.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1.0f);
            }
            else if (pointList[i]["Species"].ToString() == "virginica")
            {
                dataPoint.GetComponent<Renderer>().material.color = new Color(0, 0, 1, 1.0f);
            }


            // Assigns original values to dataPointName
            string dataPointName =
            pointList[i][xName] + " "
            + pointList[i][yName] + " "
            + pointList[i][zName];

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;

        }

    }

    // find the max value in a column
    private float FindMaxValue(string columnName)
    {
        //set initial value to first value
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing maxValue if new value is larger
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }

        //Spit out the max value
        return maxValue;
    }

    // find the min value in a column
    private float FindMinValue(string columnName)
    {

        float minValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing minValue if new value is smaller
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }

}
