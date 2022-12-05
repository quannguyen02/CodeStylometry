# interest.py
# This program improves our simple.py program
# from Lab 1 to calculate interest and account
# balances for multiple months
#
# Name: Jenny Rowlett
# Date: 9/17/2020

print("Welcome to the Interest Calculator!")
   
savings= float(input("Enter your initial savings: "))
interest_rate= float(input("Enter the monthly interest rate: "))
monthly_contribution= float(input("Enter your monthly contribution: "))
months= int(input("How many months would you like computed: "))

print("Initally you put in $", savings, sep="")
for i in range (months):
  total_savings= savings + (savings * interest_rate + monthly_contribution)
  print("After month ", i+1, " you would have $", int(100*total_savings)/100, sep="")
  savings= total_savings
  
  