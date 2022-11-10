Archivo: prueba.cpp
Fecha: 09/11/2022 07:57:49 a. m.
#make_COM#
include emu8086.inc
ORG 100h
;Variables: 
	 area DW 0
	 radio DW 0
	 pi DW 0
	 resultado DW 0
	 a DW 0
	 d DW 0
	 altura DW 0
	 x DW 0
	 y DW 0
	 i DW 0
	 j DW 0
InicioFor0:
MOV AX, 0
PUSH AX
POP AX
MOV i, AX
MOV AX, 3
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE 
POP AX
