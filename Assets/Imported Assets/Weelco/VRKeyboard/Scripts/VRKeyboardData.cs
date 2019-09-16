using UnityEngine;

namespace Weelco.VRKeyboard {

    public abstract class VRKeyboardData : MonoBehaviour {

        public const string ABC = "abc";
        public const string SYM = "sym";
        public const string NUM = "123";
        public const string BACK = "BACK";
        public const string ENTER = "ENTER";
        public const string UP = "UP";
        public const string LOW = "LOW";

        public static readonly string[] allLettersLowercase = new string[]
        {
            "1","2","3","4","5","6","7","8","9","0",
            "q","w","e","r","t","y","u","i","o","p",
            SYM,"a","s","d","f","g","h","j","k","l",
            UP,"z","x","c","v","b","n","m",BACK,
            ".com","@"," ",".",ENTER
        };

        public static readonly string[] allLettersUppercase = new string[]
        {
            "1","2","3","4","5","6","7","8","9","0",
            "Q","W","E","R","T","Y","U","I","O","P",
            SYM,"A","S","D","F","G","H","J","K","L",
            LOW,"Z","X","C","V","B","N","M",BACK,
            ".com","@"," ",".",ENTER
        };

        public static readonly string[] allSpecials = new string[]
        {
            "1","2","3","4","5","6","7","8","9","0",
            "!","~","#","$","%","^","&","*","(",")",
            ABC,"-","_","+","=","\\",";",":","'","\"",
            "№","{","}","<",">",",","/","?",BACK,
            ".com","@"," ",".",ENTER
        };

        public static readonly string[] extraLettersLowercase = new string[]
        {
            "q","w","e","r","t","y","u","i","o","p",
            "a","s","d","f","g","h","j","k","l","/",
            UP,"z","x","c","v","b","n","m",BACK,
            NUM,"-"," ",".",ENTER
        };

        public static readonly string[] extraLettersUppercase = new string[]
        {
            "Q","W","E","R","T","Y","U","I","O","P",
            "A","S","D","F","G","H","J","K","L","/",
            LOW,"Z","X","C","V","B","N","M",BACK,
            NUM,"-"," ",".",ENTER
        };

        public static readonly string[] extraSpecials = new string[]
        {
            "+","1","2","3",".",
            "-","4","5","6",",",
            " ","7","8","9",BACK,
            ABC,"*","0","#",ENTER
        };

        public static readonly string[] liteSpecials = new string[]
        {
            "1","2","3",
            "4","5","6",
            "7","8","9",
            BACK,"0",ENTER
        };
    }
}