// $ANTLR 3.1.2 E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g 2012-09-26 21:27:43


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


public class fbxLexer : Lexer {
    public const int COLON = 5;
    public const int WS = 11;
    public const int COMENT = 18;
    public const int COMMA = 4;
    public const int LETTER = 14;
    public const int RCURLY = 7;
    public const int NUMBER = 13;
    public const int LCURLY = 6;
    public const int NL = 10;
    public const int DIGIT = 12;
    public const int MINUS = 9;
    public const int DOT = 8;
    public const int ID = 15;
    public const int EOF = -1;
    public const int OctalEscape = 16;
    public const int STRING = 17;

    // delegates
    // delegators

    public fbxLexer() 
    {
		InitializeCyclicDFAs();
    }
    public fbxLexer(ICharStream input)
		: this(input, null) {
    }
    public fbxLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g";} 
    }

    // $ANTLR start "COMMA"
    public void mCOMMA() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMA;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:7:7: ( ',' )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:7:9: ','
            {
            	Match(','); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMA"

    // $ANTLR start "COLON"
    public void mCOLON() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COLON;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:8:7: ( ':' )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:8:9: ':'
            {
            	Match(':'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COLON"

    // $ANTLR start "LCURLY"
    public void mLCURLY() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LCURLY;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:9:8: ( '{' )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:9:10: '{'
            {
            	Match('{'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LCURLY"

    // $ANTLR start "RCURLY"
    public void mRCURLY() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RCURLY;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:10:8: ( '}' )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:10:10: '}'
            {
            	Match('}'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RCURLY"

    // $ANTLR start "DOT"
    public void mDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:11:5: ( '.' )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:11:7: '.'
            {
            	Match('.'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOT"

    // $ANTLR start "MINUS"
    public void mMINUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MINUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:12:7: ( '-' )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:12:9: '-'
            {
            	Match('-'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MINUS"

    // $ANTLR start "NL"
    public void mNL() // throws RecognitionException [2]
    {
    		try
    		{
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:21:4: ( '\\r\\n' | '\\n' | '\\r' )
            int alt1 = 3;
            int LA1_0 = input.LA(1);

            if ( (LA1_0 == '\r') )
            {
                int LA1_1 = input.LA(2);

                if ( (LA1_1 == '\n') )
                {
                    alt1 = 1;
                }
                else 
                {
                    alt1 = 3;}
            }
            else if ( (LA1_0 == '\n') )
            {
                alt1 = 2;
            }
            else 
            {
                NoViableAltException nvae_d1s0 =
                    new NoViableAltException("", 1, 0, input);

                throw nvae_d1s0;
            }
            switch (alt1) 
            {
                case 1 :
                    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:21:5: '\\r\\n'
                    {
                    	Match("\r\n"); 


                    }
                    break;
                case 2 :
                    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:22:3: '\\n'
                    {
                    	Match('\n'); 

                    }
                    break;
                case 3 :
                    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:23:3: '\\r'
                    {
                    	Match('\r'); 

                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NL"

    // $ANTLR start "WS"
    public void mWS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:26:4: ( ( ' ' | '\\t' | NL ) )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:26:5: ( ' ' | '\\t' | NL )
            {
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:26:5: ( ' ' | '\\t' | NL )
            	int alt2 = 3;
            	switch ( input.LA(1) ) 
            	{
            	case ' ':
            		{
            	    alt2 = 1;
            	    }
            	    break;
            	case '\t':
            		{
            	    alt2 = 2;
            	    }
            	    break;
            	case '\n':
            	case '\r':
            		{
            	    alt2 = 3;
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
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:26:6: ' '
            	        {
            	        	Match(' '); 

            	        }
            	        break;
            	    case 2 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:26:10: '\\t'
            	        {
            	        	Match('\t'); 

            	        }
            	        break;
            	    case 3 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:26:16: NL
            	        {
            	        	mNL(); 

            	        }
            	        break;

            	}

            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WS"

    // $ANTLR start "DIGIT"
    public void mDIGIT() // throws RecognitionException [2]
    {
    		try
    		{
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:30:7: ( ( '0' .. '9' ) )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:30:8: ( '0' .. '9' )
            {
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:30:8: ( '0' .. '9' )
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:30:9: '0' .. '9'
            	{
            		MatchRange('0','9'); 

            	}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIGIT"

    // $ANTLR start "NUMBER"
    public void mNUMBER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NUMBER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:8: ( ( MINUS )? ( DIGIT )+ ( DOT ( DIGIT )+ )? ( 'e' ( MINUS )? ( DIGIT )+ )? )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:11: ( MINUS )? ( DIGIT )+ ( DOT ( DIGIT )+ )? ( 'e' ( MINUS )? ( DIGIT )+ )?
            {
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:11: ( MINUS )?
            	int alt3 = 2;
            	int LA3_0 = input.LA(1);

            	if ( (LA3_0 == '-') )
            	{
            	    alt3 = 1;
            	}
            	switch (alt3) 
            	{
            	    case 1 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:11: MINUS
            	        {
            	        	mMINUS(); 

            	        }
            	        break;

            	}

            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:18: ( DIGIT )+
            	int cnt4 = 0;
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);

            	    if ( ((LA4_0 >= '0' && LA4_0 <= '9')) )
            	    {
            	        alt4 = 1;
            	    }


            	    switch (alt4) 
            		{
            			case 1 :
            			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:18: DIGIT
            			    {
            			    	mDIGIT(); 

            			    }
            			    break;

            			default:
            			    if ( cnt4 >= 1 ) goto loop4;
            		            EarlyExitException eee4 =
            		                new EarlyExitException(4, input);
            		            throw eee4;
            	    }
            	    cnt4++;
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whinging that label 'loop4' has no statements

            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:25: ( DOT ( DIGIT )+ )?
            	int alt6 = 2;
            	int LA6_0 = input.LA(1);

            	if ( (LA6_0 == '.') )
            	{
            	    alt6 = 1;
            	}
            	switch (alt6) 
            	{
            	    case 1 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:26: DOT ( DIGIT )+
            	        {
            	        	mDOT(); 
            	        	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:30: ( DIGIT )+
            	        	int cnt5 = 0;
            	        	do 
            	        	{
            	        	    int alt5 = 2;
            	        	    int LA5_0 = input.LA(1);

            	        	    if ( ((LA5_0 >= '0' && LA5_0 <= '9')) )
            	        	    {
            	        	        alt5 = 1;
            	        	    }


            	        	    switch (alt5) 
            	        		{
            	        			case 1 :
            	        			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:30: DIGIT
            	        			    {
            	        			    	mDIGIT(); 

            	        			    }
            	        			    break;

            	        			default:
            	        			    if ( cnt5 >= 1 ) goto loop5;
            	        		            EarlyExitException eee5 =
            	        		                new EarlyExitException(5, input);
            	        		            throw eee5;
            	        	    }
            	        	    cnt5++;
            	        	} while (true);

            	        	loop5:
            	        		;	// Stops C# compiler whinging that label 'loop5' has no statements


            	        }
            	        break;

            	}

            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:39: ( 'e' ( MINUS )? ( DIGIT )+ )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);

            	if ( (LA9_0 == 'e') )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:40: 'e' ( MINUS )? ( DIGIT )+
            	        {
            	        	Match('e'); 
            	        	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:45: ( MINUS )?
            	        	int alt7 = 2;
            	        	int LA7_0 = input.LA(1);

            	        	if ( (LA7_0 == '-') )
            	        	{
            	        	    alt7 = 1;
            	        	}
            	        	switch (alt7) 
            	        	{
            	        	    case 1 :
            	        	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:45: MINUS
            	        	        {
            	        	        	mMINUS(); 

            	        	        }
            	        	        break;

            	        	}

            	        	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:52: ( DIGIT )+
            	        	int cnt8 = 0;
            	        	do 
            	        	{
            	        	    int alt8 = 2;
            	        	    int LA8_0 = input.LA(1);

            	        	    if ( ((LA8_0 >= '0' && LA8_0 <= '9')) )
            	        	    {
            	        	        alt8 = 1;
            	        	    }


            	        	    switch (alt8) 
            	        		{
            	        			case 1 :
            	        			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:33:52: DIGIT
            	        			    {
            	        			    	mDIGIT(); 

            	        			    }
            	        			    break;

            	        			default:
            	        			    if ( cnt8 >= 1 ) goto loop8;
            	        		            EarlyExitException eee8 =
            	        		                new EarlyExitException(8, input);
            	        		            throw eee8;
            	        	    }
            	        	    cnt8++;
            	        	} while (true);

            	        	loop8:
            	        		;	// Stops C# compiler whinging that label 'loop8' has no statements


            	        }
            	        break;

            	}


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NUMBER"

    // $ANTLR start "LETTER"
    public void mLETTER() // throws RecognitionException [2]
    {
    		try
    		{
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:37:8: ( ( 'a' .. 'z' ) | ( 'A' .. 'Z' ) )
            int alt10 = 2;
            int LA10_0 = input.LA(1);

            if ( ((LA10_0 >= 'a' && LA10_0 <= 'z')) )
            {
                alt10 = 1;
            }
            else if ( ((LA10_0 >= 'A' && LA10_0 <= 'Z')) )
            {
                alt10 = 2;
            }
            else 
            {
                NoViableAltException nvae_d10s0 =
                    new NoViableAltException("", 10, 0, input);

                throw nvae_d10s0;
            }
            switch (alt10) 
            {
                case 1 :
                    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:37:9: ( 'a' .. 'z' )
                    {
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:37:9: ( 'a' .. 'z' )
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:37:10: 'a' .. 'z'
                    	{
                    		MatchRange('a','z'); 

                    	}


                    }
                    break;
                case 2 :
                    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:37:20: ( 'A' .. 'Z' )
                    {
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:37:20: ( 'A' .. 'Z' )
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:37:21: 'A' .. 'Z'
                    	{
                    		MatchRange('A','Z'); 

                    	}


                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LETTER"

    // $ANTLR start "ID"
    public void mID() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ID;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:40:4: ( LETTER ( DIGIT | LETTER | '_' )* )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:40:6: LETTER ( DIGIT | LETTER | '_' )*
            {
            	mLETTER(); 
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:40:13: ( DIGIT | LETTER | '_' )*
            	do 
            	{
            	    int alt11 = 4;
            	    switch ( input.LA(1) ) 
            	    {
            	    case '0':
            	    case '1':
            	    case '2':
            	    case '3':
            	    case '4':
            	    case '5':
            	    case '6':
            	    case '7':
            	    case '8':
            	    case '9':
            	    	{
            	        alt11 = 1;
            	        }
            	        break;
            	    case 'A':
            	    case 'B':
            	    case 'C':
            	    case 'D':
            	    case 'E':
            	    case 'F':
            	    case 'G':
            	    case 'H':
            	    case 'I':
            	    case 'J':
            	    case 'K':
            	    case 'L':
            	    case 'M':
            	    case 'N':
            	    case 'O':
            	    case 'P':
            	    case 'Q':
            	    case 'R':
            	    case 'S':
            	    case 'T':
            	    case 'U':
            	    case 'V':
            	    case 'W':
            	    case 'X':
            	    case 'Y':
            	    case 'Z':
            	    case 'a':
            	    case 'b':
            	    case 'c':
            	    case 'd':
            	    case 'e':
            	    case 'f':
            	    case 'g':
            	    case 'h':
            	    case 'i':
            	    case 'j':
            	    case 'k':
            	    case 'l':
            	    case 'm':
            	    case 'n':
            	    case 'o':
            	    case 'p':
            	    case 'q':
            	    case 'r':
            	    case 's':
            	    case 't':
            	    case 'u':
            	    case 'v':
            	    case 'w':
            	    case 'x':
            	    case 'y':
            	    case 'z':
            	    	{
            	        alt11 = 2;
            	        }
            	        break;
            	    case '_':
            	    	{
            	        alt11 = 3;
            	        }
            	        break;

            	    }

            	    switch (alt11) 
            		{
            			case 1 :
            			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:40:14: DIGIT
            			    {
            			    	mDIGIT(); 

            			    }
            			    break;
            			case 2 :
            			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:40:20: LETTER
            			    {
            			    	mLETTER(); 

            			    }
            			    break;
            			case 3 :
            			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:40:27: '_'
            			    {
            			    	Match('_'); 

            			    }
            			    break;

            			default:
            			    goto loop11;
            	    }
            	} while (true);

            	loop11:
            		;	// Stops C# compiler whining that label 'loop11' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ID"

    // $ANTLR start "OctalEscape"
    public void mOctalEscape() // throws RecognitionException [2]
    {
    		try
    		{
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:45:5: ( ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | ( '0' .. '7' ) ( '0' .. '7' ) | ( '0' .. '7' ) )
            int alt12 = 3;
            int LA12_0 = input.LA(1);

            if ( ((LA12_0 >= '0' && LA12_0 <= '3')) )
            {
                int LA12_1 = input.LA(2);

                if ( ((LA12_1 >= '0' && LA12_1 <= '7')) )
                {
                    int LA12_3 = input.LA(3);

                    if ( ((LA12_3 >= '0' && LA12_3 <= '7')) )
                    {
                        alt12 = 1;
                    }
                    else 
                    {
                        alt12 = 2;}
                }
                else 
                {
                    alt12 = 3;}
            }
            else if ( ((LA12_0 >= '4' && LA12_0 <= '7')) )
            {
                int LA12_2 = input.LA(2);

                if ( ((LA12_2 >= '0' && LA12_2 <= '7')) )
                {
                    alt12 = 2;
                }
                else 
                {
                    alt12 = 3;}
            }
            else 
            {
                NoViableAltException nvae_d12s0 =
                    new NoViableAltException("", 12, 0, input);

                throw nvae_d12s0;
            }
            switch (alt12) 
            {
                case 1 :
                    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:45:10: ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:45:10: ( '0' .. '3' )
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:45:11: '0' .. '3'
                    	{
                    		MatchRange('0','3'); 

                    	}

                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:45:21: ( '0' .. '7' )
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:45:22: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}

                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:45:32: ( '0' .. '7' )
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:45:33: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}


                    }
                    break;
                case 2 :
                    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:46:10: ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:46:10: ( '0' .. '7' )
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:46:11: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}

                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:46:21: ( '0' .. '7' )
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:46:22: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}


                    }
                    break;
                case 3 :
                    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:47:10: ( '0' .. '7' )
                    {
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:47:10: ( '0' .. '7' )
                    	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:47:11: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}


                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OctalEscape"

    // $ANTLR start "STRING"
    public void mSTRING() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STRING;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:49:8: ( '\"' (~ ( '\"' ) )* '\"' )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:49:10: '\"' (~ ( '\"' ) )* '\"'
            {
            	Match('\"'); 
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:49:14: (~ ( '\"' ) )*
            	do 
            	{
            	    int alt13 = 2;
            	    int LA13_0 = input.LA(1);

            	    if ( ((LA13_0 >= '\u0000' && LA13_0 <= '!') || (LA13_0 >= '#' && LA13_0 <= '\uFFFF')) )
            	    {
            	        alt13 = 1;
            	    }


            	    switch (alt13) 
            		{
            			case 1 :
            			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:49:16: ~ ( '\"' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '!') || (input.LA(1) >= '#' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop13;
            	    }
            	} while (true);

            	loop13:
            		;	// Stops C# compiler whining that label 'loop13' has no statements

            	Match('\"'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "STRING"

    // $ANTLR start "COMENT"
    public void mCOMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:52:8: ( ';' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n' )
            // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:52:10: ';' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n'
            {
            	Match(';'); 
            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:52:14: (~ ( '\\n' | '\\r' ) )*
            	do 
            	{
            	    int alt14 = 2;
            	    int LA14_0 = input.LA(1);

            	    if ( ((LA14_0 >= '\u0000' && LA14_0 <= '\t') || (LA14_0 >= '\u000B' && LA14_0 <= '\f') || (LA14_0 >= '\u000E' && LA14_0 <= '\uFFFF')) )
            	    {
            	        alt14 = 1;
            	    }


            	    switch (alt14) 
            		{
            			case 1 :
            			    // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:52:14: ~ ( '\\n' | '\\r' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop14;
            	    }
            	} while (true);

            	loop14:
            		;	// Stops C# compiler whining that label 'loop14' has no statements

            	// E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:52:28: ( '\\r' )?
            	int alt15 = 2;
            	int LA15_0 = input.LA(1);

            	if ( (LA15_0 == '\r') )
            	{
            	    alt15 = 1;
            	}
            	switch (alt15) 
            	{
            	    case 1 :
            	        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:52:28: '\\r'
            	        {
            	        	Match('\r'); 

            	        }
            	        break;

            	}

            	Match('\n'); 
            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMENT"

    override public void mTokens() // throws RecognitionException 
    {
        // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:8: ( COMMA | COLON | LCURLY | RCURLY | DOT | MINUS | WS | NUMBER | ID | STRING | COMENT )
        int alt16 = 11;
        alt16 = dfa16.Predict(input);
        switch (alt16) 
        {
            case 1 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:10: COMMA
                {
                	mCOMMA(); 

                }
                break;
            case 2 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:16: COLON
                {
                	mCOLON(); 

                }
                break;
            case 3 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:22: LCURLY
                {
                	mLCURLY(); 

                }
                break;
            case 4 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:29: RCURLY
                {
                	mRCURLY(); 

                }
                break;
            case 5 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:36: DOT
                {
                	mDOT(); 

                }
                break;
            case 6 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:40: MINUS
                {
                	mMINUS(); 

                }
                break;
            case 7 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:46: WS
                {
                	mWS(); 

                }
                break;
            case 8 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:49: NUMBER
                {
                	mNUMBER(); 

                }
                break;
            case 9 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:56: ID
                {
                	mID(); 

                }
                break;
            case 10 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:59: STRING
                {
                	mSTRING(); 

                }
                break;
            case 11 :
                // E:\\DEVELOP\\Visual Studio Projects\\Vsual Studio 2010\\GameEngineSlimDX\\GEngine\\ResourcesManagers\\FBXParser\\fbx.g:1:66: COMENT
                {
                	mCOMENT(); 

                }
                break;

        }

    }


    protected DFA16 dfa16;
	private void InitializeCyclicDFAs()
	{
	    this.dfa16 = new DFA16(this);
	}

    const string DFA16_eotS =
        "\x06\uffff\x01\x0c\x06\uffff";
    const string DFA16_eofS =
        "\x0d\uffff";
    const string DFA16_minS =
        "\x01\x09\x05\uffff\x01\x30\x06\uffff";
    const string DFA16_maxS =
        "\x01\x7d\x05\uffff\x01\x39\x06\uffff";
    const string DFA16_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01\uffff\x01"+
        "\x07\x01\x08\x01\x09\x01\x0a\x01\x0b\x01\x06";
    const string DFA16_specialS =
        "\x0d\uffff}>";
    static readonly string[] DFA16_transitionS = {
            "\x02\x07\x02\uffff\x01\x07\x12\uffff\x01\x07\x01\uffff\x01"+
            "\x0a\x09\uffff\x01\x01\x01\x06\x01\x05\x01\uffff\x0a\x08\x01"+
            "\x02\x01\x0b\x05\uffff\x1a\x09\x06\uffff\x1a\x09\x01\x03\x01"+
            "\uffff\x01\x04",
            "",
            "",
            "",
            "",
            "",
            "\x0a\x08",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA16_eot = DFA.UnpackEncodedString(DFA16_eotS);
    static readonly short[] DFA16_eof = DFA.UnpackEncodedString(DFA16_eofS);
    static readonly char[] DFA16_min = DFA.UnpackEncodedStringToUnsignedChars(DFA16_minS);
    static readonly char[] DFA16_max = DFA.UnpackEncodedStringToUnsignedChars(DFA16_maxS);
    static readonly short[] DFA16_accept = DFA.UnpackEncodedString(DFA16_acceptS);
    static readonly short[] DFA16_special = DFA.UnpackEncodedString(DFA16_specialS);
    static readonly short[][] DFA16_transition = DFA.UnpackEncodedStringArray(DFA16_transitionS);

    protected class DFA16 : DFA
    {
        public DFA16(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 16;
            this.eot = DFA16_eot;
            this.eof = DFA16_eof;
            this.min = DFA16_min;
            this.max = DFA16_max;
            this.accept = DFA16_accept;
            this.special = DFA16_special;
            this.transition = DFA16_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( COMMA | COLON | LCURLY | RCURLY | DOT | MINUS | WS | NUMBER | ID | STRING | COMENT );"; }
        }

    }

 
    
}
