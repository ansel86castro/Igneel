// $ANTLR 3.1.2 E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g 2012-09-26 21:27:43



using Igneel.Importers.FBX;
using System.Collections.Generic;



using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


public class fbxParser : Parser 
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"COMMA", 
		"COLON", 
		"LCURLY", 
		"RCURLY", 
		"DOT", 
		"MINUS", 
		"NL", 
		"WS", 
		"DIGIT", 
		"NUMBER", 
		"LETTER", 
		"ID", 
		"OctalEscape", 
		"STRING", 
		"COMENT"
    };

    public const int COLON = 5;
    public const int WS = 11;
    public const int COMENT = 18;
    public const int COMMA = 4;
    public const int LETTER = 14;
    public const int RCURLY = 7;
    public const int NUMBER = 13;
    public const int LCURLY = 6;
    public const int DIGIT = 12;
    public const int NL = 10;
    public const int MINUS = 9;
    public const int ID = 15;
    public const int DOT = 8;
    public const int EOF = -1;
    public const int OctalEscape = 16;
    public const int STRING = 17;

    // delegates
    // delegators



        public fbxParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public fbxParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();

             
       }
        

    override public string[] TokenNames {
		get { return fbxParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g"; }
    }



    // $ANTLR start "document"
    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:55:1: document returns [FBXDocument doc] : (dec= declaration )+ ;
    public FBXDocument document() // throws RecognitionException [1]
    {   
        FBXDocument doc = null;

        FBXDeclarationNode dec = null;


         doc = new FBXDocument();
        try 
    	{
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:57:2: ( (dec= declaration )+ )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:57:4: (dec= declaration )+
            {
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:57:4: (dec= declaration )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( (LA1_0 == ID) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:57:5: dec= declaration
            			    {
            			    	PushFollow(FOLLOW_declaration_in_document380);
            			    	dec = declaration();
            			    	state.followingStackPointer--;

            			    	 doc.Add(dec); 

            			    }
            			    break;

            			default:
            			    if ( cnt1 >= 1 ) goto loop1;
            		            EarlyExitException eee1 =
            		                new EarlyExitException(1, input);
            		            throw eee1;
            	    }
            	    cnt1++;
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whinging that label 'loop1' has no statements


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return doc;
    }
    // $ANTLR end "document"


    // $ANTLR start "declaration"
    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:60:1: declaration returns [FBXDeclarationNode dec] : id= ID COLON (dec1= list[id.Text] | dec2= objectDeclaration[id.Text] ) ;
    public FBXDeclarationNode declaration() // throws RecognitionException [1]
    {   
        FBXDeclarationNode dec = null;

        IToken id = null;
        FBXDeclarationNode dec1 = null;

        FBXDeclarationNode dec2 = null;


        try 
    	{
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:61:2: (id= ID COLON (dec1= list[id.Text] | dec2= objectDeclaration[id.Text] ) )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:61:4: id= ID COLON (dec1= list[id.Text] | dec2= objectDeclaration[id.Text] )
            {
            	id=(IToken)Match(input,ID,FOLLOW_ID_in_declaration401); 
            	Match(input,COLON,FOLLOW_COLON_in_declaration403); 
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:62:3: (dec1= list[id.Text] | dec2= objectDeclaration[id.Text] )
            	int alt2 = 2;
            	switch ( input.LA(1) ) 
            	{
            	case NUMBER:
            		{
            	    int LA2_1 = input.LA(2);

            	    if ( (LA2_1 == EOF || LA2_1 == COMMA || LA2_1 == RCURLY || LA2_1 == ID) )
            	    {
            	        alt2 = 1;
            	    }
            	    else if ( (LA2_1 == LCURLY) )
            	    {
            	        alt2 = 2;
            	    }
            	    else 
            	    {
            	        NoViableAltException nvae_d2s1 =
            	            new NoViableAltException("", 2, 1, input);

            	        throw nvae_d2s1;
            	    }
            	    }
            	    break;
            	case STRING:
            		{
            	    switch ( input.LA(2) ) 
            	    {
            	    case COMMA:
            	    	{
            	        int LA2_5 = input.LA(3);

            	        if ( (LA2_5 == STRING) )
            	        {
            	            int LA2_6 = input.LA(4);

            	            if ( (LA2_6 == EOF || LA2_6 == COMMA || LA2_6 == RCURLY || LA2_6 == ID) )
            	            {
            	                alt2 = 1;
            	            }
            	            else if ( (LA2_6 == LCURLY) )
            	            {
            	                alt2 = 2;
            	            }
            	            else 
            	            {
            	                NoViableAltException nvae_d2s6 =
            	                    new NoViableAltException("", 2, 6, input);

            	                throw nvae_d2s6;
            	            }
            	        }
            	        else if ( (LA2_5 == NUMBER || LA2_5 == ID) )
            	        {
            	            alt2 = 1;
            	        }
            	        else 
            	        {
            	            NoViableAltException nvae_d2s5 =
            	                new NoViableAltException("", 2, 5, input);

            	            throw nvae_d2s5;
            	        }
            	        }
            	        break;
            	    case EOF:
            	    case RCURLY:
            	    case ID:
            	    	{
            	        alt2 = 1;
            	        }
            	        break;
            	    case LCURLY:
            	    	{
            	        alt2 = 2;
            	        }
            	        break;
            	    	default:
            	    	    NoViableAltException nvae_d2s2 =
            	    	        new NoViableAltException("", 2, 2, input);

            	    	    throw nvae_d2s2;
            	    }

            	    }
            	    break;
            	case ID:
            		{
            	    alt2 = 1;
            	    }
            	    break;
            	case LCURLY:
            		{
            	    alt2 = 2;
            	    }
            	    break;
            		default:
            		    NoViableAltException nvae_d2s0 =
            		        new NoViableAltException("", 2, 0, input);

            		    throw nvae_d2s0;
            	}

            	switch (alt2) 
            	{
            	    case 1 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:63:4: dec1= list[id.Text]
            	        {
            	        	PushFollow(FOLLOW_list_in_declaration415);
            	        	dec1 = list(id.Text);
            	        	state.followingStackPointer--;

            	        	dec = dec1;

            	        }
            	        break;
            	    case 2 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:64:4: dec2= objectDeclaration[id.Text]
            	        {
            	        	PushFollow(FOLLOW_objectDeclaration_in_declaration427);
            	        	dec2 = objectDeclaration(id.Text);
            	        	state.followingStackPointer--;

            	        	dec = dec2;

            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return dec;
    }
    // $ANTLR end "declaration"


    // $ANTLR start "list"
    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:68:1: list[string id] returns [FBXDeclarationNode dec] : (tok= NUMBER | tok= STRING | tok= ID ) ( COMMA (tok= NUMBER | tok= STRING | tok= ID ) )* ;
    public FBXDeclarationNode list(string id) // throws RecognitionException [1]
    {   
        FBXDeclarationNode dec = null;

        IToken tok = null;

         
        	 	int numbers = 0;
        	 	int strings = 0;
        	 	List<string>list = new List<string>();	 	
        	 
        try 
    	{
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:74:2: ( (tok= NUMBER | tok= STRING | tok= ID ) ( COMMA (tok= NUMBER | tok= STRING | tok= ID ) )* )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:74:4: (tok= NUMBER | tok= STRING | tok= ID ) ( COMMA (tok= NUMBER | tok= STRING | tok= ID ) )*
            {
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:74:4: (tok= NUMBER | tok= STRING | tok= ID )
            	int alt3 = 3;
            	switch ( input.LA(1) ) 
            	{
            	case NUMBER:
            		{
            	    alt3 = 1;
            	    }
            	    break;
            	case STRING:
            		{
            	    alt3 = 2;
            	    }
            	    break;
            	case ID:
            		{
            	    alt3 = 3;
            	    }
            	    break;
            		default:
            		    NoViableAltException nvae_d3s0 =
            		        new NoViableAltException("", 3, 0, input);

            		    throw nvae_d3s0;
            	}

            	switch (alt3) 
            	{
            	    case 1 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:74:5: tok= NUMBER
            	        {
            	        	tok=(IToken)Match(input,NUMBER,FOLLOW_NUMBER_in_list460); 
            	        	numbers++;

            	        }
            	        break;
            	    case 2 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:74:30: tok= STRING
            	        {
            	        	tok=(IToken)Match(input,STRING,FOLLOW_STRING_in_list467); 
            	        	strings++;

            	        }
            	        break;
            	    case 3 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:74:55: tok= ID
            	        {
            	        	tok=(IToken)Match(input,ID,FOLLOW_ID_in_list474); 
            	        	strings++;

            	        }
            	        break;

            	}

            	list.Add(tok.Text);
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:75:8: ( COMMA (tok= NUMBER | tok= STRING | tok= ID ) )*
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);

            	    if ( (LA5_0 == COMMA) )
            	    {
            	        alt5 = 1;
            	    }


            	    switch (alt5) 
            		{
            			case 1 :
            			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:75:9: COMMA (tok= NUMBER | tok= STRING | tok= ID )
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_list489); 
            			    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:75:15: (tok= NUMBER | tok= STRING | tok= ID )
            			    	int alt4 = 3;
            			    	switch ( input.LA(1) ) 
            			    	{
            			    	case NUMBER:
            			    		{
            			    	    alt4 = 1;
            			    	    }
            			    	    break;
            			    	case STRING:
            			    		{
            			    	    alt4 = 2;
            			    	    }
            			    	    break;
            			    	case ID:
            			    		{
            			    	    alt4 = 3;
            			    	    }
            			    	    break;
            			    		default:
            			    		    NoViableAltException nvae_d4s0 =
            			    		        new NoViableAltException("", 4, 0, input);

            			    		    throw nvae_d4s0;
            			    	}

            			    	switch (alt4) 
            			    	{
            			    	    case 1 :
            			    	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:75:16: tok= NUMBER
            			    	        {
            			    	        	tok=(IToken)Match(input,NUMBER,FOLLOW_NUMBER_in_list494); 
            			    	        	numbers++;

            			    	        }
            			    	        break;
            			    	    case 2 :
            			    	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:75:41: tok= STRING
            			    	        {
            			    	        	tok=(IToken)Match(input,STRING,FOLLOW_STRING_in_list501); 
            			    	        	strings++;

            			    	        }
            			    	        break;
            			    	    case 3 :
            			    	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:75:66: tok= ID
            			    	        {
            			    	        	tok=(IToken)Match(input,ID,FOLLOW_ID_in_list508); 
            			    	        	strings++;

            			    	        }
            			    	        break;

            			    	}

            			    	list.Add(tok.Text);

            			    }
            			    break;

            			default:
            			    goto loop5;
            	    }
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements


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

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return dec;
    }
    // $ANTLR end "list"


    // $ANTLR start "objectDeclaration"
    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:93:1: objectDeclaration[string id] returns [FBXDeclarationNode dec] : ( (name= STRING ( COMMA type= STRING )? | index= NUMBER ) )? LCURLY (node= declaration )* RCURLY ;
    public FBXDeclarationNode objectDeclaration(string id) // throws RecognitionException [1]
    {   
        FBXDeclarationNode dec = null;

        IToken name = null;
        IToken type = null;
        IToken index = null;
        FBXDeclarationNode node = null;


        try 
    	{
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:94:2: ( ( (name= STRING ( COMMA type= STRING )? | index= NUMBER ) )? LCURLY (node= declaration )* RCURLY )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:94:4: ( (name= STRING ( COMMA type= STRING )? | index= NUMBER ) )? LCURLY (node= declaration )* RCURLY
            {
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:94:4: ( (name= STRING ( COMMA type= STRING )? | index= NUMBER ) )?
            	int alt8 = 2;
            	int LA8_0 = input.LA(1);

            	if ( (LA8_0 == NUMBER || LA8_0 == STRING) )
            	{
            	    alt8 = 1;
            	}
            	switch (alt8) 
            	{
            	    case 1 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:94:6: (name= STRING ( COMMA type= STRING )? | index= NUMBER )
            	        {
            	        	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:94:6: (name= STRING ( COMMA type= STRING )? | index= NUMBER )
            	        	int alt7 = 2;
            	        	int LA7_0 = input.LA(1);

            	        	if ( (LA7_0 == STRING) )
            	        	{
            	        	    alt7 = 1;
            	        	}
            	        	else if ( (LA7_0 == NUMBER) )
            	        	{
            	        	    alt7 = 2;
            	        	}
            	        	else 
            	        	{
            	        	    NoViableAltException nvae_d7s0 =
            	        	        new NoViableAltException("", 7, 0, input);

            	        	    throw nvae_d7s0;
            	        	}
            	        	switch (alt7) 
            	        	{
            	        	    case 1 :
            	        	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:94:8: name= STRING ( COMMA type= STRING )?
            	        	        {
            	        	        	name=(IToken)Match(input,STRING,FOLLOW_STRING_in_objectDeclaration542); 
            	        	        	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:94:20: ( COMMA type= STRING )?
            	        	        	int alt6 = 2;
            	        	        	int LA6_0 = input.LA(1);

            	        	        	if ( (LA6_0 == COMMA) )
            	        	        	{
            	        	        	    alt6 = 1;
            	        	        	}
            	        	        	switch (alt6) 
            	        	        	{
            	        	        	    case 1 :
            	        	        	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:94:21: COMMA type= STRING
            	        	        	        {
            	        	        	        	Match(input,COMMA,FOLLOW_COMMA_in_objectDeclaration545); 
            	        	        	        	type=(IToken)Match(input,STRING,FOLLOW_STRING_in_objectDeclaration549); 

            	        	        	        }
            	        	        	        break;

            	        	        	}


            	        	        }
            	        	        break;
            	        	    case 2 :
            	        	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:94:43: index= NUMBER
            	        	        {
            	        	        	index=(IToken)Match(input,NUMBER,FOLLOW_NUMBER_in_objectDeclaration559); 

            	        	        }
            	        	        break;

            	        	}

            	        	 
            	        			if(name!=null)
            	        			 	dec = new FBXObject(name.Text, type != null? type.Text : null);
            	        			 else 
            	        			 	dec = new FBXObject();
            	        		 	dec.Type =id;
            	        		 	if(index!=null)
            	        		 		((FBXObject)dec).Index = int.Parse(index.Text);
            	        		 

            	        }
            	        break;

            	}

            	Match(input,LCURLY,FOLLOW_LCURLY_in_objectDeclaration570); 
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:105:4: (node= declaration )*
            	do 
            	{
            	    int alt9 = 2;
            	    int LA9_0 = input.LA(1);

            	    if ( (LA9_0 == ID) )
            	    {
            	        alt9 = 1;
            	    }


            	    switch (alt9) 
            		{
            			case 1 :
            			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:105:10: node= declaration
            			    {
            			    	PushFollow(FOLLOW_declaration_in_objectDeclaration583);
            			    	node = declaration();
            			    	state.followingStackPointer--;

            			    	  
            			    		 	       if(dec == null)
            			    			 	{ 
            			    			 		 dec = new FBXObject();
            			    			 		dec.Type =id;
            			    			 	}
            			    		 	       ((FBXObject)dec).Add(node); 
            			    		 	

            			    }
            			    break;

            			default:
            			    goto loop9;
            	    }
            	} while (true);

            	loop9:
            		;	// Stops C# compiler whining that label 'loop9' has no statements

            	Match(input,RCURLY,FOLLOW_RCURLY_in_objectDeclaration599); 
            	 
            		 	if(dec == null) 
            		 	{
            		 		dec = new FBXObject(); 	 		
            		 		dec.Type =id;
            		 	}
            		 

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return dec;
    }
    // $ANTLR end "objectDeclaration"

    // Delegated rules


	private void InitializeCyclicDFAs()
	{
	}

 

    public static readonly BitSet FOLLOW_declaration_in_document380 = new BitSet(new ulong[]{0x0000000000008002UL});
    public static readonly BitSet FOLLOW_ID_in_declaration401 = new BitSet(new ulong[]{0x0000000000000020UL});
    public static readonly BitSet FOLLOW_COLON_in_declaration403 = new BitSet(new ulong[]{0x000000000002A040UL});
    public static readonly BitSet FOLLOW_list_in_declaration415 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_objectDeclaration_in_declaration427 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NUMBER_in_list460 = new BitSet(new ulong[]{0x0000000000000012UL});
    public static readonly BitSet FOLLOW_STRING_in_list467 = new BitSet(new ulong[]{0x0000000000000012UL});
    public static readonly BitSet FOLLOW_ID_in_list474 = new BitSet(new ulong[]{0x0000000000000012UL});
    public static readonly BitSet FOLLOW_COMMA_in_list489 = new BitSet(new ulong[]{0x000000000002A000UL});
    public static readonly BitSet FOLLOW_NUMBER_in_list494 = new BitSet(new ulong[]{0x0000000000000012UL});
    public static readonly BitSet FOLLOW_STRING_in_list501 = new BitSet(new ulong[]{0x0000000000000012UL});
    public static readonly BitSet FOLLOW_ID_in_list508 = new BitSet(new ulong[]{0x0000000000000012UL});
    public static readonly BitSet FOLLOW_STRING_in_objectDeclaration542 = new BitSet(new ulong[]{0x0000000000000050UL});
    public static readonly BitSet FOLLOW_COMMA_in_objectDeclaration545 = new BitSet(new ulong[]{0x0000000000020000UL});
    public static readonly BitSet FOLLOW_STRING_in_objectDeclaration549 = new BitSet(new ulong[]{0x0000000000000040UL});
    public static readonly BitSet FOLLOW_NUMBER_in_objectDeclaration559 = new BitSet(new ulong[]{0x0000000000000040UL});
    public static readonly BitSet FOLLOW_LCURLY_in_objectDeclaration570 = new BitSet(new ulong[]{0x0000000000008080UL});
    public static readonly BitSet FOLLOW_declaration_in_objectDeclaration583 = new BitSet(new ulong[]{0x0000000000008080UL});
    public static readonly BitSet FOLLOW_RCURLY_in_objectDeclaration599 = new BitSet(new ulong[]{0x0000000000000002UL});

}
