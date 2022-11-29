//OBJETO ASM//
class ASM
{
	public:
		ASM();
		short Suma(short Num1, short Num2);
		short Resta(short Num1, short Num2);
		short Multiplicacion(short Num1, short Num2);
		short Division(short Num1, short Num2);
		short Modulo(short Num1, short Num2);
		short Potencia(short Num1, short Num2);
		short Raiz(short Num1);
};

//CONSTRUCTOR//
extern "C" _declspec(dllexport) void * Create()
{
	return (void*) new ASM();
}

//SUMA//
extern "C" _declspec(dllexport) short SumaASM(ASM* NewAsm,short Num1, short Num2)
{
	return NewAsm->Suma(Num1, Num2);
}

//RESTA//
extern "C" _declspec(dllexport) short RestaASM(ASM * NewAsm, short Num1, short Num2)
{
	return NewAsm->Resta(Num1, Num2);
}

//MULTIPLICACION//
extern "C" _declspec(dllexport) short MultiASM(ASM * NewAsm, short Num1, short Num2)
{
	return NewAsm->Multiplicacion(Num1, Num2);
}

//DIVISION//
extern "C" _declspec(dllexport) short DivASM(ASM * NewAsm, short Num1, short Num2)
{
	return NewAsm->Division(Num1, Num2);
}

//MODULO//
extern "C" _declspec(dllexport) short ModASM(ASM * NewAsm, short Num1, short Num2)
{
	return NewAsm->Modulo(Num1, Num2);
}

//POTENCIA//
extern "C" _declspec(dllexport) short PotASM(ASM * NewAsm, short Num1, short Num2)
{
	return NewAsm->Potencia(Num1, Num2);
}

//RAIZ//
extern "C" _declspec(dllexport) short RaizASM(ASM * NewAsm, short Num1)
{
	return NewAsm->Raiz(Num1);
}

