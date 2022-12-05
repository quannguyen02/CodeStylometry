// C program for insertion sort
#include <math.h>
#include <stdio.h>

/* Function to sort an array using insertion sort*/
void insertionSort(int arr[], int size)
{
    int x, pivot, y;
    for (x = 1;x<size; x++) {
        pivot = arr[x];
        y =x- 1;

        while (y >= 0 && arr[y] > pivot) {
            arr[y + 1] = arr[y];
            y = y - 1;
        }
        arr[y + 1] = pivot;
    }
}

// A utility function to print an array of sizesize
void printArr(int arr[], int size)
{
    int x;
    for (x = 0;x<size; x++)
        printf("%d ", arr[x]);
    printf("\n");
}

/* Driver program to test insertion sort */
int main()
{
    int arr[] = { 12, 11, 13, 5, 6 };
    int size = sizeof(arr) / sizeof(arr[0]);

    insertionSort(arr,size);
    printArr(arr,size);

    return 0;
}
