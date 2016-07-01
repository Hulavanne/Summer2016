using UnityEngine;
using System.Collections;

public class TextList : MonoBehaviour {
    // declaring all different buttons

    // declaring all test assets

    public TextAsset Text1;
    public TextAsset Text2;
    public TextAsset Text3;
    public TextAsset Text4;
    public TextAsset Text5;
    public TextAsset Text6;
    public TextAsset Text7;
    public TextAsset Text8;
    public TextAsset Text9;
    public TextAsset Text10;
    public TextAsset SaveText;

    // for now, the arrays can only have power of 2 (I can change it to whatever though)
    // to activate either yes or no buttons, just place the line on which you want to activate them
    // to not activate them, set the value to -1

    public int[] buttonsYesNoIntro = { -1, -1 };
    public int[] buttonsOptIntro = { -1, -1 };

    public int[] buttonsYesNo1 = { -1, -1 };
    public int[] buttonsOpt1 = { -1, -1 };

    public int[] buttonsYesNo2 = { -1, -1 };
    public int[] buttonsOpt2 = { -1, -1 };

    public int[] buttonsYesNo3 = { -1, -1 };
    public int[] buttonsOpt3 = { -1, -1 };

    public int[] buttonsYesNoSave = { -1, -1 };
    public int[] buttonsOptSave = { -1, -1 };

    public int textIntroStartLine = 0;
    public int textIntroEndLine = 2;

    public int text1StartLine = 0;
    public int text1EndLine = 4;

    public int text2StartLine = 0;
    public int text2EndLine = 2;

    public int text3StartLine = 3;
    public int text3EndLine = 4;

    public int textSavStartLine = 0;
    public int textSavEndLine = 0;

}
