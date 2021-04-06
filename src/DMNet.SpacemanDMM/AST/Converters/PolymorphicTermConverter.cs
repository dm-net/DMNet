using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PolymorphicTermConverter : CborConverterBase<Term>
    {
        public override Term Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseTerm(cborValue);
        }

        public static Term ParseTerm(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            if(element is CborString str)
            {
                switch (str.Value<string>())
                {
                    case "Null":
                        return new Term.Null();
                    default:
                        Console.WriteLine($"{str}");
                        throw new Exception();
                }
            }
            var obj = (CborObject)element;
            foreach (var prop in obj)
            {
                switch (prop.Key.Value<string>())
                {
                    case "Int":
                        return new Term.Int()
                        {
                            Value = prop.Value.Value<int>()
                        };
                    case "Float":
                        return new Term.Float()
                        {
                            Value = prop.Value.Value<float>()
                        };
                    case "Ident":
                        return new Term.Ident()
                        {
                            Value = prop.Value.Value<string>()
                        };
                    case "String":
                        return new Term.String()
                        {
                            Value = prop.Value.Value<string>()
                        };
                    case "Resource":
                        return new Term.Resource()
                        {
                            Value = prop.Value.Value<string>()
                        };
                    
                    // As


                    case "Expr":
                        return new Term.Expr()
                        {
                            Value = PolymorphicExpressionConverter.ParseExpression(prop.Value)
                        };
                    case "Prefab": // {"path":[["Slash","datum"],["Slash","job"],["Slash","ai"]],"vars":[]}
                        return new Term.Prefab()
                        {
                            Value = PrefabConverter.ParsePrefab(prop.Value)
                        };
                    case "InterpString": // ["",[[{ "Base":{ "unary":[],"term":{ "location":{ "file":45,"line":64,"column":4},"elem":{ "Int":1345} },"follow":[]} },""]]]
                        var interpString = (CborArray)prop.Value;
                        var interpStringRest = new List<Tuple<Expression, string>>();
                        foreach (var tuple in (CborArray)interpString[1])
                        {
                            var realTuple = (CborArray)tuple;
                            interpStringRest.Add(new Tuple<Expression, string>(PolymorphicExpressionConverter.ParseExpression(realTuple[0]), realTuple[1].Value<string>()));
                        }
                        return new Term.InterpString()
                        {
                            Start = interpString[0].Value<string>(),
                            Rest = interpStringRest.ToArray()

                        };
                    case "Call":
                        var callTuple = (CborArray)prop.Value;
                        return new Term.Call()
                        {
                            Identifier = callTuple[0].Value<string>(),
                            Args = ((CborArray)callTuple[1]).ToCollection<Expression[]>()
                        };
                    case "SelfCall":
                        return new Term.SelfCall()
                        {
                            Args = ((CborArray)prop.Value).ToCollection<Expression[]>()
                        };
                    case "ParentCall":
                        return new Term.ParentCall()
                        {
                            Args = ((CborArray)prop.Value).ToCollection<Expression[]>()
                        };
                    case "New": // {"type_":"Implicit","args":null}
                        var newObj = (CborObject)prop.Value;
                        Expression[] newArgs = null;
                        if(newObj["args"] is CborArray cborArray)
                        {
                            newArgs = cborArray.ToCollection<Expression[]>();
                        }
                        return new Term.New()
                        {
                            Type = PolymorphicNewTypeConverter.ParseNewType(newObj["type_"]),
                            Args = newArgs
                        };
                    case "List":
                        var listArgs = (CborArray)prop.Value;
                        return new Term.List()
                        {
                            Items = listArgs.ToCollection<Expression[]>()
                        };
                    case "Input":
                        var inputObj = (CborObject)prop.Value;
                        return new Term.Input()
                        {
                            Args = ((CborArray)inputObj["args"]).ToCollection<Expression[]>(),
                            InputType = FlagConverter<InputType>.ParseFlag(inputObj["input_type"]),
                            In = PolymorphicExpressionConverter.ParseExpression(inputObj["in_list"])
                        };
                    case "Locate":
                        var locateObj = (CborObject)prop.Value;
                        return new Term.Locate()
                        {
                            Args = ((CborArray)locateObj["args"]).ToCollection<Expression[]>(),
                            In = PolymorphicExpressionConverter.ParseExpression(locateObj["in_list"])
                        };
                    case "Pick":
                        var pickValues = (CborArray)prop.Value;
                        var pickValueList = new List<Tuple<Expression, Expression>>();
                        foreach (var item in pickValues)
                        {
                            var tuple = (CborArray)item;
                            var item1 = PolymorphicExpressionConverter.ParseExpression(tuple[0]);
                            var item2 = PolymorphicExpressionConverter.ParseExpression(tuple[1]);
                            pickValueList.Add(new Tuple<Expression, Expression>(item1, item2));
                        }
                        return new Term.Pick()
                        {
                            Values = pickValueList.ToArray()
                        };
                    case "DynamicCall":
                        var dynamicCallTuple = (CborArray)prop.Value;
                        return new Term.DynamicCall()
                        {
                            Indentifiers = ((CborArray)dynamicCallTuple[0]).ToCollection<Expression[]>(),
                            Arguments = ((CborArray)dynamicCallTuple[1]).ToCollection<Expression[]>()
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
