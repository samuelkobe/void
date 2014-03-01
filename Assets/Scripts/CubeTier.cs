using UnityEngine;
using System.Collections;

public class CubeTier : MonoBehaviour
{
    public int faceDirection = 0;  /// 0,1,2,3 for 0(360),90,180,270
    static float rotateAccuracy = 5f;
    Quaternion rotate90Degree = Quaternion.Euler(0,90f + rotateAccuracy,0);  //new Quaternion(0f, 0.7071068f, 0f, 0.7071068f);  // a compared to the original rotation (0,0,0)
    Quaternion rotate180Degree = Quaternion.Euler(0,180 + rotateAccuracy,0);
    Quaternion rotate270Degree = Quaternion.Euler(0,270 + rotateAccuracy,0);
    Quaternion rotate360Degree = Quaternion.Euler(0, rotateAccuracy, 0);
    public float rotateSpeed = 2f;
    public bool canRotate = true; // is player is in this tier, this tier can't be rotated
    public CubeTier otherCube;  ///@ make it a array in the future;

    private void increaseDirection()
    {
        faceDirection = (faceDirection + 1) % 4 ;
    }

    private void decreaseDirection()
    {
        if (faceDirection - 1 == -1)
            faceDirection = 3;
        else
            faceDirection-- ;
    }

    public bool rotateTier( bool rotateDirection )  /// return true indicate that the rotation is finished
    {
        if (rotateDirection)
        {
            switch (faceDirection)   /// need a distance compare and set the direction when rotate finshed
            {
                case 0:
                    {
                        if (Quaternion.Angle(gameObject.transform.rotation, rotate90Degree) >= rotateAccuracy)/////////////
                        {
                            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotate90Degree, rotateSpeed * Time.deltaTime);
                            break;
                        }
                        else
                        {
                            increaseDirection();
                            return true;
                        }
                    }
                case 1:
                    {
                        if (Quaternion.Angle(gameObject.transform.rotation, rotate180Degree) >= rotateAccuracy)
                        {
                            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotate180Degree, rotateSpeed * Time.deltaTime);
                            return false;
                        }
                        else
                        {
                            increaseDirection();
                            return true;
                        }
                    }
                case 2:
                    {
                        if (Quaternion.Angle(gameObject.transform.rotation, rotate270Degree) >= rotateAccuracy)
                        {
                            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotate270Degree, rotateSpeed * Time.deltaTime);
                            break;
                        }
                        else
                        {
                            increaseDirection();
                            return true;
                        }
                    }
                case 3:
                    {
                        if (Quaternion.Angle(gameObject.transform.rotation, rotate360Degree) >= rotateAccuracy)
                        {
                            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotate360Degree, rotateSpeed * Time.deltaTime);
                            break;
                        }
                        else
                        {
                            increaseDirection();
                            return true;
                        }
                    }

            }
        }
        else
        {
            switch (faceDirection)   /// need a distance compare and set the direction when rotate finshed
            {
                case 0:
                    {
                        if (Quaternion.Angle(gameObject.transform.rotation, rotate270Degree) >= rotateAccuracy)/////////////
                        {
                            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotate270Degree, rotateSpeed * Time.deltaTime);
                            break;
                        }
                        else
                        {
                            increaseDirection();
                            return true;
                        }
                    }
                case 1:
                    {
                        if (Quaternion.Angle(gameObject.transform.rotation, rotate360Degree) >= rotateAccuracy)
                        {
                            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotate360Degree, rotateSpeed * Time.deltaTime);
                            return false;
                        }
                        else
                        {
                            increaseDirection();
                            return true;
                        }
                    }
                case 2:
                    {
                        if (Quaternion.Angle(gameObject.transform.rotation, rotate90Degree) >= rotateAccuracy)
                        {
                            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotate90Degree, rotateSpeed * Time.deltaTime);
                            break;
                        }
                        else
                        {
                            increaseDirection();
                            return true;
                        }
                    }
                case 3:
                    {
                        if (Quaternion.Angle(gameObject.transform.rotation, rotate180Degree) >= rotateAccuracy)
                        {
                            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotate180Degree, rotateSpeed * Time.deltaTime);
                            break;
                        }
                        else
                        {
                            increaseDirection();
                            return true;
                        }
                    }

            }
        }
        return false;
    }

    void OnTriggerEnter(Collider collision)
    {
        print("clllisionEnter!");
        canRotate = false;
        otherCube.canRotate = true;

    }
}
