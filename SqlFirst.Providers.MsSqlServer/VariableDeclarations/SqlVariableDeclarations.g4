grammar SqlVariableDeclarations;

root	:	element* EOF
	;
	
element	:	declaration
	|	commentary
	|	space
	;

declaration
	:	DECLARE spaces variable spaces type spaces (assignment spaces)? SEMICOLON
	;
	
commentary
	:	SINGLELINE_COMMENTARY
	|	MULTILINE_COMMENTATY
	;	
	
spaces	:	space*
	;	

space	:	(SPACE|TAB|LINEBREAK)
	;			
	
assignment	
	:	EQUATION spaces value
	;
	
value	:	stringValue
	|	intValue
	|	floatValue
	;	
	
stringValue
	:	STRING
	;
	
intValue
        :	INT
	;	
	
floatValue
        :	FLOAT
	;		

variable:	VARIABLESIGN identifier
	;
	
type	:	typeName (spaces size)?
	;
	
typeName
	:	identifier (spaces identifier)*
	;
	
size	:	OPEN_PRTH spaces length spaces CLOSE_PRTH
	;	

length
        :   intValue
        |   maxValue
        ;

maxValue
        :   MAX
        ;
		
identifier	
	:	IDENTIFIER
	;	
	
DECLARE	:	('DECLARE'|'declare'|'Declare')
	;
	
MAX	:	('MAX'|'max'|'Max')
	;

INT	:	DIGIT (DIGIT)*
	;
	
FLOAT	:	(INT+ '.' (INT)*)
	;	

STRING	:	SINGLE_QUOTE (.)*? SINGLE_QUOTE  
	;
	
IDENTIFIER
	:	(LETTER|UNDERSCOPE) (LETTER|DIGIT|UNDERSCOPE)*
	;

fragment
SINGLE_QUOTE
	:	'\''
	;

fragment
DIGIT	:	('0'..'9')
	;

fragment
LETTER	:	('a'..'z'|'A'..'Z')
	;
			
fragment
UNDERSCOPE
	:	'_'
	;

VARIABLESIGN
	:	'@'
	;
	
LINEBREAK
	:	('\r\n'|'\n')
	;
	
EQUATION
	:	'='
	;

SPACE	:	' '
	;
	
TAB	:	'\t'	
	;

OPEN_PRTH
	:	'('
	;

CLOSE_PRTH
	:	')'
	;		

SEMICOLON
	:	';'
	;
		
SINGLELINE_COMMENTARY
	:	'-''-' (.)*? LINEBREAK
	;
	
MULTILINE_COMMENTATY
	:	'/' '*' (.)*? '*' '/'	
	;
