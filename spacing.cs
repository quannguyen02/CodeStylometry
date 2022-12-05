using System;
using System.IO;

// compares spacing (\n, \t, " ") "handprint" of two files

namespace codestylometry
{
    class Spaces
    {
        //This function counts the number of characters per line and returns an array of frequencies
        static int[] countCharacters(List <string> textfile) {
            int[] frequencies=new int[6]; //number of chars in a line: 0, 1-10, 11-20, 21-30, 31-40, 41+
            int accumulator= 0;

            foreach (string line in textfile) {

                foreach (char character in line) {
                    accumulator++; // counts characters between \n
                }
                if (accumulator==0) {
                    frequencies[0]++;
                }
                else if (accumulator>0 && accumulator<=10) {
                    frequencies[1]++;
                }
                else if (accumulator>10 && accumulator<=20) {
                    frequencies[2]++;
                }
                else if (accumulator>20 && accumulator<=30) {
                    frequencies[3]++;
                }
                else if (accumulator>30 && accumulator<=40) {
                    frequencies[4]++;
                }
                else if (accumulator>40) {
                    frequencies[5]++;
                }
                accumulator=0;
            }
            return frequencies; // table where first entry is 0-9 char lines, etc
        }

        /*This function takes a list of text and figures out how many spaces, tabs, and empty/near-empty newlines in it.
        It returns the values of the number of spaces, the number of tabs, the number of empty newlines and the width of a tab */
        static void spacing(List<string> text, ref int spaces, ref int tabs, ref int newline, ref int tab_size) {
            string tab_ex = ""; //some tabs show up as a bunch of spaces. This variable is an object to compare later substrings to.
            foreach (string line in text) {
                int i = 0;
                while (i < line.Length) {
                    if (line[i] == '\t') {
                        tabs++;
                    }
                    else if (line[i] == ' ') { //is it a space, or part of a "broken up" tab?
                        if (tab_size == 0 && i == 0) {
                            do {
                                tab_size++;
                                i++;
                            } while (i < line.Length && line[i] == ' '); //consecutive spaces are really just a tab
                            tab_ex = line.Substring(0, tab_size);
                            tabs++;
                        }
                        else if (i == 0) { //Check for multiple "broken up" tabs
                            try { 
                                while (line.Substring(i, tab_size).Equals(tab_ex)) {
                                    tabs++;
                                    i += tab_size;
                                }
                            } catch (ArgumentOutOfRangeException) {
                                //no more tabs, < tab_size chars left in the line, continue with checking the rest of the line
                                //have to check if the next char is a space
                                if (i < line.Length && line[i] == ' ') { spaces++; }
                            }
                        }
                        else {
                            spaces++;
                        }
                    } //end else if
                    i++;
                }//endwhile
                

                if (i == 0) { newline++; } //empty line (only has newline)
                else {
                    i = line.Length - 1; //last valid index
                    if (line[i] == '{') { //this author separates their code blocks from their introductory phrases
                    // ie: if (STATUS)
                    // {
                    // ...
                    // }
                        int len = 0;
                        foreach (char c in line) {
                            if (!(c == ' ' || c == '\t')) { break; }
                            len++;
                        }
                        if ((len + 1) == line.Length) { newline++; }
                    }
                    else if (line[i] == '}') { //looks at closing brackets (commonly separate from code blocks, but not always)
                        int len = 0;
                        foreach (char c in line) {
                            if (!(c == ' ' || c == '\t')) { break; }
                            len++;
                        }
                        if ((len + 1) == line.Length) { newline++; }
                    }
                    else if (line[i] == ' ' || line[i] == '\t') { //check lines that have tabs/spaces but nothing else
                        int len = 0;
                        foreach (char c in line) {
                            if (c != ' ' && c != '\t') { break; }
                            len++;
                        }
                        if (len == line.Length) { newline++; }
                    }
                }//end else
            }//end foreach
        }
        
        /*This function serves as an entrance point to the other functions in this file.
            It returns a value between zero and 1 indicating the likelihood that the two files are written by the same author
        */
        public static double space_comp(List<string> file1, List<string> file2) {
            int total = 16; //the highest possible count for probability.
            double prob = 0;
            
            
            // Console.WriteLine("This part looks at spacing. The code counts how many characters per line. And then categorizes each line based on how many characters it has in it.");
            
            int [] arr1 = countCharacters(file1);
            int[] arr2 = countCharacters(file2);
            int len1 = file1.Count;
            int len2 = file2.Count;
            for (int i = 0; i < arr1.Length; i++) {
                if (Math.Abs((double)arr1[i]/len1-(double)arr2[i]/len2) < 0.5) { prob++; } //I have no idea what the threshold for this test should be. 0.5 seems too high? MC
            }
            if (prob < total/2) { prob--; } //None of the buckets had similar counts - line lengths are very different

            //Find space and tab information for each file
            int space1 = 0, tab_size1 = 0, tab1 = 0, new1 = 0;
            spacing(file1, ref space1, ref tab1, ref new1, ref tab_size1);

            int space2 = 0, tab_size2 = 0, tab2 = 0, new2 = 0;
            spacing(file2, ref space2, ref tab2, ref new2, ref tab_size2);

            //Compare information
            if (tab_size1 != tab_size2) { prob -= 3; } //probability way decreases - they were probably written in different editors
            //range is -4,6
            if (Math.Abs(((double)space1/len1) - ((double)space2/len2)) > 0.1) { //avg frequency of spaces
                //probability decreases - spacing is different
                prob--;
            }
            else { prob++; } //if the spacing is the same, probability increases
            if (Math.Abs((double)new1/len1 - (double)new2/len2) > 0.5) { prob--; } //empty newline prevalence
            else { prob++; }
            /* Debugging
            Console.WriteLine("Num spaces: " + space1);
            Console.WriteLine("Num tabs: " + tab1 + ", tab size: " + tab_size1);
            Console.WriteLine("Frequency of spaces: " + ((double)space1/len1));
            Console.WriteLine("Number of empty lines: " + new1 + ", Frequency of empty/near-empty lines: " + ((double)new1/len1));
            */
            prob += 8; //so no negative numbers
            return prob/total;
        }
    }
}