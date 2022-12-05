using System;
using System.IO; //contans File
using System.Linq;
using System.Collections.Generic;

// compares instances of for loop 

// information lost: everything except how often the programmer chooses to use a for or while loop
// advantages: fairly direct comparison, 

namespace codestylometry 
{
    class keywords
    {
        static int[] counter( List<string> textfile, int for_count, int do_count, int while_count, int lines) {
            foreach (String line in textfile) { // for each line in files
                if (line.Contains("for")) { // counts for loops, etc
                    for_count++;
                }
                if (line.Contains("do")) {
                    do_count++;
                }
                if (line.Contains("while")) {
                    while_count++;
                }
                lines++; // counts lines
            }
            return new int[] {for_count, do_count, while_count};
        }
        
        public static int loopcount( List<string> unknowntextfile, List<string> knowntextfile)
        {
            int probability = 0;

            int for_count_unknown=0; //counts how many times 'for' appears in code 1
            int do_count_unknown=0;
            int while_count_unknown=0;
            int lines_in_unknown=0;

            int[] unknown_counts;

            int for_count_known=0; //counts how many times 'for' appears in code 2
            int do_count_known=0;
            int while_count_known=0;
            int lines_in_known=0;
            
            int[] known_counts;

            unknown_counts = counter( unknowntextfile, for_count_unknown, do_count_unknown, while_count_unknown, lines_in_unknown);
            known_counts = counter( knowntextfile, for_count_known, do_count_known, while_count_known, lines_in_known);

            probability = comparer.comparator( unknown_counts, known_counts);

            return probability;

        }
    }
}