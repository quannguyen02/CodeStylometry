# concordance.py
# This program creates a concordance for a given text 
# file.  For each word in the file, it keeps track of all 
# of the lines where the word occurs in the file.
#
# Partner allowed (and encouraged).
#
# Name(s): Jenny Rowlett
# Date: 10/15/20

# Resources I used (reccomended by a lab helper): 
# https://www.w3schools.com/python/ref_func_sorted.asp
# https://www.w3schools.com/python/python_try_except.asp


def AddWord(word,lineNumber,Concordance): 
  #adds word and line number to concordance 
  if word in Concordance.keys():
    lineNumbers = Concordance[word]
    lineNumbers.append(lineNumber)
    Concordance[word] = lineNumbers
  else:
    Concordance[word] = [lineNumber]

def PrintEntry(Concordance,totalLineNumbers): 
  x = sorted(Concordance)
  for word in x:
    print(word,end=" ") 
    for num in Concordance[word]:
      print(num,end=" ")
    print()
  print("I found",totalLineNumbers,"lines containing", len(x),"unique words.")

def RemovePunctuation(s):
  x = [] #new list 
  for word in s: #loop through all the words in the list
    x.append(word.lower()) #makes all words lowercase and adds it to the list x
  removedPunct = [] 
  for eachWord in x: #loop through each word in the list of lowercased words
    removedPunct.append(eachWord.strip('''[.,:;"!'?]-''')) #get rid of all special characters
  return removedPunct

def main():

  fileName = input("Please enter the file that you would like use: ")
  
  try:
    file = open(fileName,"r")
    lines = file.readlines() #reads file and puts each line in a list
    file.close()

    lineNumber = 0
    Concordance = {}
    totalLineNumbers= 0

    for line in lines: #looks at each line in the list
      line = line.strip() #gets rid of leading and traiiling spaces around words 
      if line != "\n" and line != "":
        totalLineNumbers+=1  #total number of lines
      if len(line) > 0: #if line length is greater than 0
        lineNumber+=1
        line = line.split(" ") #split each word 
        line = RemovePunctuation(line) #remove punctuation
        for eachword in line: 
          if len(eachword) > 0:
            AddWord(eachword,lineNumber,Concordance)
    PrintEntry(Concordance,totalLineNumbers)
  except:
    print("An error ocurred.")

main()

