namespace Gu.Diff.Xml

type Diff<'a> = 
    { First : 'a
      Other : 'a
      Property : System.Reflection.PropertyInfo }

module internal CreateDiff =
    let For first other prop =
        { First= first; Other = other; Property= prop }