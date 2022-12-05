using System;
using System.IO; 

namespace codestylometry
{
    class expressions
    {
        // This function searches for a "for loop" nearby. 
        /* For example, let's say the known file has a for loop has on line 48, but the plagarized file has one on line 49. This will search in the nearby area. 
         * Returns linenumber of a nearby for loop. If none found, returns 0
         */
        static int searching(List <String> unknownFile, int lineNumber) {
            int lowerBound = lineNumber-3;
            if (lowerBound < 0) { //Handling if it occurs at the beginning of the file
                lowerBound = 0;
            }
            int upperBound = lineNumber+3;
            
            for (int i = lowerBound; i < upperBound; i++) {
                if (unknownFile[i].Contains("for")) {
                    lineNumber=i;
                    break;
                }
                else {
                    lineNumber=0;
                }
            }
            return lineNumber;
        }

        // This helper funciton gets the parts of a for loop
        /* If the line is "for (int i = 0; i < 8; i++) {"
         * The function will return ["int i = 0","i < 8", i++"]
         */
        static void getLoopParts(String line, ref List<String> loopParts) {
            String [] parts = line.Split(';');
            parts[0]=parts[0].TrimStart('f','o','r',' ');
            foreach (String part in parts) {
                loopParts.Add(part.Trim(')',' ','(',';','{'));
            }
        }
       
        //Helper function
        static void split (List <String> loopParts, ref String [] section, ref  int index) {
            while (index != 3) {
                if (loopParts[index].Contains('>')) {
                    section = loopParts[index].Split('>');
                    index++;
                    break;
                }
                else if (loopParts[index].Contains('<')) {
                    section = loopParts[index].Split('<');
                    index++;
                    break;
                }
                else if (loopParts[index].Contains('=')) {
                    section = loopParts[index].Split('=');
                    index++;
                    break;
                }
            }
        }

        // This function creates a dictionary of tokens.
        /* Example: Using the same example as above
         * {"data type": "int", "Variable":"i", "min": "0", "max":"8", "counter":1"}
         */
        static Dictionary<String, String> getTokens (String line) {
            List<String> loopParts=new();
            getLoopParts(line,ref loopParts);

            Dictionary <String, String> loopTokens = new();
            String [] start= {};
            String [] middle= {};
            String [] end= {};

            int index =0;
            split(loopParts,ref start, ref index);
            split(loopParts,ref middle, ref index);

            loopTokens.Add("Data Type",start[0].Substring(0,start[0].IndexOf(" ")));
            loopTokens.Add("Variable",middle[0].Trim());
            loopTokens.Add("Start",start[1].Trim());
            loopTokens.Add("End",middle[1].Trim());
            if (loopParts[2].EndsWith('+')) {
                loopTokens.Add("Counter","+");
            }
            else if (loopParts[2].EndsWith('-')) {
                loopTokens.Add("Counter","-");
            }
            else {
                split(loopParts,ref end, ref index);
                loopTokens.Add("Counter",end[1]);
            }
            return loopTokens;
        }

        // Calculates range of a for loop
        static int calcRange (String start, String end) {
            int Start;
            int End;
            try {
                Start = Int32.Parse(start);
            } catch(Exception) {
                return 0;
            }
            try {
                End = Int32.Parse(end);
            } catch(Exception) {
                return 0;
            }
            int range = Math.Abs(Start-End);
            return range;
        }

        // Compares the range of the for loop
        // Scoring system: returns 1 if the difference between the ranges is 1 or 0; otherwise return 0
       static int cmpRange (int unknownRange, int knownRange) {
            if (Math.Abs(unknownRange - knownRange)==1 || unknownRange==knownRange) {
                return 1;
            }
            return 0;
        }

        // This function compares tokens of one for loop to another;
        /* Example: 
         * Dict 1: {"data type": "int", "Variable":"i", "min": "0", "max":"8", "counter":1"}
         * Dict 2: {"data type": "double", "Variable":"x", "min": "-1", "max":"9", "counter":1"}
         * 
         * Scoring system: 
         *   Returns 1 if all parameters are the same
         *   Returns 0.95 if range + counter + data type are the same but variable is diffent
         *   Returns 0.85 if range + counter + variable are the same but data type is diffent
         *   Returns 0.8 if range + counter are the same but variable + datatype are diffent
         *   Returns 0 otherwise
         */
        static double cmpLoop (Dictionary <String, String> unknownTokens, Dictionary <String, String> knownTokens) {
            
            //Calculating range
            int unknownRange = calcRange(unknownTokens["Start"], unknownTokens["End"]);
            int knownRange = calcRange(knownTokens["Start"], knownTokens["End"]);

            if (cmpRange(unknownRange, knownRange) == 1 &&
                unknownTokens["Counter"] == knownTokens["Counter"]) {
                if (unknownTokens["Data Type"] == knownTokens["Data Type"] && 
                    unknownTokens["Variable"] == knownTokens["Variable"]) {
                    return 1;
                }
                if (unknownTokens["Data Type"] == knownTokens["Data Type"] && 
                    unknownTokens["Variable"] != knownTokens["Variable"]) {
                    return 0.95;
                }
                else if (unknownTokens["Data Type"]!= knownTokens["Data Type"] && 
                    unknownTokens["Variable"]== knownTokens["Variable"]) {
                    return 0.85;
                }
                else {
                    return 0.80;
                }
            }
           return 0.5;
        }

        //Calculates likelihood that for loops are plagarized 
        static void cmpLoops(List<double>scores, ref double probability) {
            foreach (double score in scores) {
                probability +=score;
            }
            probability=probability/scores.Count;
        }

        public static double forLoopAnalyzer(List <String> unknownFile, List <String> knownFile)
        {
            // This code finds all the lines that have a for loop in the known file
            Dictionary <List<String>, int> forLoops = new();

            // Finding out which file is longer; setting max to the shorter file
            int max = 0;
            if (knownFile.Count > unknownFile.Count) {
                max=unknownFile.Count;
                //Console.WriteLine(max);

            }
            else {
                max=knownFile.Count;
                //Console.WriteLine(max);

            }
            
            Dictionary <String, String> knownTokens=new();
            Dictionary <String, String> unknownTokens=new();
            List <double> scores = new();

            for (int i = 0; i < max; i++) {
                //Console.WriteLine(i);
                if (knownFile[i].StartsWith("//") || knownFile[i].StartsWith("/*")) {
                    continue; //Found a comment
                }
                else if (knownFile[i].Contains("for") && unknownFile[i].Contains("for")) {
                    knownTokens= getTokens(knownFile[i]); 
                    unknownTokens= getTokens(unknownFile[i]); 
                    double score = cmpLoop(unknownTokens,knownTokens);
                    //Console.WriteLine("Score1:" + score);
                    scores.Add(score);
                }
                else if (!knownFile[i].Contains("for") && unknownFile[i].Contains("for")) {
                    scores.Add(0);
                    //unknownTokens= getTokens(unknownFile[i]); //get tokens for unknownfile
                    //int newLineNum = searching(knownFile, i);
                    /*if (newLineNum!=0) { //searches nearby lines to see if there is a for loop
                        knownTokens= getTokens(knownFile[newLineNum]);
                        double score = cmpLoop(unknownTokens,knownTokens);
                        Console.WriteLine("Score2:" + score);
                        scores.Add(score);
                    }
                    else {
                        scores.Add(0); //does not match original file
                    }*/
                }
                knownTokens.Clear();
                unknownTokens.Clear();
            }
            
            //Calculate probability
            double probability = 0;
            cmpLoops(scores,ref probability);
            //Console.WriteLine("Probability that for loops have been changed: " +probability);

            return probability;
        }
    }
}
