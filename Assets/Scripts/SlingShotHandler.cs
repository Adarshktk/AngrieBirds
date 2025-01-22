using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;


public class SlingShotHandler : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;

    [Header("Transform referances")]
    [SerializeField] private Transform leftStartPosition;
    [SerializeField] private Transform rightStartPosition;
    [SerializeField] private Transform centerPosition;
    [SerializeField] private Transform idlePosition;

    [SerializeField] private Transform elasticTransform;

    [Header("Slingshot Stats")]
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private float shotForce = 6f;
    [SerializeField] private float timeBetweenBirdRespawns = 2f;
    [SerializeField] private float elasticDivider = 1.2f;
    [SerializeField] private AnimationCurve elasticCurve;
    private float maxAnimationTime = 1f ;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea slingshotArea;
    [SerializeField] private CameraManager cameraManager;

    private Vector2 slingShotLinesPosition;

    private Vector2 _direction;
    private Vector2 directionNormalized;

    [Header("Birds")]
    [SerializeField] private AngieBird angieBirdPrefab;
    [SerializeField] private float angiebirdPositionOffset = 2f;

    [Header("Sounds")]
    [SerializeField] private AudioClip leatherStretch;
    [SerializeField] private AudioClip onAngieRelease;

    private AudioSource audioSource;
    private bool clickedWithinArea;
    private bool birdOnSlingShot;
    private bool wasSlingShotDrawnWithinArea;

    private AngieBird spawnedAngieBird;



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;
        SpawnAngieBird();
    }

    private void Update()
    {
        if (InputManager.wasLeftMouseButtonPressed && slingshotArea.IsWithinSlingshotArea())
        {
            clickedWithinArea = true;
            if (birdOnSlingShot)
            {
                SoundManager.Instance.PlayClip(leatherStretch, audioSource);
                cameraManager.SwitchToFollowCam(spawnedAngieBird.transform);
            }
        }

        if (InputManager.isLeftButtonPressed && clickedWithinArea && birdOnSlingShot)
        {
           
            
            DrawSlingShot();
            PositionAndRotateAngieBird();
            wasSlingShotDrawnWithinArea = true;
        }
        if (InputManager.wasLeftMouseButtonReleased && birdOnSlingShot && wasSlingShotDrawnWithinArea)
        {
            SoundManager.Instance.PlayClip(onAngieRelease, audioSource);

            if (GameManager.Instance.hasEnoughShots())
            {
                clickedWithinArea = false;

                spawnedAngieBird.LaunchBird(_direction, shotForce);

                GameManager.Instance.UseShot();

                birdOnSlingShot = false;

                AnimateSlingShot();

                if (GameManager.Instance.hasEnoughShots())
                {
                    StartCoroutine(SpawnAngieBirdAfterTime());
                }
            }
            wasSlingShotDrawnWithinArea = false;


        }

    }

    #region Slingshot methods
    private void DrawSlingShot()
    {
        
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.mousePosition);

        slingShotLinesPosition = centerPosition.position + Vector3.ClampMagnitude(touchPosition - centerPosition.position , maxDistance);

        SetLines(slingShotLinesPosition);

        _direction = (Vector2)centerPosition.position - slingShotLinesPosition;
        directionNormalized = _direction.normalized;
        

    }
    private void SetLines(Vector2 position)
    {

        if (!leftLineRenderer.enabled && !rightLineRenderer.enabled)
        {
            leftLineRenderer.enabled = true;
            rightLineRenderer.enabled = true;
        }

        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftStartPosition.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1, rightStartPosition.position);
        

    }

    #endregion


    #region AngieBird Methods

    private void SpawnAngieBird()
    {

        elasticTransform.DOComplete();
        SetLines(idlePosition.position);

        Vector2 dir = (centerPosition.position - idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)idlePosition.position + dir * angiebirdPositionOffset;

        spawnedAngieBird = Instantiate(angieBirdPrefab, spawnPosition, Quaternion.identity);
        spawnedAngieBird.transform.right = dir;

        birdOnSlingShot = true;
    }

    private void PositionAndRotateAngieBird()
    {
        
        spawnedAngieBird.transform.position = slingShotLinesPosition + directionNormalized * angiebirdPositionOffset;
        spawnedAngieBird.transform.right = directionNormalized;
        
        
        
    }

    private IEnumerator SpawnAngieBirdAfterTime()
    {
        yield return new WaitForSeconds(timeBetweenBirdRespawns);

        SpawnAngieBird();
        cameraManager.SwitchToIdleCam();
    }


    #endregion


    #region Animate Slingshot


    private void AnimateSlingShot()
    {
        elasticTransform.position = leftLineRenderer.GetPosition(0);

        float dist = Vector2.Distance(elasticTransform.position, centerPosition.position);
        float time = dist / elasticDivider;

        elasticTransform.DOMove(centerPosition.position, time).SetEase(elasticCurve);
        StartCoroutine(AnimateSlingShotLines(elasticTransform, time));
    }

    private IEnumerator AnimateSlingShotLines(Transform trans, float time)
    {
        float elapsedTime = 0f;
        while(elapsedTime < time && elapsedTime < maxAnimationTime)
        {
            elapsedTime += Time.deltaTime;
            SetLines(trans.position);

            yield return null;
        }
    }


    #endregion

}
