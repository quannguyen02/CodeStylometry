// Jenny Rowlett and Sophia Ungar
// 7 November 2021

#include <stdio.h>
#include <ctype.h>

// gets the arguments from the commandline and returns the first integer
int getdigit(){
  int c=0; 
  int d=0;
  c=getchar(); //getchar() returns decimal value 
  d=getchar();
  do {
    if (c==45 && isdigit(d)) {
      d=d-'0';
      return -d;
    }
    if (isdigit(c)){
      return c-'0';
    }
    c=d;
    d=getchar();
  }
  while (c!=EOF);
  return EOF; 
}

// creates the diamond, which is inverted if the number is negative
// returns 0 if successful
int main(){
  printf("I will print a diamond for you, enter a size between 1-9:\n" );
  int a=getdigit();
  if (a!=EOF) {
  char inside;
  char outside;
  if (a>0) {
    inside='*';
    outside=' ';
  }
  else {
    a=-a;
    inside=' ';
    outside='*';
  }
  int b = a-1;
  int c =1;
  for (int i=0; i<a; i++){
    for (int i=0; i<b; i++) {
      printf("%c",outside);
    }
    for (int i=0; i<c; i++) {
      printf("%c",inside);
    }
    for (int i=0;i<b;i++) {
      printf("%c",outside);
    }
    printf("\n");
    b = b-1;
    c = c+2;
  }
  // bottom half of the diamond
  b = b+2;
  c = c-4;
  for (int i=0; i<a-1; i++){
    for (int i=0; i<b; i++) {
      printf("%c",outside);
    }
    for (int i=0; i<c; i++) {
      printf("%c",inside);
    }
    for (int i=0; i<b;i++) {
      printf("%c",outside);
    }
    printf("\n");
    b = b+1;
    c = c-2;
    }
  }
  return (0);
}