using System;
using System.Collections;
using System.Collections.Generic;
using Snippets.Tutorial;
using UnityEngine;

[Serializable]
public class TestStep3 : TutorialStep
{
    protected override void OnBegin()
    {

    }

    protected override void OnComplete()
    {

    }

    public override void OnUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
            Complete();
    }
}
