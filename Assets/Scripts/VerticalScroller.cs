using UnityEngine;

public class VerticalScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private Transform[] backgroundElements;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform lavaTransform;

    private float backgroundHeight;

    private float distanceTravelled;

    private int currentBG;
    private int thirdBG;

    private void Awake()
    {
        backgroundHeight = backgroundElements[0].GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void Update()
    {
        cameraTransform.position += Vector3.up * scrollSpeed * Time.deltaTime;
        lavaTransform.position += Vector3.up * scrollSpeed * Time.deltaTime;

        distanceTravelled += scrollSpeed * Time.deltaTime;

        ScrollBG();
    }

    private void ScrollBG()
    {
        if (distanceTravelled >= backgroundHeight)
        {
            SwitchPositions();
            distanceTravelled = 0f;
        }
    }

    private void SwitchPositions()
    {
        thirdBG = currentBG + 2;
        if (thirdBG >= backgroundElements.Length)
            thirdBG -= backgroundElements.Length;

        backgroundElements[currentBG].position = backgroundElements[thirdBG].position + new Vector3(0, 10.4f, 0);

        currentBG = currentBG + 1;
        if (currentBG >= backgroundElements.Length)
            currentBG = 0;
    }
}