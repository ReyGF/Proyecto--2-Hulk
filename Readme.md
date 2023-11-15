# HULK: Havana University Language for Kompilers

En este proyecto usted implementará un intérprete del lenguaje de programación [HULK](https://matcom.in/hulk).

Para completar el proyecto, usted debe implementar un subconjunto de HULK, que definiremos a continuación. 
HULK es un lenguaje mucho más grande que lo requerido en este proyecto, y usted tiene la libertad de implementar cualquier funcionalidad adicional que desee.

> En 3er año, en la asignatura de Compilación, usted verá como implementar un compilador completamente funcional del lenguaje HULK en su totalidad.

## El lenguaje HULK (simplificado)

HULK es un lenguaje de programación imperativo, funcional, estática y fuertemente tipado. Casi todas las instrucciones en HULK son expresiones. 
En particular, el subconjunto de HULK que usted implementar se compone solamente de expresiones que pueden escribirse en una línea.

### Expresiones básicas

Todas las instrucciones en HULK terminan en `;`. La instrucción más simple en HULK que hace algo es la siguiente:

```js
print("Hello World");
```

HULK además tiene expresiones aritméticas:

```js
print((((1 + 2) ^ 3) * 4) / 5);
```

Y funciones matemáticas básicas:

```js
print(sin(2 * PI) ^ 2 + cos(3 * PI / log(4, 64)));
```

> HULK soporta también expresiones multi-línea, pero esas no son requeridas en este proyecto.
> HULK tiene tres tipos básicos: `string`, `number`, y `boolean`. Además en HULK se pueden definir tipos nuevos, pero en este proyecto no es requerido.

### Funciones

En HULK hay dos tipos de funciones, las funciones _inline_ y las funciones regulares. En este proyecto solo debe implementar las funciones _inline_. Tienen la siguiente forma:

```js
function tan(x) => sin(x) / cos(x);
```

Una vez definida una función, puede usarse en una expresión cualquiera:

```js
print(tan(PI/2));
```

El cuerpo de una función _inline_ es una expresión cualquiera, que por supuesto puede incluir otras funciones y expresiones básicas, o cualquier combinación.

### Variables

En HULK es posible declarar variables usando la expresión `let-in`, que funciona de la siguiente forma:

```js
let x = PI/2 in print(tan(x));
```

En general, una expresión `let-in` consta de una o más declaraciones de variables, y un cuerpo, que puede ser cualquier expresión donde además se pueden utilizar las variables declaradas en el `let`. 
Fuera de una expresión `let-in` las variables dejan de existir.

Por ejemplo, con dos variables:

```js
let number = 42, text = "The meaning of life is" in print(text @ number);
```

Que es equivalente a:

```js
let number = 42 in (let text = "The meaning of life is" in (print(text @ number)));
```

El valor de retorno de una expresión `let-in` es el valor de retorno del cuerpo, por lo que es posible hacer:

```js
print(7 + (let x = 2 in x * x));
```

Que da como resultado `11`.

> La expresión `let-in` permite hacer mucho más, pero para este proyecto usted solo necesita implementar las funcionalidades anteriores.

### Condicionales

Las condiciones en HULK se implementan con la expresión `if-else`, que recibe una expresión booleana entre paréntesis, y dos expresiones para el cuerpo del `if` y el `else` respectivamente.
Siempre deben incluirse ambas partes:

```js
let a = 42 in if (a % 2 == 0) print("Even") else print("odd");
```

Como `if-else` es una expresión, se puede usar dentro de otra expresión (al estilo del operador ternario en C#):

```js
let a = 42 in print(if (a % 2 == 0) "even" else "odd");
```

> En HULK hay expresiones condicionales con más de una condición, usando `elif`, pero para este proyecto usted no tiene que implementarlas.

### Recursión

Dado que HULK tiene funciones compuestas, por definición tiene también soporte para recursión. Un ejemplo de una función recursiva en HULK es la siguiente:

```js
function fib(n) => if (n > 1) fib(n-1) + fib(n-2) else 1;
```

Usted debe garantizar que su implementación permite este tipo de definiciones recursivas.

## El intérprete

Su intérprete de HULK será una aplicación de consola, donde el usuario puede introducir una expresión de HULK, presionar ENTER, e immediatamente se verá el resultado de evaluar expresión (si lo hubiere)
Este es un ejemplo de una posible interacción:

```js
> let x = 42 in print(x);
42
> function fib(n) => if (n > 1) fib(n-1) + fib(n-2) else 1;
> fib(5)
13
> let x = 3 in fib(x+1);
8
> print(fib(6));
21
```

Cada línea que comienza con `>` representa una entrada del usuario, e immediatamente después se imprime el resultado de evaluar esa expresión, si lo hubiere.

> Note que cuando una expresión tiene valor de retorno (como en el caso de un llamado a una función), directamente se imprime el valor retornado, aunque no haya una instrucción `print`.

Todas las funciones declaradas anteriormente son visibles en cualquier expresión subsiguiente. Las funciones no pueden redefinirse.

### Errores

En HULK hay 3 tipos de errores que usted debe detectar. En caso de detectarse un error, el intérprete debe imprimir una línea indicando el error que sea lo más informativa posible.

#### Error léxico

Errores que se producen por la presencia de tokens inválidos. Por ejemplo:

```js
> let 14a = 5 in print(14a); 
! LEXICAL ERROR: `14a` is not valid token.
```

#### Error sintático

Errores que se producen por expresiones mal formadas como paréntesis no balanceados o expresiones incompletas. Por ejemplo:

```js
> let a = 5 in print(a;
! SYNTAX ERROR: Missing closing parenthesis after `a`.
> let a = 5 inn print(a);
! SYNTAX ERROR: Invalid token `inn` in `let-in` expression.
> let a = in print(a);
! SYNTAX ERROR: Missing expression in `let-in` after variable `a`.
```

### Error semántico

Errores que se producen por el uso incorrecto de los tipos y argumentos. Por ejemplo:

```js
> let a = "hello world" in print(a + 5);
! SEMANTIC ERROR: Operator `+` cannot be used between `string` and `number`.
> print(fib("hello world"));
! SEMANTIC ERROR: Function `fib` receives `number`, not `string`.
> print(fib(4,3));
! SEMANTIC ERROR: Function `fib` receives 1 argument(s), but 2 were given.
```

En caso de haber más de un error, usted debe detectar solamente **uno** de los errores.

## Instrucciones de uso:

Este proyecto esta dividido en tres partes fundamentales : expresiones matematicas, condicionales y funciones. Al ejecutar el proyecto este no dejara de leer lineas de codigo hasta que reciba una linea vacia. Para las expresiones matematicas es libre de usar numeros, variables, parentesis y operaciones matematicas, por ejemplo "1" , "(1+2*3)/ 5^2", x+1, y++, etc. 

En las expresiones condicionales podra usar booleanos true or false, estos soprtan operadores como | -> or & -> and ! -> negation == -> Equal, si desea comparar un numero y un booleao por ejemplo 1 | true el numero tomara valor true si es distinto de 0 y false si es 0, aclarar que true == 1 saltara un error. El interpetre soprota hasta 10 lineas seguidas de codigo por ejemplo linea 1; linea 2; linea 3... aunque es lo mismo que escribir una linea presiona enter y escribir otra el interprete funcionara exactamente igual. A la hora de declarar variables se puede hacer con una expresion let-in o con una expresion let ejemplo let x = 1 in x; esto imprime 1 pero tambien se puede declarar una variable con let x = "hola mundo"; y mas tarde imprimir esta variable, tambien se puede declarar o asignar valores a varias variables al mismo tiempo por ejemplo let x = 5, y = 3; o let x; let y; x=5, y = 3; print(x + y); para imprimir un valor no es necesario la expresion print, una instruccion ejemplo "arboles" sin un print devuelve "arboles".El resto de las cosas de condicionales funciona igual que en la orientacion del proyecto. 

En el apartado de las funciones tenemos que esta consta de dos parte la declaracion y el llamado, para declarar funciones existe la palabra reservada function y la manera de declarar seria function namefuntion(parameters) => expression; en la declaracion de los parametros podra inizializar parametros con valores ejemplo function f(x, y = 3) => (); en el llamado podra elegir si cambiar el valor o mantenerlo ejemplo f(1,2) o f(1) aclarar que si declaramos un parametro function(x= 5, y) a la hora de la llamada f(1) modificara la x por que se pasan los parametros por posicion. Podra implementar funcions recursivas a su gusto.

El programa devolvera siempre el primer error que encuentre.