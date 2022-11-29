# Sem_traductores2

EL proposito de este repositorio es el de agregar los trabajos que se estaran realizando a lo largo del curso de la materia de Seminario de Tradcutor de Lenguajes 2 con el profesor MICHEL EMANUEL LOPEZ FRANCO.

Los archivos-trabajos que estan aqui fueron hechos por su servidor JORGE IVAN REYES ARRIAGA de La Universidad de Guadalajara en el centro universitario CUCEI.

Entre los trabajos incluidos estan las actividades en sus estados de prototipo y su version final, incluyendo el proyecto final de la materia

El orden de los trabajos es:

1: Mini analizador léxico.
2: Mini analizador sintáctico.
3: Analizador sintáctico (Implementación usando objetos).
4: Gramática del compilador.
5: Mostrar árbol sintáctico.
6: Analizador semántico.

Cada tarea es un paso para el avance del proyecto final

Nota: Los trabajos apenas se estan subiendo con la fecha del 21-Octubre-2022.
Aprovechando que el semestre pasado trabaje con C#, voy a implementar lo que aprendi en Visual Studio para este proyecto.

# Que es un analizador Léxico?

Un analizador léxico o analizador lexicográfico es la primera fase de un compilador,
consistente en un programa que recibe como entrada el código fuente de otro
programa y produce una salida compuesta de tokens o símbolos. Estos tokens sirven
para una posterior etapa del proceso de traducción, siendo la entrada para el analizador
sintáctico.
Esta etapa está basada usualmente en una máquina de estados finitos. Esta
máquina contiene la información de las posibles secuencias de caracteres que
puede conformar cualquier token que sea parte del lenguaje (las instancias
individuales de estas secuencias de caracteres son denominados lexemas). Por
ejemplo, un token de naturaleza entera puede contener cualquier secuencia de
caracteres numéricos.

Un token léxico o simplemente token es una cadena con un significado asignado y, por lo
tanto, identificado. Está estructurado como un par que consta de un «nombre de token» y un
«valor de token» opcional. El nombre del token es una categoría de unidad léxica.1

Los nombres

de token comunes son
● identificador: nombres que elige el programador (x, color, ARRIBA);
● palabra clave: nombres que ya están en el lenguaje de programación (si,
mientras, retorno);
● separador (también conocidos como signos de puntuación): caracteres de
puntuación y delimitadores emparejados(| }, (, ;|);
● operador: símbolos que operan sobre argumentos y producen resultados (+, <, =);
● literal: literales numéricos, lógicos, textuales, de referencia (verdadero, 6,02e23,
"música");
● comentario: línea, bloque (Depende del compilador si el compilador implementa
comentarios como tokens, de lo contrario, se eliminará) (/* Recupera datos de
usuario */, // debe ser negativo).

# Imagenes finales del generador de código

![Foto 1](https://user-images.githubusercontent.com/116375899/204606996-c4ee3bc9-c432-4cb2-99ee-0a0260c089fb.PNG)

![foto 2](https://user-images.githubusercontent.com/116375899/204607021-41e2d8f8-d843-466d-bb07-5cf7fb403f94.PNG)

![Foto 3](https://user-images.githubusercontent.com/116375899/204607040-08eb4045-9a89-4e6c-8894-949188d42554.png)

Hubo un error pequeño que nunca pude solucionar, y es que en el apartado de Operaciones la tabla salia recortada. Pienso que el origen de este error fue una actializacion de Visual Studio o del .Net con el que trabaja.
