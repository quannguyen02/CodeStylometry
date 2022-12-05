//This part of the program analyzes comments left by the user. It calls methods in other classes (wordAnalysis and spacingAnalysis, specifically)
using System;
using System.IO;
using System.Text.RegularExpressions;


namespace codestylometry {
    class comments 
    {
        //getSingleLineComments gets all the single line comments from a text file and stores them in a list. 
        public static List<String> getSingleLineComments(String language, List <String> textFile) {

            List<String> fileComments=new();
            String commentStart = "";
            if (language == "C") {
                commentStart="//";
            }
            else if (language=="Python") {
                commentStart="#";
            }
            else if (language=="HTML") {
                commentStart="<!--";
            }
            
            //@TODO: Handle case if langauge is none of the above
            foreach(var comment in textFile) { //parses textfile for comments
                if (comment.Contains(commentStart)) {
                    int index= comment.IndexOf(commentStart);
                    if (language=="C") {
                        fileComments.Add(comment.Substring(index+2));
                    }
                    if (language=="Python") {
                        fileComments.Add(comment.Substring(index+1));
                    }
                    if (language=="HTML") {
                        fileComments.Add(comment.Substring(index+4).TrimEnd('-','>'));
                    }
                }
            }
            return fileComments;
        }

        //getMultiLineComments gets all the multiline comments from a text file and stores them in a list. 
        public static List<String> getMultiLineComments(String language, List<String> textFile) {
            List<String> fileComments=new();
            String holder = "";
            bool commentFound=false;
            if (language=="C") {
                foreach(var comment in textFile) { //parses textfile for comments
                    if (comment.Contains("/*")) {
                        int index=comment.IndexOf("/*");
                        holder+=comment.Substring(index+2);
                        commentFound=true;
                    }
                    else if (commentFound==true && !comment.Contains("*/")) {
                        holder+=comment;
                    }
                    else if (commentFound==true && comment.Contains("*/")) {
                        int index=comment.IndexOf("*/");
                        holder+=comment.TrimEnd('*','/');
                        fileComments.Add(holder);
                        holder="";
                        commentFound=false;
                    }
                }
                //@TODO: Handle HTML and Python
            }
            return fileComments;
        }

        public static List<String> getAllComments(List<String> singleComments, List<String> multiLineComments) {
            if (singleComments!=null) {
                singleComments.AddRange(multiLineComments);
                return singleComments;
            }
            else {
                return multiLineComments;
            }
        }

        //Compares words in comments to another file, utilizing code from wordAnalysis
        public static double cmprWord(List <string> unknownFile, List<string> knownFile) {
            Dictionary<string, double> unknownConcord; 
            Dictionary<string, double> knownConcord;
            int wordcount = 0;
            
            unknownConcord = wordfreq.makeConcordance(unknownFile, ref wordcount);
            knownConcord = wordfreq.makeConcordance(knownFile, ref wordcount);
            double probability = wordfreq.compareConcordance(unknownConcord, knownConcord, wordcount);
            return probability;
        }

        //Compares characters in comments to another file


        //Counts number of comments per file
        public static int getNumComments(List<String>comments) {
            return comments.Count;
        }

        //Compares number of comments to another file
        //Probably don't need this?
        public static int cmpNumComments(int knownComments, int unknownComments) {
            if (knownComments == unknownComments) {
                return 1;
            }
            else {
                return 0;
            }
        }

        //Returns frequencies of a comment starting with a capitalized letter
        static int countCapitalLetters(List <String> comments) {
            char start=' ';
            int uppercaseCount=0;
            foreach (var comment in comments) {
                start = comments[0][0];
            }
            if (Char.IsUpper(start)) {
                uppercaseCount+=1;
            }
            return uppercaseCount/comments.Count;
        }
        
        //Looks at placement of comments


        //Compares placements of comments to another file
      

        //Tests
        public static void comment_tests(String codelanguage, List <String> unknownFile, List<String> knownFile) {
            //TEST 1: Single line comments
            List<String>single_comments = getSingleLineComments(codelanguage,unknownFile); //what a wordy test lmao
            foreach (var comment in single_comments) {
                Console.WriteLine(comment);
            }  
            //TEST 2: Multiline comments
            List<String>multi_comments = getMultiLineComments(codelanguage, unknownFile);
            foreach (var comment in multi_comments) {
                Console.WriteLine(comment);
            }

            //TEST 3: All comments
            List<String>unknown_comments = getAllComments(single_comments, multi_comments);
            foreach (var comment in multi_comments) {
                Console.WriteLine(comment);
            }

            //TEST 4: Comparing comments
            List<String>single_comments2 = getSingleLineComments(codelanguage,knownFile); //what a wordy test lmao
            List<String>multi_comments2 = getMultiLineComments(codelanguage,knownFile); //what a wordy test lmao
            List<String>known_comments = getAllComments(single_comments2, multi_comments2);
            Console.WriteLine(cmprWord(known_comments,unknown_comments));

        } 
    }
}
