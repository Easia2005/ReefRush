
using UnityEngine;

public class SeabedLoop : MonoBehaviour
{
    public Transform player;
    public Transform seabed1;
    public Transform seabed2;
    public Transform seabed3;
    public Transform seabed4;

    public float seabedLength = 30f;

    private float[] startingZ = new float[4];

    void Start()
    {
        SaveStartingPositions();
    }

    void Update()
    {
        if (player == null || seabed1 == null ||
            seabed2 == null || seabed3 == null || seabed4 == null)
            return;

        Transform[] pieces = {
            seabed1, seabed2, seabed3, seabed4
        };

        Transform firstPiece = pieces[0];

        for (int i = 1; i < pieces.Length; i++)
        {
            if (pieces[i].position.z < firstPiece.position.z)
                firstPiece = pieces[i];
        }

        if (player.position.z > firstPiece.position.z + seabedLength)
        {
            float furthestZ = pieces[0].position.z;

            for (int i = 1; i < pieces.Length; i++)
            {
                if (pieces[i].position.z > furthestZ)
                    furthestZ = pieces[i].position.z;
            }

            firstPiece.position = new Vector3(
                firstPiece.position.x,
                firstPiece.position.y,
                furthestZ + seabedLength
            );
        }
    }

    void SaveStartingPositions()
    {
        startingZ[0] = seabed1.position.z;
        startingZ[1] = seabed2.position.z;
        startingZ[2] = seabed3.position.z;
        startingZ[3] = seabed4.position.z;
    }

    public void ResetSeabed()
    {
        seabed1.position = new Vector3(
            seabed1.position.x, seabed1.position.y, startingZ[0]);

        seabed2.position = new Vector3(
            seabed2.position.x, seabed2.position.y, startingZ[1]);

        seabed3.position = new Vector3(
            seabed3.position.x, seabed3.position.y, startingZ[2]);

        seabed4.position = new Vector3(
            seabed4.position.x, seabed4.position.y, startingZ[3]);
    }
}
