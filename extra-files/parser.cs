/* Uses functions and data from variables.cs and functions.cs to parse through a piece of code
*/

namespace codestylometry
{
    class parser
    {
        static void parse(List<String> file, ref Dictionary<String, double> vars, ref Dictionary<String, double> funcs) {
            vars = variables.makeVariableDict(file);
            funcs = functions.makeFunctionDict(file);
        }

        public static double parse_comp(List<String> unknown, List<String> known) {
            double probability = 0.5;
            Dictionary<String, double> unknownvars = new();
            Dictionary<String, double> unknownfuncs = new();
            parse(unknown, ref unknownvars, ref unknownfuncs);
            
            Dictionary<String, double> knownvars = new();
            Dictionary<String, double> knownfuncs = new();
            parse(known, ref knownvars, ref knownfuncs);

            probability += 0;
            probability += 0;

            return probability;
        }

        public static void tests(List<String> file) {

        }
    }
}
