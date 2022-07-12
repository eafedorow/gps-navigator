using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsResponse
{
    public int id { get; set; }
    public string name { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public DateTime createOn { get; set; }
    public object modifiedOn { get; set; }
    public object deletedOn { get; set; }
}
