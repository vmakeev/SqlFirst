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
	
value	:	string
	|	int
	|	float
	;	
	
string
	:	STRING
	;
	
int	:	INT
	;	
	
float	:	FLOAT
	;		

variable:	VARIABLESIGN identifier
	;
	
type	:	typeName (spaces size)?
	;
	
typeName
	:	identifier (spaces identifier)*
	;
	
size	:	OPEN_PRTH spaces int spaces CLOSE_PRTH
	;	
		
identifier	
	:	IDENTIFIER
	;	
	
DECLARE	:	('DECLARE'|'declare'|'Declare')
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
