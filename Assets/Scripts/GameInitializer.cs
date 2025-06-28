using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Awake()
{
    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    {
        Screen.orientation = ScreenOrientation.AutoRotation;

        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }
}
}
