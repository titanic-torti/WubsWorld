using UnityEngine;

public class GameplayTestEnv : MonoBehaviour
{
    [Header("GameObject Reference")]
    [SerializeField] GameObject Floor1;
    [SerializeField] GameObject Floor2;
    [SerializeField] GameObject LeftWall;
    [SerializeField] GameObject RightWall;
    [SerializeField] GameObject Pit;
    [SerializeField] GameObject LeftPitWall;
    [SerializeField] GameObject RightPitWall;
    [SerializeField] GameObject AnchorPoint1;
    [SerializeField] GameObject AnchorPoint2;
    [SerializeField] GameObject AnchorPoint3;

    [Header("Parameters")]
    [SerializeField] float floorWidths;
    [SerializeField] float floor2HeightOffset;
    [SerializeField] float ceilingHeight;
    [SerializeField] float pitWidth;
    [SerializeField] float pitDepth;

    [Header("Anchor Points")]
    [SerializeField] bool Anchor1Active;
    [SerializeField] float anchorPoint1Height;
    [SerializeField] float anchorPoint1XOffset;

    [SerializeField] bool Anchor2Active;
    [SerializeField] float anchorPoint2Height;
    [SerializeField] float anchorPoint2XOffset;

    [SerializeField] bool Anchor3Active;
    [SerializeField] float anchorPoint3Height;
    [SerializeField] float anchorPoint3XOffset;

    void Start()
    {
        AdjustScene();
    }

    void OnValidate()
    {
        if (ObjectsReferenced())
        {
            AdjustScene();
        }
    }

    void Update()
    {
        AnchorPoint1.SetActive(Anchor1Active);
        AnchorPoint2.SetActive(Anchor2Active);
        AnchorPoint3.SetActive(Anchor3Active);
    }

    bool ObjectsReferenced()
    {
        return Floor1 && Floor2 &&
            Pit && LeftPitWall && RightPitWall &&
            AnchorPoint1 && AnchorPoint2 && AnchorPoint3 &&
            LeftWall && RightWall;
    }

    void AdjustScene()
    {
        // Left Wall
        LeftWall.transform.localScale = new Vector2(1, ceilingHeight);
        LeftWall.transform.position = new Vector2(-LeftWall.transform.localScale.x/2, ceilingHeight/2 - Floor1.transform.localScale.y/2);

        // Floor 1
        Floor1.transform.localScale = new Vector2(floorWidths, 1);
        Floor1.transform.position = new Vector2(Floor1.transform.localScale.x/2, 0);

        // Floor 2
        Floor2.transform.localScale = Floor1.transform.localScale;
        Floor2.transform.position = new Vector2(
            Floor1.transform.position.x + Floor1.transform.localScale.x/2 + pitWidth + Floor2.transform.localScale.x/2,
            Floor1.transform.position.y + floor2HeightOffset
        );

        // Right Wall
        RightWall.transform.localScale = new Vector2(1, ceilingHeight);
        RightWall.transform.position = new Vector2(
            Floor2.transform.position.x + Floor2.transform.localScale.x/2 + RightWall.transform.localScale.x/2,
            ceilingHeight/2 - Floor2.transform.localScale.y/2 + floor2HeightOffset
        );

        // Pit
        Pit.transform.localScale = new Vector2(pitWidth, 1);
        Pit.transform.position = new Vector2(
            Floor1.transform.position.x + Floor1.transform.localScale.x/2 + pitWidth/2,
            Floor1.transform.position.y - pitDepth
        );

        // Left Pit Wall
        LeftPitWall.transform.localScale = new Vector2(1, pitDepth - Floor1.transform.localScale.y);
        LeftPitWall.transform.position = new Vector2(
            Floor1.transform.localScale.x - LeftPitWall.transform.localScale.x/2,
            -pitDepth/2
        );

        // Right Pit Wall
        RightPitWall.transform.localScale = new Vector2(1, pitDepth - Floor2.transform.localScale.y + floor2HeightOffset);
        RightPitWall.transform.position = new Vector2(
            Pit.transform.position.x + pitWidth/2 + RightPitWall.transform.localScale.x/2,
            floor2HeightOffset/2 - pitDepth/2
        );

        // AnchorPoints
        AnchorPoint1.transform.position = new Vector2(
            Floor1.transform.position.x + Floor1.transform.localScale.x/2 + pitWidth/2 + anchorPoint1XOffset,
            Floor1.transform.position.y + anchorPoint1Height
        );
        AnchorPoint2.transform.position = new Vector2(
            Floor1.transform.position.x + Floor1.transform.localScale.x/2 + pitWidth/2 + anchorPoint2XOffset,
            Floor1.transform.position.y + anchorPoint2Height
        );
        AnchorPoint3.transform.position = new Vector2(
            Floor1.transform.position.x + Floor1.transform.localScale.x/2 + pitWidth/2 + anchorPoint3XOffset,
            Floor1.transform.position.y + anchorPoint3Height
        );
    }
}
