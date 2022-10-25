using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoiceController : MonoBehaviour
{
    private static int selfId = 0;
    private static int oppId = 0;

    public static void setSelfId(int id)
    {
        selfId = id;
    }

    public static void setOppId(int id)
    {
        oppId = id;
    }

    public static int getSelfId()
    {
        return selfId;
    }

    public static int getOppId()
    {
        return oppId;
    }
}
