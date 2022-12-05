using System;
using System.IO;
using System.Text.RegularExpressions;  

namespace codestylometry 
{
    class variables
    {
        //All of this code assumes we are in C

        //@TODO:Needs to be fixed or abandon
        static String findVariableParameters(String line) {
            String [] dataTypes = {"char", "int", "short", "long"};
            String variable="";
            line = line.Trim(); //gets rid of whitespace
            foreach (string type in dataTypes) {
                if (line.StartsWith(type) && line.Contains('{')) {
                    variable=line.Substring(type.Length, line.Length-1); 
                }
            }
            return variable;
        }
        //This helper function finds a variablename
        static String findVariableInCode (String line) {
            String variable = ""; 
            //int eqIndex=0;
            line = line.Trim(); //getting rid of any whitespace

            if (line.StartsWith("//")) { //You found a line with a comment, so there will be no variable names
                return ""; 
            }
            else if (line.Contains("//") || line.Contains("/*")) { //Checks the case if "for" is used in a comment and maybe the comment is ad the end of the line
                int index = line.IndexOf("//");  //returns -1 if no //, HELP: Is there a better way to code this?
                //Maybe just look for the first instance of a slash? It's unlikely you'll have a comment on a line with division MC
                if (index==-1) {
                    index = line.IndexOf("/*");
                }
                line=line.Substring(0,index); //get rid of the comment part
            }

            String[] words = line.Split(); //splits String into an array based off spaces; I think this is helpful if there is junk in front of a variable
            //int variableLength= 0;

            //NOTE: Not sure if this covers all the cases, but it covers some
            for (int i = 0; i < words.Length; i++) {
                //Case 1: If the user has spaces around the =, >=, <=, !=
                //        The user's code is "int counter = 0;"
                //        words = ["int","counter","=","0"]
                if ((words[i].Contains("=") | words[i].Contains(">=") | words[i].Contains("<=") | words[i].Contains("!=")) && words.Length > 1){
                    if (i == 0) { return ""; }
                    variable = words[i-1];
                    variable = stripVar(variable);
                }
                //Case 2: The user's code is "int counter= 0;" or "int counter=0;"
                //        words = ["int","counter=","0"] or words=["int","counter=0"]
                else if (words[i].Contains("=") && words.Length >1) {
                    //variable = words[i]; //Pretty sure this line is unneccessary
                    variable = stripVar(variable.Substring(0, words[i].IndexOf("="))); //FYI substring is (startIndex, length of substring)
                }
            }   
            return variable;
        }

        /*This function strips chars around from a given variable and returns the clean variable
            Covers intArray[max] -> intArray
            Covers if(rightPointer -> rightPointer
        */
        static String stripVar(String variable) {
            if (variable.EndsWith(']')) {
                //Console.WriteLine(variable.IndexOf(']')); //debug
                if (variable.IndexOf("[") == -1) {
                    return variable.Substring(0, variable.Length-1);
                }
                variable=variable.Substring(0, variable.IndexOf('['));
            }
            if (variable.StartsWith("for(") | variable.StartsWith("if(")) {
               variable=variable.Substring(variable.IndexOf('(')+1, variable.Length-variable.IndexOf('(')-1);
            }
            return variable;
        }

        //This function goes through the file and gets all the variable names. It returns a dictionary with variable and frequencies equal to 0
        public static Dictionary <String, double> makeVariableDict(List<String> textFile) {
            Dictionary <String, double> varNames=new();
            for (int line = 0; line < textFile.Count; line++) {
           // foreach (String line in textFile) {
                String variable=findVariableInCode(textFile[line]);
                if (variable != "") {  //If a variable was found in the line
                    if ( !varNames.ContainsKey(variable)) {   //Add the variable to the concordance if it isn't in there yet
                        varNames.Add(variable, 0);
                    }
                }
            }
            return varNames;
        }

        //This function goes through the file and searches the file for their frequencies. It returns an updated dictionary
        static void getVarFreq(ref Dictionary <String, double> varNames, ref int varCount, List<String> textFile) {
            char[] delimiter = { ' ', ',', '.', ':', '@', '!', '#', '$', '%', '^', '&', '*', '(', ')', '[',']', '{','}', '\t', '\n', '+', '-', '=', ';'};
            // for each line
            for (int line =0 ; line < textFile.Count; line++) {
                // split the words by finding the delimiter above
                string[] words = textFile[line].Split(delimiter);
                // if the already split word in array is in dictionary, increment the value at that key
                foreach (string word in words) {
                    if (varNames.ContainsKey(word)) {
                        varNames[word] ++;
                    }
                    varCount++;
                }
            }
        }
        
        public static double cmpVariables(List <String> unknownfile, List <String> knownfile) {
            Dictionary <String, double> unknownVar= makeVariableDict(unknownfile);
            Dictionary <String, double> knownVar=makeVariableDict(knownfile);
            int varCount = 0;
            getVarFreq(ref unknownVar, ref varCount, unknownfile);
            getVarFreq(ref knownVar, ref varCount, knownfile);
            //Console.WriteLine("this is var count" + varCount);
            return wordfreq.compareConcordance(unknownVar, knownVar, varCount/2);
        }

        //Tests Jenny wrote
        public static void tests(List <String> testFile) {
           //TEST 1: makeVariableDict
            Dictionary <String, double> variables = makeVariableDict(testFile);
            /*foreach (var key in varNames) {
                Console.WriteLine(key);
            }*/

            //TEST 2: getVariableFreq
           /* getVarFreq(ref variables, testFile);
            foreach (var key in variables) {
                Console.WriteLine(key);
            }*/
        }
    }
}
