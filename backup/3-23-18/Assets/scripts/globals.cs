using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class globals { //This is where all the global variables are kept

    public static bool human1; //Whether player 1 is human or not
    public static bool human2; //Whether player 2 is human or not
    public static bool train; //Whether we are training or not
    public static int victor; //Who the victor is
    public static string victoryMessage; //Message top display upon victory
    public static float winBonus = 2; //What the previous and current modifier of the victory bonus is. Defaults to 2

}
