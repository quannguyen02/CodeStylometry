# use sys to get list of command line arguments
import sys

def main():
  # create variable names for commandline arguments
  # s is the string, filename is the file with the words we're using to make anagrams, length is the minimum word length for words in anagrams, maximum is the max number of words in the anagram
  s = sys.argv[1]
  filename = sys.argv[2]
  length = int(sys.argv[3])
  maximum = int(sys.argv[4])

  # read in file, only adding words to set word if the word meets the minimum requirement and is in s
  file = open(filename, "r")
  words = set()
  for line in file:
    word = line.strip("\n")
    bool, x = contains(s, word)
    if bool == True:
      if len(word)>=length:
        words.add(word)
  # call function
  gram(s, words, [], maximum)

# tells us whether the letters in a word are found in s
def contains(s, word):
  x = s
  for i in word:
    if i in x:
      # replaces the characters if they are the same in word and x
      x = x.replace(i, "", 1)
      word = word.replace(i, "", 1)
    else: 
      return (False,"")
  # returns boolean word in s, what s is without word if word is in s
  return (True, x)

# anagramifies (finds all the anagrams) in s
def gram(s, words, sofar, maximum):
  # prints list sofar (which contains the words that make up the anagram) if s is empty because it has an anagram (if it is composed entirely of other words)
  # base case
  if s == "":
    print(" ".join(sofar)) 

  # if an anagram has not been found yet, the function is called recursively until all the anagrams are found
  else:
    #limits the amount of words allowed in the list
    if len(sofar) < maximum:
      #if a word from the set of words is found in string s, it is added to a word list (newsofar)
      for w in words:
        bool, x = contains(s, w)
        if bool == True:
          #newsofar is a new list that contains the contents of sofar
          newsofar=[]
          newsofar.extend(sofar)
          #adds word to list
          newsofar.append(w)
          # recursively calls function with remainder of s
          gram(x, words, newsofar, maximum)

main()