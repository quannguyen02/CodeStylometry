//oldParser - holds a prior version of our parsing code.
//Created 1/19/22

namespace codestylometry
{
    class oldParser {
        /*Looks through the entire file (as a single line of text) to find functions and variable names
            Returns a dictionary of variable names and their frequecies, and a dictionary of functionnames and their frequencies.
            Do we count parameters as variables? If so, do we count their presence in the original function definition?
        */
        // ideally works okay for any language
        static void generalParser( string text, ref Dictionary<string,double> variablenames, ref Dictionary<string,double> functionnames) {
            char character = '\0';
            int startindex = 0;
            int endindex = 0;
            string word = "";
            // loop through the entire text
            for ( int i=0; i< text.Length; i++) { // likely a variable name
                character = text[i];
                if ( character == '=') { // likely assigning variable
                    startindex = i;
                    endindex = i;
                    while( text[startindex]!='\n') { //what is this loop supposed to do? I cannot tell. MC
                        while( Char.IsLetter(text[startindex])) { //what about variables with numbers, hyphens or underscores? What about when you are assigning the value of one variable to another? MC
                            // Or the return value of a function - do we count that as an iteration of the function name. I guess that would be caught by the next case though. MC
			                startindex--;
                        }
                        word = text.Substring( startindex, endindex-startindex); //because the second param is length, not the final index
                        word.Trim(); //because there may or may not be spaces between the equals sign and the variable

                        //Add the variable to the concordance, or increment its frequency if it already exists.
                        if ( variablenames.ContainsKey(word)) {
                            variablenames[word]++;
                        } else {
                            variablenames.Add(word, 1);
                        }
                        break; // I can't tell what anything after this line is supposed to do. MC

                        /*while( !Char.IsLetter(text[startindex])) {
                            startindex--;
                        }

                        endindex=startindex; */
                    }//end while
                }//end if
                
                else if ( character == '(') { //before is a function (or a math expression), after is more variables (or more math)
                    startindex = i;
                    endindex = i;
                    while( text[startindex]!='\n') { //again, no idea what this loop is for MC
                        while( Char.IsLetter(text[startindex])) {
                            startindex--;
                        }
                        word = text.Substring( startindex, endindex-startindex);
                        word.Trim();

                        if ( functionnames.ContainsKey(word)) {
                            functionnames[word]++;
                        } else {
                            functionnames.Add(word, 1);
                        }
                        break;

                        /*while( !Char.IsLetter(text[startindex])) { //Can't tell what any of this does either. MC
                            startindex--;
                        }

                        endindex=startindex;*/
                         
                    }//end while
                } //end else if
            }//end loop
            //return 0;
        }

        static int parsComparison( ref Dictionary<string, double> variablenames, ref Dictionary<string, double> functionnames) {
            
            
            return 0;
        }

        // calls correct function
        public static int parser( string codelanguage, string unknownfilename, string knownfilename) {
            string anonymoustext = System.IO.File.ReadAllText(unknownfilename);
            string comparisontext = System.IO.File.ReadAllText(knownfilename);
            // maybe a switch block here?

            Dictionary<string,double> variablenames = new();
            Dictionary<string,double> functionnames = new();

            generalParser( anonymoustext, ref variablenames, ref functionnames);
            //generalParser( comparisontext, ref variablenames, ref functionnames );

            /*foreach {

            }*/

            return 0;
        }
    }
}