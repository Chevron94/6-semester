program PL0(input,output);
{$APPTYPE CONSOLE}

uses
  SysUtils,Windows;
(* PL/0 compile with Code generation *)
const
  NumberOfReservedWords = 11;        (* no. of reserved words *)
  IdentifierTableSize = 100;          (* length of IdentifierTable *)
  NumberDigits = 14;            (* max. no of digits in numbers *)
  IdentifierSize = 10;              (* length of identifiers *)
  CharSetSize = 128;      (* for ASCII character set *)
  NumberErrors = 31;          (* max. no. of errors *)
  MaximumAddress = 2048;          (* maximaum address *)
  MaximumLevelOfBlockDepth = 5;           (* maximium depth of block nesting *)
  CodeArraySize = 200;          (* size of Code array *)

type
  TSymbol =
    (S_Null,S_Identifier,S_Number,S_Plus,S_Minus,S_Multiply,S_Slash,S_Parity,
     S_Equal,S_Sharp,S_Less,S_LessOrEqual,S_Greater,S_GreaterOrEqual,S_LeftBracket,S_RightBracket,S_Comma,S_SemiColon,
     S_Point,S_Assign,S_Begin,S_End,S_If,S_Then,
     S_While,S_Do,S_Call,S_Constant,S_Variable,S_Procedure);
  alfa = packed array [1..IdentifierSize] of char;
  TObject = (O_Constant,O_Variable,O_Link,O_Procedure);
  TSymbolSet = set of TSymbol;
  TFunctionCode = (lit,opr,lod,sto,cal,int,jmp,jpc,sft,lfa,sfa,wra);
  TInstruction = packed record
    FunctionCode: TFunctionCode;
    Level: 0..MaximumLevelOfBlockDepth;
    Address: 0..MaximumAddress;
  end;
(*      SFT 0,Address             : скопировать значение с вершины стека на Address элементов
        WRA Level,Address         : Загрузить адрес переменной на вершину стека
        LFA Level,Address         : Загрузить значение из указанного адреса
        SFA Level,Address         : Записать значение по указанному адресу
*)

var
  LastReadedCharacter:char;            (* last character read *)
  LastReadedSymbol: TSymbol;          (* last TSymbol read *)
  LastReadedIdentifier: alfa;             (* last identifier read *)
  LastReadedNumber: integer;         (* last number read *)
  NumberOfCharacters: integer;          (* character count *)
  LineLength: integer;          (* Line length *)
  IndxOfTMPCharMassive:integer;
  ErrorCode: integer;
  CountParams:Integer;
  CodeAllocationIndex: integer;          (* Code allocation index *)
  Line: array[1..2048] of char;
  TMPCharMassive: alfa;
  Code: array[0..CodeArraySize] of TInstruction;
  TextReservedWords: array[1..NumberOfReservedWords] of alfa;
  SymbolReservedWords: array[1..NumberOfReservedWords] of TSymbol;
  ReservedSymbols: array [char] of TSymbol;
  TextFunctionInstructions: array[TFunctionCode] of packed array [1..5] of char;
  DeclarationBeginSymbolsSet: TSymbolSet;
  StartBeginSymbolsSet: TSymbolSet;
  FactorBeginSymbolsSet: TSymbolSet;
  IdentifierTable: array [0..IdentifierTableSize] of
    record
      Name: alfa;
      case Kind: TObject of
      O_Constant: (Value: integer);
      O_Variable, O_Link, O_Procedure:(Level,CodeStartAddress,DataSegmentSize,CountParams:integer; Used:Boolean);
    end;
  InputFile,OutputFile: text;

procedure error(n: integer);
begin
  Write('Ошибка № ',n,': ');
  case n of
    1:
      begin
        writeln(OutputFile,'= вместо :=');
        Writeln('= вместо :=');
      end;
    2:
      begin
        writeln(OutputFile,'Нет числа после =');
        Writeln('Нет числа после =');
      end;
    3:
      begin
        writeln(OutputFile,'Нет = после идентификатора');
        Writeln('Нет = после идентификатора');
      end;
    4:
      begin
        writeln(OutputFile,'Нет идентификатора после const, var, procedure');
        Writeln('Нет идентификатора после const, var, procedure');
      end;
    5:
      begin
        writeln(OutputFile,'Пропущена запятая или точка с запятой');
        Writeln('Пропущена запятая или точка с запятой');
      end;
    6:
      begin
        writeln(OutputFile,'Неверный символ после описания процедуры');
        Writeln('Неверный символ после описания процедуры');
      end;
    7:
      begin
        writeln(OutputFile,'Нет оператора');
        Writeln('Нет оператора');
      end;
    8:
      begin
        writeln(OutputFile,'Неверный символ после операторной части блока');
        Writeln('Неверный символ после операторной части блока');
      end;
    9:
      begin
        writeln(OutputFile,'Нет многоточия');
        Writeln('Нет многоточия');
      end;
    10:
      begin
        writeln(OutputFile,'Пропущена точка с запятой между операторами');
        Writeln('Пропущена точка с запятой между операторами');
      end;
    11:
      begin
        writeln(OutputFile,'Неописанный идентификатор');
        Writeln('Неописанный идентификатор');
      end;
    12:
      begin
        writeln(OutputFile,'Недопустимое присваивание константе или процедуре.');
        Writeln('Недопустимое присваивание константе или процедуре.');
      end;
    13:
      begin
        writeln(OutputFile,'Требуется :=');
        Writeln('Требуется :=');
      end;
    14:
      begin
        writeln(OutputFile,'Нет идентификатора после call');
        Writeln('Нет идентификатора после call');
      end;
    15:
      begin
        writeln(OutputFile,'Вылов константы или переменной вместо процедуры');
        Writeln('Вылов константы или переменной вместо процедуры');
      end;
    16:
      begin
        writeln(OutputFile,'Требуется then');
        Writeln('Требуется then');
      end;
    17:
      begin
        writeln(OutputFile,'Требуется точка с запятой или end');
        Writeln('Требуется точка с запятой или end');
      end;
    18:
      begin
        writeln(OutputFile,'Требуется do');
        Writeln('Требуется do');
      end;
    19:
      begin
        writeln(OutputFile,'Неверный символ после оператора');
        Writeln('Неверный символ после оператора');
      end;
    20:
      begin
        writeln(OutputFile,'Требуется сравнение');
        Writeln('Требуется сравнение');
      end;
    21:
      begin
        writeln(OutputFile,'Выражение содержит идентификатор процедуры');
        Writeln('Выражение содержит идентификатор процедуры');
      end;
    22:
      begin
        writeln(OutputFile,'Отсутствует правая скобка');
        Writeln('Отсутствует правая скобка');
      end;
    23:
      begin
        writeln(OutputFile,'Неверный символ после множителя');
        Writeln('Неверный символ после множителя');
      end;
    24:
      begin
        writeln(OutputFile,'Неверный символ в начале выражения');
        Writeln('Неверный символ в начале выражения');
      end;
    30:
      begin
        writeln(OutputFile,'Слишком большое число');
        Writeln('Слишком большое число');
      end;
    31:
      begin
        writeln(OutputFile,'Отсутствует левая, или правая скобка');
        Writeln('Отсутствует левая, или правая скобка');
      end;
    32:
      begin
        writeln(OutputFile,'Ошибка при перечислении аргументов процедуры');
        Writeln('Ошибка при перечислении аргументов процедуры');
      end;
    33:
      begin
        writeln(OutputFile,'По ссылке может передаваться только переменная');
        Writeln('По ссылке может передаваться только переменная');
      end;
  end;
  ErrorCode := ErrorCode + 1;
  if ErrorCode > NumberErrors
    then halt
end(* error *);

procedure listall;
var i:integer;
begin(* list all the code generated for the program *)
  writeln('All the PL/0 object code:');
  writeln(OutputFile,'All the PL/0 object code:');
  for i:=0 to CodeAllocationIndex-1 do
    with Code[i] do
     begin
      writeln(i:3,TextFunctionInstructions[FunctionCode]:5,Level:3,Address:5);
      writeln(OutputFile,i:3,TextFunctionInstructions[FunctionCode]:5,Level:3,Address:5)
     end;
end (*listall*);

procedure getsym;
  var i,j,k:integer;
  procedure getch;
  begin
    if NumberOfCharacters = LineLength
      then
        begin
          if eof(InputFile)
            then
              begin
                write('program incomplete');
                halt
              end;
          LineLength:= 0; NumberOfCharacters:= 0;
          while not (eoln(InputFile)) do
          begin
            LineLength:=LineLength+1;
            read(InputFile,LastReadedCharacter);
            write(LastReadedCharacter);
            write(OutputFile,LastReadedCharacter);
            if (Ord(LastReadedCharacter) <> 10)
              then Line[LineLength]:= LastReadedCharacter
              else LineLength:=LineLength-1;

          end;
          writeln;
          writeln(OutputFile);
          LineLength:=LineLength+1;
          readln(InputFile);
          Line[LineLength]:=' '
        end;
    NumberOfCharacters:=NumberOfCharacters+1;
    LastReadedCharacter:=Line[NumberOfCharacters]
  end(* getch *);

(* getsym *)
begin (* getsym *)
  while LastReadedCharacter=' ' do getch;
  if LastReadedCharacter in ['a'..'z']
    then
      begin (* identifier or reserved word *) k:=0;
        repeat
          if k < IdentifierSize
            then
              begin
                k:=k+1;
                TMPCharMassive[k]:=LastReadedCharacter
              end;
          getch
        until not (LastReadedCharacter in ['a'..'z','0'..'9']);
        if k>=IndxOfTMPCharMassive
          then IndxOfTMPCharMassive:=k
          else
            repeat
              TMPCharMassive[IndxOfTMPCharMassive]:=' ';
              IndxOfTMPCharMassive:=IndxOfTMPCharMassive-1
            until IndxOfTMPCharMassive=k;
        LastReadedIdentifier:=TMPCharMassive; i:=1; j:=NumberOfReservedWords;
        repeat
          k:=(i+j)div 2;
          if LastReadedIdentifier<=TextReservedWords[k]
            then j:=k-1;
          if LastReadedIdentifier>=TextReservedWords[k]
            then i:=k+1
        until i>j;
        if i-1>j
          then LastReadedSymbol:=SymbolReservedWords[k]
          else LastReadedSymbol:=S_Identifier
      end
    else
      if LastReadedCharacter in ['0'..'9']
        then
          begin (* S_Number *)
            k:=0;
            LastReadedNumber:=0;
            LastReadedSymbol:=S_Number;
            repeat
              LastReadedNumber:=10*LastReadedNumber+(ord(LastReadedCharacter)-ord('0'));
              k:=k+1;
              getch
            until not (LastReadedCharacter in ['0'..'9']);
            if k> NumberDigits
              then error(30)
          end
        else
          if LastReadedCharacter=':'
            then
              begin getch;
                if LastReadedCharacter='='
                  then
                    begin
                      LastReadedSymbol:=S_Assign;
                      getch
                    end
                  else LastReadedSymbol:=S_Null;
              end
            else
              if LastReadedCharacter='<'
                then
                  begin
                    getch;
                    if LastReadedCharacter='='
                      then
                        begin
                          LastReadedSymbol:=S_LessOrEqual;
                          getch
                        end
                      else LastReadedSymbol:=S_Less
                  end
                else
                  if LastReadedCharacter='>'
                    then
                      begin
                        getch;
                        if LastReadedCharacter='='
                          then
                            begin
                              LastReadedSymbol:=S_GreaterOrEqual;
                              getch
                            end
                          else LastReadedSymbol:=S_Greater
                      end
                    else
                      begin
                        LastReadedSymbol:=ReservedSymbols[LastReadedCharacter];
                        getch
                      end
end(* getsym *);

procedure gen(x:TFunctionCode;y,z:integer);
begin
  if CodeAllocationIndex>CodeArraySize
    then
      begin
        write(' program too long');
        halt
      end;
  with Code[CodeAllocationIndex] do
  begin
    FunctionCode:=x;
    Level:=y;
    Address:=z
  end;
   CodeAllocationIndex:=CodeAllocationIndex+1;
end(* gen *);

procedure test(s1,s2:TSymbolSet; n:integer);
begin
  if not(LastReadedSymbol in s1)
    then
      begin
        error(n);
        s1:=s1+s2;
        while not(LastReadedSymbol in s1) do getsym
      end
end(*test*);

procedure block(CurrentLevel,TableIndex:integer;fsys:TSymbolSet);
var
  DataAllocationIndex:integer;       (*data allocation index*)
  InitialTableIndex:integer;       (*initial IdentifierTable index*)
  InitialCodeIndex:integer;       (*initial Code index*)
  ParamsCount:Integer;
  DataAllocationIndexTMP:Integer;
  ProcAddr:Integer;
  i,j:Integer;
  Shift:Integer;

  procedure enter(k:TObject; CParams:Integer = 0);
  begin (*enter object into IdentifierTable*)
    TableIndex:=TableIndex+1;
    with IdentifierTable[TableIndex] do
    begin
      Name:=LastReadedIdentifier;
      Kind:=k;
      case k of
        O_Constant:
          begin
            if LastReadedNumber>MaximumAddress
              then
                begin
                  error(31);
                  LastReadedNumber:=0
                end;
                Value:=LastReadedNumber
          end;
        O_Variable:
          begin
            Level:=CurrentLevel;
            CodeStartAddress:=DataAllocationIndex;
            DataAllocationIndex:=DataAllocationIndex+1;
          end;
        O_Link:
          begin
            Level:=CurrentLevel;
            CodeStartAddress:=DataAllocationIndex;
            DataAllocationIndex:=DataAllocationIndex+1;
          end;
        O_Procedure:
          begin
            Level:=CurrentLevel;
            CountParams:=CParams;
          end
      end
    end
  end(*enter*);

  function position(LastReadedIdentifier:alfa):integer;
  var
    i:integer;
  begin(* find identifier LastReadedIdentifier in IdentifierTable *)
    IdentifierTable[0].Name:=LastReadedIdentifier;
    i:=TableIndex;
    while IdentifierTable[i].Name<>LastReadedIdentifier do i:=i-1;
    position:=i
  end(* position *);

  procedure constdeclaration;
  begin
    if LastReadedSymbol=S_Identifier
      then
        begin
          getsym;
          if LastReadedSymbol in [S_Equal,S_Assign]
            then
              begin
                if LastReadedSymbol=S_Assign
                  then error(1);
                getsym;
                if LastReadedSymbol=S_Number
                  then
                    begin
                      enter(O_Constant);
                      getsym
                    end
                  else error(2)
              end
            else error(3)
        end
      else error(4)
  end (* constdeclaration *);

  procedure vardeclaration(Link:Boolean = False);
  begin
    if LastReadedSymbol=S_Identifier
      then
        begin
          if (not Link)
            then enter(O_Variable)
            else enter(O_Link);
          getsym
        end
      else
      error(4)
  end(* vardeclaration *);

  procedure listcode;
  var i:integer;
  begin(*list Code generated for this block*)
    for i:=InitialCodeIndex to CodeAllocationIndex-1 do
    with Code[i] do
    begin
      writeln(i,TextFunctionInstructions[FunctionCode]:5,Level:3,Address:5);
      writeln(OutputFile,i,TextFunctionInstructions[FunctionCode]:5,Level:3,Address:5)
    end;
  end (*listcode*);

  procedure statement(fsys:TSymbolSet);
  var i,cx1,cx2:integer;

    procedure expression(fsys:TSymbolSet);
    var addop:TSymbol;

      procedure term(fsys:TSymbolSet);
      var mulop:TSymbol;

        procedure factor(fsys:TSymbolSet);
        var i:integer;
        begin
          test(FactorBeginSymbolsSet,fsys,24);
          while LastReadedSymbol in FactorBeginSymbolsSet do
          begin
            if LastReadedSymbol=S_Identifier
              then
                begin
                  i:=position(LastReadedIdentifier);
                  if i=0
                    then error(11)
                    else
                      with IdentifierTable[i] do
                      case Kind of
                        O_Constant: gen(lit,0,Value);
                        O_Variable: gen(lod,CurrentLevel-Level,CodeStartAddress);
                        O_Link: gen(lfa,CurrentLevel-Level,CodeStartAddress);
                        O_Procedure: error(21)
                      end;
                  getsym
                end
              else
                if LastReadedSymbol=S_Number
                  then
                    begin
                      if LastReadedNumber>MaximumAddress
                        then
                          begin
                            error(31);
                            LastReadedNumber:=0
                          end;
                      gen(lit,0,LastReadedNumber);
                      getsym
                    end
                  else
            if LastReadedSymbol=S_LeftBracket
              then
                begin
                  getsym;
                  expression([S_RightBracket]+fsys);
                  if LastReadedSymbol=S_RightBracket
                    then getsym
                    else error(22)
                end;
            test(fsys,[S_LeftBracket],23)
          end
        end(*factor*);

(* term *)
      begin(* term *)
        factor(fsys+[S_Multiply,S_Slash]);
        while LastReadedSymbol in [S_Multiply,S_Slash] do
        begin
          mulop:=LastReadedSymbol;
          getsym;
          factor(fsys+[S_Multiply,S_Slash]);
          if mulop=S_Multiply
            then gen(opr,0,4)
            else gen(opr,0,5)
        end
      end(* term *);

(* expression *)
    begin (* expression *)
      if LastReadedSymbol in [S_Plus,S_Minus]
        then
          begin
            addop:=LastReadedSymbol;
            getsym;
            term(fsys+[S_Plus,S_Minus]);
            if addop=S_Minus
              then gen(opr,0,1)
          end
        else term(fsys+[S_Plus,S_Minus]);
      while LastReadedSymbol in [S_Plus,S_Minus] do
      begin
        addop:=LastReadedSymbol;
        getsym;
        term(fsys+[S_Plus,S_Minus]);
        if addop=S_Plus
          then gen(opr,0,2)
          else gen(opr,0,3)
      end
    end(*expression*);

    procedure condition(fsys:TSymbolSet);
    var relop:TSymbol;
    begin
      if LastReadedSymbol=S_Parity
        then
          begin
            getsym;
            expression(fsys);
            gen(opr,0,6)
          end
        else
          begin
            expression([S_Equal,S_Sharp,S_Less,S_Greater,S_LessOrEqual,S_GreaterOrEqual]+fsys);
            if not(LastReadedSymbol in [S_Equal,S_Sharp,S_Less,S_LessOrEqual,S_Greater,S_GreaterOrEqual])
              then error(20)
              else
                begin
                  relop:=LastReadedSymbol;
                  getsym;
                  expression(fsys);
                  case relop of
                    S_Equal:gen(opr,0,8);
                    S_Sharp:gen(opr,0,9);
                    S_Less:gen(opr,0,10);
                    S_GreaterOrEqual:gen(opr,0,11);
                    S_Greater:gen(opr,0,12);
                    S_LessOrEqual:gen(opr,0,13);
                  end
                end
          end
    end(*condition*);

(*statement*)
  begin(*statement*)
    if not(LastReadedSymbol in fsys+[S_Identifier])
      then
        begin
          error(10);
          repeat
            getsym
          until LastReadedSymbol in fsys
        end;
    if LastReadedSymbol=S_Identifier
      then
        begin
          i:=position(LastReadedIdentifier);
          if i=0
            then error(11)
            else
              if (IdentifierTable[i].Kind<>O_Variable) and (IdentifierTable[i].Kind<>O_Link)
                then
                  begin (*assignment to non-O_Variable*)
                    error(12);
                    i:=0
                  end;
          getsym;
          if LastReadedSymbol=S_Assign
            then getsym
            else error(13);
          expression(fsys);
          if i<>0
            then
              with IdentifierTable[i] do
              begin
                if (Kind = O_Variable)
                  then gen(sto,CurrentLevel-Level,CodeStartAddress)
                  else gen(sfa,CurrentLevel-Level,CodeStartAddress);
              end;
        end
      else
        if LastReadedSymbol=S_Call
          then
            begin
              getsym;
              if LastReadedSymbol<>S_Identifier
                then error(14)
                else
                  begin          // Начало моего кода. Вызов процедуры!
                    i:=position(LastReadedIdentifier);
                    if i=0
                      then error(11)
                      else
                        with IdentifierTable[i] do
                          if Kind=O_Procedure
                            then
                              begin
                                if (IdentifierTable[i].CountParams = 0)
                                  then
                                    begin
                                      gen(cal,CurrentLevel-Level,CodeStartAddress);
                                      getsym();
                                      if (LastReadedSymbol = S_LeftBracket)
                                        then
                                          begin
                                            getsym();
                                            if (LastReadedSymbol <> S_RightBracket)
                                              then error(31)
                                              else getsym();
                                          end;
                                    end
                                  else
                                    begin
                                      getsym();
                                      if (LastReadedSymbol <> S_LeftBracket)
                                        then error(31)
                                        else
                                          begin //++++++++++++++++++++++++++++++++
                                            ParamsCount:=0;
                                            Shift:=3;
                                            repeat
                                              getsym();
                                              if (IdentifierTable[i+ParamsCount+1].Kind = O_Variable)
                                                then
                                                  begin
                                                    expression(fsys+[S_Comma,S_RightBracket]);
                                                    gen(sft,0,Shift+IdentifierTable[i].CountParams);
                                                  end
                                                else
                                                  if (IdentifierTable[i+ParamsCount+1].Kind = O_Link)
                                                    then
                                                      begin
                                                        j := position(LastReadedIdentifier);
                                                        if ((IdentifierTable[j].Kind = O_Constant) or (IdentifierTable[j].Kind = O_Procedure) or (LastReadedSymbol <> S_Identifier))
                                                          then error(33)
                                                          else
                                                            begin
                                                              if (IdentifierTable[j].Kind = O_Variable)
                                                              then
                                                                begin
                                                                  gen(wra,CurrentLevel-IdentifierTable[j].Level,IdentifierTable[j].CodeStartAddress);
                                                                  gen(sft,0,Shift+IdentifierTable[i].CountParams);
                                                                end
                                                              else
                                                                begin
                                                                  gen(lod,CurrentLevel-IdentifierTable[i+ParamsCount+1].Level,IdentifierTable[i+ParamsCount+1].CodeStartAddress);
                                                                  gen(sft,0,Shift+IdentifierTable[i].CountParams);
                                                                end;
                                                            end;
                                                        //Shift:=Shift+1;
                                                        getsym();
                                                      end;
                                              Inc(ParamsCount);
                                              if (LastReadedSymbol <> S_Comma) and (ParamsCount <> IdentifierTable[i].CountParams )
                                                then
                                                  begin
                                                    CountParams:=-1;
                                                    error(32);
                                                  end
                                            until (ParamsCount = IdentifierTable[i].CountParams) or (ParamsCount = -1);

                                            if (LastReadedSymbol <> S_RightBracket)
                                              then error(31)
                                              else
                                                begin
                                                  getsym();
                                                  if (LastReadedSymbol <> S_SemiColon)
                                                    then error(5)
                                                    else  gen(cal,CurrentLevel-Level,CodeStartAddress);
                                                end;
                                           //++++++++++++++++++++++++++++++++++++
                                          end;
                                    end;
                              end
                            else error(15);
                    //getsym
                  end   //Конец.
            end
            else
    if LastReadedSymbol=S_If
      then
        begin
          getsym;
          condition([S_Then,S_Do]+fsys);
          if LastReadedSymbol=S_Then
            then getsym
            else error(16);
          cx1:=CodeAllocationIndex;
          gen(jpc,0,0);
          statement(fsys);
          Code[cx1].Address:=CodeAllocationIndex
        end
      else
        if LastReadedSymbol=S_Begin
          then
            begin
              getsym;
              statement([S_SemiColon,S_End]+fsys);
              while LastReadedSymbol in [S_SemiColon]+StartBeginSymbolsSet do
              begin
                if LastReadedSymbol=S_SemiColon
                  then getsym
                  else error(10);
                statement([S_SemiColon,S_End]+fsys)
              end;
              if LastReadedSymbol=S_End
                then getsym
                else error(17)
            end
          else
    if LastReadedSymbol=S_While
      then
        begin
          cx1:=CodeAllocationIndex;
          getsym; condition([S_Do]+fsys);
          cx2:=CodeAllocationIndex; gen(jpc,0,0);
          if LastReadedSymbol=S_Do
            then getsym
            else error(18);
          statement(fsys);
          gen(jmp,0,cx1);
          Code[cx2].Address:=CodeAllocationIndex
        end;
    test(fsys,[],19)
  end(*statement*);

(*block*)
begin(*block*)
  DataAllocationIndex:=3;
  //  Проверяем на параметры функции
  if ((IdentifierTable[TableIndex].Level = CurrentLevel) and (TableIndex>0))
    then DataAllocationIndex:= IdentifierTable[TableIndex].CodeStartAddress+1;
  //
  InitialTableIndex:=TableIndex-CountParams;
  ////
  IdentifierTable[TableIndex-CountParams].CodeStartAddress:=CodeAllocationIndex;
  ////
  gen(jmp,0,0);
  if CurrentLevel>MaximumLevelOfBlockDepth
    then error(32);
  repeat
    if LastReadedSymbol=S_Constant
      then
        begin
          getsym;
          repeat
            constdeclaration;
            while LastReadedSymbol=S_Comma do
            begin
              getsym;
              constdeclaration
            end;
            if LastReadedSymbol=S_SemiColon
              then getsym
              else error(5)
          until LastReadedSymbol<>S_Identifier
        end;
    if LastReadedSymbol=S_Variable
      then
        begin
          getsym;
          repeat
            vardeclaration;
            while LastReadedSymbol=S_Comma do
            begin
              getsym;
              vardeclaration
            end;
            if LastReadedSymbol=S_SemiColon
              then getsym
              else error(5)
          until LastReadedSymbol<>S_Identifier;
        end;
    while LastReadedSymbol=S_Procedure do
    begin
      getsym;
      if LastReadedSymbol=S_Identifier
        then
          begin                 // Начало моего кода. Объявление процедуры!!!
            getsym();
            CountParams := 0;
            ProcAddr:=TableIndex+1;
            enter(O_Procedure, CountParams);
            //Сохраняем адрес на текущем уровне
            DataAllocationIndexTMP:=DataAllocationIndex;
            DataAllocationIndex:=3;
            //Для сохранения параметров функции сбрасываем значение DAI
            if (LastReadedSymbol = S_LeftBracket)
              then
                begin
                  getsym();
                  if (LastReadedSymbol <> S_RightBracket)
                  then
                    begin
                      CurrentLevel:=CurrentLevel+1;
                      if (LastReadedSymbol = S_Variable)
                        then
                          begin
                            getsym();
                            vardeclaration(true);
                          end
                        else vardeclaration();
                      CountParams:=CountParams+1;
                      while LastReadedSymbol=S_Comma do
                      begin
                        getsym;
                        if (LastReadedSymbol = S_Variable)
                        then
                          begin
                            getsym();
                            vardeclaration(true);
                          end
                        else vardeclaration();
                        CountParams := CountParams+1;
                      end;

                      IdentifierTable[ProcAddr].CountParams:=CountParams;

                      if LastReadedSymbol<>S_RightBracket
                        then error(5);
                      CurrentLevel:=CurrentLevel-1;
                    end;
                    getsym();
                end
          end        //Конец моего блока. Дальше я ничего не трогал.
        else error(4);

      if LastReadedSymbol=S_SemiColon
        then getsym
        else error(5);

      block(CurrentLevel+1,TableIndex,[S_SemiColon]+fsys);
      DataAllocationIndex:=DataAllocationIndexTMP;

      if LastReadedSymbol=S_SemiColon
        then
          begin
            getsym;
            test(StartBeginSymbolsSet+[S_Identifier,S_Procedure],fsys,6)
          end
      else error(5)
    end;
    test(StartBeginSymbolsSet+[S_Identifier],DeclarationBeginSymbolsSet,7)
  until not(LastReadedSymbol in DeclarationBeginSymbolsSet);
  ///////////////////
  Code[IdentifierTable[InitialTableIndex].CodeStartAddress].Address:=CodeAllocationIndex;//+CountParams-1;
  ///////////////////
  with IdentifierTable[InitialTableIndex] do
  begin
    CodeStartAddress:=CodeAllocationIndex; (* start Address of Code*)
    DataSegmentSize:=DataAllocationIndex; (*size of data segment*)
  end;

  InitialCodeIndex:=CodeAllocationIndex;
  gen(int,0,DataAllocationIndex);
  statement([S_SemiColon,S_End]+fsys);
  gen(opr,0,0); (*return*)
  test(fsys,[],8);
  //listcode;
end(*block*);

procedure interpret;
const stacksize=100*MaximumLevelOfBlockDepth;
var
  p,b,t:integer; (*program-,base-,topstack-registers*)
  i:TInstruction; (*TInstruction register*)
  s:array[1..stacksize] of integer; (*datastore*)

  function base(Level:integer):integer;
  var b1:integer;
  begin
    b1:=b;  (*find base Level levels down*)
    while Level>0 do
    begin
      b1:=s[b1];
      Level:=Level-1
    end;
    base:=b1
  end(*base*);

(* interpret *)
begin
  writeln('Start PL/0');
  writeln(OutputFile,'Start PL/0');
  t:=0;
  b:=1;
  p:=0;
  s[1]:=0;
  s[2]:=0;
  s[3]:=0;
  repeat
    i:=Code[p];
    p:=p+1;
    with i do
      case FunctionCode of
        lit:
          begin
            t:=t+1;
            s[t]:=Address
          end;
        sft:  //Сместить значение на Address елементов
          begin
            s[t+Address]:=s[t];
            //s[t]:=s[t-Address];
            Writeln(s[t], ' запись значения или адреса переменной в аргумент процедуры');
            writeln(OutputFile,s[t]);
          end;
        sfa:         //Записсать значение по указанному адресу
          begin
            s[s[base(Level)+Address]]:=s[t];
            writeln(s[t],' запись значения по адресу ',s[base(Level)+Address]);
            writeln(OutputFile,s[t]);
            t:=t-1
          end;
        wra:        // Загрузить адрес переменной на вершину стека
          begin
            t:=t+1;
            s[t]:=base(Level)+Address;
          end;
        lfa:       //Загрузить значение из указанного адреса
          begin
            t:=t+1;
            s[t]:=s[s[base(Level)+Address]];
          end;
        opr:
          case Address of        
            0:
              begin
                t:=b-1;
                p:=s[t+3];
                b:=s[t+2];
              end;
            1:
              s[t]:=-s[t];
            2:
              begin
                t:=t-1;
                s[t]:=s[t]+s[t+1]
              end;
            3:
              begin
                t:=t-1;
                s[t]:=s[t]-s[t+1]
              end;
            4:
              begin
                t:=t-1;
                s[t]:=s[t]*s[t+1]
              end;
            5:
              begin
                t:=t-1;
                s[t]:=s[t] div s[t+1]
              end;
            6:
              s[t]:=ord(odd(s[t]));
            8:
              begin
                t:=t-1;
                s[t]:=ord(s[t]=s[t+1])
              end;
            9:
              begin
                t:=t-1;
                s[t]:=ord(s[t]<>s[t+1])
              end;
            10:
              begin
                t:=t-1;
                s[t]:=ord(s[t]<s[t+1])
              end;
            11:
              begin
                t:=t-1;
                s[t]:=ord(s[t]>=s[t+1])
              end;
            12:
              begin
                t:=t-1;
                s[t]:=ord(s[t]>s[t+1])
              end;
            13:
              begin
                t:=t-1;
                s[t]:=ord(s[t]<=s[t+1])
              end;
          end;
        lod:
          begin
            t:=t+1;
            s[t]:=s[base(Level)+Address]
          end;
        sto:
          begin
            s[base(Level)+Address]:=s[t];
            writeln(s[t],' сохранение значения в переменную');
            writeln(OutputFile,s[t]); t:=t-1
          end;
        cal:
          begin
              s[t+1]:=base(Level);
              s[t+2]:=b;
              s[t+3]:=p;
              b:=t+1;
              p:=Address;
          end;
        int:
          t:=t+Address;
        jmp:
          p:=Address;
        jpc:
          begin
            if s[t]=0
              then p:=Address;
            t:=t-1;
          end
      end;
  until p=0;
  write('End PL/0');
  write(OutputFile,'End PL/0');
end(*interpret*);

(* main program *)
begin(* main program *)
  SetConsoleCP(1251);
  SetConsoleOutputCP(1251);
  assign(InputFile,'testin.pl0');
  assign(OutputFile,'testout.txt');
  reset(InputFile);
  rewrite(OutputFile);
  for LastReadedCharacter:=chr(0) to chr(CharSetSize-1) do ReservedSymbols[LastReadedCharacter]:=S_Null;
  TextReservedWords[ 1]:='begin     '; TextReservedWords[ 2]:='call      ';
  TextReservedWords[ 3]:='const     '; TextReservedWords[ 4]:='do        ';
  TextReservedWords[ 5]:='end       '; TextReservedWords[ 6]:='if        ';
  TextReservedWords[ 7]:='odd       '; TextReservedWords[ 8]:='procedure ';
  TextReservedWords[ 9]:='then      '; TextReservedWords[10]:='var       ';
  TextReservedWords[11]:='while     ';
  SymbolReservedWords[ 1]:=S_Begin;
  SymbolReservedWords[ 2]:=S_Call;
  SymbolReservedWords[ 3]:=S_Constant;
  SymbolReservedWords[ 4]:=S_Do;
  SymbolReservedWords[ 5]:=S_End;
  SymbolReservedWords[ 6]:=S_If;
  SymbolReservedWords[ 7]:=S_Parity;
  SymbolReservedWords[ 8]:=S_Procedure;
  SymbolReservedWords[ 9]:=S_Then;
  SymbolReservedWords[10]:=S_Variable;
  SymbolReservedWords[11]:=S_While;
  ReservedSymbols['+']:=S_Plus;
  ReservedSymbols['-']:=S_Minus;
  ReservedSymbols['*']:=S_Multiply;
  ReservedSymbols['/']:=S_Slash;
  ReservedSymbols['(']:=S_LeftBracket;
  ReservedSymbols[')']:=S_RightBracket;
  ReservedSymbols['=']:=S_Equal;
  ReservedSymbols[',']:=S_Comma;
  ReservedSymbols['.']:=S_Point;
  ReservedSymbols['#']:=S_Sharp;
  ReservedSymbols['<']:=S_Less;
  ReservedSymbols['>']:=S_Greater;
  ReservedSymbols[';']:=S_SemiColon;
  TextFunctionInstructions[lit]:=' LIT ';
  TextFunctionInstructions[opr]:=' OPR ';
  TextFunctionInstructions[lod]:=' LOD ';
  TextFunctionInstructions[sto]:=' STO ';
  TextFunctionInstructions[cal]:=' CAL ';
  TextFunctionInstructions[int]:=' INT ';
  TextFunctionInstructions[jmp]:=' JMP ';
  TextFunctionInstructions[jpc]:=' JPC ';
  TextFunctionInstructions[sft]:=' SFT ';
  TextFunctionInstructions[sfa]:=' SFA ';
  TextFunctionInstructions[lfa]:=' LFA ';
  TextFunctionInstructions[wra]:=' WRA ';
  DeclarationBeginSymbolsSet:=[S_Constant,S_Variable,S_Procedure];
  StartBeginSymbolsSet:=[S_Begin,S_Call,S_If,S_While];
  FactorBeginSymbolsSet:=[S_Identifier,S_Number,S_LeftBracket];
  ErrorCode:=0;
  NumberOfCharacters:=0;
  CodeAllocationIndex:=0;
  LineLength:=0;
  LastReadedCharacter:=' ';
  IndxOfTMPCharMassive:=IdentifierSize;
  CountParams:=0;
  getsym;
  block(0,0,[S_Point]+DeclarationBeginSymbolsSet+StartBeginSymbolsSet);
  if LastReadedSymbol<>S_Point
    then error(9);
  if ErrorCode=0
    then
      begin
        listall;
        interpret;
      end
    else write('Errors in PL/0 program');
  writeln;
  writeln(OutputFile);
  close(OutputFile);
  Readln;
end.
