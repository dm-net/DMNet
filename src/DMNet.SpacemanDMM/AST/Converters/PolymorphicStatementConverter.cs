using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PolymorphicStatementConverter : CborConverterBase<Statement>
    {
        public override Statement Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseStatement(cborValue);
        }

        public static Statement ParseStatement(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            var obj = (CborObject)element;
            foreach (var prop in obj)
            {
                var valObj = prop.Value as CborObject;
                var valArr = prop.Value as CborArray;
                switch (prop.Key.Value<string>())
                {
                    case "Expr":
                        return new Statement.Expr()
                        {
                            Expression = PolymorphicExpressionConverter.ParseExpression(prop.Value)
                        };
                    case "Return":
                        return new Statement.Return()
                        {
                            Expression = PolymorphicExpressionConverter.ParseExpression(prop.Value)
                        };
                    case "Throw":
                        return new Statement.Throw()
                        {
                            Expression = PolymorphicExpressionConverter.ParseExpression(prop.Value)
                        };
                    case "While":
                        return new Statement.While()
                        {
                            Condition = PolymorphicExpressionConverter.ParseExpression(valObj["condition"]),
                            Block = BlockConverter.ParseBlock(valObj["block"])
                        };
                    case "DoWhile":
                        return new Statement.DoWhile()
                        {
                            Condition = ((CborObject)valObj["condition"]).ToObject<Spaned<Expression>>(),
                            Block = BlockConverter.ParseBlock(valObj["block"])
                        };
                    case "If":
                        var ifArms = (CborArray)valObj["arms"];
                        var ifArmsList = new List<Tuple<Spaned<Expression>, Block>>();
                        foreach (var item in ifArms)
                        {
                            var tuple = (CborArray)item;
                            var item1 = ((CborObject)tuple[0]).ToObject<Spaned<Expression>>();
                            var item2 = BlockConverter.ParseBlock(tuple[1]);
                            ifArmsList.Add(new Tuple<Spaned<Expression>, Block>(item1, item2));
                        }
                        return new Statement.If()
                        {
                            Arms = ifArmsList.ToArray(),
                            Else = BlockConverter.ParseBlock(valObj["else_arm"])
                        };
                    case "ForLoop":
                        return new Statement.ForLoop()
                        {
                            Init = ParseStatement(valObj["init"]),
                            Test = PolymorphicExpressionConverter.ParseExpression(valObj["test"]),
                            Inc = ParseStatement(valObj["inc"]),
                            Block = BlockConverter.ParseBlock(valObj["block"]),
                        };

                    case "ForList":
                        return new Statement.ForList()
                        {
                            VarType =  valObj["var_type"].ToObject<VarType>(),
                            Identifier = valObj["name"].Value<string>(),
                            InputType = FlagConverter<InputType>.ParseFlag(valObj["input_type"]),
                            List = PolymorphicExpressionConverter.ParseExpression(valObj["in_list"]),
                            Block = BlockConverter.ParseBlock(valObj["block"]),
                        };
                    case "ForRange":
                        return new Statement.ForRange()
                        {
                            VarType = valObj["var_type"].ToObject<VarType>(),
                            Identifier = valObj["name"].Value<string>(),
                            Start = PolymorphicExpressionConverter.ParseExpression(valObj["start"]),
                            End = PolymorphicExpressionConverter.ParseExpression(valObj["end"]),
                            Step = PolymorphicExpressionConverter.ParseExpression(valObj["step"]),
                            Block = BlockConverter.ParseBlock(valObj["block"]),
                        };
                    case "Var":
                        return new Statement.Var()
                        {
                            Value = valObj.ToObject<VarStatement>()
                        };
                    case "Vars":
                        return new Statement.Vars()
                        {
                            Value = valArr.ToCollection<VarStatement[]>()
                        };
                    case "Setting":
                        return new Statement.Setting()
                        {
                            Name = valObj["name"].Value<string>(),
                            Mode = Enum.Parse<SettingMode>(valObj["mode"].Value<string>()),
                            Value = PolymorphicExpressionConverter.ParseExpression(valObj["value"]),
                        };
                    case "Spawn":
                        return new Statement.Spawn()
                        {
                            Delay = PolymorphicExpressionConverter.ParseExpression(valObj["delay"]),
                            Block = BlockConverter.ParseBlock(valObj["block"]),
                        };
                    case "Switch":
                        var switchCases = (CborArray)valObj["cases"];
                        var cases = new List<Tuple<Spaned<Case[]>, Block>>();
                        foreach (var item in switchCases)
                        {
                            var tupleCase = (CborArray)item;
                            var caseValue = ((CborObject)tupleCase[0]).ToObject<Spaned<Case[]>>();
                            var blockValue = BlockConverter.ParseBlock(tupleCase[1]);
                            cases.Add(new Tuple<Spaned<Case[]>, Block>(caseValue, blockValue));
                        }
                        return new Statement.Switch()
                        {
                            Input = PolymorphicExpressionConverter.ParseExpression(valObj["input"]),
                            Cases = cases.ToArray(),
                            Default = BlockConverter.ParseBlock(valObj["default"]),
                        };
                    case "TryCatch":
                        return new Statement.TryCatch()
                        {
                            Try = BlockConverter.ParseBlock(valObj["try_block"]),
                            Params = ((CborArray)valObj["catch_params"]).ToCollection<string[][]>(),
                            Catch = BlockConverter.ParseBlock(valObj["catch_block"]),
                        };
                    // TryCatch

                    case "Continue":
                        string continueIdentifier = null;
                        if (prop.Value is CborString cborString)
                            continueIdentifier = cborString.Value<string>();
                        return new Statement.Continue()
                        {
                            Identifier = continueIdentifier
                        };
                    case "Break":
                        string breakdentifier = null;
                        if (prop.Value is CborString cborString2)
                            breakdentifier = cborString2.Value<string>();
                        return new Statement.Break()
                        {
                            Identifier = breakdentifier
                        };

                    case "Goto":
                        return new Statement.Goto()
                        {
                            Identifier = prop.Value.Value<string>(),
                        };
                    case "Label":
                        return new Statement.Label()
                        {
                            Identifier = valObj["name"].Value<string>(),
                            Block = BlockConverter.ParseBlock(valObj["block"])
                        };
                    case "Del":
                        return new Statement.Del()
                        {
                            Expression = PolymorphicExpressionConverter.ParseExpression(prop.Value),
                        };
                    case "Crash":
                        return new Statement.Crash()
                        {
                            Expression = PolymorphicExpressionConverter.ParseExpression(prop.Value),
                        };
                    default:
                        Console.WriteLine($"{prop.Key}: {prop.Value}");
                        throw new Exception();
                }
            }
            return null;
        }
    }
}
