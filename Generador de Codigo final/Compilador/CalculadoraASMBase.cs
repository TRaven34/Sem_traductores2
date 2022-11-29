using System.Runtime.InteropServices;

namespace Analizador_Lexico__Traductor_
{
    internal class CalculadoraASMBase
    {
        IntPtr ASMOp = Create();

        //METODOS//
        [DllImport("Operaciones.dll")]
        public static extern IntPtr Create();
    }
}