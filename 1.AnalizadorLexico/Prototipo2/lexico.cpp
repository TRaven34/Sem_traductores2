#include <cstdlib>
#include <iostream>
#include <string>

#include "lexico.h"

Lexico::Lexico(string fuente){
     ind= 0;                  
     this->fuente= fuente;
}      

Lexico::Lexico(){
     ind= 0;                  
}      

string Lexico::tipoAcad(int tipo){
     string cad= "";

     switch (tipo){
          case TipoSimbolo::IDENTIFICADOR:
               cad= "Identificador";
               break;

          case TipoSimbolo::OPADIC:
               cad= "Op. Adicion";
               break;

          case TipoSimbolo::OPMULT:
               cad= "Op. Multiplicacion";
               break;            

          case TipoSimbolo::PESOS:
               cad= "Fin de la Entrada";
               break;            

          case TipoSimbolo::ENTERO:
               cad= "Entero";
               break;

          case TipoSimbolo::REAL:
               cad= "Real";
               break;

          case TipoSimbolo::OPRELAC:
               cad= "Op. Relacional";
               break;

          case TipoSimbolo::OPOR:
               cad= "Op. Or";
               break;

          case TipoSimbolo::OPAND:
               cad= "Op. And";
               break;

          case TipoSimbolo::OPNOT:
               cad= "Op. Not";
               break;
          
          case TipoSimbolo::OPIGUALDAD:
               cad= "Op. Igualdad";
               break;
          
          case TipoSimbolo::PUNTOYCOMA:
               cad= "Punto y coma";
               break;
          
          case TipoSimbolo::COMA:
               cad= "Coma";
               break;
          
          case TipoSimbolo::PARENIZQ:
               cad= "Parentesis abierto";
               break;

          case TipoSimbolo::PARENDER:
               cad= "Parentesis cerrado";
               break;

          case TipoSimbolo::CORCHETEIZQ:
               cad= "Corchete abierto";
               break;

          case TipoSimbolo::CORCHETEDER:
               cad= "Corchete cerrado";
               break;

          case TipoSimbolo::IGUAL:
               cad= "Igual";
               break;

          case TipoSimbolo::IF:
               cad= "If";
               break;

          case TipoSimbolo::WHILE:
               cad= "While";
               break;

          case TipoSimbolo::RETURN:
               cad= "Return";
               break;

          case TipoSimbolo::ELSE:
               cad= "Else";
               break;

          case TipoSimbolo::CADENA:
               cad= "Cadena";
               break;

          case TipoSimbolo::TIPO:
               cad= "Tipo";
               break;
          default:
          	   cad="ERROR";
          	   break;
     }
     return cad;
}

void Lexico::entrada(string fuente){
     ind= 0;//se inicializa el contador que recorre la cadena              
     this->fuente= fuente;//guardamas la cadena que dio el usuario en fuente
}

int Lexico::sigSimbolo(){
     estado=0; //inicializamos estado en 0
     continua= true; 
     simbolo= "";
     bool entero = true;
     bool reales = true;
     bool cadena = false;
     //Inicio del Automata
     while (continua){
          //llamamos a la funcion que separa las palabras de la cadena
          c = sigCaracter();
          int longitud = c.length();//obtenemos la longitud de la palabra
          switch (estado){
               case 0:
                    if ( c == "+" || c=="-"){
                         aceptacion(2);
                         break;
                    }
                    else
                    if ( c == "*" || c=="/"){ 
                         aceptacion(5);
                         break;
                    }
                    else
                    if ( c == "<" || c=="<=" || c == ">" || c==">="){ 
                         aceptacion(7);
                         break;
                    }
                    else
                    if ( c == "||"){ 
                         aceptacion(8);
                         break;
                    }
                    else
                    if ( c == "&&"){ 
                         aceptacion(9);
                         break;
                    }
                    else
                    if ( c == "!"){ 
                         aceptacion(10);
                         break;
                    }
                    else
                    if ( c == "!=" || c == "=="){ 
                         aceptacion(11);
                         break;
                    }
                    else
                    if ( c == ";"){ 
                         aceptacion(12);
                         break;
                    }
                    else
                    if ( c == ","){ 
                         aceptacion(13);
                         break;
                    }
                    else
                    if ( c == "("){ 
                         aceptacion(14);
                         break;
                    }
                    else
                    if ( c == ")"){ 
                         aceptacion(15);
                         break;
                    }
                    else
                    if ( c == "{"){ 
                         aceptacion(16);
                         break;
                    }
                    else
                    if ( c == "}"){ 
                         aceptacion(17);
                         break;
                    }
                    else
                    if ( c == "="){ 
                         aceptacion(18);
                         break;
                    }
                    else
                    if ( c == "if" ){ 
                         aceptacion(19);
                         break;
                    }
                    else
                    if ( c == "while" ){
                         aceptacion(20);
                         break;
                    }
                    else
                    if ( c == "return" ){ 
                         aceptacion(21);
                         break;
                    }
                    else
                    if ( c == "else" ){
                         aceptacion(22);
                         break;
                    }
                    else
                    if ( c == "int" || c=="float" || c == "void" || c=="char"){ 
                         aceptacion(24);
                         break;
                    }
                    else
                    if ( c == "$" ){ 
                         aceptacion(3);
                         break;
                    }
					
                    //para palabras que son numeros variables o cadenas se hace un for para comprobar que tipo de palabra es
                    for (int i=0; i < longitud ; i++){ 
                         bool x = isdigit(c[i]);
                         if(x == 0)entero = false;//si los caracteres de la palabra no son solo digitos sognifica que no es un entero
                         if(x == 0 && c[i] != '.')reales = false;//si uno de los caracteres no es digito y no es un punto no es un real
                         if(x == 0 && c[i] == '\"')cadena = true;//si el caracter no es digito, ni es punto pero es comilla entonces es cadena.
                    }
                    if (entero == true){ 
                         aceptacion(4);
                         break;
                    }
                    else
                    if (reales == true){ 
                         aceptacion(6);
                         break;
                    }
                    else
                    if (cadena == true){
                         aceptacion(23);
                         break;
                    }
                    else{//si al analizar la palabra no fue ni palabra reservada, ni entero, ni real, ni cadena entonces es identificador
                         aceptacion(1);
                         break;
                    }
               break;                       
          }  
     }    
    //Fin del Automata
     switch (estado){
          case 1:
               tipo = TipoSimbolo::IDENTIFICADOR;
               break; 
          case 2:
               tipo = TipoSimbolo::OPADIC;
               break; 
          case 3:
               tipo = TipoSimbolo::PESOS;
               break; 
          case 4:
               tipo = TipoSimbolo::ENTERO;
               break;
          case 5:
               tipo = TipoSimbolo::OPMULT;
               break;
          case 6:
               tipo = TipoSimbolo::REAL;
               break;
          case 7:
               tipo = TipoSimbolo::OPRELAC;
               break;
          case 8:
               tipo = TipoSimbolo::OPOR;
               break;
          case 9:
               tipo = TipoSimbolo::OPAND;
               break;
          case 10:
               tipo = TipoSimbolo::OPNOT;
               break;
          case 11:
               tipo = TipoSimbolo::OPIGUALDAD;
               break;
          case 12:
               tipo = TipoSimbolo::PUNTOYCOMA;
               break;
          case 13:
               tipo = TipoSimbolo::COMA;
               break;
          case 14:
               tipo = TipoSimbolo::PARENIZQ;
               break;
          case 15:
               tipo = TipoSimbolo::PARENDER;
               break;
          case 16:
               tipo = TipoSimbolo::CORCHETEIZQ;
               break;
          case 17:
               tipo = TipoSimbolo::CORCHETEDER;
               break;
          case 18:
               tipo = TipoSimbolo::IGUAL;
               break;
          case 19:
               tipo = TipoSimbolo::IF;
               break;
          case 20:
               tipo = TipoSimbolo::WHILE;
               break;
          case 21:
               tipo = TipoSimbolo::RETURN;
               break;
          case 22:
               tipo = TipoSimbolo::ELSE;
               break;
          case 23:
               tipo = TipoSimbolo::CADENA;
               break;
          case 24:
               tipo = TipoSimbolo::TIPO;
               break;
          case -1:                 
               tipo= TipoSimbolo::ERROR;
     }
	return tipo;
}

string Lexico::sigCaracter(){
     string palabra;
     //revisamos si enos encontramos al final de la cadena
     if (terminado()){ 
          return "$";
     }
     //revisamos si hay un simbolo de $ antes del final de la cadena para terminar precipitadamente
     if(fuente[ind] == '$'){
          return "$";
     }
     // recorremos la cadena letra por leta y se suman en una variable, cuando hay un espacio
     // esta variable forma una "palabra" y esta se regresa para analizar de que tipo es
     else{
          while(fuente[ind] != ' '){
               palabra += fuente[ind];
               ind++;
          }
          if(fuente[ind] == ' '){
               ind++;
          }
          return palabra;
     }
}

//asigna a estado el numero de oprador del que se trata la palabra.
void Lexico::sigEstado(int estado){
     this->estado = estado;
     simbolo += c;
}

//llama a la funcion para asignar estado y pausa el siclo while principal
void Lexico::aceptacion(int estado){
     sigEstado(estado);
     continua= false;
}

bool Lexico::terminado(){//fin de cadena
     if (ind >= fuente.length()){
          return true;
     }
}

int main(int argc, char *argv[]){
    
    Lexico lexico;
    //llamamos a la funcion entreda de lexico
    lexico.entrada("reyes arriaga + } 1998 / 135.48 >= ! != && , if ( ) else while int { = return ; ");
    
    cout << "Resultado del Analisis Lexico" << endl << endl;
    cout << "Simbolo\t\tTipo" << endl;
    
    while (lexico.simbolo.compare("$") != 0 ){
        //llamamos a la funcion sigSimbolo
        lexico.sigSimbolo();
        cout <<  lexico.simbolo << "\t\t" << lexico.tipoAcad(lexico.tipo) << endl;          
    }
        
    cin.get();
    return 0;
}
