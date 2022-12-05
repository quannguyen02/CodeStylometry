/* Creates an ASCII art diamond based on user input
Authors: Quan and Miriam
Date: November 4th, 2021
Extra credit 2: invert the diamond upon negative number
*/

#include <stdio.h>

#include <ctype.h>

#define CHAR_TO_NUM (int)'0'

/*
getdigit: returns the first signed digit from user input
*/
int getdigit(){
  while (1) {
    int user_char = getchar();
    if (user_char == -1)
      return 0xff; //handles EOF signals

    else if (isdigit(user_char)) {
      //Converts char ASCII representation of a digit
      // to intended int value
      return user_char - CHAR_TO_NUM;
    }

    //Checks for negative int input
    //"-" followed by digit
    else if (user_char == (int) '-') {
      user_char = getchar();
      if (isdigit(user_char)){
        return (user_char - CHAR_TO_NUM)*(-1);
      } else {continue;}
    }
  }
}

int main() {
  char in,out;
  printf("I will print a diamond for you, enter a size between 1 and 9: ");
   int size = getdigit();
  if (size == 0xff) {
    return 0; }
  //Set diamond to have stars inside if size positive,
  // spaces inside if negative
  if (size >= 0){
    in='*';
    out=' ';
  } else {
    size = -size; //Absolute value of size so loops work
    in=' ';
    out='*';
  }

  // Top half with middle row
  for (int i=0; i<size; i++){
    //chars on left
    for (int j = 0; j < size-i-1; j++){
      printf("%c",out);
    }
    //inside of diamond
    for (int k = 0; k < 2*i + 1; k++){
      printf("%c",in);
    }
    //chars on right
    for (int j = 0; j < size-i-1; j++){
      printf("%c",out);
    }
    printf("\n");
  }

  //bottom half
  //set i to size-2 so that middle row will not be reprinted.
  for (int i=size-2; i>=0; i--){
    //chars on left
    for (int j = 0; j < size-i-1; j++){
      printf("%c",out);
    }
    //chars inside
    for (int k = 0; k < 2*i + 1; k++){
      printf("%c",in);
    }
    //chars on right
    for (int j = 0; j < size-i-1; j++){
      printf("%c",out);
    }
    printf("\n");
  }
}
