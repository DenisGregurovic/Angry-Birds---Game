using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SlingShotHandler : MonoBehaviour
{

	[Header("Line Renderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;

    [Header("Transform References")]
    [SerializeField] private Transform leftStartPosition;
    [SerializeField] private Transform rightStartPosition;
    [SerializeField] private Transform centerPosition;
    [SerializeField] private Transform idlePostion;
    [SerializeField] private Transform elasticTransform;

    [Header("Slingshot Stats")]
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private float shotForce = 20f;
    [SerializeField] private float timeBetweenBirdRespawns = 2f;
    [SerializeField] private float elasticDivider = 1.2f;
    [SerializeField] private AnimationCurve elasticCurve;
    [SerializeField] private float maxAnimationTime=2f;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea slingShotArea;

    [Header("Bird")]
    [SerializeField] private AngieBird angieBirdPrefab;
    [SerializeField] private float angieBirdPositionOffset = 0.275f;

    [Header("Sounds")]
    [SerializeField] private AudioClip elasticPulledClip;
    [SerializeField] private AudioClip elasticReleaseClip;

    private Vector2 slingShotLinesPosition;

    private Vector2 direction;
    private Vector2 directionNormalized;

    private bool clickedWithinArea;
    private bool birdOnSlingShot;

    private AngieBird spawnedAngieBird;

    private AudioSource audioSource;

	private void Awake()
	{
        audioSource = GetComponent<AudioSource>();

        leftLineRenderer.enabled=false;
        rightLineRenderer.enabled = false;

        SpawnAngieBird();
	}

	void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && slingShotArea.IsWithinSlingSHotArea())
        {
            clickedWithinArea = true;

            if (birdOnSlingShot)
            {
                SoundManager.instance.PlayClip(elasticPulledClip,audioSource);
            }
        }

        if (Mouse.current.leftButton.isPressed && clickedWithinArea && birdOnSlingShot)
        {
            DrawSlingShot();
            PositionAndRotateAngieBird();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame && birdOnSlingShot && clickedWithinArea)
        {
            if (GameManager.instance.HasEnoughShots())
            {
                clickedWithinArea = false;
                birdOnSlingShot = false;

                spawnedAngieBird.LaunchBird(direction, shotForce);

                SoundManager.instance.PlayClip(elasticReleaseClip, audioSource);

                GameManager.instance.UseShot();
               // SetLines(centerPosition.position);
                AnimateSlingshot();

                if (GameManager.instance.HasEnoughShots())
                {
                    StartCoroutine(SpawnAngieBirdAfterTime());
                }
            }
        }
    }

	#region SlingShot Methods

	private void DrawSlingShot()
    {
        Vector3 touchPosition =Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        slingShotLinesPosition =centerPosition.position+Vector3.ClampMagnitude(touchPosition-centerPosition.position,maxDistance);
        SetLines(slingShotLinesPosition);

        direction =(Vector2)centerPosition.position-slingShotLinesPosition;
        directionNormalized = direction.normalized;
    }

    private void SetLines(Vector2 position)
    {
        if (!leftLineRenderer.enabled && !rightLineRenderer.enabled)
        {
            leftLineRenderer.enabled = true;
            rightLineRenderer.enabled = true;
        }

        leftLineRenderer.SetPosition(0,position);
        leftLineRenderer.SetPosition(1,leftStartPosition.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1,rightStartPosition.position);
    }

    #endregion

    #region Angie Bird Methods

    private void SpawnAngieBird()
    {
        elasticTransform.DOComplete();
        SetLines(idlePostion.position);

        Vector2 dir = (centerPosition.position-idlePostion.position).normalized;
        Vector2 spawnPostion = (Vector2)idlePostion.position + dir * angieBirdPositionOffset;

        spawnedAngieBird=Instantiate(angieBirdPrefab,spawnPostion, Quaternion.identity);
        spawnedAngieBird.transform.right = dir;

        birdOnSlingShot = true;
    }

    private void PositionAndRotateAngieBird()
	{
        spawnedAngieBird.transform.position = slingShotLinesPosition+directionNormalized*angieBirdPositionOffset;
        spawnedAngieBird.transform.right = directionNormalized;
    }

    private IEnumerator SpawnAngieBirdAfterTime()
    {
        yield return new WaitForSeconds(timeBetweenBirdRespawns);

        SpawnAngieBird();
    }

    #endregion

    #region Animate Slingshot

    private void AnimateSlingshot()
    {
        elasticTransform.position = leftLineRenderer.GetPosition(0);

        float dist = Vector2.Distance(elasticTransform.position, centerPosition.position)*3;

        float time = dist / elasticDivider;

        elasticTransform.DOMove(centerPosition.position,time).SetEase(elasticCurve);
        StartCoroutine(AnimateSlingShotLines(elasticTransform, time));
    }

    private IEnumerator AnimateSlingShotLines(Transform trans, float time)
    {
        float elapsedTime = 0f;
        while(elapsedTime<time && elapsedTime<maxAnimationTime)
		{
            elapsedTime += Time.deltaTime;
            SetLines(trans.position);
            yield return null;
		}
    }

	#endregion
}
