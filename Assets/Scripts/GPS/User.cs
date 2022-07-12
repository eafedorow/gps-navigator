using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserData
{
    public int id { get; set; }
    public string login { get; set; }
    public string name { get; set; }
    public int role { get; set; }
    public DateTime createOn { get; set; }
    public DateTime modifiedOn { get; set; }
    public object deletedOn { get; set; }
}

public class User
{
    public UserData user { get; set; }
    public string token { get; set; }
    public DateTime expiration { get; set; }
}
