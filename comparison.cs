using System;
using System.IO;

// comparison to be used in other functions
// not used in wordfreq because arrays

namespace codestylometry
{
    class comparer {

        public static int comparator( int[] unknown, int[] known) {
            int probability = 0;

            for ( int i=0; i<unknown.Length; i++) {
                if ( Math.Abs( unknown[i]-known[i])<.2) {
                    probability += 1/unknown.Length;
                }
                else if ( Math.Abs( unknown[i]-known[i])>.5) {
                    probability -= 1/unknown.Length;
                }

                
            }
            
            return probability;
        }
    }
}