using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public static class RandomWord
    {
        //saved in strings and not chars if something like "th" or "au" is needed
        private static string[] vowels ={
            "a",
            "e",
            "i",
            "o",
            "u",
            "au",
            "eu",
            "ui",
            "ê",
        };

        private static string[] inWordConsonants = {
            //"b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "'", "bb", "dd", "ff", "gg", "kk", "ll", "mm", "nn", "pp", "rr", "ss", "tt"
            "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "'"
        };

        private static string[] starterConsonants = {
            "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "Th", "Sh", "Ch", "Sch", "Tr", "Gr", "Pr", "Gl", "Br",
        };

        private static char[] uppercaseAlphabet = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        private static char[] lowercaseAlphabet = {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        private static char[] possibleNumbers = {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        private static char[] randomChars = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '"', '§', '$', '%', '&', '/', '!', ')', '=', '?', '`', '*', '+', '~', '#', 
            '°', '^', '<', '>', '|', ',', ';', '.', ':', '-', '_'
        };

        private static char[] randomID = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        };

        //https://www.ssa.gov/oact/babynames/
        private static string[] namePrefix = {
            "Brun", "Samu", "Char", "Vanes" , "Li", "No", "Oli", "Du"
        };

        private static string[] namePostFix = {
            "hilde", "el", "sa", "hardt", "am", "ah", "via", "ens"
        };

        public static string Name(){
            string prefix = GenericUtility<string>.RandomFromArray(namePrefix);
            string postfix= GenericUtility<string>.RandomFromArray(namePostFix);
            return prefix + postfix;
        }

        public static string Coherent(int length){
            string cached = "";
            bool useVowel = false;
            for (int i = 0; i < length; i++)
            {
                string toAdd ="";
                if(i == 0){
                    toAdd = starterConsonants[Random.Range(0, starterConsonants.Length)];
                }
                else{
                    toAdd = useVowel? vowels[Random.Range(0, vowels.Length)] : inWordConsonants[Random.Range(0, inWordConsonants.Length)];
                }           
                cached += toAdd;
                useVowel = !useVowel;
            }
            return cached;
        }

        public static string Incoherent(int length){
            string cached = "";
            for (int i = 0; i < length; i++)
            {
                char toAdd;
                if(i == 0){
                    toAdd = uppercaseAlphabet[Random.Range(0, uppercaseAlphabet.Length)];
                }
                else{
                    toAdd = lowercaseAlphabet[Random.Range(0, lowercaseAlphabet.Length)];
                }         
                cached += toAdd;
            }
            return cached; 
        }

        public static string RandomString(int length){
            string cached = "";
            for (int i = 0; i < length; i++)
            {
                char toAdd;
                toAdd = randomChars[Random.Range(0, randomChars.Length)];
                cached += toAdd;
            }
            return cached;
        }

        public static string RandomID(int length){
            string cached = "";
            for (int i = 0; i < length; i++)
            {
                char toAdd;
                toAdd = randomID[Random.Range(0, randomID.Length)];
                cached += toAdd;
            }
            return cached;
        }
    }

}
