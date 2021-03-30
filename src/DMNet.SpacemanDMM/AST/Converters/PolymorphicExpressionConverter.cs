using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PolymorphicExpressionConverter : CborConverterBase<Expression>
    {
        public override Expression Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseExpression(cborValue);
        }

        public static Expression ParseExpression(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            var obj = (CborObject)element;
            foreach (var prop in obj)
            {
                var cnt = (CborObject)prop.Value;
                switch (prop.Key.Value<string>())
                {
                    case "Base": // { "unary":[],"term":{ "location":{ "file":3,"line":18,"column":14},"elem":{ "Int":8} },"follow":[]}
                        {
                            var unaryOps = (CborArray)cnt["unary"];
                            var unaryOpsList = new List<Operators.UnaryOp>();
                            foreach (var item in unaryOps)
                            {
                                unaryOpsList.Add(Enum.Parse<Operators.UnaryOp>(item.Value<string>()));
                            }
                            var term = (CborObject)cnt["term"];
                            var termObj = term.ToObject<Spaned<Term>>();
                            var follows = (CborArray)cnt["follow"];
                            return new Expression.Base()
                            {
                                UnaryOps = unaryOpsList.ToArray(),
                                Term = termObj,
                                Follows = follows.ToCollection<Spaned<Follow>[]>()
                            };
                        }
                    case "BinaryOp": // {"op":"Add","lhs":{"Base":{"unary":[],"term":{"location":{"file":2,"line":29,"column":28},"elem":{"Call":["text2num",[{"Base":{"unary":[],"term":{"location":{"file":2,"line":29,"column":37},"elem":{"Call":["time2text",[{"Base":{"unary":[],"term":{"location":{"file":2,"line":29,"column":47},"elem":{"Ident":"world"}},"follow":[{"location":{"file":2,"line":29,"column":52},"elem":{"Field":["Dot","realtime"]}}]}},{"Base":{"unary":[],"term":{"location":{"file":2,"line":29,"column":63},"elem":{"String":"YYYY"}},"follow":[]}}]]}},"follow":[]}}]]}},"follow":[]}},"rhs":{"Base":{"unary":[],"term":{"location":{"file":2,"line":29,"column":74},"elem":{"Int":442}},"follow":[]}}}
                        {
                            var Obj = (CborObject)prop.Value;
                            return new Expression.BinaryOp()
                            {
                                Op = Enum.Parse<Operators.BinaryOp>(Obj["op"].Value<string>()),
                                Lhs = ParseExpression(Obj["lhs"]),
                                Rhs = ParseExpression(Obj["rhs"])
                            };
                        }
                    case "AssignOp": // {"op":"Assign","lhs":{"Base":{"unary":[],"term":{"location":{"file":2,"line":149,"column":2},"elem":{"String":"Command"}},"follow":[]}},"rhs":{"Base":{"unary":[],"term":{"location":{"file":2,"line":149,"column":14},"elem":{"Int":10000}},"follow":[]}}}
                        {
                            var Obj = (CborObject)prop.Value;
                            return new Expression.AssignOp()
                            {
                                Op = Enum.Parse<Operators.AssignOp>(Obj["op"].Value<string>()),
                                Lhs = ParseExpression(Obj["lhs"]),
                                Rhs = ParseExpression(Obj["rhs"])
                            };
                        }
                    case "TernaryOp":
                        {
                            var Obj = (CborObject)prop.Value;
                            return new Expression.TernaryOp()
                            {
                                Condition = ParseExpression(Obj["cond"]),
                                True = ParseExpression(Obj["if_"]),
                                False = ParseExpression(Obj["else_"])
                            };
                        }
                    default:
                        Console.WriteLine($"{prop.Key}: {prop.Value}");
                        throw new NotImplementedException();
                }
            }
            return null;
        }
    }
}
