using System;
using System.IO;

// compares how often words are used in two files

// information lost: sequence of words, what they mean
// advantage: relatively simple, keeps different kinds of word

namespace codestylometry
{
    class wordfreq
    {
        // creates a concordance
        public static Dictionary<string, double> makeConcordance( List <string> textfile, ref int wordcount) {
            
                Dictionary<string, double> concordance = new();

                string[] linewords;
                foreach( string line in textfile) { // for line in file

                    linewords = line.Split(); 

                    foreach( string word in linewords) { // for word in line
                    
                        if ( concordance.ContainsKey(word)) { // if there's an entry for it already
                            concordance[word]++;
                        } else {
                            concordance.Add(word, 1); // otherwise make one
                        }
                        wordcount++;
                    }
                }

                // from total of that word to proportion of words that word
                foreach( var entry in concordance) {
                   concordance[ entry.Key] = entry.Value/wordcount;
                }

                return concordance;
        }

        // compares two word frequency dictionaries
        public static double compareConcordance( Dictionary<string, double> unknownauthorconcordance, Dictionary<string, double> knownauthorconcordance, int wordcount) {
            
            double probability = 0; // probability written by the same author, approximation
            int total = 0;

            foreach ( var entry in knownauthorconcordance) { // for every word the known author uses
                if ( unknownauthorconcordance.ContainsKey( entry.Key)) {
                    if ( Math.Abs(entry.Value - unknownauthorconcordance[entry.Key]) < 0.2) { // if similarly used in unknown text increase probability
                        //probability = probability+entry.Value;
                        probability++;
                    }

                    else if ( (Math.Abs(entry.Value - unknownauthorconcordance[entry.Key]) < 0.5) && entry.Value*wordcount>10) { // if big difference in use decrease
                        //probability = probability-entry.Value;
                        probability--;
                    }
                    else { probability--; } // -= entry.Value; } //case 3: word frequencies are greater than .5. Very unlikely, but still possible
                }
                else { //key does not exist in uac
                   //probability -= entry.Value/2;
                   probability -= 0.5;
                }
                total++;
            }
            //probability currently ranges from -total to total. We need to scale that to 0 to 1.
            probability += total;
            return probability/(2*total);
        }


        // overall program for comparing vocabulary
        public static double concordance( List<string> unknownauthorfile, List<string> knownauthorfile)
        {
            // source for information on C#:
            // https://www.w3schools.com/cs/cs_arrays.php

            //Console.WriteLine( "This compares the word choice of the author or authors of the two files.");

            Dictionary<string, double> unknownconcordance; //it needs to be a double because we end up storing ratios, not absolute counts
            Dictionary<string, double> knownconcordance;
            int wordcount = 0;
            double probability; // also double because we store a ratio

            // create a concordance
            unknownconcordance = makeConcordance( unknownauthorfile, ref wordcount); // pass by reference with ref
            //Console.Write("Wordcount Unknown Author: ");
            //Console.WriteLine(wordcount);
            int wordcount2 = 0;
            knownconcordance = makeConcordance( knownauthorfile, ref wordcount2);
            //Console.Write("Wordcount Known Author: ");
            //Console.WriteLine(wordcount2);

            // compare the two vocabularies
            probability = compareConcordance( unknownconcordance, knownconcordance, wordcount); //what if the two files have different wordcounts?
            //Console.Write("Probability: "); //debugging
            //Console.WriteLine(probability);

            return probability;
        }  
    }
}
