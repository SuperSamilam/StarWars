using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform rotation;

    [SerializeField] Slider slider;


    public void RotateRotation(int rotAmount)
    {
        StartCoroutine(RotateObject(transform, new Vector3(0, rotAmount, 0), 90));
    }

    IEnumerator RotateObject(Transform transform, Vector3 targetRotation, float speed)
    {
        float totalAngle = Quaternion.Angle(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + targetRotation));

        while (totalAngle > 0.01f)
        {
            float rotationAmount = Mathf.Min(speed * Time.deltaTime, totalAngle);

            transform.Rotate(targetRotation.normalized * rotationAmount);

            totalAngle -= rotationAmount;

            yield return null;
        }
    }

    public void ChangeAudio(float amount)
    {
        PlayerPrefs.SetFloat("Audio", Mathf.Clamp01(PlayerPrefs.GetFloat("Audio") + amount));
        slider.value = PlayerPrefs.GetFloat("Audio");
    }

    public void ChangeScene(int index)
    {
        SceneLoader.LoadScene(index);
    }
}
