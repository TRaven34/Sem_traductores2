#include "pch.h"
#include "OperacionesASM.h"

ASM::ASM()
{

}

short ASM::Suma(short Num1, short Num2)
{
    _asm
    {
        mov ax, Num1;
        mov bx, Num2;
        add ax, bx;
        mov Num1, ax;
    }
    return Num1;
}

short ASM::Resta(short Num1, short Num2)
{
    _asm
    {
        mov ax, Num1;
        mov bx, Num2;
        sub ax, bx;
        mov Num1, ax;
    }
    return Num1;
}

short ASM::Multiplicacion(short Num1, short Num2)
{
    _asm
    {
        mov ax, Num1;
        mov bx, Num2;
        mul bx;
        mov Num1, ax;
    }
    return Num1;
}

short ASM::Division(short Num1, short Num2)
{
    _asm
    {
        xor dx, dx;
        mov ax, Num1;
        mov bx, Num2;
        div bx;
        mov Num1, ax;
    }
    return Num1;
}

short ASM::Modulo(short Num1, short Num2)
{
    short rest;
    _asm
    {
        xor dx, dx;
        mov ax, Num1;
        mov bx, Num2;
        div bx;
        mov rest, dx;
    }
    return rest;
}

short ASM::Potencia(short Num1, short Num2)
{
    short shPotencia = Num1;
    _asm
    {
        mov cx, Num2;
        loop Pot;
    }
    Pot:_asm
    {
        mov ax, Num1;
        mov bx, shPotencia;
        mul bx;
        mov shPotencia, ax; 
    }
    return shPotencia;
}

short ASM::Raiz(short Num1)
{
    short RaizC = 0;
    _asm
    {
    siguiente:
        inc RaizC
            mov ax, RaizC
            mul ax
            cmp ax, Num1
            jbe siguiente;
        dec RaizC
    }
    return RaizC;
}
