/* Part of parser.cs
Finds functions within a piece of code
*/

namespace codestylometry
{
    class functions {
          static String findFunction(String line) {
            String function = "";
            line = line.Trim(); //getting rid of any whitespace

            if (line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*")) { //You found a line with a comment, so there will be no variable names
                return ""; 
            }
            else if (line.Contains("//") || line.Contains("/*")) { //Checks if there is a comment at the end of the line
                int index = line.IndexOf("/");
                line=line.Substring(0,index); //get rid of the comment part
            }

            String[] words = line.Split();
            for (int i = 0; i < words.Length; i++) {
                if (words[i].Contains("(")) { //cases where there is no space between something & a "(": if(..., for(, func(
                    String func = words[i].Substring(0, words[i].IndexOf("("));
                    func.Trim();
                    if (func.Equals("for") || func.Equals("if") || func.Equals("while")) {
                        return "";
                    }
                    else if (func.Contains(")")) { //this is the character before the original "("
                        //it could also be a math/casting expression
                        return "";
                        
                    } 
                    else { //it's probably a function 
                        //may need to trim periods and other chars
                        if (func.Contains(".")) {
                            function = func.Substring(func.LastIndexOf("."), func.Length-func.LastIndexOf("."));
                        }
                        else {
                            function = func;
                        }
                    }
                }
                if (words[i].StartsWith("(") && i != 0) {
                    //look at the previous word/token
                    String func = words[i-1].Trim();
                    //case 1: it's a loop/keyword
                    if (func.Equals("for") || func.Equals("if") || func.Equals("while")) {
                        return "";
                    }
                    //case 2: it's part of a multi line comment
                    //case 3: it's a math expression (less likely)
                    //case 4: it's actually a function
                    else {
                        if (func.Contains(".")) {
                            function = func.Substring(func.LastIndexOf("."), func.Length-func.LastIndexOf("."));
                        }
                        else {
                            function = func;
                        }
                    }
                }
            }
            return function;
        }
         public static Dictionary<String, double> makeFunctionDict(List<String> textFile) {
            Dictionary <String, double> functions=new();
            for (int line = 0; line < textFile.Count; line++) {
           // foreach (String line in textFile) {
                String function=findFunction(textFile[line]);
                if (function != "") {  //If a function was found in the line
                    if ( !functions.ContainsKey(function)) {   //Add the function to the concordance if it isn't in there yet
                        functions.Add(function, 0);
                    }
                }
            }
            return functions;
        }

        static void getFunctionFrequencies(ref Dictionary<String, double> functionNames, ref int count, List<String> textFile) {
            char[] delimiter = { ' ', ',', '.', ':', '@', '!', '#', '$', '%', '^', '&', '*', '(', ')', '[',']', '{','}', '\t', '\n', '+', '-', '=', ';'};
            // for each line
            for (int line =0 ; line < textFile.Count; line++) {
                // split the words by finding the delimiter above
                string[] words = textFile[line].Split(delimiter);
                // if the already split word in array is in dictionary, increment the value at that key
                foreach (string word in words) {
                    if (functionNames.ContainsKey(word)) {
                        functionNames[word]++;
                    }
                    count++;
                }
            }
        }

        public static double cmpFunctions(List <String> unknownfile, List <String> knownfile) {
            Dictionary <String, double> unknownFuncs= makeFunctionDict(unknownfile);
            Dictionary <String, double> knownFuncs=makeFunctionDict(knownfile);
            int varCount = 0;
            getFunctionFrequencies(ref unknownFuncs, ref varCount, unknownfile);
            getFunctionFrequencies(ref knownFuncs, ref varCount, knownfile);
            //Console.WriteLine("this is func count" + varCount);
            double probability= wordfreq.compareConcordance(unknownFuncs, knownFuncs, varCount/2);
            return probability;
        }
        
        //Copied tests from variables.cs
        public static void tests(List <String> testFile) {
           //TEST 1: makeVariableDict
            Dictionary <String, double> functions = makeFunctionDict(testFile);
            foreach (var key in functions) {
                Console.WriteLine(key);
            }

            //TEST 2: getVariableFreq
            int test = 0;
            getFunctionFrequencies(ref functions, ref test, testFile);
            foreach (var key in functions) {
                Console.WriteLine(key + ", " + key.Value);
            }
        }
    }
}