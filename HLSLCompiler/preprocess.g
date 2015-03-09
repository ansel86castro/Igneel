lexer grammar preprocess;

options {
	language='CSharp';	
}


@header{
using System.Collections.Generic;	
using Igneel.Compiling;
using Igneel.Compiling.Declarations;
using Igneel.Compiling.Expressions;
using Igneel.Compiling.Statements;
using Igneel.Compiling.Preprocessors;
using System.Text;
}

@lexer::namespace {
	Igneel.Compiling.Parser
}

@lexer::members
{
	Preprocessor p;
	public Preprocessor Preprocessor{ get{ return p;} set{p = value;} }
}

DOCUMENT : ((NL{p.AppendLine();}|WS)* PREPROC WS* ( NL{p.AppendLine();} (NL{p.AppendLine();}|WS)* PREPROC WS*)* NL{p.AppendLine();})? 
   	  (NL{p.AppendLine();}|WS)*		
	  (t=TEXT{p.Append(t.Text);})? EOF	
	 ;

fragment
PREPROC	
@init{
	bool ndef= false;
}
	:'#' ( DEFINE WS+ id=ID (WS+(v=ID |v=NUMBER))? {p.AddMacro(id.Text, v!=null?v.Text:null);} 
		| INCLUDE WS* f=INCLUDE_FILE{p.AddInclude(f.Text);}		
		| (IFDEF{ndef=false;}|IFNDEF{ndef=true;}) WS+ id=ID{p.AddCondition(id.Text, ndef);} WS* NL 
			 WS* '#'(  INCLUDE WS* f=INCLUDE_FILE{ p.AddInclude(f.Text);}
			         | DEFINE WS+ id=ID (WS+(v=ID |v=NUMBER))? {p.AddMacro(id.Text, v!=null?v.Text:null);})			 			 
		   NL {p.RemoveCondition();}
		  '#' ENDIF
              )
	;


fragment DEFINE:'define';
fragment INCLUDE:'include';
fragment IFNDEF:'ifndef';
fragment IFDEF:'ifdef';
fragment IF:'if';
fragment ELIF:'elif';
fragment ELSE:'else';
fragment ENDIF:'endif';


fragment
WS  :   (' '| '\t')
    ;	
    
fragment
NL :'\r' '\n'? | '\n'
   ;
   
fragment  
ID  
    :	(LETTER | '_') (LETTER | '0'..'9' | '_')*
    ;
    
fragment 
LETTER
	:   ('a'..'z'|'A'..'Z')
	;

fragment
TEXT	: (~'#') ( options {greedy=true;} : . )*
	;   
	
fragment 
INCLUDE_FILE 	
	  : '<' ( ' ' | '!' | '#' ..';' | '=' | '?' .. '[' | ']' .. '\u00FF')+ '>'
     	|   '"' ( ' ' | '!' | '#' ..';' | '=' | '?' .. '[' | ']' .. '\u00FF')+ '"'
     	;  

fragment      	
 NUMBER	: '0'..'9'+ ('.' '0'..'9'+)? EXPONENT? ('f' | 'F')?
 	;
 fragment
 EXPONENT 
	: ('e'|'E') ( '+' | '-')? '0'..'9'+
	;
