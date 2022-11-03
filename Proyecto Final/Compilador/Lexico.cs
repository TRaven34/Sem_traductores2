﻿using System.Runtime.InteropServices;

namespace Analizador_Lexico__Traductor_
{
    internal class Lexico
    {
        //ATRIBUTOS//
        private string StringAux;
        private int State;
        private List<Token> LTokens;
        private List<Token> Instrucciones;
        public List<Error> Errors;
        public List<Variables> LVariables;
        private List<CalculadoraASM> Calculadora;

        //CONTADORES SINTACTICOS//
        public int CFor;
        public int CParentesis;
        public int CCorchetes;
        public bool IF;
        public bool SWITCH;
        public bool Cad;
        public bool IsASMActive;

        //CONSTRUCTOR//
        public Lexico() { }


        //ANALIZADOR LEXICO//

        //METODOS//
        public List<Token> Scan(string Entry)
        {
            //INICIALIZAR//
            Entry += ' ';
            char Character;
            LTokens = new List<Token>();
            Instrucciones = new List<Token>();
            Errors = new List<Error>();
            LVariables = new List<Variables>();
            Calculadora = new List<CalculadoraASM>();
            State = 0;
            StringAux = "";
            bool FlagNum = false, FlagLetter = false, IsASM = false;

            CFor = 0;
            CParentesis = 0;
            CCorchetes = 0;
            IF = false;
            SWITCH = false;
            Cad = false;
            IsASMActive = false;
            //RECORRER EL CODIGO//
            for (int i = 0; i < Entry.Length; i++)
            {
                Character = Entry[i];
                //SWITCH DE ESTADOS// 
                switch (State)
                {
                    case 0:

                        //EVALUAR SI ES UNA LETRA O NUMERO//
                        if (char.IsLetter(Character) || char.IsDigit(Character))
                        {
                            State = 1;
                            StringAux += Character;

                            if (char.IsDigit(Character))
                            {
                                FlagNum = true;
                            }
                            if (char.IsLetter(Character))
                            {
                                FlagLetter = true;
                            }
                        }
                        else if (Character == '"')
                        {
                            StringAux += Character;
                            Cad = true;
                            State = 2;

                        }

                        //SALTO DE LINEA O ;//
                        else if (char.IsWhiteSpace(Character) || Character == ';')
                        {
                            if (Instrucciones.Count != 0)
                            {
                                Sintactico();
                                State = 0;
                            }
                        }

                        //CARACTER ESPECIAL//
                        else
                        {
                            State = 2;
                            StringAux += Character;
                        }
                        break;

                    //PALABRAS RESERVADAS
                    case 1:
                        string SAux = StringAux;
                        bool Flag = IsOperator(Character.ToString());

                        //EVALUAR SI ES UNA LETRA O NUMERO//                       
                        if (char.IsLetter(Character) || char.IsDigit(Character))
                        {
                            StringAux += Character;
                            if (char.IsDigit(Character))
                            {
                                FlagNum = true;
                            }
                            if (char.IsLetter(Character))
                            {
                                FlagLetter = true;
                            }
                        }

                        //FIN DE LA CAPTURA//                      
                        else if (IsSpace(Character) || Character == ';' || Flag == true)
                        {
                            //ES UNA VARIABLE O PALABRA RESERVADA//
                            if (FlagLetter == true && FlagNum == false)
                            {
                                //EVALUAR SI CORRESPONDE A UN TIPO DE DATO//
                                if (DataTypes(StringAux))
                                {
                                    State = 0;
                                    FlagLetter = false;
                                    FlagNum = false;
                                }
                                
                                //EVALUAR SI CORRESPONDE A UNA PALABRA RESERVADA//
                                else if (ResWord(StringAux))
                                {
                                    State = 0;
                                    FlagLetter = false;
                                    FlagNum = false;
                                }
                                else if (Flag == true)
                                {
                                    AddToken(SAux, "Identificador", "Identificador");
                                    FlagLetter = false;
                                    FlagNum = false;
                                }
                                
                                //SINO ES UNA VARIABLE//
                                else
                                {
                                    AddToken(StringAux, "Identificador", "Identificador");
                                    FlagLetter = false;
                                    FlagNum = false;
                                }
                            }

                            //ES UNA CONSTANTE//
                            else if (FlagNum == true && FlagLetter == false)
                            {
                                AddToken(StringAux, "Constante", "Constante");
                                FlagLetter = false;
                                FlagNum = false;
                            }

                            else if (Character == ';')
                            {
                                Sintactico();
                                State = 0;
                            }

                            //SINO ES UNA VARIABLE//
                            else
                            {
                                AddToken(StringAux, "Identificador", "Identificador");
                                FlagLetter = false;
                                FlagNum = false;
                            }

                        }
                        break;

                    //CARACTERES ESPECIALES//
                    case 2:
                        //CADENA//
                        if (IsASM == true)
                        {
                            if (Character == 'a' || Character == 's' || Character == 'm' || Character == 'A' || Character == 'S' || Character == 'M')
                            {
                                StringAux += Character;
                            }
                            else if (StringAux == "_asm")
                            {
                                AddToken(StringAux, "Instruccion ensamblador", "Declaracion Ensamblador");
                                IsASM = false;
                                IsASMActive = true;
                            }
                            else
                            {
                                Error error = new Error(StringAux, "Declaracion Ensamblador invalida.");
                                Errors.Add(error);
                            }
                        }

                        else if (Cad == true)
                        {
                            StringAux += Character;
                            if (Character == '"')
                            {
                                AddToken(StringAux, "Cadena", "Cadena");
                                Cad = false;
                            }
                        }

                        else if ((StringAux[0] == '_'))
                        {
                            StringAux += Character;
                            IsASM = true;
                        }

                        //EVALUAR SI ES UN CARACTER//
                        else if ((Character >= '!' && Character <= ')') || (Character == '|') || (Character == '+') || (Character == '=') || (Character == '<') || (Character == '>') || (Character == ':'))
                        {
                            StringAux += Character;
                        }

                        //FIN DE LA CAPTURA//
                        else if (IsSpace(Character) || Character == ';')
                        {
                            //ES UN OPERADOR//
                            if (IsOperator(StringAux))
                            {
                                State = 0;
                            }

                            //ES UN CARACTER ESPECIAL//
                            else if (IsSpecial(StringAux))
                            {
                                State = 0;
                            }
                            else if (Character == ';')
                            {
                                Sintactico();
                                State = 0;
                            }

                            //CARACTER NO VALIDO//
                            else
                            {
                                AddToken(StringAux, "Caracter No Valido", "NULL");
                            }
                        }
                        break;
                }

            }

            if (CCorchetes != 0)
            {
                if (CCorchetes < 0)
                {
                    Error error = new Error("}", "Existen mas corchetes de cierre que de apertura.");
                    Errors.Add(error);
                }
                else
                {
                    Error error = new Error("{", "Existen mas corchetes de apertura que de cierre.");
                    Errors.Add(error);
                }

            }
            if (CParentesis != 0)
            {
                if (CParentesis < 0)
                {
                    Error error = new Error(")", "Existen mas parentesis de cierre que de apertura.");
                    Errors.Add(error);
                }
                else
                {
                    Error error = new Error("(", "Existen mas parentesis de apertura que de cierre.");
                    Errors.Add(error);
                }
            }
            return LTokens;


        }

        public void AddToken(string NewCharacter, string NewType, string GeneralType)
        {
            Token Token1 = new Token(NewCharacter, NewType, GeneralType);
            LTokens.Add(Token1);
            Instrucciones.Add(Token1);
            StringAux = "";
            State = 0;
        }

        //FUNCIONES DE VALIDACION//

        //CARACTERES//
        bool IsSpace(char NewC)
        {
            return char.IsWhiteSpace(NewC) || NewC.CompareTo('\t') == 0 || NewC.CompareTo('\n') == 0;
        }

        //OPERADORES
        bool IsOperator(string str)
        {
            if (str == "+")
            {
                AddToken(str, "Operador de Suma", "Operador");
                return true;
            }
            else if (str == "++")
            {
                AddToken(str, "Operador de Incremento", "Operador");
                return true;
            }
            else if (str == "-")
            {
                AddToken(str, "Operador de Resta", "Operador");
                return true;
            }
            else if (str == "*")
            {
                AddToken(str, "Operador de Multiplicacion", "Operador");
                return true;
            }
            else if (str == "/")
            {
                AddToken(str, "Operador de Division", "Operador");
                return true;
            }
            else if (str == "%")
            {
                AddToken(str, "Operador de Modulo", "Operador");
                return true;
            }
            else if (str == ">")
            {
                AddToken(str, "Operador de Comparación", "Operador");
                return true;
            }
            else if (str == "<")
            {
                AddToken(str, "Operador de Comparación", "Operador");
                return true;
            }
            else if (str == "=")
            {
                AddToken(str, "Operador de Asignación", "Operador");
                return true;
            }
            else if (str == "^")
            {
                AddToken(str, "Operador de Potencia", "Operador");
                return true;
            }
            else if (str == "&&")
            {
                AddToken(str, "Operador AND", "Operador");
                return true;
            }
            else if (str == "||")
            {
                AddToken(str, "Operador OR", "Operador");
                return true;
            }
            else if (str == "==")
            {
                AddToken(str, "Operador de Igualdad", "Operador");
                return true;
            }
            else if (str == "<=")
            {
                AddToken(str, "Operador Menor o Igual", "Operador");
                return true;
            }
            else if (str == ">=")
            {
                AddToken(str, "Operador Mayor o Igual", "Operador");
                return true;
            }
            else if (str == "!=")
            {
                AddToken(str, "Operador Diferente De", "Operador");
                return true;
            }
            else if (str == "--")
            {
                AddToken(str, "Operador de Decremento", "Operador");
                return true;
            }
            else if (str == "+=")
            {
                AddToken(str, "Igualdad e Incremento", "Operador");
                return true;
            }
            else if (str == "-=")
            {
                AddToken(str, "Igualdad y Decremento", "Operador");
                return true;
            }
            else if (str == "*=")
            {
                AddToken(str, "Igualdad y Multiplicacion", "Operador");
                return true;
            }
            else if (str == "/=")
            {
                AddToken(str, "Igualdad y Division", "Operador");
                return true;
            }
            else if (str == "<<")
            {
                AddToken(str, "Flujo de Salida", "Operador");
                return true;
            }
            else if (str == ">>")
            {
                AddToken(str, "Flujo de Entrada", "Operador");
                return true;
            }
            else if (str == "::")
            {
                AddToken(str, "Pertenencia de Metodos a una Clase", "Operador");
                return true;
            }
            else
            {
                return false;
            }
        }

        //TIPOS DE DATOS
        bool DataTypes(string str)
        {
            if (str == "int")
            {
                AddToken(str, "Tipo de dato Entero", "Tipo de Dato");
                return true;
            }
            else if (str == "float")
            {
                AddToken(str, "Tipo de dato Flotante", "Tipo de Dato");
                return true;
            }
            else if (str == "double")
            {
                AddToken(str, "Tipo de dato Doble Punto Flotante", "Tipo de Dato");
                return true;
            }
            else if (str == "bool")
            {
                AddToken(str, "Tipo de dato Booleano", "Tipo de Dato");
                return true;
            }
            else if (str == "string")
            {
                AddToken(str, "Tipo de dato Cadena", "Tipo de Dato");
                return true;
            }
            else if (str == "char")
            {
                AddToken(str, "Tipo de dato Caracter", "Tipo de Dato");
                return true;
            }
            else if (str == "long")
            {
                AddToken(str, "Tipo de dato Entero Largo", "Tipo de Dato");
                return true;
            }
            else if (str == "void")
            {
                AddToken(str, "Sin retorno", "Palabra Reservada");
                return true;
            }
            else if (str == "static")
            {
                AddToken(str, "Valor Estatico", "Palabra Reservada");
                return true;
            }
            else if (str == "const")
            {
                AddToken(str, "Valor Constante", "Palabra Reservada");
                return true;
            }
            else if (str == "null")
            {
                AddToken(str, "Dato Nulo", "Palabra Reservada");
                return true;
            }
            else if (str == "nullptr")
            {
                AddToken(str, "Puntero hacia nulo", "Palabra Reservada");
                return true;
            }
            else
            {
                return false;
            }
        }

        //PALABRAS RESERVADAS//
        bool ResWord(string str)
        {
            if (str == "for")
            {
                AddToken(str, "Ciclo For", "Palabra Reservada");
                return true;
            }
            else if (str == "if")
            {
                AddToken(str, "Afirmacion", "Palabra Reservada");
                IF = true;
                return true;
            }
            else if (str == "else")
            {
                AddToken(str, "Negacion", "Palabra Reservada");
                return true;
            }
            else if (str == "while")
            {
                AddToken(str, "Ciclo While", "Palabra Reservada");
                return true;
            }
            else if (str == "do")
            {
                AddToken(str, "Ciclo Do", "Palabra Reservada");
                return true;
            }
            else if (str == "return")
            {
                AddToken(str, "Retorno", "Palabra Reservada");
                return true;
            }
            else if (str == "class")
            {
                AddToken(str, "Clase", "Palabra Reservada");
                return true;
            }
            else if (str == "case")
            {
                AddToken(str, "Caso de un Switch", "Palabra Reservada");
                return true;
            }
            else if (str == "switch")
            {
                AddToken(str, "Switch", "Palabra Reservada");
                return true;
            }
            else if (str == "private")
            {
                AddToken(str, "Metodo de acceso Privado", "Palabra Reservada");
                return true;
            }
            else if (str == "public")
            {
                AddToken(str, "Metodo de acceso Publico", "Palabra Reservada");
                return true;
            }
            else if (str == "protected")
            {
                AddToken(str, "Metodo de acceso Protegido", "Palabra Reservada");
                return true;
            }
            else if (str == "cout")
            {
                AddToken(str, "Flujo de Salida", "Palabra Reservada");
                return true;
            }
            else if (str == "cin")
            {
                AddToken(str, "Flujo de Entrada", "Palabra Reservada");
                return true;
            }
            else if (str == "getline")
            {
                AddToken(str, "Lectura de una linea de caracteres", "Palabra Reservada");
                return true;
            }
            else if (str == "break")
            {
                AddToken(str, "Ruptura de un Ciclo Repetitivo", "Palabra Reservada");
                return true;
            }
            else if (str == "default")
            {
                AddToken(str, "Opcion por defecto en un Switch", "Palabra Reservada");
                return true;
            }
            else if (str == "false")
            {
                AddToken(str, "Valor falso", "Palabra Reservada");
                return true;
            }
            else if (str == "true")
            {
                AddToken(str, "Valor verdadera", "Palabra Reservada");
                return true;
            }
            else if (str == "operator")
            {
                AddToken(str, "Sobrecarga de un Operador", "Palabra Reservada");
                return true;
            }
            else if (str == "this")
            {
                AddToken(str, "Puntero al objeto contenido", "Palabra Reservada");
                return true;
            }
            else if (str == "try")
            {
                AddToken(str, "Evaluar una o mas expresiones", "Palabra Reservada");
                return true;
            }
            else if (str == "catch")
            {
                AddToken(str, "Interceptar una interrupcion", "Palabra Reservada");
                return true;
            }
            else if (str == "new")
            {
                AddToken(str, "Reserva de memoria", "Palabra Reservada");
                return true;
            }
            else if (str == "define")
            {
                AddToken(str, "Definicion", "Palabra Reservada");
                return true;
            }
            else if (str == "include")
            {
                AddToken(str, "Integracion al archivo actual", "Palabra Reservada");
                return true;
            }
            else if (str == "main")
            {
                AddToken(str, "Funcion principal", "Palabra Reservada");
                return true;
            }
            else if (str == "endl")
            {
                AddToken(str, "Salto de Linea", "Palabra Reservada");
                return true;
            }

            else if (str == "add")
            {
                AddToken(str, "Suma Ensamblador", "Ensamblador");
                return true;
            }
            else if (str == "sub")
            {
                AddToken(str, "Resta Ensamblador", "Ensamblador");
                return true;
            }
            else if (str == "mul")
            {
                AddToken(str, "Multiplicacion Ensamblador", "Ensamblador");
                return true;
            }
            else if (str == "div")
            {
                AddToken(str, "Division Ensamblador", "Ensamblador");
                return true;
            }
            else if (str == "pot")
            {
                AddToken(str, "Potencia Ensamblador", "Ensamblador");
                return true;
            }
            else if (str == "mod")
            {
                AddToken(str, "Modulo Ensamblador", "Ensamblador");
                return true;
            }
            else if (str == "mov")
            {
                AddToken(str, "Movimiento Ensamblador", "Ensamblador");
                return true;
            }
            else
            {
                return false;
            }
        }

        //CARACTER ESPECIAL//
        bool IsSpecial(string str)
        {
            if (str == "(")
            {
                AddToken(str, "Parentesis de Apertura", "Caracter");
                CParentesis++;
                return true;
            }
            else if (str == "&")
            {
                AddToken(str, "Referencia", "Caracter");
                return true;
            }
            else if (str == ")")
            {
                AddToken(str, "Parentesis de Cierre", "Caracter");
                CParentesis--;
                return true;
            }
            else if (str == "{")
            {
                AddToken(str, "Corchete de Apertura", "Caracter");
                CCorchetes++;
                return true;
            }
            else if (str == "}")
            {
                AddToken(str, "Corchete de Cierre", "Caracter");
                CCorchetes--;
                return true;
            }
            else if (str == ":")
            {
                AddToken(str, "Caracter de atributos de Clase", "Caracter");
                return true;
            }
            else if (str == "[")
            {
                AddToken(str, "Apertura de Accceso a Memoria", "Caracter");
                return true;
            }
            else if (str == "]")
            {
                AddToken(str, "Cierre de Acceso a Memoria", "Caracter");
                return true;
            }
            else
            {
                return false;
            }
        }


        //ANALIZADOR SINTACTICO//

        //TIPO DE ARBOL//
        void Sintactico()
        {
            if (Instrucciones[0].Caracteres == "{" || Instrucciones[0].Caracteres == "}" || Instrucciones[0].Caracteres == "_asm")
            {
                Instrucciones.Clear();
            }
            else if (Instrucciones[0].General() == "Tipo de Dato" && CFor == 0)
            {
                bool Flag = VarOrConst(Instrucciones);
                if (Flag == false)
                {
                    string Cadena = "";
                    for (int i = 0; i < Instrucciones.Count; i++)
                    {
                        Cadena += Instrucciones[i].Caracteres + " ";
                    }
                    Error error = new Error(Cadena, "La declaracion de un nuevo Tipo de Dato no es correcta");
                    Errors.Add(error);
                    Instrucciones.Clear();
                }
                else
                {
                    Instrucciones.Clear();
                }
            }
            else if (Instrucciones[0].General() == "Identificador" && CFor == 0)
            {
                bool Flag = Var(Instrucciones);
                if (Flag == false)
                {
                    string Cadena = "";
                    for (int i = 0; i < Instrucciones.Count; i++)
                    {
                        Cadena += Instrucciones[i].Caracteres + " ";
                    }
                    Error error = new Error(Cadena, "El uso de la Variable no es correcto");
                    Errors.Add(error);
                    Instrucciones.Clear();
                }
                else
                {
                    Instrucciones.Clear();
                }
            }
            else if (Instrucciones[0].General() == "Palabra Reservada" || CFor > 0)
            {
                bool Flag = Pal(Instrucciones);
                if (Flag == false)
                {
                    string Cadena = "";
                    for (int i = 0; i < Instrucciones.Count; i++)
                    {
                        Cadena += Instrucciones[i].Caracteres + " ";
                    }
                    Error error = new Error(Cadena, "El uso de la palabra reservada no es valido");
                    Errors.Add(error);
                    Instrucciones.Clear();
                }
                else
                {
                    Instrucciones.Clear();
                }
            }
            else if (Instrucciones[0].General() == "Ensamblador")
            {
                bool Flag = Ens(Instrucciones);
                if (Flag == false)
                {
                    string Cadena = "";
                    for (int i = 0; i < Instrucciones.Count; i++)
                    {
                        Cadena += Instrucciones[i].Caracteres + " ";
                    }
                    Error error = new Error(Cadena, "El uso de ensamblador no es valido");
                    Errors.Add(error);
                    Instrucciones.Clear();
                }
                else
                {
                    Instrucciones.Clear();
                }
            }

            // ARBOL 1: DECLARAR VARIABLES Y CONSTANTES //
            bool VarOrConst(List<Token> LToken)
            {
                //SI ES UN TIPO DE DATO//
                if (LToken[0].General() == "Tipo de Dato")
                {
                    if (LToken[1].Caracteres == "main")
                    {
                        return true;
                    }
                    //SI ES UNA VARIABLE//
                    if (LToken[1].General() == "Identificador")
                    {
                        //INTRODUCIR VARIABLES//
                        Variables Var = new Variables(LToken[1].Caracteres);
                        Var.SetTipo(LToken[0].Tipos);
                        bool Inside = false;
                        for (int i = 0; i < LVariables.Count; i++)
                        {
                            if (LVariables[i].Identificadores == Var.Identificador)
                            {
                                Inside = true;
                            }
                        }
                        if (Inside == true)
                        {
                            bool In = false;
                            for (int s = 0; s < Errors.Count; s++)
                            {
                                if (Errors[s].Instruccion == Var.Identificador)
                                {
                                    In = true;
                                }
                            }
                            if (In == false)
                            {
                                Error NewError = new Error(Var.Identificador, "La variable esta declarada mas de una vez");
                                Errors.Add(NewError);
                            }
                        }
                        else
                        {
                            LVariables.Add(Var);
                        }
                        //SI ES EL FIN DE LA SENTENCIA//
                        if (LToken.Count == 2)
                        {
                            return true;
                        }
                        //SI TIENE UN OPERADOR//
                        else if (LToken[2].General() == "Operador")
                        {
                            for (int c = 0; c < LVariables.Count; c++)
                            {
                                if (LVariables[c].Identificadores == LToken[1].Caracteres)
                                {
                                    LVariables[c].SetValor();
                                    LVariables[c].SetUsada();
                                }
                            }
                            bool Flag = false; int i = 3;
                            while (i < LToken.Count)
                            {
                                if ((LToken[i].General() == "Identificador" && i % 2 != 0) || (LToken[i].General() == "Constante" && i % 2 != 0) || (LToken[i].General() == "Cadena" && i % 2 != 0))
                                {
                                    Var.SetValorNum(LToken[i].Caracteres);
                                    if (LToken[i].General() == "Cadena")
                                    {
                                        if (Var.Tipos != "Tipo de dato Cadena")
                                        {
                                            Error NewError = new Error(Var.Identificador, "No se puede transformar la cadena " + LToken[i].Caracteres + " a tipo " + Var.Tipos);
                                            Errors.Add(NewError);
                                        }
                                    }
                                    else if (LToken[i].General() == "Identificador")
                                    {
                                        //INTRODUCIR VARIABLES//
                                        Variables Var2 = new Variables(LToken[i].Caracteres);

                                        bool Declarada = false;
                                        for (int c = 0; c < LVariables.Count; c++)
                                        {
                                            if (LVariables[c].Identificadores == Var2.Identificador)
                                            {
                                                Declarada = true;
                                                Var2.SetTipo(LVariables[c].Tipos);
                                                if (LVariables[c].GetValor() == false)
                                                {
                                                    Error NewError = new Error(Var2.Identificador, "La variable no tiene un valor asignado");
                                                    Errors.Add(NewError);
                                                }
                                                LVariables[c].SetUsada();
                                            }
                                        }
                                        if (Declarada == false)
                                        {
                                            Error NewError = new Error(Var2.Identificador, "La variable no esta declarada");
                                            Errors.Add(NewError);
                                        }
                                        if (Var.Tipos != Var2.Tipos)
                                        {
                                            Error NewError = new Error(Var.Identificador, "No se puede transformar la variable " + Var2.Identificador + " de tipo " + Var2.Tipos + " a tipo " + Var.Tipos);
                                            Errors.Add(NewError);
                                        }

                                    }

                                    Flag = true;
                                }
                                else if ((LToken[i].General() == "Operador" && i % 2 == 0))
                                {
                                    if (i == LToken.Count - 1)
                                    {
                                        Flag = false;
                                    }
                                    else
                                    {

                                        Flag = true;
                                    }
                                }
                                else if ((LToken[i].General() == "Caracter" && LToken[i].Caracteres == "("))
                                {
                                    bool Sign = false;
                                    while (Sign != true)
                                    {
                                        if (i == LToken.Count)
                                        {
                                            return false;
                                        }
                                        if ((LToken[i].General() == "Identificador" && i % 2 == 0) || (LToken[i].General() == "Constante" && i % 2 == 0))
                                        {
                                            if (LToken[i].General() == "Identificador")
                                            {
                                                //INTRODUCIR VARIABLES//
                                                Variables Var2 = new Variables(LToken[i].Caracteres);
                                                Var2.SetTipo(LToken[i].Tipos);
                                                bool Declarada = false;
                                                bool Usada = false;
                                                for (int c = 0; c < LVariables.Count; c++)
                                                {
                                                    if (LVariables[c].Identificadores == Var2.Identificador)
                                                    {
                                                        Declarada = true;
                                                        if (LVariables[c].GetValor() == true)
                                                        {
                                                            Usada = true; ;
                                                        }
                                                        LVariables[c].SetUsada();
                                                    }
                                                }
                                                if (Declarada == false)
                                                {
                                                    Error NewError = new Error(Var2.Identificador, "La variable no esta declarada");
                                                    Errors.Add(NewError);
                                                }
                                                else if (Usada == false)
                                                {
                                                    Error NewError = new Error(Var2.Identificador, "La variable no tiene un valor asignado");
                                                    Errors.Add(NewError);
                                                }
                                            }
                                            Flag = true;
                                        }
                                        else if ((LToken[i].General() == "Operador" && i % 2 != 0))
                                        {
                                            if (i == LToken.Count - 1)
                                            {
                                                Flag = false;
                                            }
                                            else
                                            {
                                                Flag = true;
                                            }
                                        }
                                        else if ((LToken[i].General() == "Caracter" && LToken[i].Caracteres == ")"))
                                        {
                                            Sign = true;
                                        }


                                        i++;
                                    }
                                    if (Flag == false)
                                    {
                                        return Flag;
                                    }
                                }

                                else if ((LToken[i].General() == "Operador" && i % 2 == 0))
                                {
                                    if (i == LToken.Count - 1)
                                    {
                                        Flag = false;
                                    }
                                    else
                                    {
                                        Flag = true;
                                    }
                                }

                                else
                                {
                                    Flag = false;
                                }
                                i++;
                            }
                            return Flag;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }

            // ARBOL 2: PALABRAS RESERVADAS //
            bool Pal(List<Token> LToken)
            {
                if (LToken[0].General() == "Palabra Reservada" || CFor != 0)
                {
                    // IF //
                    if (LToken[0].Caracteres == "if")
                    {
                        if (LToken[1].Caracteres == "(")
                        {
                            // IF SIMPLE //
                            if (LToken[2].General() == "Identificador")
                            {
                                Variables Var = new Variables(LToken[2].Caracteres);
                                for (int i = 0; i < LVariables.Count; i++)
                                {
                                    if (LVariables[i].Identificador == LToken[2].Caracteres)
                                    {
                                        Var.SetTipo(LVariables[i].Tipo);
                                    }
                                }
                                ValidarVar(LToken[2]);
                                if (LToken[3].General() == "Operador")
                                {
                                    if (LToken[4].General() == "Identificador" || LToken[4].General() == "Constante" || LToken[4].General() == "Cadena")
                                    {
                                        bool Declarada = false;
                                        for (int c = 0; c < LVariables.Count; c++)
                                        {
                                            if (LVariables[c].Identificadores == LToken[4].Caracteres)
                                            {
                                                Declarada = true;
                                                if (LVariables[c].GetValor() == false)
                                                {
                                                    Error NewError = new Error(LToken[4].Caracteres, "La variable no tiene un valor asignado");
                                                    Errors.Add(NewError);
                                                }
                                                LVariables[c].SetUsada();
                                                if (LVariables[c].Tipos != Var.Tipos)
                                                {
                                                    Error NewError = new Error(LVariables[c].Identificador, "La variable no puede ser comparada con la variable " + Var.Identificadores);
                                                    Errors.Add(NewError);
                                                }
                                            }
                                        }
                                        if (Declarada == false)
                                        {
                                            Error NewError = new Error(LToken[4].Caracteres, "La variable no esta declarada");
                                            Errors.Add(NewError);
                                        }
                                        if (LToken[4].General() == "Identificador")
                                        {
                                            ValidarVar(LToken[2]);
                                        }

                                        if (LToken[5].Caracteres == ")")
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else if (LToken[2].Caracteres == "(")
                            {
                                // IF COMPUESTO //
                                bool Flag = false;
                                int GeneralC = 2;
                                while (GeneralC < LToken.Count - 1)
                                {
                                    if (LToken[GeneralC].Caracteres == "(")
                                    {
                                        bool Count = false;
                                        Variables Var = new Variables();
                                        while (Count != true)
                                        {
                                            if ((LToken[GeneralC].General() == "Identificador" && GeneralC % 2 != 0) || (LToken[GeneralC].General() == "Constante" && GeneralC % 2 != 0) || (LToken[GeneralC].General() == "Cadena" && GeneralC % 2 != 0))
                                            {


                                                if (LToken[GeneralC].General() == "Identificador")
                                                {
                                                    ValidarVar(LToken[2]);
                                                }
                                                Flag = true;
                                            }
                                            else if ((LToken[GeneralC].General() == "Operador" && GeneralC % 2 == 0))
                                            {
                                                if (GeneralC == LToken.Count - 1)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    Flag = true;
                                                }
                                            }
                                            else if (GeneralC == LToken.Count - 1)
                                            {
                                                return false;
                                            }
                                            else if ((LToken[GeneralC].General() == "Caracter" && LToken[GeneralC].Caracteres == ")"))
                                            {
                                                Count = true;
                                            }
                                            GeneralC++;
                                        }
                                    }
                                    else if (LToken[GeneralC].Caracteres == "&&" && GeneralC % 2 != 0)
                                    {
                                        Flag = true;
                                    }
                                    else if (LToken[GeneralC].Caracteres == "||" && GeneralC % 2 != 0)
                                    {
                                        Flag = true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                    GeneralC++;
                                }
                                return Flag;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //ELSE//                
                    if (LToken[0].Caracteres == "else")
                    {
                        if (IF == false)
                        {
                            return false;
                        }
                        if (LToken.Count == 1)
                        {
                            IF = false;
                            return true;
                        }
                        //ELSE IF SIMPLE//
                        else if (LToken[1].Caracteres == "if")
                        {
                            if (LToken[2].Caracteres == "(")
                            {
                                if (LToken[3].General() == "Identificador")
                                {
                                    ValidarVar(LToken[3]);
                                    if (LToken[4].General() == "Operador")
                                    {
                                        if (LToken[5].General() == "Identificador" || LToken[5].General() == "Constante")
                                        {
                                            if (LToken[5].General() == "Identificador")
                                            {
                                                ValidarVar(LToken[5]);
                                            }
                                            if (LToken[6].Caracteres == ")")
                                            {
                                                return true;
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else if (LToken[2].Caracteres == "(")
                                {
                                    // ELSE IF COMPUESTO //
                                    bool Flag = false;
                                    int GeneralC = 2;
                                    while (GeneralC < LToken.Count - 1)
                                    {
                                        if (LToken[GeneralC].Caracteres == "(")
                                        {
                                            bool Count = false;
                                            while (Count != true)
                                            {
                                                if ((LToken[GeneralC].General() == "Identificador" && GeneralC % 2 == 0) || (LToken[GeneralC].General() == "Constante" && GeneralC % 2 == 0))
                                                {
                                                    if (LToken[GeneralC].General() == "Identificador")
                                                    {
                                                        ValidarVar(LToken[GeneralC]);
                                                    }
                                                    Flag = true;
                                                }
                                                else if ((LToken[GeneralC].General() == "Operador" && GeneralC % 2 != 0))
                                                {
                                                    if (GeneralC == LToken.Count - 1)
                                                    {
                                                        return false;
                                                    }
                                                    else
                                                    {
                                                        Flag = true;
                                                    }
                                                }
                                                else if (GeneralC == LToken.Count - 1)
                                                {
                                                    return false;
                                                }
                                                else if ((LToken[GeneralC].General() == "Caracter" && LToken[GeneralC].Caracteres == ")"))
                                                {
                                                    Count = true;
                                                }
                                                GeneralC++;
                                            }
                                        }
                                        else if (LToken[GeneralC].Caracteres == "&&" && GeneralC % 2 == 0)
                                        {
                                            Flag = true;
                                        }
                                        else if (LToken[GeneralC].Caracteres == "||" && GeneralC % 2 == 0)
                                        {
                                            Flag = true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                        GeneralC++;
                                    }
                                    return Flag;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    // FOR //
                    if (LToken[0].Caracteres == "for" || CFor != 0)
                    {
                        // PARTE 1//
                        if (CFor == 0)
                        {

                            if (LToken[1].Caracteres == "(")
                            {
                                if (LToken[2].General() == "Tipo de Dato")
                                {
                                    if (LToken[3].General() == "Identificador")
                                    {
                                        //INTRODUCIR VARIABLES//
                                        Variables Var = new Variables(LToken[3].Caracteres);
                                        Var.SetTipo(LToken[3].Tipos);
                                        bool Inside = false;
                                        for (int i = 0; i < LVariables.Count; i++)
                                        {
                                            if (LVariables[i].Identificadores == Var.Identificador)
                                            {
                                                Inside = true;
                                            }
                                        }
                                        if (Inside == true)
                                        {
                                            Error NewError = new Error(Var.Identificador, "La variable esta declarada mas de una vez");
                                            Errors.Add(NewError);
                                        }
                                        else
                                        {
                                            LVariables.Add(Var);
                                        }
                                        if (LToken[4].General() == "Operador")
                                        {
                                            LVariables[LVariables.Count - 1].SetUsada();
                                            if (LToken[5].General() == "Identificador" || LToken[5].General() == "Constante")
                                            {
                                                if (LToken[5].General() == "Identificador")
                                                {
                                                    ValidarVar(LToken[5]);
                                                }
                                                CFor++;
                                                return true;
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else if (LToken[2].General() == "Identificador")
                                {
                                    ValidarVar(LToken[2]);
                                    if (LToken[3].General() == "Operador" && LToken[3].Caracteres == "=")
                                    {
                                        if (LToken[4].General() == "Identificador" || LToken[4].General() == "Constante")
                                        {
                                            if (LToken[4].General() == "Identificador")
                                            {
                                                ValidarVar(LToken[4]);
                                            }
                                            CFor++;
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }

                        }
                        // PARTE 2 //
                        else if (CFor == 1)
                        {
                            if (LToken[0].General() == "Identificador")
                            {
                                ValidarVar(LToken[0]);
                                if (LToken[1].General() == "Operador")
                                {
                                    if (LToken[2].General() == "Identificador" || LToken[2].General() == "Constante")
                                    {
                                        if (LToken[2].General() == "Identificador")
                                        {
                                            ValidarVar(LToken[2]);
                                        }
                                        CFor++;
                                        return true;
                                    }
                                    else
                                    {
                                        return false;

                                    }

                                }
                                else
                                {

                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        // PARTE 3 //
                        else if (CFor == 2)
                        {
                            CFor = 0;
                            if (LToken[0].General() == "Identificador")
                            {
                                ValidarVar(LToken[0]);
                                if (LToken[1].General() == "Operador")
                                {
                                    if (LToken[2].General() == "Caracter" && LToken[2].Caracteres == ")")
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    //WHILE//                
                    if (LToken[0].Caracteres == "while")
                    {
                        if (LToken[1].Caracteres == "(")
                        {
                            // WHILE SIMPLE //
                            if (LToken[2].General() == "Identificador")
                            {
                                ValidarVar(LToken[2]);
                                if (LToken[3].General() == "Operador")
                                {
                                    if (LToken[4].General() == "Identificador" || LToken[4].General() == "Constante")
                                    {
                                        if (LToken[4].General() == "Identificador")
                                        {
                                            ValidarVar(LToken[4]);
                                        }

                                        if (LToken[5].Caracteres == ")")
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else if (LToken[2].Caracteres == "(")
                            {
                                // WHILE COMPUESTO //
                                bool Flag = false;
                                int GeneralC = 2;
                                while (GeneralC < LToken.Count - 1)
                                {
                                    if (LToken[GeneralC].Caracteres == "(")
                                    {
                                        bool Count = false;
                                        while (Count != true)
                                        {
                                            if ((LToken[GeneralC].General() == "Identificador" && GeneralC % 2 != 0) || (LToken[GeneralC].General() == "Constante" && GeneralC % 2 != 0))
                                            {
                                                if (LToken[GeneralC].General() == "Identificador")
                                                {
                                                    ValidarVar(LToken[GeneralC]);
                                                }
                                                Flag = true;
                                            }
                                            else if ((LToken[GeneralC].General() == "Operador" && GeneralC % 2 == 0))
                                            {
                                                if (GeneralC == LToken.Count - 1)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    Flag = true;
                                                }
                                            }
                                            else if (GeneralC == LToken.Count - 1)
                                            {
                                                return false;
                                            }
                                            else if ((LToken[GeneralC].General() == "Caracter" && LToken[GeneralC].Caracteres == ")"))
                                            {
                                                Count = true;
                                            }
                                            GeneralC++;
                                        }
                                    }
                                    else if (LToken[GeneralC].Caracteres == "&&" && GeneralC % 2 != 0)
                                    {
                                        Flag = true;
                                    }
                                    else if (LToken[GeneralC].Caracteres == "||" && GeneralC % 2 != 0)
                                    {
                                        Flag = true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                    GeneralC++;
                                }
                                return Flag;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //SWITCH//              
                    if (LToken[0].Caracteres == "switch")
                    {
                        if (LToken[1].Caracteres == "(")
                        {
                            if (LToken[2].General() == "Identificador")
                            {
                                ValidarVar(LToken[2]);
                                if (LToken[3].Caracteres == ")")
                                {
                                    if (LToken.Count == 4)
                                    {
                                        SWITCH = true;
                                        return true;
                                    }
                                    else
                                    {

                                        if (LToken[4].Caracteres == "{")
                                        {
                                            SWITCH = true;
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //CASE//
                    if (LToken[0].Caracteres == "case")
                    {
                        if (SWITCH == true)
                        {
                            int Num = 0;
                            bool FlagN = Int32.TryParse(LToken[1].Caracteres, out Num);
                            if (FlagN == true)
                            {
                                if (LToken.Count == 2)
                                {
                                    return true;
                                }
                                else if (LToken[2].Caracteres == ":")
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //DEFAULT//
                    if (LToken[0].Caracteres == "default")
                    {
                        if (SWITCH == true)
                        {
                            if (LToken.Count == 2)
                            {
                                return true;
                            }
                            else if (LToken[2].Caracteres == ":")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }

                    }
                    //COUT//
                    if (LToken[0].Caracteres == "cout")
                    {
                        if (LToken[1].Caracteres == "<<")
                        {
                            int i = 2;
                            bool Flag = false;
                            while (i < LToken.Count)
                            {
                                if ((LToken[i].General() == "Identificador" || LToken[i].General() == "Cadena") && i % 2 == 0)
                                {
                                    if (LToken[i].General() == "Identificador")
                                    {
                                        ValidarVar(LToken[i]);
                                    }
                                    Flag = true;
                                }
                                else if ((LToken[i].Caracteres == "endl") && i % 2 == 0)
                                {
                                    Flag = true;
                                }
                                else if ((LToken[i].Caracteres == "<<") && i % 2 != 0)
                                {
                                    Flag = true;
                                }
                                else
                                {
                                    return false;
                                }
                                i++;
                            }
                            return Flag;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //CIN//
                    if (LToken[0].Caracteres == "cin")
                    {
                        if (LToken[1].Caracteres == ">>")
                        {
                            int i = 2;
                            bool Flag = false;
                            while (i < LToken.Count)
                            {
                                if (LToken[i].General() == "Identificador" && i % 2 == 0)
                                {
                                    //INTRODUCIR VARIABLES//
                                    Variables Var2 = new Variables(LToken[i].Caracteres);
                                    Var2.SetTipo(LToken[i].Tipos);
                                    bool Declarada = false;
                                    for (int c = 0; c < LVariables.Count; c++)
                                    {
                                        if (LVariables[c].Identificadores == Var2.Identificador)
                                        {
                                            Declarada = true;
                                        }
                                    }
                                    if (Declarada == false)
                                    {
                                        Error NewError = new Error(Var2.Identificador, "La variable no esta declarada");
                                        Errors.Add(NewError);
                                    }
                                    Flag = true;
                                }
                                else if ((LToken[i].Caracteres == ">>") && i % 2 != 0)
                                {
                                    Flag = true;
                                }
                                else
                                {
                                    return false;
                                }
                                i++;
                            }
                            return Flag;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
                return false;
            }

            // ARBOL 3: VARIABLE //
            bool Var(List<Token> LToken)
            {
                Variables Var1 = new Variables();
                //SI ES UNA VARIABLE//
                if (LToken[0].General() == "Identificador")
                {
                    for (int c = 0; c < LVariables.Count; c++)
                    {
                        if (LVariables[c].Identificadores == LToken[0].Caracteres)
                        {
                            LVariables[c].SetValor();
                            LVariables[c].SetUsada();
                            Var1 = new Variables(LVariables[c].Identificador);
                            Var1.SetTipo(LVariables[c].Tipo);
                        }
                    }
                    ValidarVar(LToken[0]);
                    //SI TIENE UN OPERADOR//
                    if (LToken[1].General() == "Operador") // AGREGAR QUE OPERADORES //
                    {
                        bool Flag = false; int i = 2;
                        if (LToken[1].Caracteres == "++" || LToken[1].Caracteres == "--")
                        {
                            return true;
                        }
                        while (i < LToken.Count)
                        {
                            if ((LToken[i].General() == "Identificador" && i % 2 == 0) || (LToken[i].General() == "Constante" && i % 2 == 0) || (LToken[i].General() == "Cadena" && i % 2 == 0))
                            {
                                Var1.SetValorNum(LToken[i].Caracteres);
                                if (LToken[i].General() == "Cadena")
                                {
                                    if (Var1.Tipos != "Tipo de dato Cadena")
                                    {
                                        Error NewError = new Error(Var1.Identificador, "No se puede transformar la cadena " + LToken[i].Caracteres + " a tipo " + Var1.Tipos);
                                        Errors.Add(NewError);
                                    }
                                }
                                if (LToken[i].General() == "Identificador")
                                {
                                    Variables Var2 = new Variables();
                                    Var1.SetValorNum(LToken[i].Caracteres);
                                    for (int c = 0; c < LVariables.Count; c++)
                                    {
                                        if (LVariables[c].Identificadores == LToken[i].Caracteres)
                                        {
                                            LVariables[c].SetUsada();
                                            Var2 = new Variables(LVariables[c].Identificador);
                                            Var2.SetTipo(LVariables[c].Tipo);
                                        }
                                    }
                                    ValidarVar(LToken[i]);
                                    if (Var1.Tipos != Var2.Tipos)
                                    {
                                        Error NewError = new Error(Var1.Identificador, "No se puede transformar la variable " + Var2.Identificador + " de tipo " + Var2.Tipos + " a tipo " + Var1.Tipos);
                                        Errors.Add(NewError);
                                    }
                                }
                                Flag = true;

                            }
                            else if ((LToken[i].General() == "Operador" && i % 2 != 0))
                            {
                                if (i == LToken.Count - 1)
                                {
                                    Flag = false;

                                }
                                else
                                {
                                    Flag = true;
                                }
                            }
                            else if ((LToken[i].General() == "Caracter" && LToken[i].Caracteres == "("))
                            {
                                bool Sign = false;
                                while (Sign != true)
                                {

                                    if ((LToken[i].General() == "Identificador" && i % 2 != 0) || (LToken[i].General() == "Constante" && i % 2 != 0))
                                    {
                                        Var1.SetValorNum(LToken[i].Caracteres);
                                        if (LToken[i].General() == "Cadena")
                                        {
                                            if (Var1.Tipos != "Cadena")
                                            {
                                                Error NewError = new Error(Var1.Identificador, "No se puede transformar la cadena " + LToken[i].Caracteres + " a tipo " + Var1.Tipos);
                                                Errors.Add(NewError);
                                            }
                                        }
                                        if (LToken[i].General() == "Identificador")
                                        {
                                            for (int c = 0; c < LVariables.Count; c++)
                                            {
                                                if (LVariables[c].Identificadores == LToken[i].Caracteres)
                                                {
                                                    LVariables[c].SetUsada();
                                                }
                                            }
                                            ValidarVar(LToken[i]);
                                        }
                                        Flag = true;
                                    }
                                    else if ((LToken[i].General() == "Operador" && LToken[i + 1].General() == "Caracter"))
                                    {
                                        return false;
                                    }
                                    else if ((LToken[i].General() == "Operador" && i % 2 == 0))
                                    {
                                        if (i == LToken.Count - 1)
                                        {
                                            return false;

                                        }
                                        else
                                        {
                                            Flag = true;
                                        }
                                    }

                                    else if ((LToken[i].General() == "Caracter" && LToken[i].Caracteres == ")"))
                                    {
                                        Sign = true;
                                    }
                                    else if (i == LToken.Count - 1)
                                    {

                                        return false;

                                    }

                                    i++;
                                }
                                if (Flag == false)
                                {
                                    return Flag;
                                }
                            }
                            else
                            {
                                Flag = false;
                            }
                            i++;
                        }
                        return Flag;
                    }
                    else
                    {

                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            //ARBOL 4: ENSAMBLADOR//
            bool Ens(List<Token> LToken)
            {
                if (LToken[0].General() == "Ensamblador")
                {
                    //SUMA ENSAMBLADOR//
                    if (LToken[0].Caracteres == "add")
                    {
                        CalculadoraASM NewOperacion = new CalculadoraASM();
                        //VALIDAR SI ES UN DENTIFICADOR//
                        if (LToken[1].General() == "Identificador")
                        {
                            Variables Var = new Variables(LToken[1].Caracteres);

                            //VALIDAR SI ES UN REGISTRO//
                            if (LToken[1].Caracteres == "bx" || LToken[1].Caracteres == "ax" || LToken[1].Caracteres == "cx" || LToken[1].Caracteres == "dx" || LToken[1].Caracteres == "al" || LToken[1].Caracteres == "bl" || LToken[1].Caracteres == "cl" || LToken[1].Caracteres == "dl")
                            {
                                string Search = "", Valor = "0";
                                Var.SetTipo("Registro ensamblador");
                                if (LToken[1].Caracteres == "ax") Search = "AH";
                                else if (LToken[1].Caracteres == "bx") Search = "BH";
                                else if (LToken[1].Caracteres == "cx") Search = "CH";
                                else if (LToken[1].Caracteres == "dx") Search = "DH";
                                else if (LToken[1].Caracteres == "al") Search = "AL";
                                else if (LToken[1].Caracteres == "bl") Search = "BL";
                                else if (LToken[1].Caracteres == "cl") Search = "CL";
                                else if (LToken[1].Caracteres == "dl") Search = "DL";

                                for (int i =0; i< Calculadora.Count; i++)
                                {
                                    if(Search == "AH" && Calculadora[i].GetAH()!=0)
                                    {
                                        Valor = Calculadora[i].GetAH().ToString();
                                    }
                                    if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                    {
                                        Valor = Calculadora[i].GetBH().ToString();
                                    }
                                    if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                    {
                                        Valor = Calculadora[i].GetCH().ToString();
                                    }
                                    if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                    {
                                        Valor = Calculadora[i].GetDH().ToString();
                                    }
                                    if(Search == "AL" && Calculadora[i].GetAL()!=0)
                                    {
                                        Valor = Calculadora[i].GetAL().ToString();
                                    }
                                    if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                    {
                                        Valor = Calculadora[i].GetBL().ToString();
                                    }
                                    if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                    {
                                        Valor = Calculadora[i].GetCL().ToString();
                                    }
                                    if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                    {
                                        Valor = Calculadora[i].GetDL().ToString();
                                    }
                                }
                                NewOperacion.SetOperacion(LToken[0].Caracteres + " " + Valor + " ");
                                NewOperacion.SetRegistros(LToken[1].Caracteres + " ");
                                LVariables.Add(Var);
                            }

                            //VALIDAR SI ES OTRA VARIABLE//
                            else if (LToken[1].Caracteres[0] != '1' && LToken[1].Caracteres[0] != '2' && LToken[1].Caracteres[0] != '3' && LToken[1].Caracteres[0] != '4' && LToken[1].Caracteres[0] != '5' && LToken[1].Caracteres[0] != '6' && LToken[1].Caracteres[0] != '7' && LToken[1].Caracteres[0] != '8' && LToken[1].Caracteres[0] != '9' && LToken[1].Caracteres[0] != '0')
                            {
                                bool Declarada = false;
                                for (int c = 0; c < LVariables.Count; c++)
                                {
                                    if (LVariables[c].Identificadores == LToken[1].Caracteres)
                                    {
                                        Declarada = true;
                                        if (LVariables[c].GetValor() == false)
                                        {
                                            Error NewError = new Error(LToken[1].Caracteres, "La variable no tiene un valor asignado");
                                            Errors.Add(NewError);
                                        }
                                        else
                                        {
                                            NewOperacion.SetOperacion(LToken[0].Caracteres + " " + LVariables[c].GetValorNum() + " ");
                                        }
                                        LVariables[c].SetUsada();
                                    }
                                }
                                if (Declarada == false)
                                {
                                    Error NewError = new Error(LToken[1].Caracteres, "La variable no esta declarada");
                                    Errors.Add(NewError);
                                }
                                if (LToken[1].General() == "Identificador")
                                {
                                    ValidarVar(LToken[1]);
                                }
                            }

                            //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                            else
                            {
                                Var.SetTipo("Constante ensamblador");
                                for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                {
                                    string New = LToken[1].Caracteres;
                                    NewOperacion.SetOperacion(New[i].ToString() + " ");
                                }
                                LVariables.Add(Var);
                            }

                            //NO DEBE TERMINAR AQUI//
                            if (LToken.Count == 2)
                            {
                                return false;
                            }

                            else if (LToken[2].General() == "Identificador")
                            {
                                Variables Var2 = new Variables(LToken[2].Caracteres);
                                //VALIDAR SI ES UN REGISTRO//
                                if (LToken[2].Caracteres == "bx" || LToken[2].Caracteres == "ax" || LToken[2].Caracteres == "cx" || LToken[2].Caracteres == "dx" || LToken[2].Caracteres == "al" || LToken[2].Caracteres == "bl" || LToken[2].Caracteres == "cl" || LToken[2].Caracteres == "dl")
                                {
                                    string Search = "", Valor = "0";
                                    Var2.SetTipo("Registro ensamblador");
                                    if (LToken[2].Caracteres == "ax") Search = "AH";
                                    else if (LToken[2].Caracteres == "bx") Search = "BH";
                                    else if (LToken[2].Caracteres == "cx") Search = "CH";
                                    else if (LToken[2].Caracteres == "dx") Search = "DH";
                                    else if (LToken[2].Caracteres == "al") Search = "AL";
                                    else if (LToken[2].Caracteres == "bl") Search = "BL";
                                    else if (LToken[2].Caracteres == "cl") Search = "CL";
                                    else if (LToken[2].Caracteres == "dl") Search = "DL";

                                    for (int i = 0; i < Calculadora.Count; i++)
                                    {
                                        if (Search == "AH" && Calculadora[i].GetAH() != 0)
                                        {
                                            Valor = Calculadora[i].GetAH().ToString();
                                        }
                                        if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                        {
                                            Valor = Calculadora[i].GetBH().ToString(); 
                                        }
                                        if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                        {
                                            Valor = Calculadora[i].GetCH().ToString();
                                        }
                                        if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                        {
                                            Valor = Calculadora[i].GetDH().ToString();
                                        }
                                        if (Search == "AL" && Calculadora[i].GetAL() != 0)
                                        {
                                            Valor = Calculadora[i].GetAL().ToString();
                                        }
                                        if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                        {
                                            Valor = Calculadora[i].GetBL().ToString();
                                        }
                                        if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                        {
                                            Valor = Calculadora[i].GetCL().ToString();
                                        }
                                        if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                        {
                                            Valor = Calculadora[i].GetDL().ToString();
                                        }
                                    }
                                    int iValor = int.Parse(Valor);
                                    
                                    NewOperacion.SetOperacion(iValor.ToString("X") + " ");

                                    NewOperacion.SetRegistros(LToken[2].Caracteres + " ");

                                    LVariables.Add(Var2);
                                }

                                //VALIDAR SI ES OTRA VARIABLE//
                                else if (LToken[2].Caracteres[0] != '1' && LToken[2].Caracteres[0] != '2' && LToken[2].Caracteres[0] != '3' && LToken[2].Caracteres[0] != '4' && LToken[2].Caracteres[0] != '5' && LToken[2].Caracteres[0] != '6' && LToken[2].Caracteres[0] != '7' && LToken[2].Caracteres[0] != '8' && LToken[2].Caracteres[0] != '9' && LToken[2].Caracteres[0] != '0')
                                {
                                    bool Declarada = false;
                                    for (int c = 0; c < LVariables.Count; c++)
                                    {
                                        if (LVariables[c].Identificadores == LToken[2].Caracteres)
                                        {
                                            Declarada = true;
                                            if (LVariables[c].GetValor() == false)
                                            {
                                                Error NewError = new Error(LToken[2].Caracteres, "La variable no tiene un valor asignado");
                                                Errors.Add(NewError);
                                            }
                                            else
                                            {
                                                NewOperacion.SetOperacion(LVariables[c].GetValorNum() + " ");
                                            }
                                        }
                                    }
                                    if (Declarada == false)
                                    {
                                        Error NewError = new Error(LToken[2].Caracteres, "La variable no esta declarada");
                                        Errors.Add(NewError);
                                    }

                                    if (LToken[2].General() == "Identificador")
                                    {
                                        ValidarVar(LToken[2]);
                                    }
                                }

                                //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                                else
                                {
                                    Var2.SetTipo("Constante ensamblador");
                                    for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                    {
                                        string New = LToken[1].Caracteres;
                                        NewOperacion.SetOperacion(New[i].ToString() + " ");
                                    }
                                    LVariables.Add(Var2);
                                }

                                //DEBE TERMINAR AQUI//
                                if (LToken.Count == 3)
                                {
                                    if (IsASMActive == false)
                                    {

                                        return false;
                                    }
                                    else
                                    {
                                        NewOperacion.AnalizarOperacion();
                                        Calculadora.Add(NewOperacion);
                                        return true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }

                            }

                        }
                    }

                    //RESTA ENSAMBLADOR//
                    else if (LToken[0].Caracteres == "sub")
                    {
                        CalculadoraASM NewOperacion = new CalculadoraASM();
                        //VALIDAR SI ES UN DENTIFICADOR//
                        if (LToken[1].General() == "Identificador")
                        {
                            Variables Var = new Variables(LToken[1].Caracteres);

                            //VALIDAR SI ES UN REGISTRO//
                            if (LToken[1].Caracteres == "bx" || LToken[1].Caracteres == "ax" || LToken[1].Caracteres == "cx" || LToken[1].Caracteres == "dx" || LToken[1].Caracteres == "al" || LToken[1].Caracteres == "bl" || LToken[1].Caracteres == "cl" || LToken[1].Caracteres == "dl")
                            {
                                string Search = "", Valor = "0";
                                Var.SetTipo("Registro ensamblador");
                                if (LToken[1].Caracteres == "ax") Search = "AH";
                                else if (LToken[1].Caracteres == "bx") Search = "BH";
                                else if (LToken[1].Caracteres == "cx") Search = "CH";
                                else if (LToken[1].Caracteres == "dx") Search = "DH";
                                else if (LToken[1].Caracteres == "al") Search = "AL";
                                else if (LToken[1].Caracteres == "bl") Search = "BL";
                                else if (LToken[1].Caracteres == "cl") Search = "CL";
                                else if (LToken[1].Caracteres == "dl") Search = "DL";

                                for (int i = 0; i < Calculadora.Count; i++)
                                {
                                    if (Search == "AH" && Calculadora[i].GetAH() != 0)
                                    {
                                        Valor = Calculadora[i].GetAH().ToString();
                                    }
                                    if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                    {
                                        Valor = Calculadora[i].GetBH().ToString();
                                    }
                                    if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                    {
                                        Valor = Calculadora[i].GetCH().ToString();
                                    }
                                    if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                    {
                                        Valor = Calculadora[i].GetDH().ToString();
                                    }
                                    if (Search == "AL" && Calculadora[i].GetAL() != 0)
                                    {
                                        Valor = Calculadora[i].GetAL().ToString();
                                    }
                                    if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                    {
                                        Valor = Calculadora[i].GetBL().ToString();
                                    }
                                    if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                    {
                                        Valor = Calculadora[i].GetCL().ToString();
                                    }
                                    if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                    {
                                        Valor = Calculadora[i].GetDL().ToString();
                                    }
                                }
                                NewOperacion.SetOperacion(LToken[0].Caracteres + " " + Valor + " ");
                                NewOperacion.SetRegistros(LToken[1].Caracteres + " ");
                                Var.SetTipo("Registro ensamblador");
                                LVariables.Add(Var);
                            }

                            //VALIDAR SI ES OTRA VARIABLE//
                            else if (LToken[1].Caracteres[0] != '1' && LToken[1].Caracteres[0] != '2' && LToken[1].Caracteres[0] != '3' && LToken[1].Caracteres[0] != '4' && LToken[1].Caracteres[0] != '5' && LToken[1].Caracteres[0] != '6' && LToken[1].Caracteres[0] != '7' && LToken[1].Caracteres[0] != '8' && LToken[1].Caracteres[0] != '9' && LToken[1].Caracteres[0] != '0')
                            {
                                bool Declarada = false;
                                for (int c = 0; c < LVariables.Count; c++)
                                {
                                    if (LVariables[c].Identificadores == LToken[1].Caracteres)
                                    {
                                        Declarada = true;
                                        if (LVariables[c].GetValor() == false)
                                        {
                                            Error NewError = new Error(LToken[1].Caracteres, "La variable no tiene un valor asignado");
                                            Errors.Add(NewError);
                                        }
                                        else
                                        {
                                            NewOperacion.SetOperacion(LToken[0].Caracteres + " " + LVariables[c].GetValorNum() + " ");
                                        }
                                        LVariables[c].SetUsada();
                                    }
                                }
                                if (Declarada == false)
                                {
                                    Error NewError = new Error(LToken[1].Caracteres, "La variable no esta declarada");
                                    Errors.Add(NewError);
                                }
                                if (LToken[1].General() == "Identificador")
                                {
                                    ValidarVar(LToken[1]);
                                }
                            }

                            //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                            else
                            {
                                Var.SetTipo("Constante ensamblador");
                                for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                {
                                    string New = LToken[1].Caracteres;
                                    NewOperacion.SetOperacion(New[i].ToString() + " ");
                                }
                                LVariables.Add(Var);
                            }

                            //NO DEBE TERMINAR AQUI//
                            if (LToken.Count == 2)
                            {
                                return false;
                            }

                            else if (LToken[2].General() == "Identificador")
                            {
                                Variables Var2 = new Variables(LToken[2].Caracteres);

                                //VALIDAR SI ES UN REGISTRO//
                                if (LToken[2].Caracteres == "bx" || LToken[2].Caracteres == "ax" || LToken[2].Caracteres == "cx" || LToken[2].Caracteres == "dx" || LToken[2].Caracteres == "al" || LToken[2].Caracteres == "bl" || LToken[2].Caracteres == "cl" || LToken[2].Caracteres == "dl")
                                {
                                    string Search = "", Valor = "0";
                                    Var2.SetTipo("Registro ensamblador");
                                    if (LToken[2].Caracteres == "ax") Search = "AH";
                                    else if (LToken[2].Caracteres == "bx") Search = "BH";
                                    else if (LToken[2].Caracteres == "cx") Search = "CH";
                                    else if (LToken[2].Caracteres == "dx") Search = "DH";
                                    else if (LToken[2].Caracteres == "al") Search = "AL";
                                    else if (LToken[2].Caracteres == "bl") Search = "BL";
                                    else if (LToken[2].Caracteres == "cl") Search = "CL";
                                    else if (LToken[2].Caracteres == "dl") Search = "DL";

                                    for (int i = 0; i < Calculadora.Count; i++)
                                    {
                                        if (Search == "AH" && Calculadora[i].GetAH() != 0)
                                        {
                                            Valor = Calculadora[i].GetAH().ToString();
                                        }
                                        if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                        {
                                            Valor = Calculadora[i].GetBH().ToString();
                                        }
                                        if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                        {
                                            Valor = Calculadora[i].GetCH().ToString();
                                        }
                                        if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                        {
                                            Valor = Calculadora[i].GetDH().ToString();
                                        }
                                        if (Search == "AL" && Calculadora[i].GetAL() != 0)
                                        {
                                            Valor = Calculadora[i].GetAL().ToString();
                                        }
                                        if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                        {
                                            Valor = Calculadora[i].GetBL().ToString();
                                        }
                                        if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                        {
                                            Valor = Calculadora[i].GetCL().ToString();
                                        }
                                        if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                        {
                                            Valor = Calculadora[i].GetDL().ToString();
                                        }
                                    }
                                    int iValor = int.Parse(Valor);

                                    NewOperacion.SetOperacion(iValor.ToString("X") + " ");
                                    NewOperacion.SetRegistros(LToken[2].Caracteres + " ");
                                    LVariables.Add(Var2);
                                }

                                //VALIDAR SI ES OTRA VARIABLE//
                                else if (LToken[2].Caracteres[0] != '1' && LToken[2].Caracteres[0] != '2' && LToken[2].Caracteres[0] != '3' && LToken[2].Caracteres[0] != '4' && LToken[2].Caracteres[0] != '5' && LToken[2].Caracteres[0] != '6' && LToken[2].Caracteres[0] != '7' && LToken[2].Caracteres[0] != '8' && LToken[2].Caracteres[0] != '9' && LToken[2].Caracteres[0] != '0')
                                {
                                    bool Declarada = false;
                                    for (int c = 0; c < LVariables.Count; c++)
                                    {
                                        if (LVariables[c].Identificadores == LToken[2].Caracteres)
                                        {
                                            Declarada = true;
                                            if (LVariables[c].GetValor() == false)
                                            {
                                                Error NewError = new Error(LToken[2].Caracteres, "La variable no tiene un valor asignado");
                                                Errors.Add(NewError);
                                            }
                                            else
                                            {
                                                NewOperacion.SetOperacion(LVariables[c].GetValorNum() + " ");
                                            }
                                            LVariables[c].SetUsada();
                                        }
                                    }
                                    if (Declarada == false)
                                    {
                                        Error NewError = new Error(LToken[2].Caracteres, "La variable no esta declarada");
                                        Errors.Add(NewError);
                                    }
                                    if (LToken[2].General() == "Identificador")
                                    {
                                        ValidarVar(LToken[2]);
                                    }
                                }

                                //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                                else
                                {
                                    Var2.SetTipo("Constante ensamblador");
                                    for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                    {
                                        string New = LToken[1].Caracteres;
                                        NewOperacion.SetOperacion(New[i].ToString() + " ");
                                    }
                                    LVariables.Add(Var2);
                                }

                                //DEBE TERMINAR AQUI//
                                if (LToken.Count == 3)
                                {
                                    if (IsASMActive == false)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        NewOperacion.AnalizarOperacion();
                                        Calculadora.Add(NewOperacion);
                                        return true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }

                            }

                        }
                    }

                    //MULTIPLICACION ENSAMBLADOR//
                    else if (LToken[0].Caracteres == "mul")
                    {
                        CalculadoraASM NewOperacion = new CalculadoraASM();
                        //VALIDAR SI ES UN DENTIFICADOR//
                        if (LToken[1].General() == "Identificador")
                        {
                            Variables Var = new Variables(LToken[1].Caracteres);

                            //VALIDAR SI ES UN REGISTRO//
                            if (LToken[1].Caracteres == "bx" || LToken[1].Caracteres == "ax" || LToken[1].Caracteres == "cx" || LToken[1].Caracteres == "dx" || LToken[1].Caracteres == "al" || LToken[1].Caracteres == "bl" || LToken[1].Caracteres == "cl" || LToken[1].Caracteres == "dl")
                            {
                                string Search = "", Valor = "0";
                                Var.SetTipo("Registro ensamblador");
                                if (LToken[1].Caracteres == "ax") Search = "AH";
                                else if (LToken[1].Caracteres == "bx") Search = "BH";
                                else if (LToken[1].Caracteres == "cx") Search = "CH";
                                else if (LToken[1].Caracteres == "dx") Search = "DH";
                                else if (LToken[1].Caracteres == "al") Search = "AL";
                                else if (LToken[1].Caracteres == "bl") Search = "BL";
                                else if (LToken[1].Caracteres == "cl") Search = "CL";
                                else if (LToken[1].Caracteres == "dl") Search = "DL";

                                for (int i = 0; i < Calculadora.Count; i++)
                                {
                                    if (Search == "AH" && Calculadora[i].GetAH() != 0)
                                    {
                                        Valor = Calculadora[i].GetAH().ToString();
                                    }
                                    if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                    {
                                        Valor = Calculadora[i].GetBH().ToString();
                                    }
                                    if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                    {
                                        Valor = Calculadora[i].GetCH().ToString();
                                    }
                                    if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                    {
                                        Valor = Calculadora[i].GetDH().ToString();
                                    }
                                    if (Search == "AL" && Calculadora[i].GetAL() != 0)
                                    {
                                        Valor = Calculadora[i].GetAL().ToString();
                                    }
                                    if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                    {
                                        Valor = Calculadora[i].GetBL().ToString();
                                    }
                                    if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                    {
                                        Valor = Calculadora[i].GetCL().ToString();
                                    }
                                    if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                    {
                                        Valor = Calculadora[i].GetDL().ToString();
                                    }
                                }
                                NewOperacion.SetOperacion(LToken[0].Caracteres + " " + Valor + " ");
                                NewOperacion.SetRegistros(LToken[1].Caracteres + " ");
                                Var.SetTipo("Registro ensamblador");
                                LVariables.Add(Var);
                            }

                            //VALIDAR SI ES OTRA VARIABLE//
                            else if (LToken[1].Caracteres[0] != '1' && LToken[1].Caracteres[0] != '2' && LToken[1].Caracteres[0] != '3' && LToken[1].Caracteres[0] != '4' && LToken[1].Caracteres[0] != '5' && LToken[1].Caracteres[0] != '6' && LToken[1].Caracteres[0] != '7' && LToken[1].Caracteres[0] != '8' && LToken[1].Caracteres[0] != '9' && LToken[1].Caracteres[0] != '0')
                            {
                                bool Declarada = false;
                                for (int c = 0; c < LVariables.Count; c++)
                                {
                                    if (LVariables[c].Identificadores == LToken[1].Caracteres)
                                    {
                                        Declarada = true;
                                        if (LVariables[c].GetValor() == false)
                                        {
                                            Error NewError = new Error(LToken[1].Caracteres, "La variable no tiene un valor asignado");
                                            Errors.Add(NewError);
                                        }
                                        else
                                        {
                                            NewOperacion.SetOperacion(LToken[0].Caracteres + " " + LVariables[c].GetValorNum() + " ");
                                        }
                                        LVariables[c].SetUsada();
                                    }
                                }
                                if (Declarada == false)
                                {
                                    Error NewError = new Error(LToken[1].Caracteres, "La variable no esta declarada");
                                    Errors.Add(NewError);
                                }
                                if (LToken[1].General() == "Identificador")
                                {
                                    ValidarVar(LToken[1]);
                                }
                            }

                            //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                            else
                            {
                                Var.SetTipo("Constante ensamblador");
                                for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                {
                                    string New = LToken[1].Caracteres;
                                    NewOperacion.SetOperacion(New[i].ToString() + " ");
                                }
                                LVariables.Add(Var);
                            }

                            //NO DEBE TERMINAR AQUI//
                            if (LToken.Count == 2)
                            {
                                return false;
                            }

                            else if (LToken[2].General() == "Identificador")
                            {
                                Variables Var2 = new Variables(LToken[2].Caracteres);

                                //VALIDAR SI ES UN REGISTRO//
                                if (LToken[2].Caracteres == "bx" || LToken[2].Caracteres == "ax" || LToken[2].Caracteres == "cx" || LToken[2].Caracteres == "dx" || LToken[2].Caracteres == "al" || LToken[2].Caracteres == "bl" || LToken[2].Caracteres == "cl" || LToken[2].Caracteres == "dl")
                                {
                                    string Search = "", Valor = "0";
                                    Var2.SetTipo("Registro ensamblador");
                                    if (LToken[2].Caracteres == "ax") Search = "AH";
                                    else if (LToken[2].Caracteres == "bx") Search = "BH";
                                    else if (LToken[2].Caracteres == "cx") Search = "CH";
                                    else if (LToken[2].Caracteres == "dx") Search = "DH";
                                    else if (LToken[2].Caracteres == "al") Search = "AL";
                                    else if (LToken[2].Caracteres == "bl") Search = "BL";
                                    else if (LToken[2].Caracteres == "cl") Search = "CL";
                                    else if (LToken[2].Caracteres == "dl") Search = "DL";

                                    for (int i = 0; i < Calculadora.Count; i++)
                                    {
                                        if (Search == "AH" && Calculadora[i].GetAH() != 0)
                                        {
                                            Valor = Calculadora[i].GetAH().ToString();
                                        }
                                        if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                        {
                                            Valor = Calculadora[i].GetBH().ToString();
                                        }
                                        if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                        {
                                            Valor = Calculadora[i].GetCH().ToString();
                                        }
                                        if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                        {
                                            Valor = Calculadora[i].GetDH().ToString();
                                        }
                                        if (Search == "AL" && Calculadora[i].GetAL() != 0)
                                        {
                                            Valor = Calculadora[i].GetAL().ToString();
                                        }
                                        if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                        {
                                            Valor = Calculadora[i].GetBL().ToString();
                                        }
                                        if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                        {
                                            Valor = Calculadora[i].GetCL().ToString();
                                        }
                                        if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                        {
                                            Valor = Calculadora[i].GetDL().ToString();
                                        }
                                    }
                                    int iValor = int.Parse(Valor);

                                    NewOperacion.SetOperacion(iValor.ToString("X") + " ");
                                    NewOperacion.SetRegistros(LToken[2].Caracteres + " ");
                                    LVariables.Add(Var2);
                                }

                                //VALIDAR SI ES OTRA VARIABLE//
                                else if (LToken[2].Caracteres[0] != '1' && LToken[2].Caracteres[0] != '2' && LToken[2].Caracteres[0] != '3' && LToken[2].Caracteres[0] != '4' && LToken[2].Caracteres[0] != '5' && LToken[2].Caracteres[0] != '6' && LToken[2].Caracteres[0] != '7' && LToken[2].Caracteres[0] != '8' && LToken[2].Caracteres[0] != '9' && LToken[2].Caracteres[0] != '0')
                                {
                                    bool Declarada = false;
                                    for (int c = 0; c < LVariables.Count; c++)
                                    {
                                        if (LVariables[c].Identificadores == LToken[2].Caracteres)
                                        {
                                            Declarada = true;
                                            if (LVariables[c].GetValor() == false)
                                            {
                                                Error NewError = new Error(LToken[2].Caracteres, "La variable no tiene un valor asignado");
                                                Errors.Add(NewError);
                                            }
                                            else
                                            {
                                                NewOperacion.SetOperacion(LVariables[c].GetValorNum() + " ");
                                            }
                                            LVariables[c].SetUsada();
                                        }
                                    }
                                    if (Declarada == false)
                                    {
                                        Error NewError = new Error(LToken[2].Caracteres, "La variable no esta declarada");
                                        Errors.Add(NewError);
                                    }
                                    if (LToken[2].General() == "Identificador")
                                    {
                                        ValidarVar(LToken[2]);
                                    }
                                }

                                //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                                else
                                {
                                    Var2.SetTipo("Constante ensamblador");
                                    for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                    {
                                        string New = LToken[1].Caracteres;
                                        NewOperacion.SetOperacion(New[i].ToString() + " ");
                                    }
                                    LVariables.Add(Var2);
                                }

                                //DEBE TERMINAR AQUI//
                                if (LToken.Count == 3)
                                {
                                    if (IsASMActive == false)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        NewOperacion.AnalizarOperacion();
                                        Calculadora.Add(NewOperacion);
                                        return true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    //DIVISION ENSAMBLADOR//
                    else if (LToken[0].Caracteres == "div")
                    {
                        CalculadoraASM NewOperacion = new CalculadoraASM();
                        //VALIDAR SI ES UN DENTIFICADOR//
                        if (LToken[1].General() == "Identificador")
                        {
                            Variables Var = new Variables(LToken[1].Caracteres);

                            //VALIDAR SI ES UN REGISTRO//
                            if (LToken[1].Caracteres == "bx" || LToken[1].Caracteres == "ax" || LToken[1].Caracteres == "cx" || LToken[1].Caracteres == "dx" || LToken[1].Caracteres == "al" || LToken[1].Caracteres == "bl" || LToken[1].Caracteres == "cl" || LToken[1].Caracteres == "dl")
                            {
                                string Search = "", Valor = "0";
                                Var.SetTipo("Registro ensamblador");
                                if (LToken[1].Caracteres == "ax") Search = "AH";
                                else if (LToken[1].Caracteres == "bx") Search = "BH";
                                else if (LToken[1].Caracteres == "cx") Search = "CH";
                                else if (LToken[1].Caracteres == "dx") Search = "DH";
                                else if (LToken[1].Caracteres == "al") Search = "AL";
                                else if (LToken[1].Caracteres == "bl") Search = "BL";
                                else if (LToken[1].Caracteres == "cl") Search = "CL";
                                else if (LToken[1].Caracteres == "dl") Search = "DL";

                                for (int i = 0; i < Calculadora.Count; i++)
                                {
                                    if (Search == "AH" && Calculadora[i].GetAH() != 0)
                                    {
                                        Valor = Calculadora[i].GetAH().ToString();
                                    }
                                    if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                    {
                                        Valor = Calculadora[i].GetBH().ToString();
                                    }
                                    if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                    {
                                        Valor = Calculadora[i].GetCH().ToString();
                                    }
                                    if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                    {
                                        Valor = Calculadora[i].GetDH().ToString();
                                    }
                                    if (Search == "AL" && Calculadora[i].GetAL() != 0)
                                    {
                                        Valor = Calculadora[i].GetAL().ToString();
                                    }
                                    if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                    {
                                        Valor = Calculadora[i].GetBL().ToString();
                                    }
                                    if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                    {
                                        Valor = Calculadora[i].GetCL().ToString();
                                    }
                                    if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                    {
                                        Valor = Calculadora[i].GetDL().ToString();
                                    }
                                }
                                NewOperacion.SetOperacion(LToken[0].Caracteres + " " + Valor + " ");
                                NewOperacion.SetRegistros(LToken[1].Caracteres + " ");
                                Var.SetTipo("Registro ensamblador");
                                LVariables.Add(Var);
                            }

                            //VALIDAR SI ES OTRA VARIABLE//
                            else if (LToken[1].Caracteres[0] != '1' && LToken[1].Caracteres[0] != '2' && LToken[1].Caracteres[0] != '3' && LToken[1].Caracteres[0] != '4' && LToken[1].Caracteres[0] != '5' && LToken[1].Caracteres[0] != '6' && LToken[1].Caracteres[0] != '7' && LToken[1].Caracteres[0] != '8' && LToken[1].Caracteres[0] != '9' && LToken[1].Caracteres[0] != '0')
                            {
                                bool Declarada = false;
                                for (int c = 0; c < LVariables.Count; c++)
                                {
                                    if (LVariables[c].Identificadores == LToken[1].Caracteres)
                                    {
                                        Declarada = true;
                                        if (LVariables[c].GetValor() == false)
                                        {
                                            Error NewError = new Error(LToken[1].Caracteres, "La variable no tiene un valor asignado");
                                            Errors.Add(NewError);
                                        }
                                        else
                                        {
                                            NewOperacion.SetOperacion(LToken[0].Caracteres + " " + LVariables[c].GetValorNum() + " ");
                                        }
                                        LVariables[c].SetUsada();
                                    }
                                }
                                if (Declarada == false)
                                {
                                    Error NewError = new Error(LToken[1].Caracteres, "La variable no esta declarada");
                                    Errors.Add(NewError);
                                }
                                if (LToken[1].General() == "Identificador")
                                {
                                    ValidarVar(LToken[1]);
                                }
                            }

                            //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                            else
                            {
                                Var.SetTipo("Constante ensamblador");
                                for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                {
                                    string New = LToken[1].Caracteres;
                                    NewOperacion.SetOperacion(New[i].ToString() + " ");
                                }
                                LVariables.Add(Var);
                            }

                            //NO DEBE TERMINAR AQUI//
                            if (LToken.Count == 2)
                            {
                                return false;
                            }

                            else if (LToken[2].General() == "Identificador")
                            {
                                Variables Var2 = new Variables(LToken[2].Caracteres);

                                //VALIDAR SI ES UN REGISTRO//
                                if (LToken[2].Caracteres == "bx" || LToken[2].Caracteres == "ax" || LToken[2].Caracteres == "cx" || LToken[2].Caracteres == "dx" || LToken[2].Caracteres == "al" || LToken[2].Caracteres == "bl" || LToken[2].Caracteres == "cl" || LToken[2].Caracteres == "dl")
                                {
                                    string Search = "", Valor = "0";
                                    Var2.SetTipo("Registro ensamblador");
                                    if (LToken[2].Caracteres == "ax") Search = "AH";
                                    else if (LToken[2].Caracteres == "bx") Search = "BH";
                                    else if (LToken[2].Caracteres == "cx") Search = "CH";
                                    else if (LToken[2].Caracteres == "dx") Search = "DH";
                                    else if (LToken[2].Caracteres == "al") Search = "AL";
                                    else if (LToken[2].Caracteres == "bl") Search = "BL";
                                    else if (LToken[2].Caracteres == "cl") Search = "CL";
                                    else if (LToken[2].Caracteres == "dl") Search = "DL";

                                    for (int i = 0; i < Calculadora.Count; i++)
                                    {
                                        if (Search == "AH" && Calculadora[i].GetAH() != 0)
                                        {
                                            Valor = Calculadora[i].GetAH().ToString();
                                        }
                                        if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                        {
                                            Valor = Calculadora[i].GetBH().ToString();
                                        }
                                        if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                        {
                                            Valor = Calculadora[i].GetCH().ToString();
                                        }
                                        if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                        {
                                            Valor = Calculadora[i].GetDH().ToString();
                                        }
                                        if (Search == "AL" && Calculadora[i].GetAL() != 0)
                                        {
                                            Valor = Calculadora[i].GetAL().ToString();
                                        }
                                        if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                        {
                                            Valor = Calculadora[i].GetBL().ToString();
                                        }
                                        if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                        {
                                            Valor = Calculadora[i].GetCL().ToString();
                                        }
                                        if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                        {
                                            Valor = Calculadora[i].GetDL().ToString();
                                        }
                                    }
                                    int iValor = int.Parse(Valor);

                                    NewOperacion.SetOperacion(iValor.ToString("X") + " ");
                                    NewOperacion.SetRegistros(LToken[2].Caracteres + " ");
                                    LVariables.Add(Var2);
                                }

                                //VALIDAR SI ES OTRA VARIABLE//
                                else if (LToken[2].Caracteres[0] != '1' && LToken[2].Caracteres[0] != '2' && LToken[2].Caracteres[0] != '3' && LToken[2].Caracteres[0] != '4' && LToken[2].Caracteres[0] != '5' && LToken[2].Caracteres[0] != '6' && LToken[2].Caracteres[0] != '7' && LToken[2].Caracteres[0] != '8' && LToken[2].Caracteres[0] != '9' && LToken[2].Caracteres[0] != '0')
                                {
                                    bool Declarada = false;
                                    for (int c = 0; c < LVariables.Count; c++)
                                    {
                                        if (LVariables[c].Identificadores == LToken[2].Caracteres)
                                        {
                                            Declarada = true;
                                            if (LVariables[c].GetValor() == false)
                                            {
                                                Error NewError = new Error(LToken[2].Caracteres, "La variable no tiene un valor asignado");
                                                Errors.Add(NewError);
                                            }
                                            else
                                            {
                                                NewOperacion.SetOperacion(LVariables[c].GetValorNum() + " ");
                                            }
                                            LVariables[c].SetUsada();
                                        }
                                    }
                                    if (Declarada == false)
                                    {
                                        Error NewError = new Error(LToken[2].Caracteres, "La variable no esta declarada");
                                        Errors.Add(NewError);
                                    }
                                    if (LToken[2].General() == "Identificador")
                                    {
                                        ValidarVar(LToken[2]);
                                    }
                                }

                                //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                                else
                                {
                                    Var2.SetTipo("Constante ensamblador");
                                    for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                    {
                                        string New = LToken[1].Caracteres;
                                        NewOperacion.SetOperacion(New[i].ToString() + " ");
                                    }
                                    LVariables.Add(Var2);
                                }

                                //DEBE TERMINAR AQUI//
                                if (LToken.Count == 3)
                                {
                                    if (IsASMActive == false)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        NewOperacion.AnalizarOperacion();
                                        Calculadora.Add(NewOperacion);
                                        return true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    //POTENCIA ENSAMBLADOR//
                    else if (LToken[0].Caracteres == "mod")
                    {
                        CalculadoraASM NewOperacion = new CalculadoraASM();
                        //VALIDAR SI ES UN DENTIFICADOR//
                        if (LToken[1].General() == "Identificador")
                        {
                            Variables Var = new Variables(LToken[1].Caracteres);

                            //VALIDAR SI ES UN REGISTRO//
                            if (LToken[1].Caracteres == "bx" || LToken[1].Caracteres == "ax" || LToken[1].Caracteres == "cx" || LToken[1].Caracteres == "dx" || LToken[1].Caracteres == "al" || LToken[1].Caracteres == "bl" || LToken[1].Caracteres == "cl" || LToken[1].Caracteres == "dl")
                            {
                                string Search = "", Valor = "0";
                                Var.SetTipo("Registro ensamblador");
                                if (LToken[1].Caracteres == "ax") Search = "AH";
                                else if (LToken[1].Caracteres == "bx") Search = "BH";
                                else if (LToken[1].Caracteres == "cx") Search = "CH";
                                else if (LToken[1].Caracteres == "dx") Search = "DH";
                                else if (LToken[1].Caracteres == "al") Search = "AL";
                                else if (LToken[1].Caracteres == "bl") Search = "BL";
                                else if (LToken[1].Caracteres == "cl") Search = "CL";
                                else if (LToken[1].Caracteres == "dl") Search = "DL";

                                for (int i = 0; i < Calculadora.Count; i++)
                                {
                                    if (Search == "AH" && Calculadora[i].GetAH() != 0)
                                    {
                                        Valor = Calculadora[i].GetAH().ToString();
                                    }
                                    if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                    {
                                        Valor = Calculadora[i].GetBH().ToString();
                                    }
                                    if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                    {
                                        Valor = Calculadora[i].GetCH().ToString();
                                    }
                                    if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                    {
                                        Valor = Calculadora[i].GetDH().ToString();
                                    }
                                    if (Search == "AL" && Calculadora[i].GetAL() != 0)
                                    {
                                        Valor = Calculadora[i].GetAL().ToString();
                                    }
                                    if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                    {
                                        Valor = Calculadora[i].GetBL().ToString();
                                    }
                                    if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                    {
                                        Valor = Calculadora[i].GetCL().ToString();
                                    }
                                    if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                    {
                                        Valor = Calculadora[i].GetDL().ToString();
                                    }
                                }
                                NewOperacion.SetOperacion(LToken[0].Caracteres + " " + Valor + " ");
                                NewOperacion.SetRegistros(LToken[1].Caracteres + " ");
                                Var.SetTipo("Registro ensamblador");
                                LVariables.Add(Var);
                            }

                            //VALIDAR SI ES OTRA VARIABLE//
                            else if (LToken[1].Caracteres[0] != '1' && LToken[1].Caracteres[0] != '2' && LToken[1].Caracteres[0] != '3' && LToken[1].Caracteres[0] != '4' && LToken[1].Caracteres[0] != '5' && LToken[1].Caracteres[0] != '6' && LToken[1].Caracteres[0] != '7' && LToken[1].Caracteres[0] != '8' && LToken[1].Caracteres[0] != '9' && LToken[1].Caracteres[0] != '0')
                            {
                                bool Declarada = false;
                                for (int c = 0; c < LVariables.Count; c++)
                                {
                                    if (LVariables[c].Identificadores == LToken[1].Caracteres)
                                    {
                                        Declarada = true;
                                        if (LVariables[c].GetValor() == false)
                                        {
                                            Error NewError = new Error(LToken[1].Caracteres, "La variable no tiene un valor asignado");
                                            Errors.Add(NewError);
                                        }
                                        else
                                        {
                                            NewOperacion.SetOperacion(LToken[0].Caracteres + " " + LVariables[c].GetValorNum() + " ");
                                        }
                                        LVariables[c].SetUsada();
                                    }
                                }
                                if (Declarada == false)
                                {
                                    Error NewError = new Error(LToken[1].Caracteres, "La variable no esta declarada");
                                    Errors.Add(NewError);
                                }
                                if (LToken[1].General() == "Identificador")
                                {
                                    ValidarVar(LToken[1]);
                                }
                            }

                            //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                            else
                            {
                                Var.SetTipo("Constante ensamblador");
                                for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                {
                                    string New = LToken[1].Caracteres;
                                    NewOperacion.SetOperacion(New[i].ToString() + " ");
                                }
                                LVariables.Add(Var);
                            }

                            //NO DEBE TERMINAR AQUI//
                            if (LToken.Count == 2)
                            {
                                return false;
                            }

                            else if (LToken[2].General() == "Identificador")
                            {
                                Variables Var2 = new Variables(LToken[2].Caracteres);

                                //VALIDAR SI ES UN REGISTRO//
                                if (LToken[2].Caracteres == "bx" || LToken[2].Caracteres == "ax" || LToken[2].Caracteres == "cx" || LToken[2].Caracteres == "dx" || LToken[2].Caracteres == "al" || LToken[2].Caracteres == "bl" || LToken[2].Caracteres == "cl" || LToken[2].Caracteres == "dl")
                                {
                                    string Search = "", Valor = "0";
                                    Var2.SetTipo("Registro ensamblador");
                                    if (LToken[2].Caracteres == "ax") Search = "AH";
                                    else if (LToken[2].Caracteres == "bx") Search = "BH";
                                    else if (LToken[2].Caracteres == "cx") Search = "CH";
                                    else if (LToken[2].Caracteres == "dx") Search = "DH";
                                    else if (LToken[2].Caracteres == "al") Search = "AL";
                                    else if (LToken[2].Caracteres == "bl") Search = "BL";
                                    else if (LToken[2].Caracteres == "cl") Search = "CL";
                                    else if (LToken[2].Caracteres == "dl") Search = "DL";

                                    for (int i = 0; i < Calculadora.Count; i++)
                                    {
                                        if (Search == "AH" && Calculadora[i].GetAH() != 0)
                                        {
                                            Valor = Calculadora[i].GetAH().ToString();
                                        }
                                        if (Search == "BH" && Calculadora[i].GetBH() != 0)
                                        {
                                            Valor = Calculadora[i].GetBH().ToString();
                                        }
                                        if (Search == "CH" && Calculadora[i].GetCH() != 0)
                                        {
                                            Valor = Calculadora[i].GetCH().ToString();
                                        }
                                        if (Search == "DH" && Calculadora[i].GetDH() != 0)
                                        {
                                            Valor = Calculadora[i].GetDH().ToString();
                                        }
                                        if (Search == "AL" && Calculadora[i].GetAL() != 0)
                                        {
                                            Valor = Calculadora[i].GetAL().ToString();
                                        }
                                        if (Search == "BL" && Calculadora[i].GetBL() != 0)
                                        {
                                            Valor = Calculadora[i].GetBL().ToString();
                                        }
                                        if (Search == "CL" && Calculadora[i].GetCL() != 0)
                                        {
                                            Valor = Calculadora[i].GetCL().ToString();
                                        }
                                        if (Search == "DL" && Calculadora[i].GetDL() != 0)
                                        {
                                            Valor = Calculadora[i].GetDL().ToString();
                                        }
                                    }
                                    int iValor = int.Parse(Valor);

                                    NewOperacion.SetOperacion(iValor.ToString("X") + " ");
                                    NewOperacion.SetRegistros(LToken[2].Caracteres + " ");
                                    LVariables.Add(Var2);
                                }

                                //VALIDAR SI ES OTRA VARIABLE//
                                else if (LToken[2].Caracteres[0] != '1' && LToken[2].Caracteres[0] != '2' && LToken[2].Caracteres[0] != '3' && LToken[2].Caracteres[0] != '4' && LToken[2].Caracteres[0] != '5' && LToken[2].Caracteres[0] != '6' && LToken[2].Caracteres[0] != '7' && LToken[2].Caracteres[0] != '8' && LToken[2].Caracteres[0] != '9' && LToken[2].Caracteres[0] != '0')
                                {
                                    bool Declarada = false;
                                    for (int c = 0; c < LVariables.Count; c++)
                                    {
                                        if (LVariables[c].Identificadores == LToken[2].Caracteres)
                                        {
                                            Declarada = true;
                                            if (LVariables[c].GetValor() == false)
                                            {
                                                Error NewError = new Error(LToken[2].Caracteres, "La variable no tiene un valor asignado");
                                                Errors.Add(NewError);
                                            }
                                            else
                                            {
                                                NewOperacion.SetOperacion(LVariables[c].GetValorNum() + " ");
                                            }
                                            LVariables[c].SetUsada();
                                        }
                                    }
                                    if (Declarada == false)
                                    {
                                        Error NewError = new Error(LToken[2].Caracteres, "La variable no esta declarada");
                                        Errors.Add(NewError);
                                    }
                                    if (LToken[2].General() == "Identificador")
                                    {
                                        ValidarVar(LToken[2]);
                                    }
                                }

                                //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                                else
                                {
                                    Var2.SetTipo("Constante ensamblador");
                                    for (int i = 0; i < (LToken[1].Caracteres).Length - 1; i++)
                                    {
                                        string New = LToken[1].Caracteres;
                                        NewOperacion.SetOperacion(New[i].ToString() + " ");
                                    }
                                    LVariables.Add(Var2);
                                }

                                //DEBE TERMINAR AQUI//
                                if (LToken.Count == 3)
                                {
                                    if (IsASMActive == false)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        NewOperacion.AnalizarOperacion();
                                        Calculadora.Add(NewOperacion);
                                        return true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    //MOV//
                    else if (LToken[0].Caracteres == "mov")
                    {
                        CalculadoraASM NewOperacion = new CalculadoraASM();
                        //VALIDAR SI ES UN DENTIFICADOR//
                        if (LToken[1].General() == "Identificador")
                        {
                            Variables Var = new Variables(LToken[1].Caracteres);

                            //VALIDAR SI ES UN REGISTRO//
                            if (LToken[1].Caracteres == "bx" || LToken[1].Caracteres == "ax" || LToken[1].Caracteres == "cx" || LToken[1].Caracteres == "dx" || LToken[1].Caracteres == "al" || LToken[1].Caracteres == "bl" || LToken[1].Caracteres == "cl" || LToken[1].Caracteres == "dl")
                            {
                                Var.SetTipo("Registro ensamblador");
                                NewOperacion.SetOperacion(LToken[0].Caracteres + " "+ LToken[1].Caracteres + " ");
                                LVariables.Add(Var);
                            }

                            //VALIDAR SI ES OTRA VARIABLE//
                            else if (LToken[1].Caracteres[0] != '1' && LToken[1].Caracteres[0] != '2' && LToken[1].Caracteres[0] != '3' && LToken[1].Caracteres[0] != '4' && LToken[1].Caracteres[0] != '5' && LToken[1].Caracteres[0] != '6' && LToken[1].Caracteres[0] != '7' && LToken[1].Caracteres[0] != '8' && LToken[1].Caracteres[0] != '9' && LToken[1].Caracteres[0] != '0')
                            {
                                bool Declarada = false;
                                for (int c = 0; c < LVariables.Count; c++)
                                {
                                    if (LVariables[c].Identificadores == LToken[1].Caracteres)
                                    {
                                        Declarada = true;
                                        if (LVariables[c].GetValor() == false)
                                        {
                                            Error NewError = new Error(LToken[1].Caracteres, "La variable no tiene un valor asignado");
                                            Errors.Add(NewError);
                                        }
                                        else
                                        {
                                            NewOperacion.SetOperacion(LToken[0].Caracteres + " "+LVariables[c].GetValorNum()+" ");
                                        }
                                        LVariables[c].SetUsada();
                                    }
                                }
                                if (Declarada == false)
                                {
                                    Error NewError = new Error(LToken[1].Caracteres, "La variable no esta declarada");
                                    Errors.Add(NewError);
                                }
                                if (LToken[1].General() == "Identificador")
                                {
                                    ValidarVar(LToken[1]);
                                }
                            }

                            //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                            else
                            {
                                Var.SetTipo("Constante ensamblador"); 
                                for(int i = 0; i<(LToken[1].Caracteres).Length-1; i++ )
                                {
                                    string New = LToken[1].Caracteres;
                                    NewOperacion.SetOperacion(New[i].ToString()+" ");
                                } 
                                LVariables.Add(Var);
                            }

                            //NO DEBE TERMINAR AQUI//
                            if (LToken.Count == 2)
                            {
                                return false;
                            }

                            else if (LToken[2].General() == "Identificador")
                            {
                                Variables Var2 = new Variables(LToken[2].Caracteres);

                                //VALIDAR SI ES UN REGISTRO//
                                if (LToken[2].Caracteres == "bx" || LToken[2].Caracteres == "ax" || LToken[2].Caracteres == "cx" || LToken[2].Caracteres == "dx" || LToken[2].Caracteres == "al" || LToken[2].Caracteres == "bl" || LToken[2].Caracteres == "cl" || LToken[2].Caracteres == "dl")
                                {
                                    Var2.SetTipo("Registro ensamblador");
                                    NewOperacion.SetOperacion(" " + LToken[2].Caracteres + " ");
                                    LVariables.Add(Var2);
                                }

                                //VALIDAR SI ES OTRA VARIABLE//
                                else if (LToken[2].Caracteres[0] != '1' && LToken[2].Caracteres[0] != '2' && LToken[2].Caracteres[0] != '3' && LToken[2].Caracteres[0] != '4' && LToken[2].Caracteres[0] != '5' && LToken[2].Caracteres[0] != '6' && LToken[2].Caracteres[0] != '7' && LToken[2].Caracteres[0] != '8' && LToken[2].Caracteres[0] != '9' && LToken[2].Caracteres[0] != '0')
                                {
                                    bool Declarada = false;
                                    for (int c = 0; c < LVariables.Count; c++)
                                    {
                                        if (LVariables[c].Identificadores == LToken[2].Caracteres)
                                        {
                                            Declarada = true;
                                            if (LVariables[c].GetValor() == false)
                                            {
                                                Error NewError = new Error(LToken[2].Caracteres, "La variable no tiene un valor asignado");
                                                Errors.Add(NewError);
                                            }
                                            else
                                            {
                                                NewOperacion.SetOperacion(LVariables[c].GetValorNum() + " ");
                                            }
                                            LVariables[c].SetUsada();
                                        }
                                    }
                                    if (Declarada == false)
                                    {
                                        Error NewError = new Error(LToken[2].Caracteres, "La variable no esta declarada");
                                        Errors.Add(NewError);
                                    }
                                    if (LToken[2].General() == "Identificador")
                                    {
                                        ValidarVar(LToken[2]);
                                    }
                                }

                                //VALIDAR SI ES UNA CONSTANTE ENSAMBLADOR
                                else
                                {
                                    Var2.SetTipo("Constante ensamblador");
                                    NewOperacion.SetOperacion(" " + LToken[2].Caracteres );
                                    LVariables.Add(Var2);
                                }

                                //DEBE TERMINAR AQUI//
                                if (LToken.Count == 3)
                                {
                                    if (IsASMActive == false)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        NewOperacion.AnalizarOperacion();
                                        Calculadora.Add(NewOperacion);
                                        return true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }

                            }

                        }
                    }

                    
 
                }
                else
                {
                    return false;
                }
                return false;
            }

            //ANALIZADOR SEMANTICO//

            void ValidarVar(Token Token1)
            {
                //INTRODUCIR VARIABLES//
                Variables Var2 = new Variables(Token1.Caracteres);
                Var2.SetTipo(Token1.Tipos);
                bool Declarada = false;
                bool Usada = false;
                for (int c = 0; c < LVariables.Count; c++)
                {
                    if (LVariables[c].Identificadores == Var2.Identificador)
                    {
                        Declarada = true;
                        LVariables[c].SetUsada();
                        if (LVariables[c].GetValor() == true)
                        {
                            Usada = true;
                        }
                    }
                }
                if (Declarada == false)
                {
                    Error NewError = new Error(Var2.Identificador, "La variable no esta declarada");
                    Errors.Add(NewError);
                }
                else if (Usada == false)
                {
                    Error NewError = new Error(Var2.Identificador, "La variable no tiene un valor asignado");
                    Errors.Add(NewError);
                }
            }
            void EstablecerUso(Token Token1)
            {
                Variables Var2 = new Variables(Token1.Caracteres);
                Var2.SetTipo(Token1.Tipos);
                for (int c = 0; c < LVariables.Count; c++)
                {
                    if (LVariables[c].Identificadores == Var2.Identificador)
                    {
                        LVariables[c].SetUsada();
                    }
                }
            }
           
        }
        public List<Variables> GetVariables()
        {
            return LVariables;
        }

        public List<CalculadoraASM> GetASM()
        {
            return Calculadora;
        }
    }
}