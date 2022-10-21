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
        
        //CONTADORES SINTACTICOS//
        public int CFor;
        public int CParentesis;
        public int CCorchetes;
        public bool IF;
        public bool SWITCH;
        public bool Cad;

        //CONSTRUCTOR//
        public Lexico() { }

        //METODOS//
        public List<Token> Scan(string Entry)
        {
            //INICIALIZAR//
            Entry += ' ';
            char Character;
            LTokens = new List<Token>();
            Instrucciones = new List<Token>();
            Errors = new List<Error>();
            State = 0;
            StringAux = "";
            bool FlagNum = false, FlagLetter = false;

            CFor = 0;
            CParentesis=0;
            CCorchetes=0;
            IF= false;
            SWITCH = false;
            Cad = false;
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
                        else if(Character=='"')
                        {
                            StringAux += Character;
                            Cad = true;
                            State = 2;

                        }

                        //SALTO DE LINEA O ;//
                        else if (char.IsWhiteSpace(Character) || Character == ';')
                        {
                            if (Instrucciones.Count!=0)
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

                        if (Cad == true)
                        {
                            StringAux += Character;
                            if (Character == '"')
                            {
                                AddToken(StringAux, "Cadena", "Cadena");
                                Cad = false;
                            }
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
                            else if (Character==';')
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
          
            if (Instrucciones[0].Caracteres == "{" || Instrucciones[0].Caracteres == "}")
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
            else if (Instrucciones[0].General() == "Palabra Reservada" || CFor>0)
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
            
        }

        // ARBOL 1: DECLARAR VARIABLES Y CONSTANTES //
        bool VarOrConst(List<Token> LToken)
        {

            //SI ES UN TIPO DE DATO//
            if (LToken[0].General() == "Tipo de Dato")
            {
                if(LToken[1].Caracteres=="main")
                {
                    return true;
                }
                //SI ES UNA VARIABLE//
                if (LToken[1].General() == "Identificador")
                {
                    //SI ES EL FIN DE LA SENTENCIA//
                    if (LToken.Count == 2)
                    {
                        return true;
                    }
                    //SI TIENE UN OPERADOR//
                    else if (LToken[2].General() == "Operador") // AGREGAR QUE OPERADORES //
                    {
                        bool Flag = false; int i = 3;
                        while (i < LToken.Count)
                        {
                            if ((LToken[i].General() == "Identificador" && i % 2 != 0) || (LToken[i].General() == "Constante" && i % 2 != 0))
                            {
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

                                    if ((LToken[i].General() == "Identificador" && i % 2 == 0) || (LToken[i].General() == "Constante" && i % 2 == 0))
                                    {

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
                                    else if (i == LToken.Count - 1)
                                    {
                                        return false;
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
                            if (LToken[3].General() == "Operador")
                            {
                                if (LToken[4].General() == "Identificador" || LToken[4].General() == "Constante")
                                {
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
                                    while (Count != true)
                                    {
                                        if ((LToken[GeneralC].General() == "Identificador" && GeneralC % 2 != 0) || (LToken[GeneralC].General() == "Constante" && GeneralC % 2 != 0))
                                        {
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
                    if(IF == false)
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
                        if(LToken[2].Caracteres=="(")
                        {
                            if (LToken[3].General() == "Identificador")
                            {
                                if (LToken[4].General() == "Operador")
                                {
                                    if (LToken[5].General()=="Identificador" || LToken[5].General()=="Constante")
                                    {
                                        if(LToken[6].Caracteres==")")
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

                                    if (LToken[4].General() == "Operador")
                                    {

                                        if (LToken[5].General() == "Identificador" || LToken[5].General() == "Constante")
                                        {
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
                                if (LToken[3].General() == "Operador" && LToken[3].Caracteres == "=")
                                {
                                    if (LToken[4].General() == "Identificador" || LToken[4].General() == "Constante")
                                    {
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
                            if (LToken[1].General() == "Operador")
                            {
                                if (LToken[2].General() == "Variable" || LToken[2].General() == "Constante")
                                {
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
                            if (LToken[3].General() == "Operador")
                            {
                                if (LToken[4].General() == "Identificador" || LToken[4].General() == "Constante")
                                {
                                    
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
                            if (LToken.Count==2)
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
                if(LToken[0].Caracteres=="cout")
                {
                    if(LToken[1].Caracteres=="<<")
                    {
                        int i = 2;
                        bool Flag=false;
                        while(i < LToken.Count)
                        {
                            if ((LToken[i].General() == "Identificador" || LToken[i].General() == "Cadena") && i % 2 == 0)
                            {
                                Flag = true;
                            }
                            else if ((LToken[i].Caracteres == "endl"))
                            {
                                if (i == LToken.Count - 1)
                                {
                                    return true;
                                }
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
                            if (LToken[i].General() == "Identificador"  && i % 2 == 0)
                            {
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

                //SI ES UNA VARIABLE//
                if (LToken[0].General() == "Identificador")
                {
                    //SI TIENE UN OPERADOR//
                    if (LToken[1].General() == "Operador") // AGREGAR QUE OPERADORES //
                    {
                        bool Flag = false; int i = 2;
                        if(LToken[1].Caracteres=="++"|| LToken[1].Caracteres == "--")
                        {
                            return true;
                        }
                        while (i < LToken.Count)
                        {
                            if ((LToken[i].General() == "Identificador" && i % 2 == 0) || (LToken[i].General() == "Constante" && i % 2 == 0))
                            {
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
    }
}

