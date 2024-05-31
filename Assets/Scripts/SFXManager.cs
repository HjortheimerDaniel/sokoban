
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;
    public AudioClip walk;
    public AudioClip touchedWall;
    public AudioClip goal;

   public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
