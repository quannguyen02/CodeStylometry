using System;
using System.IO;

// this is the main function from which everything else is called
namespace codestylometry {
    class mainProgram {
        public static void Main(string [] args) {

            //ONLY use this for debugging only, change command-line arguments in launch.json
            //Need to add inside "string [] args" inside Main (line 10)
            //Does not use launch.json for running program. You need to write the path in the command-line, which is a pain
       /*     String unknownauthorfile = args[0];
            String knownauthorfile = args[1]; 
            String codelanguage = args[2]; */

            // input
            Console.WriteLine("Welcome to a code stylometry analysis program.");
            Console.WriteLine("Please enter the file whose authorship you are trying to determine and hit enter: ");
            string? unknownauthorfile = Console.ReadLine();
            Console.WriteLine("Please enter a file by the author you suspect and hit enter: ");
            string? knownauthorfile = Console.ReadLine();
            Console.WriteLine("Please enter the language of the files, if known, and hit enter");
            string? codelanguage = Console.ReadLine();
            if (codelanguage==null) { // validation on the language
                codelanguage = "Not known";
            }
            /*
            // IF EASIER FOR TESTING:
        
            string unknownauthorfile = "testfiles/interest.py";
            string knownauthorfile = "testfiles/concordance.py";
            string codelanguage = "C";
            */
            
            if ( codelanguage=="C#" | codelanguage=="C++" | codelanguage=="Java") { // we are treating a lot of languages as C-like
                codelanguage = "C";
            }
            

            // read in file and store as list of strings
            if ( File.Exists(unknownauthorfile) && File.Exists(knownauthorfile)) { // validation on the files
                if (unknownauthorfile.Equals(knownauthorfile))  {
                    Console.WriteLine("You cannot input the same file twice."); //this should exit, but for now I want it to keep running so I can input the same file for testing
                }

                List <string> unknowntextfile = File.ReadAllLines( unknownauthorfile).ToList();
                List <string> knowntextfile = File.ReadAllLines( knownauthorfile).ToList();

                double probability = 0;
                //int total = 5; //@TODO update this with the correct number of tests

                // run comparisons
                double conc_prob = wordfreq.concordance( unknowntextfile, knowntextfile);
                double space_prob = Spaces.space_comp(unknowntextfile, knowntextfile);
                double loop_prob;
                if ( codelanguage=="C") {
                    loop_prob = expressions.forLoopAnalyzer(unknowntextfile, knowntextfile);
                } else {   
                    loop_prob = keywords.loopcount( unknowntextfile, knowntextfile);
                }
                double comm_prob = comments.cmprWord(unknowntextfile,knowntextfile); //Tests
                double var_prob = variables.cmpVariables(unknowntextfile, knowntextfile);
                double func_prob = functions.cmpFunctions(unknowntextfile, knowntextfile);
                
                /*Console.WriteLine("conc" +conc_prob);
                Console.WriteLine("space" + space_prob);
                Console.WriteLine("loop" + loop_prob);
                Console.WriteLine("var" + var_prob);
                Console.WriteLine("func" + func_prob);
                Console.WriteLine("comm" + comm_prob);*/ //debug statements

                probability = conc_prob*(0.2) + space_prob*(0.26) + loop_prob*(0.1) + var_prob*(0.08) + func_prob*(0.26) + comm_prob*(0.1);

                //output
                Console.WriteLine("Probability of plagiarism: " + probability);
            }
            else { // if file does not exist
                Console.WriteLine("One or more of your files cannot be found."); 
            }
        }
    }
}
