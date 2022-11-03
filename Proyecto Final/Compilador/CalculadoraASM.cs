using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Analizador_Lexico__Traductor_
{
    internal class CalculadoraASM
    {
        //ATRIBUTOS//
        
        //AX//
        private short AH, AL;

        //BX//
        private short BH, BL;

        //CX//
        private short CH, CL;

        //DX//
        private short DH, DL;

        //RESULTADO//
        private string Variables;
        private string Operacion;
        private short Resultado;
        IntPtr ASMOp = Create();

        public CalculadoraASM()
        {
            AH = 0;AL = 0;
            BH = 0;AL = 0;
            CH = 0;CL = 0;
            DH = 0;DL = 0;
            Variables = "";
            Operacion = "";
            Resultado = 0;
            
        }

        //METODOS//
        [DllImport("Operaciones.dll")]
        public static extern IntPtr Create();
        [DllImport("Operaciones.dll")]
        public static extern short SumaASM(IntPtr NewAsm, short Num1, short Num2);
        [DllImport("Operaciones.dll")]
        public static extern short RestaASM(IntPtr NewAsm, short Num1, short Num2);
        [DllImport("Operaciones.dll")]
        public static extern short MultiASM(IntPtr NewAsm, short Num1, short Num2);
        [DllImport("Operaciones.dll")]
        public static extern short DivASM(IntPtr NewAsm, short Num1, short Num2);
        [DllImport("Operaciones.dll")]
        public static extern short ModASM(IntPtr NewAsm, short Num1, short Num2);
        [DllImport("Operaciones.dll")]
        public static extern short PotASM(IntPtr NewAsm, short Num1, short Num2);

        [DllImport("Operaciones.dll")]
        public static extern short RaizASM(IntPtr NewAsm, short Num1);

        //SETTERS//
        public void SetAH(short New_AH)
        {
            AH = New_AH;
        }
        public void SetAL(short New_AL)
        {
            AL = New_AL;
        }

        public void SetBH(short New_BH)
        {
            BH = New_BH;
        }
        public void SetBL(short New_BL)
        {
            BL = New_BL;
        }

        public void SetCH(short New_CH)
        {
            CH = New_CH;
        }
        public void SetCL(short New_CL)
        {
            CL = New_CL;
        }

        public void SetDH(short New_DH)
        {
            DH = New_DH;
        }
        public void SetDL(short New_DL)
        {
            DL = New_DL;
        }

        public void SetResultado(short New_Resultado)
        {
            Resultado = New_Resultado;
        }

        public void SetOperacion(string New_String)
        {
            Operacion += New_String;
        }

        public void SetRegistros(string New_String)
        {
            Variables += New_String;
        }

        public void AnalizarOperacion()
        {
            string NewOperacion = Operacion;
            //MOV//
            if(NewOperacion[0]=='m')
            {
                if(NewOperacion[1] == 'o')
                {
                    if(NewOperacion[2] == 'v')
                    {
                        //OBTENER REGISTRO//
                        string Registro = "";
                        Registro += NewOperacion[4];
                        Registro += NewOperacion[5];

                        //OBTENER VALOR//
                        string Valor = "";
                        for(int i=7; i<NewOperacion.Length-1;i++)
                        {
                            Valor += NewOperacion[i];
                        }
                        
                        int decAgain = int.Parse(Valor, System.Globalization.NumberStyles.HexNumber);

                        //GUARDAR VALOR//
                        if (Registro == "ax") AH = Convert.ToInt16(decAgain);
                        else if (Registro == "bx") BH = Convert.ToInt16(decAgain);
                        else if (Registro == "cx") CH = Convert.ToInt16(decAgain);
                        else if (Registro == "dx") DH = Convert.ToInt16(decAgain);
                        else if (Registro == "al") AL = Convert.ToInt16(decAgain);
                        else if (Registro == "bl") BL = Convert.ToInt16(decAgain);
                        else if (Registro == "cl") CL = Convert.ToInt16(decAgain);
                        else if (Registro == "dl") DL = Convert.ToInt16(decAgain);

                        Operacion = "mov " + Registro + " " + decAgain.ToString();
                        Variables = Registro;
                        Resultado = Convert.ToInt16(decAgain);

                    }
                }
            }
            //ADD//
            if (NewOperacion[0] == 'a')
            {
                if (NewOperacion[1] == 'd')
                {
                    if (NewOperacion[2] == 'd')
                    {
                        //OBTENER REGISTRO//
                        string Registro = "";
                        int Contador = 4;
                        for(int i = 4; NewOperacion[i]!=' '; i++)
                        {
                            Registro += NewOperacion[i];
                            Contador++;
                        }
                        if (Registro == "") Registro = "0";

                        //OBTENER VALOR//
                        string Valor = "";
                        for (int i = Contador; i < NewOperacion.Length; i++)
                        {
                            Valor += NewOperacion[i];
                        }
                        int decAgain = int.Parse(Valor, System.Globalization.NumberStyles.HexNumber);
                        
                        //GUARDAR VALOR//
                        if (Registro == "ax") AH = Convert.ToInt16(decAgain);
                        else if (Registro == "bx") BH = Convert.ToInt16(decAgain); 
                        else if (Registro == "cx") CH = Convert.ToInt16(decAgain);
                        else if (Registro == "dx") DH = Convert.ToInt16(decAgain);
                        else if (Registro == "al") AL = Convert.ToInt16(decAgain);
                        else if (Registro == "bl") BL = Convert.ToInt16(decAgain);
                        else if (Registro == "cl") CL = Convert.ToInt16(decAgain);
                        else if (Registro == "dl") DL = Convert.ToInt16(decAgain);                      

                        short sResultado = (SumaASM(ASMOp, Convert.ToInt16(Registro), Convert.ToInt16(decAgain))); 
                        
                        Resultado= Convert.ToInt16(sResultado);                        

                        Operacion = "add " + Convert.ToInt16(Registro) + " " + decAgain;
                    }
                }
            }
            //SUB//
            if (NewOperacion[0] == 's')
            {
                if (NewOperacion[1] == 'u')
                {
                    if (NewOperacion[2] == 'b')
                    {
                        //OBTENER REGISTRO//
                        string Registro = "";
                        int Contador = 4;
                        for (int i = 4; NewOperacion[i] != ' '; i++)
                        {
                            Registro += NewOperacion[i];
                            Contador++;
                        }
                        if (Registro == "") Registro = "0";

                        //OBTENER VALOR//
                        string Valor = "";
                        for (int i = Contador; i < NewOperacion.Length; i++)
                        {
                            Valor += NewOperacion[i];
                        }
                        int decAgain = int.Parse(Valor, System.Globalization.NumberStyles.HexNumber);

                        //GUARDAR VALOR//
                        if (Registro == "ax")
                        {
                            Variables += "ax";
                            AH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "bx")
                        {
                            Variables += "bx";
                            BH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "cx") 
                        {
                            Variables += "cx";
                            CH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "dx") 
                        {
                            Variables += "dx";
                            DH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "al") 
                        {
                            Variables += "al";
                            AL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "bl") 
                        {
                            Variables += "bl";
                            BL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "cl") 
                        {
                            Variables += "cl";
                            CL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "dl") 
                        {
                            Variables += "dl";
                            DL = Convert.ToInt16(decAgain);
                        }

                        short sResultado = (RestaASM(ASMOp, Convert.ToInt16(Registro), Convert.ToInt16(decAgain)));

                        Resultado = Convert.ToInt16(sResultado);

                        Operacion = "sub " + Convert.ToInt16(Registro) + " " + decAgain;
                    }
                }
            }
            //MUL//
            if (NewOperacion[0] == 'm')
            {
                if (NewOperacion[1] == 'u')
                {
                    if (NewOperacion[2] == 'l')
                    {                       
                        //OBTENER REGISTRO//
                        string Registro = "";
                        int Contador = 4;
                        for (int i = 4; NewOperacion[i] != ' '; i++)
                        {
                            Registro += NewOperacion[i];
                            Contador++;
                        }
                        if (Registro == "") Registro = "0";                       
                        //OBTENER VALOR//
                        string Valor = "";
                        for (int i = Contador; i < NewOperacion.Length; i++)
                        {
                            Valor += NewOperacion[i];
                        }
                        int decAgain = int.Parse(Valor, System.Globalization.NumberStyles.HexNumber);

                        //GUARDAR VALOR//
                        if (Registro == "ax")
                        {
                            Variables += "ax";
                            AH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "bx")
                        {
                            Variables += "bx";
                            BH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "cx")
                        {
                            Variables += "cx";
                            CH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "dx")
                        {
                            Variables += "dx";
                            DH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "al")
                        {
                            Variables += "al";
                            AL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "bl")
                        {
                            Variables += "bl";
                            BL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "cl")
                        {
                            Variables += "cl";
                            CL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "dl")
                        {
                            Variables += "dl";
                            DL = Convert.ToInt16(decAgain);
                        }

                        short sResultado = (MultiASM(ASMOp, Convert.ToInt16(Registro), Convert.ToInt16(decAgain)));

                        Resultado = Convert.ToInt16(sResultado);

                        Operacion = "mul " + Convert.ToInt16(Registro) + " " + decAgain;
                    }
                }
            }
            //DIV//
            if (NewOperacion[0] == 'd')
            {
                if (NewOperacion[1] == 'i')
                {
                    if (NewOperacion[2] == 'v')
                    {
                        //OBTENER REGISTRO//
                        string Registro = "";
                        int Contador = 4;
                        for (int i = 4; NewOperacion[i] != ' '; i++)
                        {
                            Registro += NewOperacion[i];
                            Contador++;
                        }
                        if (Registro == "") Registro = "0";

                        //OBTENER VALOR//
                        string Valor = "";
                        for (int i = Contador; i < NewOperacion.Length; i++)
                        {
                            Valor += NewOperacion[i];
                        }
                        int decAgain = int.Parse(Valor, System.Globalization.NumberStyles.HexNumber);

                        //GUARDAR VALOR//
                        if (Registro == "ax")
                        {
                            Variables += "ax";
                            AH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "bx")
                        {
                            Variables += "bx";
                            BH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "cx")
                        {
                            Variables += "cx";
                            CH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "dx")
                        {
                            Variables += "dx";
                            DH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "al")
                        {
                            Variables += "al";
                            AL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "bl")
                        {
                            Variables += "bl";
                            BL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "cl")
                        {
                            Variables += "cl";
                            CL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "dl")
                        {
                            Variables += "dl";
                            DL = Convert.ToInt16(decAgain);
                        }

                        short sResultado = (DivASM(ASMOp, Convert.ToInt16(Registro), Convert.ToInt16(decAgain)));

                        Resultado = Convert.ToInt16(sResultado);

                        Operacion = "div " + Convert.ToInt16(Registro) + " " + decAgain;
                    }
                }
            }
            //MOD//
            if (NewOperacion[0] == 'm')
            {
                if (NewOperacion[1] == 'o')
                {
                    if (NewOperacion[2] == 'd')
                    {
                        //OBTENER REGISTRO//
                        string Registro = "";
                        int Contador = 4;
                        for (int i = 4; NewOperacion[i] != ' '; i++)
                        {
                            Registro += NewOperacion[i];
                            Contador++;
                        }
                        if (Registro == "") Registro = "0";

                        //OBTENER VALOR//
                        string Valor = "";
                        for (int i = Contador; i < NewOperacion.Length; i++)
                        {
                            Valor += NewOperacion[i];
                        }
                        int decAgain = int.Parse(Valor, System.Globalization.NumberStyles.HexNumber);

                        //GUARDAR VALOR//
                        if (Registro == "ax")
                        {
                            Variables += "ax";
                            AH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "bx")
                        {
                            Variables += "bx";
                            BH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "cx")
                        {
                            Variables += "cx";
                            CH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "dx")
                        {
                            Variables += "dx";
                            DH = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "al")
                        {
                            Variables += "al";
                            AL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "bl")
                        {
                            Variables += "bl";
                            BL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "cl")
                        {
                            Variables += "cl";
                            CL = Convert.ToInt16(decAgain);
                        }
                        else if (Registro == "dl")
                        {
                            Variables += "dl";
                            DL = Convert.ToInt16(decAgain);
                        }

                        short sResultado = (ModASM(ASMOp, Convert.ToInt16(Registro), Convert.ToInt16(decAgain)));

                        Resultado = Convert.ToInt16(sResultado);

                        Operacion = "mod " + Convert.ToInt16(Registro) + " " + decAgain;
                    }
                }
            }
        }

        //GETTERS//
        public string Registros { get { return Variables; } }
        public string Operaciones { get { return Operacion; } }
        public short Resultados { get { return Resultado; } }

        public short GetAH() { return AH; }
        public short GetBH() { return BH; }
        public short GetCH() { return CH; }
        public short GetDH() { return DH; }
        public short GetAL() { return AL; }
        public short GetBL() { return BL; }
        public short GetCL() { return CL; }
        public short GetDL() { return DL; }
    }
}
