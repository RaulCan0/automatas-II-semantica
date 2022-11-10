//Raúl Cano Briseño
#include <iostream>
#include <stdio.h>
#include <conio.h>
float area, radio, pi, resultado;
int a, d, altura;
float x;
char y;
int i;
int j;
// Este programa calcula el volumen de un cilindro.
void main()
{
   for(i = 0; i < 3; i ++){ //salida("0 1 2")
	printf(i);
	printf(" ");
}
printf("\n");
for(i = 3; i > 0; i--){ //salida("3 2 1")
	printf(i);
	printf(" ");
}
printf("\n");
for(i = 0; i < 20; i+=5){ //salida("0 5 10 15")
	printf(i);
	printf(" ");
}
printf("\n");

for(i = 1; i < 6; i*=2){ //salida("1 2 4")
	printf(i);
	printf(" ");
}
printf("\n");
i /= 4; 
printf(i); //salida: ("2")
}