Archivo: prueba.cpp
Fecha: 10/11/2022 09:46:34 a. m.
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
MOV AX, 1
PUSH AX
POP AX
MOV i, AX
MOV AX, 20
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE 
POP AX
MOV AX, 2
PUSH AX
MOV AX, 20
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE 
POP AX
MOV AX, 2
PUSH AX
MOV AX, 20
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE 
POP AX
MOV AX, 2
PUSH AX
MOV AX, 20
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE 
POP AX
MOV AX, 2
PUSH AX
MOV AX, 20
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE 
POP AX
MOV AX, 2
PUSH AX
MOV AX, 20
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE 
POP AX
MOV AX, 2
PUSH AX
RET
