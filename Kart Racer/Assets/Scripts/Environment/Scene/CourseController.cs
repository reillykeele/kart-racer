using System.Collections;
using System.Collections.Generic;
using Environment.Scene;
using Manager;
using UnityEngine;

public class CourseController : AOnSceneLoad
{
    protected override void OnSceneLoad()
    {
        GameManager.Instance.StartCountdown();
    }
}
