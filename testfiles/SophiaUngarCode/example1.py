# I assume this data is not per million people, so divide?

from matplotlib import pyplot as plt

def readFile():
  file = open("WHO COVID-19 global table data December 5th 2020 at 7.01.05 PM.csv", "r")
  lines = file.readlines()
  countrynames = []
  culmativetotals = []
  for line in lines[2:]:
    line = line.split(",")
    
    #countrynames = countrynames + ", " + line[0]
    #culmativetotals = culmativetotals + ", " + line[3]
    
    countryname = line[0]
    if line[3] !="":
      culmative = line[3]
    elif line[3] == "":
      culmative = 0
    culmative = float(culmative)
    countrynames.append(countryname)
    culmativetotals.append(culmative)
  file.close
  return countrynames, culmativetotals

def sortlists(countrynames, culmativetotals):
  for i in range(len(culmativetotals)-1):
    for x in range(1,len(culmativetotals) - i):
      if culmativetotals[x-1]>culmativetotals[x]:
        culmativetotals[x], culmativetotals[x-1] = culmativetotals[x-1], culmativetotals[x]
        countrynames[x], countrynames[x-1] = countrynames[x-1], countrynames[x]
  return countrynames, culmativetotals

def toplists(countrynames, culmativetotals):
  topcountries = countrynames[-5:]
  topculm = culmativetotals[-5:]
  # topcountries = []
  # index = len(topcountries)
  # topculm = []
  # while len(topcountries) < 10:
  #   topcountries.append(countrynames[index])
  #   topculm.append(culmativetotals[index])
  #   index = index - 1
  return topcountries, topculm

def plot(topcountries, topculm):
  plt.scatter(topcountries, topculm)
  plt.title("Country Culmative Totals Per a Million People")
  plt.xlabel("Country")
  plt.ylabel("Culmative totals per million people")
  plt.show()

def main():
  countrynames, culmativetotals = readFile()
  countrynames, culmativetotals = sortlists(countrynames, culmativetotals)
  topcountries, topculm = toplists(countrynames, culmativetotals)
  plot(topcountries, topculm)

main()