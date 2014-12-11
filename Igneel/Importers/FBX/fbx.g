grammar fbx;
options{
	language='CSharp';
}
tokens{
	COMMA=     ',';
	COLON=     ':';		
	LCURLY=    '{';
	RCURLY=    '}';
	DOT=	   '.';		
	MINUS = '-';
}
@header{

using Igneel.Loaders.FBXParser;
using System.Collections.Generic;

}

fragment
NL	:'\r\n'
	|'\n'
	|'\r'
	;
	
WS :(' '|'\t' |NL){$channel=HIDDEN;}
  ;
       
fragment		
DIGIT	:('0'..'9')
	;

NUMBER	:  MINUS? DIGIT+ (DOT DIGIT+)? ('e'  MINUS? DIGIT+)?
	;	
	
fragment
LETTER :('a'..'z')|('A'..'Z')
       ;
       
ID	: LETTER (DIGIT|LETTER|'_')*
	;
	
fragment
OctalEscape
    :    ('0'..'3') ('0'..'7') ('0'..'7')
    |    ('0'..'7') ('0'..'7')
    |    ('0'..'7')
    ;
STRING	: '"' ( ~('"') )* '"'
	;

COMENT	: ';' ~('\n'|'\r')* '\r'? '\n' {$channel=HIDDEN;}
	;

document returns [FBXDocument doc]
	@init{ doc = new FBXDocument();}
	: (dec = declaration { doc.Add(dec); } )+
	;

declaration returns[FBXDeclarationNode dec]
	: id=ID COLON 
	 (
	  dec1=list[id.Text] {dec = dec1;} |
	  dec2=objectDeclaration[id.Text] {dec = dec2;}
	  )
	;
	
list [string id] returns[FBXDeclarationNode dec]
	@init{ 
	 	int numbers = 0;
	 	int strings = 0;
	 	List<string>list = new List<string>();	 	
	 }
	: (tok=NUMBER{numbers++;} | tok=STRING{strings++;} | tok=ID{strings++;} ) {list.Add(tok.Text);}
	      (COMMA (tok=NUMBER{numbers++;} | tok=STRING{strings++;} | tok=ID{strings++;}) {list.Add(tok.Text);} )*
	{
	   if(numbers > 0 && strings==0)
	   {
	   	dec = new FBXFloatListProperty(list);
	   }
	   else if(numbers == 0 && strings > 0)
	   {
	   	dec = new FBXListProperty(list,ContentType.STRING_LIST);
	   }
	   else
	   {
	   	dec = new FBXListProperty(list, ContentType.MIXED_LIST);
	   }
	    dec.Type =id;
	}
	;
	
objectDeclaration [string id]  returns[FBXDeclarationNode dec]
	: ( ( name=STRING (COMMA type=STRING)? | index = NUMBER) 
	{ 
		if(name!=null)
		 	dec = new FBXObject(name.Text, type != null? type.Text : null);
		 else 
		 	dec = new FBXObject();
	 	dec.Type =id;
	 	if(index!=null)
	 		((FBXObject)dec).Index = int.Parse(index.Text);
	 })? 
	LCURLY
	  (     node=declaration 
	 	{  
	 	       if(dec == null)
		 	{ 
		 		 dec = new FBXObject();
		 		dec.Type =id;
		 	}
	 	       ((FBXObject)dec).Add(node); 
	 	}
	  )*
	 RCURLY 
	 { 
	 	if(dec == null) 
	 	{
	 		dec = new FBXObject(); 	 		
	 		dec.Type =id;
	 	}
	 }
	;

	
	
