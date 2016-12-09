grammar HLSL;

options {
	language='CSharp';
	k=2;
}

tokens {
	EQUAL 		= '==';
	NEQUAL 		= '!=';
	LESS 		= '<';
	LEQUAL 		= '<=';
	GREATER		= '>';
	GEQUAL 		= '>=';
	NOT		= '!';
	MUL 		= '*';
	DIV 		= '/';
	ADD 		= '+';
	INCREMENT	= '++';
	SUB 		= '-';
	DECREMENT	= '--';
	OR 		= '||';
	AND 		= '&&';
	XOR		= '^^';
	DOT		= '.';
	COMMA 		= ',';
	SEMICOLON 	= ';';
	COLON 		= ':';
	LBRACKET 	= '[';
	RBRACKET 	= ']';
	LPAREN 		= '(';
	RPAREN		= ')';
	LBRACE		= '{';
	RBRACE		= '}';
	ASSIGN 		= '=';
	ADDASSIGN 	= '+=';
	SUBASSIGN 	= '-=';
	MULASSIGN 	= '*=';
	DIVASSIGN 	= '/=';	
	QUESTION	= '?';	
	IF		= 'if';
	ELSE		= 'else';
	WHILE		= 'while';
	DO		= 'do';
	FOR		= 'for';
	BREAK		= 'break';
	CONTINUE	= 'continue';
	RETURN		= 'return';
	DISCARD		= 'discard';
	CONST		= 'const';
	UNIFORM		= 'uniform';
	STATIC		= 'static';
	CENTROID	= 'centroid';
	LINEAR		= 'linear';
	NOINTERP	= 'nointerpolation';
	IN		= 'in';
	OUT		= 'out';
	INOUT		= 'inout';
	STRUCT		= 'struct';
	REG		= 'register';
	PACK	        = 'packoffset';
	CBUFFER		= 'cbuffer';
	TBUFFER		= 'tbuffer';	
}

@header{
using System.Collections.Generic;	
using Igneel.Compiling;
using Igneel.Compiling.Declarations;
using Igneel.Compiling.Expressions;
using Igneel.Compiling.Statements;
using Igneel.Graphics;

}

@lexer::namespace {
	Igneel.Compiling.Parser
}

@parser::namespace {
	Igneel.Compiling.Parser
}

@parser::members {
Scope scope;
int line,column;

List<RecognitionException> _errors = new List<RecognitionException>();

public Scope GlobalScope{ get { return scope; } set{ scope = value; }}
public List<RecognitionException> Errors { get { return _errors; } }

 public override void ReportError(RecognitionException e)
 {
            _errors.Add(e);
            base.ReportError(e);
 }
	 
}
/*------------------------------------------------------------------
 * PARSER RULES
 *------------------------------------------------------------------*/
program returns[ProgramFile file = new ProgramFile()]
@init{ 
	List<Declaration> decs = new List<Declaration>();
}
	:d=declaration{ decs.Add(d); } ( d=declaration { decs.Add(d); } )* 
	{ 
		file.Declarations = decs.ToArray(); 		
	}  EOF
	;	

declaration returns[Declaration node]
	:
	( uni=UNIFORM | st=STATIC (con=CONST)?) type=type_ref name=ID v=variable_declaration[type] 
	   {	        
	        v.Name = name.Text;
	        v.Line = name.Line;
	        v.Column = name.CharPositionInLine;
		if(uni!=null)
			v.Storage = VarStorage.Uniform;
		else if(st!=null)
		{
		    v.Storage = con!=null?(VarStorage.Static | VarStorage.Const) : VarStorage.Static;
		}
		node = v;
	   }
	| attr=attributes type=type_ref (e=fixed_array{type.Elements = e;})? name=ID f=function_declaration 
	  { 
	  	f.ReturnTypeInfo = type;
	  	f.Line = name.Line;
	        f.Column = name.CharPositionInLine;
	  	f.Name = name.Text;	
	  	f.Attributes = attr.ToArray();  
	  	node = f;
	  }
	| type=type_ref (name=ID (v=variable_declaration[type]{ v.Name= name.Text;
							  v.Line = name.Line; 
							  v.Column = name.CharPositionInLine;
							  node = v;  } 
			  	 | f= function_declaration{ f.Name = name.Text;
			  			     f.ReturnTypeInfo = type;
			  			     f.Line = name.Line; 
						     f.Column = name.CharPositionInLine;
			  			     f.Line = name.Line; 
						     f.Column = name.CharPositionInLine;
						     node = f;}
			) 
			| e=fixed_array name=ID  f=function_declaration{ f.Name = name.Text;
						     type.Elements = e;
			  			     f.ReturnTypeInfo = type;
			  			     f.Line = name.Line; 
						     f.Column = name.CharPositionInLine;
						     node = f;} 
			)
	| s = struct_declaration   {node = s;}
	| cb = cbuffer_declaration {node = cb;}	
	;

attributes returns[List<AttributeDeclaration>attrs = new List<AttributeDeclaration>()]
	: (a=attribute{ attrs.Add(a); })+
	;

variable_declaration [TypeRef type]
		 returns[GlobalVariableDeclaration node = new GlobalVariableDeclaration()]
	:(elements=fixed_array{type.Elements = elements;})? 
	  (sem=semantic{node.Semantic = sem;})? 
	  (ASSIGN v=expression{node.Initializer = v;})? 
	  (packoffset=packoffset_modifier{node.PackOffset=packoffset;})?
	  (reg=register_modifier{ node.Register = reg; })?
	  SEMICOLON { node.TypeInfo=type; }
	;
fixed_array returns[int elements]
	: LBRACKET d=NUMBER { elements = int.Parse(d.Text);  } RBRACKET
	;
type_ref returns[TypeRef type = new TypeRef()]
@init
{	
	List<TypeRef>g = new List<TypeRef>();
}
	: id=ID{type.Name = id.Text;} (LESS p=type_ref{ g.Add(p); } ((COMMA) p=type_ref{ g.Add(p); })* GREATER)?
	{		
		type.GenericArgs = g.ToArray();
		type.Line = id.Line;
		type.Column = id.CharPositionInLine;
	}
	;
semantic returns[string s]
	: COLON ID{ s=$ID.Text; } 
	;
constructor returns[VariableInitializer ini]
@init{
	List<Expression> list = new List<Expression>();
}
	: b=LBRACE e=expression{list.Add(e);} (COMMA e=expression{list.Add(e);})* RBRACE 
	{ 
		ini = new VariableInitializer
		{
			Expressions = list.ToArray(),
			Line = b.Line,
			Column = b.CharPositionInLine
		}; 
	}
	;
	
register_modifier returns[RegisterDeclaration r]
	: COLON  REG LPAREN v=lvalue RPAREN {r = new RegisterDeclaration(v, $COLON.Line, $COLON.CharPositionInLine); }
	;
packoffset_modifier returns[PackOffsetDeclaration p]
	: COLON PACK LPAREN v=lvalue RPAREN {p = new PackOffsetDeclaration(v, $COLON.Line, $COLON.CharPositionInLine); }
	;
cbuffer_declaration returns[ConstantBufferDeclaration cb = new ConstantBufferDeclaration()]
@init
{
	List<GlobalVariableDeclaration>decs = new List<GlobalVariableDeclaration>();
}
	:  (c=CBUFFER{cb.Type = BufferType.CBuffer;}|c=TBUFFER{ cb.Type = BufferType.TBuffer; })
	    name=ID{cb.Name= name.Text;} LBRACE ( 
	    		 type=type_ref name=ID d=variable_declaration[type]
	    		  {
		    		  d.Name= name.Text;
		    		  d.Line = name.Line; 
				  d.Column = name.CharPositionInLine;
	    		  	  decs.Add(d);
	   		  })+ 
	    	 RBRACE SEMICOLON? 
	    {
	     	cb.Constants = decs.ToArray(); 
	     	cb.Line = c.Line;
	     	cb.Column = c.CharPositionInLine;
	    }
	;

struct_declaration returns[StructDeclaration dec = new StructDeclaration()]
@init{
	List<StructMemberDeclaration> members = new List<StructMemberDeclaration>();
}
	: STRUCT ID{ dec.Name = $ID.Text; } LBRACE (m=member_declaration{members.Add(m);})+ RBRACE SEMICOLON
	{
		dec.Members = members.ToArray();
		dec.Line = $STRUCT.Line;
	     	dec.Column = $STRUCT.CharPositionInLine;		
	}
	;

member_declaration returns[StructMemberDeclaration m = new StructMemberDeclaration()]
	: 	(i=interpolation{m.Interpolation=(InterpolationMode)i;})? 
		type=type_ref{m.TypeInfo=type;} name=ID{m.Name=name.Text; m.Line=name.Line; m.Column = name.CharPositionInLine; }
		  (e=fixed_array{type.Elements = e;})? (s=semantic{m.Semantic=s;})? SEMICOLON
	;

interpolation returns[InterpolationMode? i]
	:CENTROID{i = InterpolationMode.Centroid;} 
	| LINEAR {i = InterpolationMode.Linear;} 
	| NOINTERP{i = InterpolationMode.NoInterpolation;} 
	;

function_declaration returns[UserFunctionDeclaration dec = new UserFunctionDeclaration()]
	:LPAREN (p=parameters{dec.Parameters=p.ToArray();})? RPAREN 
	(s=semantic{dec.ReturnSemantic = s;})?
	body=block_statement{dec.Body=body;}
	;

attribute returns [AttributeDeclaration attr = new AttributeDeclaration()]
	:   LBRACKET ID{attr.Name = $ID.Text;} (args=attribute_parameters)? RBRACKET
	{
		attr.Arguments = args!=null?args.ToArray():null;
		attr.Line = $ID.Line;
		attr.Column = $ID.CharPositionInLine;
	}
	;
attribute_parameters returns[List<Expression>exp = new List<Expression>()]
	: (RPAREN (e=attribute_expresion{exp.Add(e);}) (COMMA e=attribute_expresion{exp.Add(e);})*  LPAREN)
	;
attribute_expresion returns[LiteralExpression lit]
	:	 e=constant_exp { lit = (LiteralExpression)e;}
		|t=ID	{lit = new SimbolExpression(t.Text){ Line = t.Line, Column = t.CharPositionInLine };}
		|t=STRING_LITERAL {lit = new LiteralExpression<string>(t.Text){ Line = t.Line, Column = t.CharPositionInLine };}
	;
parameters returns[List<ParameterDeclaration> l =new List<ParameterDeclaration>()]
	:	p=parameter_declaration{l.Add(p);} (COMMA p=parameter_declaration{l.Add(p);})*		
	;

parameter_declaration returns[ParameterDeclaration p = new ParameterDeclaration()]
	:(q=parameter_qualifier { p.Qualifier =(ParamQualifier)q; } )? 
	type=type_ref {p.TypeInfo = type;} 
	name=ID {  p.Name = name.Text; 
		   p.Line=name.Line; 
		   p.Column=name.CharPositionInLine; 
		}  
	(e=fixed_array{p.TypeInfo.Elements = e;})? 
	(s=semantic{p.Semantic = s;})? (ASSIGN v=expression{p.Initializer = v;})?
	;

parameter_qualifier returns[ParamQualifier? q]
	:	IN { q = ParamQualifier.In; }
	|	OUT { q = ParamQualifier.Out; }
	|	INOUT { q = ParamQualifier.InOut; }
	|	UNIFORM { q = ParamQualifier.Uniform; }
	;
block_statement returns[BlockStatement b = new BlockStatement()]
@init{ var l = new List<Statement>(); }
	: LBRACE (s=statement{l.Add(s);})* RBRACE
	 { 
	 	b.Statements = l.ToArray();
	 	b.Line = $LBRACE.Line;
	 	b.Column = $LBRACE.CharPositionInLine;
	 }
	;


expression returns[Expression n]
	:	e=or_expr{n=e;}(QUESTION v=expression COLON f=expression 
		{ 
			n = new TernaryExpression(e,v,f)
			{
			  Line = $QUESTION.Line,
			  Column = $QUESTION.CharPositionInLine
			};
		})?
	;

or_expr returns[Expression n]
	: e=xor_expr (o=OR r=xor_expr
	  { 
	  	e = new BinaryLogical { Left = e, Right = r, Operator = BinaryOperator.Or ,
	  		Line = o.Line,
		        Column = o.CharPositionInLine
	  	}; 
	  })* {n=e;}
	;

xor_expr returns[Expression n]
	: e=and_expr (x=XOR r =and_expr
	{
		e = new BinaryLogical 
		{ Left = e, 
		  Right = r, Operator = BinaryOperator.Xor ,
		  Line = x.Line,
		  Column = x.CharPositionInLine
		};
	})* {n=e;}
	;

and_expr returns[Expression n]
	:e=rel_exp (a=AND r=rel_exp
	{
		e = new BinaryLogical { Left = e, Right = r, Operator = BinaryOperator.And ,
			Line = a.Line,
		        Column = a.CharPositionInLine
		};
	})* {n=e;}
	;

rel_exp	returns[Expression n]
	: e=add_expr (
		       t=EQUAL  r=add_expr {e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.Equals , Line = t.Line, Column = t.CharPositionInLine };}
		     | t=NEQUAL  r=add_expr {e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.NotEqual ,Line = t.Line, Column = t.CharPositionInLine};}  
		     | t=LESS    r=add_expr {e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.Less, Line = t.Line, Column = t.CharPositionInLine };}
		     | t=LEQUAL  r=add_expr {e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.LessEquals ,Line = t.Line, Column = t.CharPositionInLine};}
		     | t=GREATER r=add_expr {e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.Greater ,Line = t.Line, Column = t.CharPositionInLine};}
		     | t=GEQUAL  r=add_expr {e = new BinaryRelational { Left = e, Right = r, Operator = BinaryOperator.GreaterEquals ,Line = t.Line, Column = t.CharPositionInLine};}
		    )? {n=e;}
;

add_expr returns [Expression n]
@init{ BinaryOperator op = default(BinaryOperator); }
	:e=mul_expr ((t=ADD{op = BinaryOperator.Addition; } | t=SUB { op = BinaryOperator.Substraction;})
         	    r=mul_expr { e = new  BinaryAritmetic{ Left = e, Right = r, Operator = op ,Line = t.Line, Column = t.CharPositionInLine }; })* 
         	    {n=e;}
	;

mul_expr returns[Expression n]
@init{ BinaryOperator op = default(BinaryOperator); }
	:e=unary_expr ((t=MUL{ op = BinaryOperator.Multiplication; } | t=DIV{ op =BinaryOperator.Division; }) 
	 r=unary_expr{  e = new  BinaryAritmetic{ Left = e, Right = r, Operator = op ,Line = t.Line, Column = t.CharPositionInLine};})* {n=e;}
	;
unary_expr returns[Expression n]
	: 
	e= increment{ n=e; }
	| t=NOT (v=lvalue | v=parent_exp 
		| b=BOOL_CONSTANT{v=new LiteralExpression<bool>(bool.Parse(b.Text)){ Line = b.Line, Column = b.CharPositionInLine }; } ) 
	{ 
		n = new UnaryExpression(v, UnaryOperator.Not ){Line = t.Line, Column = t.CharPositionInLine}; 
	}
	| t=SUB (v=lvalue | v=parent_exp | v= constant_exp ){ n = new UnaryExpression(v, UnaryOperator.Neg ){Line = t.Line, Column = t.CharPositionInLine}; } 	
	| c=parent_exp {n=c;} ((v=lvalue | v=parent_exp | v=constant_exp ){ n = new CastExpression(c, v); })? 
	| cons=constructor{ n = cons;}
	| lit=constant_exp{n =lit;}
	;
	
parent_exp returns[Expression n]
	: LPAREN c=expression { n = new ParenEncloseExpression(c, $LPAREN.Line, $LPAREN.CharPositionInLine); } RPAREN
	;	
increment returns[Expression n]
@init{ UnaryOperator op = default(UnaryOperator); }
	: e=lvalue{ n=e; } (t=INCREMENT { n = new  UnaryExpression(e,UnaryOperator.PostInc){Line = t.Line, Column = t.CharPositionInLine};} 
			   |t= DECREMENT{ n = new  UnaryExpression(e,UnaryOperator.PostDec){Line = t.Line, Column = t.CharPositionInLine};})?
	  | ( t=INCREMENT { op = UnaryOperator.PreInc;} 
	     |t=DECREMENT { op = UnaryOperator.PreDec;} ) e=lvalue {n = new UnaryExpression(e, op){Line = t.Line, Column = t.CharPositionInLine};} 
	;
		
lvalue returns [Expression n]
@init{ 	
	var function = false;
	var arrayIndex = false;
     }
	: e=primary_exp{ n = e ;}((DOT (m=ID LPAREN (args=argument_list)? RPAREN { 
						 n = new FunCallExpression{ 
							FunctionName = m.Text,
							Left = n,
							Parameters =args!=null?args.ToArray():new RealArguments[0],
							Line = m.Line,
							Column = m.CharPositionInLine};
						}
					| m=ID {
						n = new MemberExpression{
							Left = n,
							MemberName = m.Text,
							Line = m.Line,
							Column = m.CharPositionInLine};						
					})
			          | LBRACKET indexer=expression RBRACKET  { 
				          n = new IndexArrayExpression
						{
							Left = n,
							Indexer = indexer,
							Line = $LBRACKET.Line,
							Column =$LBRACKET.CharPositionInLine
						};
					} 
				  ))*
	;

primary_exp returns [Expression e]
@init{ 
	var function = false; 	
}
	:n=ID (LPAREN (args=argument_list)? RPAREN { function = true; })? 
	{
		if(function)
		{
			e = new FunCallExpression{ 				
				FunctionName = n.Text,						
				Parameters = args!=null?args.ToArray():new RealArguments[0],
				Line = n.Line,
				Column = n.CharPositionInLine
			};
		}
		else
		{
			e = new VariableExpression{ 
				Name = n.Text,
				Line = n.Line,
				Column = n.CharPositionInLine
			};
		}
	}
	;

argument_list returns[List<RealArguments> l = new List<RealArguments>()]

	: (n=ID ASSIGN)? v=expression { l.Add(new RealArguments{ ParameterName = n!=null?n.Text:null, Value = v ,Line = v.Line, Column = v.Column });} 
	  (COMMA (n=ID ASSIGN)? v=expression{ l.Add(new RealArguments{ ParameterName =n!=null?n.Text:null, Value = v ,Line = v.Line, Column = v.Column}); })*	
	;
	
constant_exp returns[Expression l]	
	: 	
//	v=INT_CONSTANT  { l = new LiteralExpression<int>(int.Parse(v.Text)){ Line = v.Line, Column = v.CharPositionInLine }; }		
//	| v=FLOAT_CONSTANT { l = new LiteralExpression<float>(float.Parse(v.Text)){ Line = v.Line, Column = v.CharPositionInLine }; }
	 v=BOOL_CONSTANT  { l = new LiteralExpression<bool>(bool.Parse(v.Text)){ Line = v.Line, Column = v.CharPositionInLine }; }	
	| t=NUMBER
	{
		int ivalue;		
		if(int.TryParse(t.Text, out ivalue))
			l = new LiteralExpression<int>(ivalue){ Line =t.Line, Column = t.CharPositionInLine }; 
		else
			l = new LiteralExpression<float>(float.Parse(t.Text)){ Line = t.Line, Column =t.CharPositionInLine }; 		
	}
	;
	
statement returns[Statement node]
	:e=lvalue_statement SEMICOLON {node=new LValueStatement(e);}
	|list=local_declarations SEMICOLON 
	{
	      if(list.Count == 1)
		node=new SingleLocalDeclarationStement(list[0]);
	     else
	     	node=new MultipleLocalDeclarationStatement(list.ToArray());
	}
	|(attr=attribute)? (n =selection_stmt{ ((SelectionStatement)n).Attribute = attr; } | n=iteration_stmt{ ((LoopStatement)n).Attribute = attr; }) {node=n;}
	|n=block_statement {node=n;}
	|n =jump_stmt SEMICOLON {node=n;}
	;
	
lvalue_statement returns[Expression node]
@init{ AssignOp op = default(AssignOp); }
	: 
	(inc=INCREMENT{t=inc;} | t=DECREMENT) n=lvalue{ node = new UnaryExpression(n, inc!=null? UnaryOperator.PreInc: UnaryOperator.PreDec ){Line = t.Line, Column = t.CharPositionInLine}; }
	| n=lvalue{node = n;} (((t=ASSIGN{ op = AssignOp.Assign;}| t=ADDASSIGN { op = AssignOp.AddAssign;} | t=SUBASSIGN  { op = AssignOp.SubAssign;} | t=MULASSIGN { op = AssignOp.MulAssign;} | t=DIVASSIGN { op = AssignOp.DivAssign;} ) 
		e=expression{
	    	     	node = new LValueAssign(op, n, e){Line = t.Line, Column = t.CharPositionInLine};
	    	     })
	    	| t=INCREMENT { node = new UnaryExpression(n, UnaryOperator.PostInc){Line = t.Line, Column = t.CharPositionInLine};} 
	        | t=DECREMENT { node = new UnaryExpression(n, UnaryOperator.PostDec){Line = t.Line, Column = t.CharPositionInLine};} )? 
	;	

local_declarations returns[List<LocalDeclaration> list = new List<LocalDeclaration>()]
	:(c=CONST)? type=type_ref l=local_declaration{ 
					l.TypeInfo.Name = type.Name;
					l.TypeInfo.GenericArgs = type.GenericArgs;
					l.Storage=(c!=null)?VarStorage.Const:VarStorage.Undefined;
					list.Add(l);
		      		} 
	 (COMMA l=local_declaration { l.TypeInfo.Name = type.Name;
				      l.TypeInfo.GenericArgs = type.GenericArgs;
				      l.Storage=(c!=null)?VarStorage.Const:VarStorage.Undefined;
				      list.Add(l);
				}
	  )*
	 
	;
	
local_declaration returns [LocalDeclaration n = new LocalDeclaration()]
	: id=ID{n.Name = id.Text;} (elements=fixed_array)? (ASSIGN e=expression{ n.Initializer = e; })?
	{
		n.Line = id.Line;
		n.Column = id.CharPositionInLine;
		n.TypeInfo = new TypeRef(null, elements, null);
	}
	;
	


selection_stmt returns[SelectionStatement n]
	 :  t=IF LPAREN c=expression RPAREN v=statement     	    
    	    	((ELSE statement)=>ELSE f=statement)? { n = new SelectionStatement(c,v,f){Line = t.Line, Column = t.CharPositionInLine}; }
	;

iteration_stmt returns[LoopStatement n]
	:	
	t=WHILE LPAREN c=expression RPAREN b=statement { n = new WhileStatement{ Condition=c, Body = b, Line = t.Line, Column = t.CharPositionInLine };}
	|t=DO b=statement WHILE LPAREN c=expression RPAREN { n = new DoWhileStatement{ Condition=c, Body = b ,Line = t.Line, Column = t.CharPositionInLine };}
	|t=FOR LPAREN (ini=for_ini)? SEMICOLON c=expression SEMICOLON (list=lvalue_statements)? RPAREN b=statement
	{
		n = new ForStatement{
			Initializer = ini.ToArray(),
			Increment = list.ToArray(),
			Condition=c, 
			Body = b,
			Line = t.Line,
		        Column = t.CharPositionInLine};
	}
	;
	
lvalue_statements returns[List<ASTNode> list = new List<ASTNode>()]
	: n=lvalue_statement{list.Add(n);} (COMMA n=lvalue_statement{list.Add(n);})*
	;
for_ini returns [List<ASTNode> node]
	: 
	  l=local_declarations
	  { 
	  	node = new List<ASTNode>();
	  	foreach (var item in l)
	  		node.Add(item);
	  }
	 |n=lvalue_statements{ node = n;}
	;

jump_stmt returns [Statement node]
	:	t=BREAK {node = new JumpStatement{ Jump = JumpType.Break, Line =t.Line, Column = t.CharPositionInLine };}
	|	t=CONTINUE {node = new JumpStatement{ Jump = JumpType.Continue, Line =t.Line, Column = t.CharPositionInLine };}
	|	t=DISCARD  {node = new JumpStatement{ Jump = JumpType.Discard, Line =t.Line, Column = t.CharPositionInLine };}
	|	t=RETURN (e=expression)? {node = new JumpStatement{ Jump = JumpType.Return, ReturnExp = e, Line =t.Line, Column = t.CharPositionInLine };}
	;
	
	
/*------------------------------------------------------------------
 * LEXER RULES
 *------------------------------------------------------------------*/
//INT_CONSTANT
//	: DECIMAL_CONSTANT 
//	|OCTAL_CONSTANT 
//	|HEX_CONSTANT 
//    ;

//fragment DECIMAL_CONSTANT
//	:	ZERO
//	|	NON_ZERO_DIGIT (DIGIT_SEQUENCE)?
//	;
	
fragment ZERO
	: '0'
	;
	
fragment NON_ZERO_DIGIT
	:	OCTAL_DIGIT |'8'|'9'
	;
	
fragment DIGIT
	:	ZERO
	|	NON_ZERO_DIGIT
	;

fragment OCTAL_CONSTANT
	:	ZERO (OCTAL_DIGIT)+
	;

fragment OCTAL_DIGIT
	:	('1'..'7')
	;

fragment HEX_CONSTANT
	:	ZERO ('x'|'X') (HEX_DIGIT)+
	;

fragment HEX_DIGIT
	:	DIGIT|'a'..'f'|'A'..'F'
	;

//FLOAT_CONSTANT
 //   :   f=FRACTIONAL_CONSTANT (e=EXPONENT)? (FLOAT_SUFFIX)? {$text=f.Text+((e!=null)?e.Text:"");}
 //   ;

//fragment FRACTIONAL_CONSTANT
//	: 	DIGIT_SEQUENCE DOT (DIGIT_SEQUENCE)?
//	|	DOT DIGIT_SEQUENCE
//	;

NUMBER	: m=DIGIT_SEQUENCE (DOT f=DIGIT_SEQUENCE)? (e=EXPONENT)? (FLOAT_SUFFIX)?
	{
	  string value = m.Text;
	  if(f!=null)
	  	value += "."+f.Text;		  	
	  if(e!=null)
	  	value += e.Text;
    	  $text = value;
	}
	;

fragment	
DIGIT_SEQUENCE
	:	DIGIT+
	;

fragment
EXPONENT 
	: ('e'|'E') ( ADD | SUB)? DIGIT_SEQUENCE 
	;
fragment
FLOAT_SUFFIX
	: 'f' | 'F'
	;
	
BOOL_CONSTANT
	: 'true'
	| 'false'
	;

ID  
    :	(LETTER | '_') (LETTER | DIGIT | '_')*
    ;
     
fragment LETTER
	:   ('a'..'z'|'A'..'Z')
	;	


COMMENT
    :   '//' ~('\n'|'\r')* ('\r'? '\n'| EOF) {$channel=HIDDEN;}
    |   '/*' ( options {greedy=false;} : . )* '*/' {$channel=HIDDEN;}
    ;

WS  :   (' '| '\t' | '\r' | '\n' ) {$channel=HIDDEN;}
    ;
STRING_LITERAL
    :  '"' ( EscapeSequence | ~('\\'|'"') )* '"'
    ;
    
fragment
EscapeSequence
    :   '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
    |   OctalEscape
    ;
    fragment
OctalEscape
    :   '\\' ('0'..'3') ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7')
    ;

