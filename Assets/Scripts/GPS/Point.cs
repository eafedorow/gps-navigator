﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Point : MonoBehaviour
{
    void Start()
    {
        this.GetComponentInChildren<TextMeshPro>().text = this.name;
    }
}
