using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void ClickSound()
    {
        SoundManager.Instance.PlayClickSound();
    }
}
